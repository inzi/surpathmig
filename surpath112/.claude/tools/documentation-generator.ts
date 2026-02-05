import * as fs from 'fs';
import * as path from 'path';
import { IgnoreParser } from './ignore-parser';
import { FolderAnalyzer, FolderInfo } from './folder-analyzer';
import { ClaudeMdWriter, SubfolderSummary } from './claude-md-writer';
import { ParallelCoordinator } from './parallel-coordinator';

export class DocumentationGenerator {
    private rootPath: string;
    private ignoreParser: IgnoreParser;
    private folderAnalyzer: FolderAnalyzer;
    private claudeMdWriter: ClaudeMdWriter;
    private parallelCoordinator: ParallelCoordinator;
    private processedFolders: string[] = [];
    private skipExisting: boolean;
    private enableCrossReferences: boolean;

    constructor(rootPath: string, skipExisting: boolean = false, enableCrossReferences: boolean = false) {
        this.rootPath = rootPath;
        this.skipExisting = skipExisting;
        this.enableCrossReferences = enableCrossReferences;
        this.ignoreParser = new IgnoreParser(rootPath);
        this.folderAnalyzer = new FolderAnalyzer(rootPath, this.ignoreParser, enableCrossReferences);
        this.claudeMdWriter = new ClaudeMdWriter(rootPath);
        this.parallelCoordinator = new ParallelCoordinator(rootPath);
    }

    /**
     * Initialize the generator
     */
    public async initialize(): Promise<void> {
        console.log('Initializing documentation generator...');

        // Load ignore patterns
        await this.ignoreParser.initialize();
        console.log(`Loaded ${this.ignoreParser.getPatterns().length} ignore patterns`);

        // Load project context
        await this.folderAnalyzer.loadProjectContext();

        // Report cross-reference status
        if (this.enableCrossReferences) {
            console.log('Cross-reference analysis enabled (using ast-grep)');
        }

        // Clean up stale locks
        this.parallelCoordinator.cleanupStaleLocks();

        // Set up cleanup on exit
        process.on('exit', () => this.parallelCoordinator.cleanup());
        process.on('SIGINT', () => {
            this.parallelCoordinator.cleanup();
            process.exit();
        });
    }

    /**
     * Generate documentation for a folder and its subfolders
     */
    public async generateDocumentation(targetPath: string): Promise<void> {
        console.log(`\nStarting documentation generation for: ${targetPath}`);

        // Validate target path
        if (!fs.existsSync(targetPath)) {
            throw new Error(`Target path does not exist: ${targetPath}`);
        }

        if (!fs.statSync(targetPath).isDirectory()) {
            throw new Error(`Target path is not a directory: ${targetPath}`);
        }

        // Build folder tree
        console.log('Building folder tree...');
        const folderTree = this.folderAnalyzer.buildFolderTree(targetPath);

        if (!folderTree) {
            console.log('Target folder is ignored or empty');
            return;
        }

        // Get folders sorted by depth (deepest first)
        const folders = this.folderAnalyzer.getFoldersByDepth(folderTree);
        console.log(`Found ${folders.length} folders to process`);

        // Process folders bottom-up
        for (const folder of folders) {
            await this.processFolderWithLock(folder);
        }

        // Update root CLAUDE.md if we processed folders
        if (this.processedFolders.length > 0) {
            console.log('\nUpdating root CLAUDE.md...');
            await this.claudeMdWriter.updateRootDocumentation(this.processedFolders);
        }

        console.log(`\nDocumentation generation complete!`);
        console.log(`Processed ${this.processedFolders.length} folders`);
    }

    /**
     * Process a single folder with lock management
     */
    private async processFolderWithLock(folderInfo: FolderInfo): Promise<void> {
        const relativePath = path.relative(this.rootPath, folderInfo.path);

        // Skip if already has CLAUDE.md and skipExisting is true
        if (this.skipExisting && folderInfo.hasClaudeMd) {
            console.log(`Skipping ${relativePath} (already has CLAUDE.md)`);
            return;
        }

        // Check if folder is locked
        if (this.parallelCoordinator.isLocked(folderInfo.path)) {
            const lockInfo = this.parallelCoordinator.getLockInfo(folderInfo.path);
            console.log(`Skipping ${relativePath} (locked by process ${lockInfo?.pid || 'unknown'})`);
            return;
        }

        // Check if parent is locked
        if (this.parallelCoordinator.isParentLocked(folderInfo.path)) {
            console.log(`Skipping ${relativePath} (parent folder is locked)`);
            return;
        }

        // Check if children are being processed
        if (this.parallelCoordinator.hasLockedChildren(folderInfo.path)) {
            console.log(`Skipping ${relativePath} (subfolders are being processed)`);
            return;
        }

        // Try to acquire lock
        console.log(`Processing ${relativePath}...`);
        const lockAcquired = await this.parallelCoordinator.acquireLock(folderInfo.path);

        if (!lockAcquired) {
            console.log(`Failed to acquire lock for ${relativePath} (timeout)`);
            return;
        }

        try {
            // Process the folder
            await this.processFolder(folderInfo);
            this.processedFolders.push(folderInfo.path);
        } catch (error) {
            console.error(`Error processing ${relativePath}: ${error}`);
        } finally {
            // Always release the lock
            this.parallelCoordinator.unlock(folderInfo.path);
        }
    }

    /**
     * Process a single folder
     */
    private async processFolder(folderInfo: FolderInfo): Promise<void> {
        const relativePath = path.relative(this.rootPath, folderInfo.path);

        // Skip if folder has no files and no subfolders
        if (folderInfo.files.length === 0 && folderInfo.subfolders.length === 0) {
            console.log(`  Skipping empty folder: ${relativePath}`);
            return;
        }

        // Analyze folder contents
        console.log(`  Analyzing code in ${relativePath}...`);
        const analysis = await this.folderAnalyzer.analyzeFolder(folderInfo);

        // Collect subfolder summaries
        const subfolderSummaries: SubfolderSummary[] = [];
        for (const subfolder of folderInfo.subfolders) {
            const claudeMd = await this.claudeMdWriter.readClaudeMd(subfolder.path);
            if (claudeMd) {
                subfolderSummaries.push({
                    name: subfolder.name,
                    path: subfolder.path,
                    summary: this.claudeMdWriter.extractSummary(claudeMd)
                });
            }
        }

        // Generate documentation
        console.log(`  Generating CLAUDE.md for ${relativePath}...`);
        const documentation = this.claudeMdWriter.generateDocumentation(
            folderInfo,
            analysis,
            subfolderSummaries
        );

        // Write documentation
        await this.claudeMdWriter.writeDocumentation(folderInfo.path, documentation);
    }

    /**
     * List all existing CLAUDE.md files
     */
    public async listExistingDocumentation(): Promise<string[]> {
        const claudeMdFiles: string[] = [];

        const findClaudeMd = (dir: string) => {
            if (this.ignoreParser.shouldIgnore(dir)) {
                return;
            }

            try {
                const entries = fs.readdirSync(dir, { withFileTypes: true });
                for (const entry of entries) {
                    const fullPath = path.join(dir, entry.name);

                    if (entry.isDirectory()) {
                        findClaudeMd(fullPath);
                    } else if (entry.isFile() && entry.name === 'CLAUDE.md') {
                        claudeMdFiles.push(fullPath);
                    }
                }
            } catch (error) {
                // Skip inaccessible directories
            }
        };

        findClaudeMd(this.rootPath);
        return claudeMdFiles;
    }

    /**
     * Clean up all CLAUDE.md files (for testing)
     */
    public async cleanupDocumentation(): Promise<void> {
        const claudeMdFiles = await this.listExistingDocumentation();

        console.log(`Found ${claudeMdFiles.length} CLAUDE.md files`);

        for (const file of claudeMdFiles) {
            if (file !== path.join(this.rootPath, 'CLAUDE.md')) {
                try {
                    fs.unlinkSync(file);
                    console.log(`Removed: ${path.relative(this.rootPath, file)}`);
                } catch (error) {
                    console.error(`Failed to remove ${file}: ${error}`);
                }
            }
        }
    }
}

// Main entry point when running directly
if (require.main === module) {
    const args = process.argv.slice(2);

    if (args.length === 0) {
        console.log('Usage: ts-node documentation-generator.ts <folder-path> [options]');
        console.log('Options:');
        console.log('  --skip-existing       Skip folders that already have CLAUDE.md');
        console.log('  --cleanup             Remove all CLAUDE.md files (except root)');
        console.log('  --list                List all existing CLAUDE.md files');
        console.log('  --cross-references    Enable cross-reference analysis using ast-grep');
        console.log('');
        console.log('Note: Cross-reference analysis requires ast-grep to be installed.');
        process.exit(1);
    }

    const targetPath = path.resolve(args[0]);
    const skipExisting = args.includes('--skip-existing');
    const cleanup = args.includes('--cleanup');
    const list = args.includes('--list');
    const enableCrossReferences = args.includes('--cross-references');

    // Determine root path (look for .git directory)
    let rootPath = targetPath;
    while (rootPath !== path.dirname(rootPath)) {
        if (fs.existsSync(path.join(rootPath, '.git'))) {
            break;
        }
        rootPath = path.dirname(rootPath);
    }

    const generator = new DocumentationGenerator(rootPath, skipExisting, enableCrossReferences);

    (async () => {
        try {
            await generator.initialize();

            if (cleanup) {
                await generator.cleanupDocumentation();
            } else if (list) {
                const files = await generator.listExistingDocumentation();
                console.log('Existing CLAUDE.md files:');
                files.forEach(f => console.log(`  ${path.relative(rootPath, f)}`));
            } else {
                await generator.generateDocumentation(targetPath);
            }
        } catch (error) {
            console.error(`Error: ${error}`);
            process.exit(1);
        }
    })();
}