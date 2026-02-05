import * as fs from 'fs';
import * as path from 'path';

export class ParallelCoordinator {
    private lockDir: string;
    private lockExtension = '.claude-doc.lock';
    private maxWaitTime = 30000; // 30 seconds max wait
    private checkInterval = 500; // Check every 500ms

    constructor(rootPath: string) {
        this.lockDir = path.join(rootPath, '.claude', 'locks');
        this.ensureLockDirectory();
    }

    /**
     * Ensure lock directory exists
     */
    private ensureLockDirectory(): void {
        if (!fs.existsSync(this.lockDir)) {
            fs.mkdirSync(this.lockDir, { recursive: true });
        }
    }

    /**
     * Get lock file path for a folder
     */
    private getLockPath(folderPath: string): string {
        // Create a safe filename from the folder path
        const safeName = folderPath
            .replace(/[^a-zA-Z0-9]/g, '_')
            .replace(/_+/g, '_')
            .toLowerCase();
        return path.join(this.lockDir, safeName + this.lockExtension);
    }

    /**
     * Check if a folder is locked
     */
    public isLocked(folderPath: string): boolean {
        const lockPath = this.getLockPath(folderPath);
        if (!fs.existsSync(lockPath)) {
            return false;
        }

        // Check if lock is stale (older than 5 minutes)
        try {
            const stats = fs.statSync(lockPath);
            const age = Date.now() - stats.mtimeMs;
            if (age > 300000) { // 5 minutes
                // Stale lock, remove it
                this.unlock(folderPath);
                return false;
            }
            return true;
        } catch (error) {
            return false;
        }
    }

    /**
     * Acquire a lock for a folder
     */
    public async acquireLock(folderPath: string): Promise<boolean> {
        const lockPath = this.getLockPath(folderPath);
        const startTime = Date.now();

        while (Date.now() - startTime < this.maxWaitTime) {
            if (!this.isLocked(folderPath)) {
                try {
                    // Try to create lock file atomically
                    const lockData = {
                        pid: process.pid,
                        timestamp: new Date().toISOString(),
                        folder: folderPath
                    };
                    fs.writeFileSync(lockPath, JSON.stringify(lockData, null, 2), {
                        flag: 'wx' // Exclusive write - fails if file exists
                    });
                    return true;
                } catch (error: any) {
                    if (error.code === 'EEXIST') {
                        // Lock file was created by another process
                        await this.sleep(this.checkInterval);
                        continue;
                    }
                    throw error;
                }
            }

            // Wait before trying again
            await this.sleep(this.checkInterval);
        }

        return false; // Timeout - couldn't acquire lock
    }

    /**
     * Release a lock for a folder
     */
    public unlock(folderPath: string): void {
        const lockPath = this.getLockPath(folderPath);
        try {
            if (fs.existsSync(lockPath)) {
                fs.unlinkSync(lockPath);
            }
        } catch (error) {
            console.warn(`Warning: Could not remove lock file: ${error}`);
        }
    }

    /**
     * Check if any parent folders are locked
     */
    public isParentLocked(folderPath: string): boolean {
        let currentPath = path.dirname(folderPath);
        const rootParent = path.parse(folderPath).root;

        while (currentPath && currentPath !== rootParent && currentPath !== '.') {
            if (this.isLocked(currentPath)) {
                return true;
            }
            const parent = path.dirname(currentPath);
            if (parent === currentPath) break; // Reached root
            currentPath = parent;
        }

        return false;
    }

    /**
     * Check if any child folders are locked
     */
    public hasLockedChildren(folderPath: string): boolean {
        try {
            const locks = fs.readdirSync(this.lockDir);
            const folderPathNormalized = folderPath.replace(/[^a-zA-Z0-9]/g, '_').toLowerCase();

            for (const lockFile of locks) {
                if (lockFile.endsWith(this.lockExtension)) {
                    const lockPath = path.join(this.lockDir, lockFile);
                    try {
                        const lockData = JSON.parse(fs.readFileSync(lockPath, 'utf-8'));
                        if (lockData.folder && lockData.folder.startsWith(folderPath)) {
                            return true;
                        }
                    } catch (error) {
                        // Invalid lock file, skip
                    }
                }
            }
        } catch (error) {
            console.warn(`Warning: Could not check for locked children: ${error}`);
        }

        return false;
    }

    /**
     * Clean up all stale locks
     */
    public cleanupStaleLocks(): void {
        try {
            if (!fs.existsSync(this.lockDir)) {
                return;
            }

            const locks = fs.readdirSync(this.lockDir);
            for (const lockFile of locks) {
                if (lockFile.endsWith(this.lockExtension)) {
                    const lockPath = path.join(this.lockDir, lockFile);
                    try {
                        const stats = fs.statSync(lockPath);
                        const age = Date.now() - stats.mtimeMs;
                        if (age > 300000) { // 5 minutes
                            fs.unlinkSync(lockPath);
                            console.log(`Cleaned up stale lock: ${lockFile}`);
                        }
                    } catch (error) {
                        // Error checking/removing lock, skip
                    }
                }
            }
        } catch (error) {
            console.warn(`Warning: Could not cleanup stale locks: ${error}`);
        }
    }

    /**
     * Get information about a lock
     */
    public getLockInfo(folderPath: string): any | null {
        const lockPath = this.getLockPath(folderPath);
        if (!fs.existsSync(lockPath)) {
            return null;
        }

        try {
            return JSON.parse(fs.readFileSync(lockPath, 'utf-8'));
        } catch (error) {
            return null;
        }
    }

    /**
     * List all active locks
     */
    public listActiveLocks(): string[] {
        const activeLocks: string[] = [];

        try {
            if (!fs.existsSync(this.lockDir)) {
                return activeLocks;
            }

            const locks = fs.readdirSync(this.lockDir);
            for (const lockFile of locks) {
                if (lockFile.endsWith(this.lockExtension)) {
                    const lockPath = path.join(this.lockDir, lockFile);
                    try {
                        const lockData = JSON.parse(fs.readFileSync(lockPath, 'utf-8'));
                        if (lockData.folder) {
                            activeLocks.push(lockData.folder);
                        }
                    } catch (error) {
                        // Invalid lock file, skip
                    }
                }
            }
        } catch (error) {
            console.warn(`Warning: Could not list active locks: ${error}`);
        }

        return activeLocks;
    }

    /**
     * Sleep helper
     */
    private sleep(ms: number): Promise<void> {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

    /**
     * Clean up on exit
     */
    public cleanup(): void {
        // Clean up any locks created by this process
        try {
            if (!fs.existsSync(this.lockDir)) {
                return;
            }

            const locks = fs.readdirSync(this.lockDir);
            const currentPid = process.pid;

            for (const lockFile of locks) {
                if (lockFile.endsWith(this.lockExtension)) {
                    const lockPath = path.join(this.lockDir, lockFile);
                    try {
                        const lockData = JSON.parse(fs.readFileSync(lockPath, 'utf-8'));
                        if (lockData.pid === currentPid) {
                            fs.unlinkSync(lockPath);
                        }
                    } catch (error) {
                        // Error reading/removing lock, skip
                    }
                }
            }
        } catch (error) {
            console.warn(`Warning: Could not cleanup locks on exit: ${error}`);
        }
    }
}