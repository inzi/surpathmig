import * as fs from 'fs';
import * as path from 'path';
import { execSync } from 'child_process';
import * as crypto from 'crypto';

export interface CrossReference {
    type: 'usage' | 'import' | 'inheritance' | 'implementation' | 'instantiation' | 'test';
    sourceFile: string;
    targetFile: string;
    lineNumber: number;
    context: string;
    symbol?: string;
}

export interface ComponentUsage {
    component: string;
    file: string;
    usages: CrossReference[];
    directConsumers: string[];
    testFiles: string[];
    impactRadius: number;
}

export interface CrossReferenceData {
    usages: Map<string, ComponentUsage>;
    imports: Map<string, string[]>;
    exports: Map<string, string[]>;
    dependencies: Map<string, Set<string>>;
    inverseDependencies: Map<string, Set<string>>;
}

interface AstGrepPattern {
    pattern: string;
    language: string;
    description: string;
    extractSymbol?: (match: string) => string;
}

export class CrossReferenceAnalyzer {
    private rootPath: string;
    private cacheDir: string;
    private crossRefData: CrossReferenceData;
    private fileCache: Map<string, string>;

    constructor(rootPath: string) {
        this.rootPath = rootPath;
        this.cacheDir = path.join(rootPath, '.claude', 'cache');
        this.fileCache = new Map();
        this.crossRefData = {
            usages: new Map(),
            imports: new Map(),
            exports: new Map(),
            dependencies: new Map(),
            inverseDependencies: new Map()
        };

        this.ensureCacheDirectory();
    }

    private ensureCacheDirectory(): void {
        if (!fs.existsSync(this.cacheDir)) {
            fs.mkdirSync(this.cacheDir, { recursive: true });
        }
    }

    /**
     * Get language from file extension
     */
    private getLanguage(filePath: string): string {
        const ext = path.extname(filePath).toLowerCase();
        const languageMap: { [key: string]: string } = {
            '.ts': 'typescript',
            '.tsx': 'tsx',
            '.js': 'javascript',
            '.jsx': 'jsx',
            '.cs': 'csharp',
            '.py': 'python',
            '.java': 'java',
            '.go': 'go',
            '.rs': 'rust',
            '.cpp': 'cpp',
            '.c': 'c',
            '.php': 'php',
            '.rb': 'ruby'
        };
        return languageMap[ext] || 'text';
    }

    /**
     * Define AST patterns for different languages and purposes
     */
    private getPatterns(language: string): AstGrepPattern[] {
        const patterns: { [key: string]: AstGrepPattern[] } = {
            csharp: [
                {
                    pattern: 'new $CLASS($$$)',
                    language: 'csharp',
                    description: 'Class instantiation',
                    extractSymbol: (match) => match.match(/new\s+(\w+)/)?.[1] || ''
                },
                {
                    pattern: '$CLASS.$METHOD($$$)',
                    language: 'csharp',
                    description: 'Static method call',
                    extractSymbol: (match) => match.split('.')[0]
                },
                {
                    pattern: 'using $NAMESPACE',
                    language: 'csharp',
                    description: 'Namespace import',
                    extractSymbol: (match) => match.replace('using', '').trim().replace(';', '')
                },
                {
                    pattern: ': $INTERFACE',
                    language: 'csharp',
                    description: 'Interface implementation',
                    extractSymbol: (match) => match.replace(':', '').trim()
                },
                {
                    pattern: 'public class $CLASS : $BASE',
                    language: 'csharp',
                    description: 'Class inheritance',
                    extractSymbol: (match) => {
                        const parts = match.match(/class\s+(\w+)\s*:\s*(\w+)/);
                        return parts ? parts[2] : '';
                    }
                }
            ],
            typescript: [
                {
                    pattern: 'new $CLASS($$$)',
                    language: 'typescript',
                    description: 'Class instantiation',
                    extractSymbol: (match) => match.match(/new\s+(\w+)/)?.[1] || ''
                },
                {
                    pattern: 'import { $$$IMPORTS } from "$MODULE"',
                    language: 'typescript',
                    description: 'Named imports',
                    extractSymbol: (match) => match.match(/from\s+['"](.*)['"]/)?.[1] || ''
                },
                {
                    pattern: 'import $DEFAULT from "$MODULE"',
                    language: 'typescript',
                    description: 'Default import',
                    extractSymbol: (match) => match.match(/from\s+['"](.*)['"]/)?.[1] || ''
                },
                {
                    pattern: 'export class $CLASS',
                    language: 'typescript',
                    description: 'Class export',
                    extractSymbol: (match) => match.match(/class\s+(\w+)/)?.[1] || ''
                },
                {
                    pattern: 'export interface $INTERFACE',
                    language: 'typescript',
                    description: 'Interface export',
                    extractSymbol: (match) => match.match(/interface\s+(\w+)/)?.[1] || ''
                },
                {
                    pattern: 'export function $FUNCTION',
                    language: 'typescript',
                    description: 'Function export',
                    extractSymbol: (match) => match.match(/function\s+(\w+)/)?.[1] || ''
                },
                {
                    pattern: 'extends $CLASS',
                    language: 'typescript',
                    description: 'Class inheritance',
                    extractSymbol: (match) => match.match(/extends\s+(\w+)/)?.[1] || ''
                },
                {
                    pattern: 'implements $INTERFACE',
                    language: 'typescript',
                    description: 'Interface implementation',
                    extractSymbol: (match) => match.match(/implements\s+(\w+)/)?.[1] || ''
                }
            ],
            python: [
                {
                    pattern: 'import $MODULE',
                    language: 'python',
                    description: 'Module import',
                    extractSymbol: (match) => match.replace('import', '').trim()
                },
                {
                    pattern: 'from $MODULE import $$$',
                    language: 'python',
                    description: 'From import',
                    extractSymbol: (match) => match.match(/from\s+(\S+)\s+import/)?.[1] || ''
                },
                {
                    pattern: 'class $CLASS($BASE)',
                    language: 'python',
                    description: 'Class definition with inheritance',
                    extractSymbol: (match) => {
                        const parts = match.match(/class\s+\w+\((\w+)\)/);
                        return parts ? parts[1] : '';
                    }
                },
                {
                    pattern: '$CLASS($$$)',
                    language: 'python',
                    description: 'Class instantiation or function call',
                    extractSymbol: (match) => match.match(/(\w+)\(/)?.[1] || ''
                }
            ]
        };

        return patterns[language] || [];
    }

    /**
     * Run ast-grep command and parse results
     */
    private runAstGrep(pattern: string, language: string, targetPath: string): any[] {
        try {
            const cacheKey = this.getCacheKey(pattern, language, targetPath);
            const cachedResult = this.getCachedResult(cacheKey);

            if (cachedResult) {
                return cachedResult;
            }

            // Escape pattern for shell
            const escapedPattern = pattern.replace(/"/g, '\\"').replace(/\$/g, '\\$');

            const command = `ast-grep --lang ${language} -p "${escapedPattern}" "${targetPath}" --json`;
            const result = execSync(command, {
                encoding: 'utf-8',
                maxBuffer: 10 * 1024 * 1024 // 10MB buffer
            });

            const matches = result.split('\n')
                .filter(line => line.trim())
                .map(line => {
                    try {
                        return JSON.parse(line);
                    } catch {
                        return null;
                    }
                })
                .filter(match => match !== null);

            this.cacheResult(cacheKey, matches);
            return matches;
        } catch (error: any) {
            // ast-grep might not be available or pattern might not match anything
            if (error.code === 'ENOENT') {
                console.warn('ast-grep is not available. Install it for cross-reference features.');
            }
            return [];
        }
    }

    /**
     * Analyze cross-references for a specific file
     */
    public async analyzeFile(filePath: string): Promise<ComponentUsage[]> {
        const language = this.getLanguage(filePath);
        const patterns = this.getPatterns(language);
        const usages: ComponentUsage[] = [];

        if (!fs.existsSync(filePath) || patterns.length === 0) {
            return usages;
        }

        // Extract components from the file
        const components = await this.extractComponents(filePath, language);

        // For each component, find its usages across the codebase
        for (const component of components) {
            const componentUsage: ComponentUsage = {
                component: component.name,
                file: filePath,
                usages: [],
                directConsumers: [],
                testFiles: [],
                impactRadius: 0
            };

            // Search for usages of this component
            const searchPatterns = this.getSearchPatternsForComponent(component, language);

            for (const searchPattern of searchPatterns) {
                const matches = this.runAstGrep(
                    searchPattern.pattern,
                    searchPattern.language,
                    this.rootPath
                );

                for (const match of matches) {
                    if (match.file && match.file !== filePath) {
                        const crossRef: CrossReference = {
                            type: this.determineReferenceType(searchPattern.description),
                            sourceFile: filePath,
                            targetFile: match.file,
                            lineNumber: match.range?.start?.line || 0,
                            context: match.text || '',
                            symbol: component.name
                        };

                        componentUsage.usages.push(crossRef);

                        // Track direct consumers
                        if (!componentUsage.directConsumers.includes(match.file)) {
                            componentUsage.directConsumers.push(match.file);
                        }

                        // Track test files
                        if (this.isTestFile(match.file)) {
                            if (!componentUsage.testFiles.includes(match.file)) {
                                componentUsage.testFiles.push(match.file);
                            }
                        }
                    }
                }
            }

            // Calculate impact radius
            componentUsage.impactRadius = await this.calculateImpactRadius(
                componentUsage.directConsumers
            );

            if (componentUsage.usages.length > 0) {
                usages.push(componentUsage);
            }
        }

        return usages;
    }

    /**
     * Extract components (classes, functions, interfaces) from a file
     */
    private async extractComponents(filePath: string, language: string): Promise<any[]> {
        const components: any[] = [];

        const extractionPatterns: { [key: string]: AstGrepPattern[] } = {
            csharp: [
                {
                    pattern: 'public class $CLASS',
                    language: 'csharp',
                    description: 'Public class',
                    extractSymbol: (match) => match.match(/class\s+(\w+)/)?.[1] || ''
                },
                {
                    pattern: 'public interface $INTERFACE',
                    language: 'csharp',
                    description: 'Public interface',
                    extractSymbol: (match) => match.match(/interface\s+(\w+)/)?.[1] || ''
                }
            ],
            typescript: [
                {
                    pattern: 'export class $CLASS',
                    language: 'typescript',
                    description: 'Exported class',
                    extractSymbol: (match) => match.match(/class\s+(\w+)/)?.[1] || ''
                },
                {
                    pattern: 'export interface $INTERFACE',
                    language: 'typescript',
                    description: 'Exported interface',
                    extractSymbol: (match) => match.match(/interface\s+(\w+)/)?.[1] || ''
                },
                {
                    pattern: 'export function $FUNCTION',
                    language: 'typescript',
                    description: 'Exported function',
                    extractSymbol: (match) => match.match(/function\s+(\w+)/)?.[1] || ''
                }
            ]
        };

        const patterns = extractionPatterns[language] || [];

        for (const pattern of patterns) {
            const matches = this.runAstGrep(pattern.pattern, language, filePath);
            for (const match of matches) {
                const symbol = pattern.extractSymbol ? pattern.extractSymbol(match.text || '') : '';
                if (symbol) {
                    components.push({
                        name: symbol,
                        type: pattern.description,
                        file: filePath
                    });
                }
            }
        }

        return components;
    }

    /**
     * Get search patterns for finding usages of a component
     */
    private getSearchPatternsForComponent(component: any, language: string): AstGrepPattern[] {
        const patterns: AstGrepPattern[] = [];
        const name = component.name;

        if (language === 'csharp') {
            patterns.push(
                {
                    pattern: `new ${name}($$$)`,
                    language: 'csharp',
                    description: 'instantiation'
                },
                {
                    pattern: `${name}.$$$`,
                    language: 'csharp',
                    description: 'static usage'
                },
                {
                    pattern: `: ${name}`,
                    language: 'csharp',
                    description: 'inheritance'
                }
            );
        } else if (language === 'typescript' || language === 'javascript') {
            patterns.push(
                {
                    pattern: `new ${name}($$$)`,
                    language: language,
                    description: 'instantiation'
                },
                {
                    pattern: `extends ${name}`,
                    language: language,
                    description: 'inheritance'
                },
                {
                    pattern: `implements ${name}`,
                    language: language,
                    description: 'implementation'
                },
                {
                    pattern: `import { ${name} }`,
                    language: language,
                    description: 'import'
                }
            );
        }

        return patterns;
    }

    /**
     * Determine the type of reference based on the description
     */
    private determineReferenceType(description: string): CrossReference['type'] {
        if (description.includes('import')) return 'import';
        if (description.includes('inherit')) return 'inheritance';
        if (description.includes('implement')) return 'implementation';
        if (description.includes('instantiat')) return 'instantiation';
        if (description.includes('test')) return 'test';
        return 'usage';
    }

    /**
     * Check if a file is a test file
     */
    private isTestFile(filePath: string): boolean {
        const fileName = path.basename(filePath).toLowerCase();
        return fileName.includes('test') ||
               fileName.includes('spec') ||
               fileName.includes('.test.') ||
               fileName.includes('.spec.');
    }

    /**
     * Calculate the impact radius of changes to a component
     */
    private async calculateImpactRadius(directConsumers: string[]): Promise<number> {
        const visited = new Set<string>();
        const queue = [...directConsumers];
        let radius = 0;

        while (queue.length > 0) {
            const current = queue.shift()!;
            if (visited.has(current)) continue;

            visited.add(current);
            radius++;

            // Find files that import/depend on the current file
            const dependents = await this.findDependents(current);
            for (const dependent of dependents) {
                if (!visited.has(dependent)) {
                    queue.push(dependent);
                }
            }

            // Limit search depth to prevent runaway
            if (radius > 100) break;
        }

        return radius;
    }

    /**
     * Find files that depend on a given file
     */
    private async findDependents(filePath: string): Promise<string[]> {
        const dependents: string[] = [];
        const fileName = path.basename(filePath, path.extname(filePath));
        const language = this.getLanguage(filePath);

        // Search for imports of this file
        const searchPatterns: AstGrepPattern[] = [];

        if (language === 'typescript' || language === 'javascript') {
            searchPatterns.push(
                {
                    pattern: `import $$ from '$$${fileName}$$'`,
                    language: language,
                    description: 'import'
                },
                {
                    pattern: `require('$$${fileName}$$')`,
                    language: language,
                    description: 'require'
                }
            );
        } else if (language === 'csharp') {
            // For C#, we look for using statements that might reference this namespace
            const namespace = this.extractNamespace(filePath);
            if (namespace) {
                searchPatterns.push({
                    pattern: `using ${namespace}`,
                    language: 'csharp',
                    description: 'using'
                });
            }
        }

        for (const pattern of searchPatterns) {
            const matches = this.runAstGrep(pattern.pattern, pattern.language, this.rootPath);
            for (const match of matches) {
                if (match.file && match.file !== filePath && !dependents.includes(match.file)) {
                    dependents.push(match.file);
                }
            }
        }

        return dependents;
    }

    /**
     * Extract namespace from a C# file
     */
    private extractNamespace(filePath: string): string | null {
        try {
            const content = fs.readFileSync(filePath, 'utf-8');
            const namespaceMatch = content.match(/namespace\s+([\w.]+)/);
            return namespaceMatch ? namespaceMatch[1] : null;
        } catch {
            return null;
        }
    }

    /**
     * Generate cache key for ast-grep results
     */
    private getCacheKey(pattern: string, language: string, targetPath: string): string {
        const hash = crypto.createHash('md5');
        hash.update(`${pattern}:${language}:${targetPath}`);
        return hash.digest('hex');
    }

    /**
     * Get cached result if available and fresh
     */
    private getCachedResult(cacheKey: string): any[] | null {
        const cachePath = path.join(this.cacheDir, `${cacheKey}.json`);

        if (!fs.existsSync(cachePath)) {
            return null;
        }

        try {
            const stats = fs.statSync(cachePath);
            const age = Date.now() - stats.mtimeMs;

            // Cache expires after 1 hour
            if (age > 3600000) {
                fs.unlinkSync(cachePath);
                return null;
            }

            const content = fs.readFileSync(cachePath, 'utf-8');
            return JSON.parse(content);
        } catch {
            return null;
        }
    }

    /**
     * Cache ast-grep results
     */
    private cacheResult(cacheKey: string, result: any[]): void {
        const cachePath = path.join(this.cacheDir, `${cacheKey}.json`);
        try {
            fs.writeFileSync(cachePath, JSON.stringify(result), 'utf-8');
        } catch {
            // Ignore cache write errors
        }
    }

    /**
     * Clear all cache files
     */
    public clearCache(): void {
        if (fs.existsSync(this.cacheDir)) {
            const files = fs.readdirSync(this.cacheDir);
            for (const file of files) {
                if (file.endsWith('.json')) {
                    fs.unlinkSync(path.join(this.cacheDir, file));
                }
            }
        }
    }

    /**
     * Get a summary of cross-references for documentation
     */
    public getSummary(usages: ComponentUsage[]): string {
        if (usages.length === 0) {
            return 'No external usages detected.';
        }

        const totalUsages = usages.reduce((sum, u) => sum + u.usages.length, 0);
        const totalConsumers = new Set(usages.flatMap(u => u.directConsumers)).size;
        const totalTests = new Set(usages.flatMap(u => u.testFiles)).size;
        const maxImpact = Math.max(...usages.map(u => u.impactRadius));

        return `Found ${totalUsages} usage${totalUsages !== 1 ? 's' : ''} across ${totalConsumers} file${totalConsumers !== 1 ? 's' : ''}. ` +
               `Covered by ${totalTests} test file${totalTests !== 1 ? 's' : ''}. ` +
               `Maximum impact radius: ${maxImpact} file${maxImpact !== 1 ? 's' : ''}.`;
    }
}