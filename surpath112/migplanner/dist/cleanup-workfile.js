#!/usr/bin/env tsx
"use strict";
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    var desc = Object.getOwnPropertyDescriptor(m, k);
    if (!desc || ("get" in desc ? !m.__esModule : desc.writable || desc.configurable)) {
      desc = { enumerable: true, get: function() { return m[k]; } };
    }
    Object.defineProperty(o, k2, desc);
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || (function () {
    var ownKeys = function(o) {
        ownKeys = Object.getOwnPropertyNames || function (o) {
            var ar = [];
            for (var k in o) if (Object.prototype.hasOwnProperty.call(o, k)) ar[ar.length] = k;
            return ar;
        };
        return ownKeys(o);
    };
    return function (mod) {
        if (mod && mod.__esModule) return mod;
        var result = {};
        if (mod != null) for (var k = ownKeys(mod), i = 0; i < k.length; i++) if (k[i] !== "default") __createBinding(result, mod, k[i]);
        __setModuleDefault(result, mod);
        return result;
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
const fs = __importStar(require("fs/promises"));
const fs_1 = require("fs");
async function cleanupWorkfile() {
    try {
        // Read workfilelist.md
        if (!(0, fs_1.existsSync)('workfilelist.md')) {
            console.log('workfilelist.md not found, nothing to cleanup');
            return;
        }
        const workfileContent = await fs.readFile('workfilelist.md', 'utf-8');
        const workLines = workfileContent.trim().split('\n').filter(line => line.trim());
        if (workLines.length === 0) {
            console.log('workfilelist.md is empty, nothing to cleanup');
            return;
        }
        // Read current filelist.md
        let filelistContent = '';
        try {
            filelistContent = await fs.readFile('filelist.md', 'utf-8');
        }
        catch (error) {
            console.log('filelist.md not found, will create it');
            filelistContent = '';
        }
        const filelistLines = filelistContent.trim().split('\n').filter(line => line.trim());
        // Combine all files and remove duplicates
        const allFiles = [...workLines, ...filelistLines];
        const uniqueFiles = [...new Set(allFiles)];
        console.log(`Found ${workLines.length} files in workfilelist.md`);
        console.log(`Found ${filelistLines.length} files in filelist.md`);
        console.log(`Total unique files: ${uniqueFiles.length}`);
        // Write back to filelist.md
        await fs.writeFile('filelist.md', uniqueFiles.join('\n') + '\n');
        // Clear workfilelist.md
        await fs.writeFile('workfilelist.md', '');
        console.log('Cleanup completed successfully');
        console.log(`Moved ${workLines.length} files from workfilelist.md back to filelist.md`);
        console.log(`Removed ${allFiles.length - uniqueFiles.length} duplicates`);
    }
    catch (error) {
        console.error('Error during cleanup:', error);
        process.exit(1);
    }
}
// Run the cleanup
if (require.main === module) {
    cleanupWorkfile();
}
//# sourceMappingURL=cleanup-workfile.js.map