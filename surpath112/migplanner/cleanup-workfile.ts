#!/usr/bin/env tsx

import * as fs from 'fs/promises';
import { existsSync } from 'fs';

async function cleanupWorkfile(): Promise<void> {
  try {
    // Read workfilelist.md
    if (!existsSync('workfilelist.md')) {
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
    } catch (error) {
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

  } catch (error) {
    console.error('Error during cleanup:', error);
    process.exit(1);
  }
}

// Run the cleanup
if (require.main === module) {
  cleanupWorkfile();
} 