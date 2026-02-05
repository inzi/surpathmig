import * as fs from 'fs';
import * as path from 'path';
import { minimatch } from 'minimatch';

export interface IgnorePatterns {
    patterns: string[];
    agentSpecificPatterns: string[];
}

export class IgnoreParser {
    private patterns: Set<string> = new Set();
    private rootPath: string;

    constructor(rootPath: string) {
        this.rootPath = rootPath;
        this.loadDefaultPatterns();
    }

    /**
     * Load default patterns that should always be ignored
     */
    private loadDefaultPatterns(): void {
        const defaults = [
            'node_modules/**',
            '**/node_modules/**',
            'bin/**',
            'obj/**',
            '**/bin/**',
            '**/obj/**',
            '.git/**',
            '.claude/**',
            '**/.git/**',
            'dist/**',
            '**/dist/**',
            'build/**',
            '**/build/**',
            '*.min.js',
            '*.min.css',
            '**/*.min.js',
            '**/*.min.css',
            '*.map',
            '**/*.map',
            '.claude-doc.lock',
            '**/.claude-doc.lock'
        ];
        defaults.forEach(pattern => this.patterns.add(pattern));
    }

    /**
     * Load patterns from .gitignore file
     */
    public async loadGitIgnore(): Promise<void> {
        const gitignorePath = path.join(this.rootPath, '.gitignore');
        if (!fs.existsSync(gitignorePath)) {
            return;
        }

        try {
            const content = fs.readFileSync(gitignorePath, 'utf-8');
            const lines = content.split('\n');

            for (const line of lines) {
                const trimmed = line.trim();
                // Skip comments and empty lines
                if (trimmed && !trimmed.startsWith('#')) {
                    // Convert gitignore patterns to glob patterns
                    let pattern = trimmed;

                    // Handle directory patterns
                    if (pattern.endsWith('/')) {
                        pattern = pattern + '**';
                    }

                    // Handle patterns that should match anywhere
                    if (!pattern.startsWith('/') && !pattern.includes('**/')) {
                        pattern = '**/' + pattern;
                    }

                    // Remove leading slash for root-relative patterns
                    if (pattern.startsWith('/')) {
                        pattern = pattern.substring(1);
                    }

                    this.patterns.add(pattern);
                }
            }
        } catch (error) {
            console.warn(`Warning: Could not load .gitignore: ${error}`);
        }
    }

    /**
     * Load patterns from AgentIgnoreList.md
     */
    public async loadAgentIgnoreList(): Promise<void> {
        const agentIgnorePath = path.join(this.rootPath, 'AgentIgnoreList.md');
        if (!fs.existsSync(agentIgnorePath)) {
            return;
        }

        try {
            const content = fs.readFileSync(agentIgnorePath, 'utf-8');
            const lines = content.split('\n');

            let inCodeBlock = false;
            for (const line of lines) {
                const trimmed = line.trim();

                // Handle code blocks
                if (trimmed.startsWith('```')) {
                    inCodeBlock = !inCodeBlock;
                    continue;
                }

                // Skip markdown headers, empty lines, and content in code blocks
                if (!inCodeBlock && trimmed && !trimmed.startsWith('#') && !trimmed.startsWith('-')) {
                    continue;
                }

                // Process patterns (lines starting with - or in code blocks)
                if (inCodeBlock || trimmed.startsWith('-')) {
                    let pattern = trimmed.startsWith('-') ? trimmed.substring(1).trim() : trimmed;

                    if (pattern) {
                        // Convert to glob pattern
                        if (pattern.endsWith('/')) {
                            pattern = pattern + '**';
                        }
                        if (!pattern.startsWith('/') && !pattern.includes('**/')) {
                            pattern = '**/' + pattern;
                        }
                        if (pattern.startsWith('/')) {
                            pattern = pattern.substring(1);
                        }

                        this.patterns.add(pattern);
                    }
                }
            }
        } catch (error) {
            console.warn(`Warning: Could not load AgentIgnoreList.md: ${error}`);
        }
    }

    /**
     * Check if a path should be ignored
     */
    public shouldIgnore(filePath: string): boolean {
        // Normalize the path relative to root
        const relativePath = path.relative(this.rootPath, filePath).replace(/\\/g, '/');

        // Check against all patterns
        for (const pattern of this.patterns) {
            if (minimatch(relativePath, pattern, { dot: true })) {
                return true;
            }
        }

        return false;
    }

    /**
     * Get all loaded patterns (for debugging)
     */
    public getPatterns(): string[] {
        return Array.from(this.patterns);
    }

    /**
     * Add a custom pattern
     */
    public addPattern(pattern: string): void {
        this.patterns.add(pattern);
    }

    /**
     * Initialize the parser with all ignore files
     */
    public async initialize(): Promise<void> {
        await this.loadGitIgnore();
        await this.loadAgentIgnoreList();
    }
}