import * as fs from 'fs';
import * as path from 'path';
import { IgnoreParser } from './ignore-parser';
import { CrossReferenceAnalyzer, ComponentUsage } from './cross-reference-analyzer';

export interface FileInfo {
    name: string;
    path: string;
    extension: string;
    size: number;
    description?: string;
    keyComponents?: string[];
    dependencies?: string[];
    crossReferences?: ComponentUsage[];
}

export interface FolderInfo {
    name: string;
    path: string;
    files: FileInfo[];
    subfolders: FolderInfo[];
    hasClaudeMd: boolean;
    depth: number;
}

export interface CodeAnalysis {
    purpose: string;
    keyComponents: string[];
    dependencies: string[];
    patterns: string[];
    businessLogic: string[];
    crossReferences?: ComponentUsage[];
    totalUsages?: number;
    impactRadius?: number;
    testCoverage?: string[];
}

export class FolderAnalyzer {
    private ignoreParser: IgnoreParser;
    private rootPath: string;
    private projectContext: string = '';
    private crossRefAnalyzer: CrossReferenceAnalyzer | null = null;
    private enableCrossReferences: boolean;

    constructor(rootPath: string, ignoreParser: IgnoreParser, enableCrossReferences: boolean = false) {
        this.rootPath = rootPath;
        this.ignoreParser = ignoreParser;
        this.enableCrossReferences = enableCrossReferences;

        if (enableCrossReferences) {
            this.crossRefAnalyzer = new CrossReferenceAnalyzer(rootPath);
        }
    }

    /**
     * Load project context from README.md
     */
    public async loadProjectContext(): Promise<void> {
        const readmePath = path.join(this.rootPath, 'README.md');
        if (fs.existsSync(readmePath)) {
            try {
                this.projectContext = fs.readFileSync(readmePath, 'utf-8');
            } catch (error) {
                console.warn(`Could not load README.md: ${error}`);
            }
        }
    }

    /**
     * Build a folder tree structure
     */
    public buildFolderTree(folderPath: string, depth: number = 0): FolderInfo | null {
        if (this.ignoreParser.shouldIgnore(folderPath)) {
            return null;
        }

        const folderName = path.basename(folderPath);
        const folderInfo: FolderInfo = {
            name: folderName,
            path: folderPath,
            files: [],
            subfolders: [],
            hasClaudeMd: false,
            depth: depth
        };

        try {
            const entries = fs.readdirSync(folderPath, { withFileTypes: true });

            for (const entry of entries) {
                const entryPath = path.join(folderPath, entry.name);

                if (this.ignoreParser.shouldIgnore(entryPath)) {
                    continue;
                }

                if (entry.isDirectory()) {
                    const subfolder = this.buildFolderTree(entryPath, depth + 1);
                    if (subfolder) {
                        folderInfo.subfolders.push(subfolder);
                    }
                } else if (entry.isFile()) {
                    if (entry.name === 'CLAUDE.md') {
                        folderInfo.hasClaudeMd = true;
                    }

                    const fileInfo: FileInfo = {
                        name: entry.name,
                        path: entryPath,
                        extension: path.extname(entry.name),
                        size: fs.statSync(entryPath).size
                    };

                    // Don't include CLAUDE.md in files list
                    if (entry.name !== 'CLAUDE.md') {
                        folderInfo.files.push(fileInfo);
                    }
                }
            }
        } catch (error) {
            console.warn(`Error reading folder ${folderPath}: ${error}`);
        }

        return folderInfo;
    }

    /**
     * Get folders sorted by depth (deepest first)
     */
    public getFoldersByDepth(folderInfo: FolderInfo): FolderInfo[] {
        const folders: FolderInfo[] = [folderInfo];

        const collectFolders = (folder: FolderInfo) => {
            for (const subfolder of folder.subfolders) {
                folders.push(subfolder);
                collectFolders(subfolder);
            }
        };

        collectFolders(folderInfo);

        // Sort by depth (deepest first)
        return folders.sort((a, b) => b.depth - a.depth);
    }

    /**
     * Analyze code files in a folder
     */
    public async analyzeFolder(folderInfo: FolderInfo): Promise<CodeAnalysis> {
        const dependenciesSet = new Set<string>();
        const analysis: CodeAnalysis = {
            purpose: '',
            keyComponents: [],
            dependencies: [],
            patterns: [],
            businessLogic: [],
            crossReferences: [],
            totalUsages: 0,
            impactRadius: 0,
            testCoverage: []
        };

        // Analyze each file
        for (const file of folderInfo.files) {
            await this.analyzeFile(file, analysis, dependenciesSet);

            // Analyze cross-references if enabled
            if (this.enableCrossReferences && this.crossRefAnalyzer) {
                try {
                    const crossRefs = await this.crossRefAnalyzer.analyzeFile(file.path);
                    if (crossRefs.length > 0) {
                        file.crossReferences = crossRefs;
                        analysis.crossReferences!.push(...crossRefs);

                        // Aggregate metrics
                        analysis.totalUsages! += crossRefs.reduce((sum, ref) => sum + ref.usages.length, 0);

                        // Track test coverage
                        const testFiles = new Set(analysis.testCoverage);
                        crossRefs.forEach(ref => ref.testFiles.forEach(f => testFiles.add(f)));
                        analysis.testCoverage = Array.from(testFiles);

                        // Calculate maximum impact radius
                        const maxImpact = Math.max(...crossRefs.map(ref => ref.impactRadius));
                        if (maxImpact > analysis.impactRadius!) {
                            analysis.impactRadius = maxImpact;
                        }
                    }
                } catch (error) {
                    console.warn(`Could not analyze cross-references for ${file.path}: ${error}`);
                }
            }
        }

        // Convert dependencies Set to Array
        analysis.dependencies = Array.from(dependenciesSet);

        // Determine folder purpose based on content
        analysis.purpose = this.determineFolderPurpose(folderInfo, analysis);

        // Identify patterns
        analysis.patterns = this.identifyPatterns(folderInfo, analysis);

        return analysis;
    }

    /**
     * Analyze a single file
     */
    private async analyzeFile(file: FileInfo, analysis: CodeAnalysis, dependenciesSet: Set<string>): Promise<void> {
        try {
            // Skip binary files and very large files
            if (this.isBinaryFile(file.extension) || file.size > 1000000) {
                return;
            }

            const content = fs.readFileSync(file.path, 'utf-8');

            // Extract imports/dependencies
            this.extractDependencies(content, file.extension, dependenciesSet);

            // Extract key components based on file type
            const components = this.extractKeyComponents(content, file.extension);
            if (components.length > 0) {
                file.keyComponents = components;
                analysis.keyComponents.push(...components.map(c => `${file.name}: ${c}`));
            }

            // Extract business logic indicators
            const businessLogic = this.extractBusinessLogic(content, file.name);
            if (businessLogic.length > 0) {
                analysis.businessLogic.push(...businessLogic);
            }

            // Generate file description
            file.description = this.generateFileDescription(file, content);

        } catch (error) {
            console.warn(`Error analyzing file ${file.path}: ${error}`);
        }
    }

    /**
     * Extract dependencies from file content
     */
    private extractDependencies(content: string, extension: string, dependencies: Set<string>): void {
        const patterns: { [key: string]: RegExp[] } = {
            '.ts': [/import .* from ['"]([^'"]+)['"]/g, /require\(['"]([^'"]+)['"]\)/g],
            '.tsx': [/import .* from ['"]([^'"]+)['"]/g],
            '.js': [/import .* from ['"]([^'"]+)['"]/g, /require\(['"]([^'"]+)['"]\)/g],
            '.jsx': [/import .* from ['"]([^'"]+)['"]/g],
            '.cs': [/using ([A-Za-z.]+);/g],
            '.py': [/import ([A-Za-z_][A-Za-z0-9_]*)/g, /from ([A-Za-z_][A-Za-z0-9_]*) import/g],
            '.java': [/import ([A-Za-z.]+);/g],
            '.go': [/import \(?["']([^'"]+)["']/g]
        };

        const filePatterns = patterns[extension] || [];
        for (const pattern of filePatterns) {
            let match;
            while ((match = pattern.exec(content)) !== null) {
                const dep = match[1];
                if (dep && !dep.startsWith('.')) {
                    dependencies.add(dep.split('/')[0]);
                }
            }
        }
    }

    /**
     * Extract key components from file content
     */
    private extractKeyComponents(content: string, extension: string): string[] {
        const components: string[] = [];
        const patterns: { [key: string]: RegExp[] } = {
            '.ts': [
                /export\s+(?:abstract\s+)?class\s+(\w+)/g,
                /export\s+interface\s+(\w+)/g,
                /export\s+(?:async\s+)?function\s+(\w+)/g,
                /export\s+const\s+(\w+)/g
            ],
            '.tsx': [
                /export\s+(?:default\s+)?(?:function|const)\s+(\w+)/g,
                /export\s+interface\s+(\w+)/g
            ],
            '.cs': [
                /public\s+(?:partial\s+)?(?:abstract\s+)?class\s+(\w+)/g,
                /public\s+interface\s+(\w+)/g,
                /public\s+(?:async\s+)?(?:Task<.*?>\s+)?(\w+)\s*\(/g
            ],
            '.py': [
                /class\s+(\w+)/g,
                /def\s+(\w+)\s*\(/g
            ],
            '.java': [
                /public\s+(?:abstract\s+)?class\s+(\w+)/g,
                /public\s+interface\s+(\w+)/g,
                /public\s+(?:static\s+)?(?:\w+\s+)?(\w+)\s*\(/g
            ]
        };

        const filePatterns = patterns[extension] || [];
        for (const pattern of filePatterns) {
            let match;
            while ((match = pattern.exec(content)) !== null) {
                if (match[1] && !components.includes(match[1])) {
                    components.push(match[1]);
                }
            }
        }

        return components.slice(0, 10); // Limit to top 10 components
    }

    /**
     * Extract business logic indicators
     */
    private extractBusinessLogic(content: string, fileName: string): string[] {
        const logic: string[] = [];
        const businessKeywords = [
            'validate', 'calculate', 'process', 'transform', 'convert',
            'authorize', 'authenticate', 'permission', 'rule', 'policy',
            'workflow', 'business', 'domain', 'service', 'repository'
        ];

        const lowerContent = content.toLowerCase();
        const lowerFileName = fileName.toLowerCase();

        for (const keyword of businessKeywords) {
            if (lowerContent.includes(keyword) || lowerFileName.includes(keyword)) {
                // Find context around the keyword
                const lines = content.split('\n');
                for (let i = 0; i < lines.length; i++) {
                    if (lines[i].toLowerCase().includes(keyword)) {
                        const context = lines[i].trim();
                        if (context.length > 20 && context.length < 200) {
                            logic.push(context);
                            break;
                        }
                    }
                }
            }
        }

        return logic.slice(0, 5); // Limit to 5 business logic items
    }

    /**
     * Generate a description for a file
     */
    private generateFileDescription(file: FileInfo, content: string): string {
        const lowerName = file.name.toLowerCase();

        // Common file patterns
        if (lowerName.includes('controller')) return 'API endpoint controller';
        if (lowerName.includes('service')) return 'Business service implementation';
        if (lowerName.includes('model') || lowerName.includes('entity')) return 'Data model/entity';
        if (lowerName.includes('dto')) return 'Data transfer object';
        if (lowerName.includes('repository')) return 'Data access layer';
        if (lowerName.includes('component')) return 'UI component';
        if (lowerName.includes('util') || lowerName.includes('helper')) return 'Utility/helper functions';
        if (lowerName.includes('config')) return 'Configuration';
        if (lowerName.includes('test') || lowerName.includes('spec')) return 'Test file';
        if (lowerName.includes('migration')) return 'Database migration';
        if (lowerName.includes('interface')) return 'Interface definition';
        if (lowerName.includes('enum')) return 'Enumeration definition';
        if (lowerName.includes('constant') || lowerName.includes('const')) return 'Constants definition';

        // Check first few lines for comments
        const lines = content.split('\n').slice(0, 10);
        for (const line of lines) {
            if (line.includes('//') || line.includes('/*') || line.includes('#')) {
                const comment = line.replace(/\/\/|\/\*|\*\/|#/g, '').trim();
                if (comment.length > 10 && comment.length < 100) {
                    return comment;
                }
            }
        }

        return 'Code file';
    }

    /**
     * Determine the purpose of a folder
     */
    private determineFolderPurpose(folderInfo: FolderInfo, analysis: CodeAnalysis): string {
        const folderName = folderInfo.name.toLowerCase();

        // Common folder patterns
        const patterns: { [key: string]: string } = {
            'controller': 'API controllers and endpoints',
            'service': 'Business logic and services',
            'model': 'Data models and entities',
            'entity': 'Domain entities',
            'dto': 'Data transfer objects',
            'repository': 'Data access layer',
            'component': 'UI components',
            'view': 'View templates and UI',
            'util': 'Utility functions and helpers',
            'helper': 'Helper functions',
            'config': 'Configuration files',
            'test': 'Test files',
            'spec': 'Test specifications',
            'migration': 'Database migrations',
            'interface': 'Interface definitions',
            'enum': 'Enumerations',
            'constant': 'Constants and configuration',
            'common': 'Shared/common functionality',
            'shared': 'Shared resources',
            'core': 'Core business logic',
            'infrastructure': 'Infrastructure and cross-cutting concerns',
            'application': 'Application layer logic',
            'domain': 'Domain layer logic',
            'api': 'API definitions and handlers',
            'auth': 'Authentication and authorization',
            'validation': 'Validation logic'
        };

        for (const [pattern, purpose] of Object.entries(patterns)) {
            if (folderName.includes(pattern)) {
                return purpose;
            }
        }

        // Analyze file content
        if (analysis.keyComponents.length > 0) {
            const components = analysis.keyComponents.join(', ');
            if (components.toLowerCase().includes('controller')) return 'API controllers';
            if (components.toLowerCase().includes('service')) return 'Service implementations';
            if (components.toLowerCase().includes('component')) return 'UI components';
        }

        // Default based on content
        if (folderInfo.files.length > 0) {
            const extensions = folderInfo.files.map(f => f.extension);
            if (extensions.every(e => ['.ts', '.tsx', '.js', '.jsx'].includes(e))) {
                return 'TypeScript/JavaScript code';
            }
            if (extensions.every(e => ['.cs'].includes(e))) {
                return 'C# code implementation';
            }
        }

        return 'Implementation files';
    }

    /**
     * Identify patterns in the folder
     */
    private identifyPatterns(folderInfo: FolderInfo, analysis: CodeAnalysis): string[] {
        const patterns: string[] = [];

        // Check for common patterns
        const fileNames = folderInfo.files.map(f => f.name.toLowerCase());

        if (fileNames.some(n => n.includes('controller'))) {
            patterns.push('MVC Controller pattern');
        }
        if (fileNames.some(n => n.includes('service'))) {
            patterns.push('Service layer pattern');
        }
        if (fileNames.some(n => n.includes('repository'))) {
            patterns.push('Repository pattern');
        }
        if (fileNames.some(n => n.includes('factory'))) {
            patterns.push('Factory pattern');
        }
        if (fileNames.some(n => n.includes('builder'))) {
            patterns.push('Builder pattern');
        }
        if (fileNames.some(n => n.includes('dto'))) {
            patterns.push('DTO pattern');
        }
        if (fileNames.some(n => n.includes('interface') || n.startsWith('i'))) {
            patterns.push('Interface-based design');
        }

        // Framework-specific patterns
        if (analysis.dependencies.includes('@angular')) {
            patterns.push('Angular framework');
        }
        if (analysis.dependencies.includes('react')) {
            patterns.push('React framework');
        }
        if (analysis.dependencies.includes('express')) {
            patterns.push('Express.js framework');
        }

        return patterns;
    }

    /**
     * Check if file is binary
     */
    private isBinaryFile(extension: string): boolean {
        const binaryExtensions = [
            '.png', '.jpg', '.jpeg', '.gif', '.bmp', '.ico', '.svg',
            '.pdf', '.zip', '.tar', '.gz', '.rar', '.7z',
            '.exe', '.dll', '.so', '.dylib',
            '.mp3', '.mp4', '.avi', '.mov',
            '.doc', '.docx', '.xls', '.xlsx', '.ppt', '.pptx'
        ];
        return binaryExtensions.includes(extension.toLowerCase());
    }
}