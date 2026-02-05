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
const ollama_1 = require("ollama");
const fs = __importStar(require("fs/promises"));
const path = __importStar(require("path"));
const fs_1 = require("fs");
// Configuration
const ollamaModel = 'deepseek-r1:7b'; // DeepSeek R1 model
const ollamaHost = 'http://localhost:11434'; // Ollama host
const maxFileSize = 1000000; // 1MB limit for processing
const maxContentLength = 100000; // Leverage 128k context window
const retryCount = 3; // Number of retries for API failures
const retryDelayBase = 1000; // Base delay for retries (ms)
// Initialize Ollama client
const ollama = new ollama_1.Ollama({ host: ollamaHost });
// Function to move the last file from workfilelist.md to donefilelist.md
async function moveToDone() {
    try {
        const workfileContent = await fs.readFile('workfilelist.md', 'utf-8');
        const lines = workfileContent.trim().split('\n').filter(line => line.trim());
        if (lines.length > 0) {
            const lastFile = lines[lines.length - 1];
            await fs.appendFile('donefilelist.md', lastFile + '\n');
            // Remove the last line from workfilelist.md
            const remainingLines = lines.slice(0, -1);
            await fs.writeFile('workfilelist.md', remainingLines.join('\n') + (remainingLines.length > 0 ? '\n' : ''));
        }
    }
    catch (error) {
        console.error('Error in moveToDone:', error);
        throw error;
    }
}
// Function to sanitize content (replace triple backticks and special characters)
function sanitizeContent(content) {
    let sanitized = content.replace(/```/g, '[CODE_BLOCK]');
    sanitized = sanitized.replace(/[\x00-\x1F\x7F]/g, '');
    sanitized = sanitized.replace(/"/g, '\\"');
    sanitized = sanitized.replace(/\r\n/g, '\n');
    return sanitized;
}
// Function to truncate XML content at tag boundaries
function truncateXmlContent(content, maxLength) {
    if (content.length <= maxLength) {
        return content;
    }
    const truncated = content.substring(0, maxLength);
    const lastTagEnd = truncated.lastIndexOf('>');
    if (lastTagEnd >= 0) {
        return truncated.substring(0, lastTagEnd + 1) + '... [truncated]';
    }
    return truncated + '... [truncated]';
}
// Function to get file language based on extension
function getLanguage(filePath) {
    const extension = path.extname(filePath).toLowerCase();
    switch (extension) {
        case '.cs':
            return 'C#';
        case '.csproj':
            return 'XML';
        case '.js':
            return 'JavaScript';
        case '.ts':
            return 'TypeScript';
        case '.html':
            return 'HTML';
        case '.css':
            return 'CSS';
        case '.json':
            return 'JSON';
        case '.xml':
            return 'XML';
        default:
            return 'Unknown';
    }
}
// Function to call Ollama API for analysis
async function invokeOllamaAnalysis(modifiedContent, unmodifiedContent, status, language, relativePath) {
    console.log(`Calling Ollama API with model: ${ollamaModel} for ${relativePath}`);
    // Sanitize content
    let sanitizedModified = sanitizeContent(modifiedContent);
    let sanitizedUnmodified = sanitizeContent(unmodifiedContent);
    // Truncate content, using XML-aware truncation for .csproj files
    if (language === 'XML') {
        sanitizedModified = truncateXmlContent(sanitizedModified, maxContentLength);
        sanitizedUnmodified = truncateXmlContent(sanitizedUnmodified, maxContentLength);
    }
    else {
        if (sanitizedModified.length > maxContentLength) {
            sanitizedModified = sanitizedModified.substring(0, maxContentLength) + '... [truncated]';
        }
        if (sanitizedUnmodified.length > maxContentLength) {
            sanitizedUnmodified = sanitizedUnmodified.substring(0, maxContentLength) + '... [truncated]';
        }
    }
    // Log file content
    const logEntry = `File: ${relativePath}\nModified Content Length: ${sanitizedModified.length}\nModified Content:\n${sanitizedModified}\nUnmodified Content Length: ${sanitizedUnmodified.length}\nUnmodified Content:\n${sanitizedUnmodified}\n\n`;
    await fs.appendFile('../content.log', logEntry);
    const prompt = status === 'Modified' ? `
You are an expert code analyst. Analyze the provided modified and unmodified file contents and generate a valid JSON response with the following fields:

- **summary**: A concise description of the modified file's content (e.g., key classes, methods, or configurations) without code snippets. Focus on functionality and structure.
- **changes**: A precise summary of differences between the modified and unmodified content (e.g., added/removed lines, new methods, configuration changes).
- **purpose**: The file's role in an ASP.NET Zero solution (e.g., dependency injection, project configuration).

Return only the JSON object, without any thinking process, markdown, or backticks. Ensure the response is valid JSON, escaping special characters like quotes or newlines.

**Modified Content**:
${sanitizedModified}

**Unmodified Content**:
${sanitizedUnmodified}

**Language**: ${language}

Respond in this JSON format:
{
  "summary": "...",
  "changes": "...",
  "purpose": "..."
}
` : `
You are an expert code analyst. Analyze the provided file content and generate a valid JSON response with the following fields:

- **summary**: A concise description of the file's content (e.g., key classes, methods, or configurations) without code snippets. Focus on functionality and structure.
- **changes**: Use "New file".
- **purpose**: The file's role in an ASP.NET Zero solution (e.g., dependency injection, project configuration).

Return only the JSON object, without any thinking process, markdown, or backticks. Ensure the response is valid JSON, escaping special characters like quotes or newlines.

**Content**:
${sanitizedModified}

**Language**: ${language}

Respond in this JSON format:
{
  "summary": "...",
  "changes": "New file",
  "purpose": "..."
}
`;
    // Log request details
    const requestLog = `Request for ${relativePath}: Model=${ollamaModel}, PromptLength=${prompt.length}, Language=${language}\n`;
    await fs.appendFile('../ollama_requests.log', requestLog);
    let attempt = 0;
    while (attempt < retryCount) {
        try {
            const response = await ollama.generate({
                model: ollamaModel,
                prompt: prompt,
                stream: false,
                options: {
                    temperature: 0.1,
                    top_p: 0.9,
                }
            });
            const responseContent = response.response;
            const responseLog = `Attempt ${attempt} for ${relativePath}: Content=${responseContent}\n`;
            await fs.appendFile('../ollama_responses.log', responseLog);
            // Extract JSON from response
            let jsonContent = responseContent;
            // Try to extract JSON if wrapped in markdown
            const jsonRegex = /```json\s*([\s\S]*?)\s*```/;
            const match = responseContent.match(jsonRegex);
            if (match) {
                jsonContent = match[1];
            }
            // Try to parse the JSON
            const analysis = JSON.parse(jsonContent);
            if (!analysis.summary || !analysis.changes || !analysis.purpose) {
                throw new Error(`Incomplete analysis response: ${jsonContent}`);
            }
            return analysis;
        }
        catch (error) {
            attempt++;
            const errorMessage = error instanceof Error ? error.message : String(error);
            const errorLog = `Attempt ${attempt} error for ${relativePath}: ${errorMessage}\n`;
            await fs.appendFile('../ollama_responses.log', errorLog);
            console.log(`Attempt ${attempt} failed for ${relativePath}: ${errorMessage}`);
            if (attempt === retryCount) {
                console.error(`Ollama analysis failed for ${relativePath} after ${retryCount} attempts: ${errorMessage}`);
                const skipLog = `${relativePath}: Analysis failed after ${retryCount} attempts - ${errorMessage}\n`;
                await fs.appendFile('../skipped_files.log', skipLog);
                return null;
            }
            const delay = retryDelayBase * Math.pow(2, attempt - 1);
            await new Promise(resolve => setTimeout(resolve, delay));
        }
    }
    return null;
}
// Function to check if files are identical
async function areFilesIdentical(path1, path2) {
    try {
        const content1 = await fs.readFile(path1, 'utf-8');
        const content2 = await fs.readFile(path2, 'utf-8');
        return content1 === content2;
    }
    catch {
        return false;
    }
}
// Main processing function
async function processFiles() {
    while (true) {
        // Ensure filelist.md exists
        if (!(0, fs_1.existsSync)('filelist.md')) {
            console.error('filelist.md not found!');
            process.exit(1);
        }
        // Get the first line from filelist.md
        let filelistContent;
        try {
            filelistContent = await fs.readFile('filelist.md', 'utf-8');
        }
        catch (error) {
            console.error('Error reading filelist.md:', error);
            process.exit(1);
        }
        const lines = filelistContent.trim().split('\n').filter(line => line.trim());
        // If filelist.md is empty, exit
        if (lines.length === 0) {
            console.log('Finished processing all files.');
            process.exit(0);
        }
        const firstLine = lines[0];
        // Append the first line to workfilelist.md
        await fs.appendFile('workfilelist.md', firstLine + '\n');
        // Remove the first line from filelist.md
        const remainingLines = lines.slice(1);
        await fs.writeFile('filelist.md', remainingLines.join('\n') + (remainingLines.length > 0 ? '\n' : ''));
        // Get the last line from workfilelist.md (the file to process)
        const workfileContent = await fs.readFile('workfilelist.md', 'utf-8');
        const workLines = workfileContent.trim().split('\n').filter(line => line.trim());
        const relativePath = workLines[workLines.length - 1];
        console.log(`Processing file: ${relativePath}`);
        // Validate the file
        const modifiedPath = path.join('../src', relativePath);
        const unmodifiedPath = path.join('../aspnetzeromvc11.4/inzibackend/src', relativePath);
        const modifiedExists = (0, fs_1.existsSync)(modifiedPath);
        const unmodifiedExists = (0, fs_1.existsSync)(unmodifiedPath);
        if (!modifiedExists) {
            console.log(`File ${relativePath} not found in ./src, skipping.`);
            await fs.appendFile('../skipped_files.log', `${relativePath}: Missing in src\n`);
            await moveToDone();
            continue;
        }
        // Check file size
        const stats = await fs.stat(modifiedPath);
        if (stats.size > maxFileSize) {
            console.log(`File ${relativePath} is too large (${stats.size} bytes), skipping.`);
            await fs.appendFile('../skipped_files.log', `${relativePath}: Too large (${stats.size} bytes)\n`);
            await moveToDone();
            continue;
        }
        // Compare files
        let status = '';
        const modifiedContent = await fs.readFile(modifiedPath, 'utf-8');
        let unmodifiedContent = '';
        if (modifiedExists && !unmodifiedExists) {
            status = 'New';
        }
        else if (modifiedExists && unmodifiedExists) {
            const identical = await areFilesIdentical(modifiedPath, unmodifiedPath);
            if (identical) {
                console.log(`File ${relativePath} is identical, skipping analysis.`);
                await moveToDone();
                continue;
            }
            else {
                status = 'Modified';
                unmodifiedContent = await fs.readFile(unmodifiedPath, 'utf-8');
            }
        }
        // Prepare metadata
        const filename = path.basename(relativePath);
        const language = getLanguage(relativePath);
        // Construct output path with src subfolder
        const outputDir = path.join('../migration-plans', 'src', path.dirname(relativePath));
        const saveTo = path.join(outputDir, `${filename}.md`);
        // Create output directory if it doesn't exist
        await fs.mkdir(outputDir, { recursive: true });
        // Generate analysis with Ollama
        const analysis = await invokeOllamaAnalysis(modifiedContent, unmodifiedContent, status, language, relativePath);
        if (!analysis) {
            console.log(`Analysis failed for ${relativePath}, leaving in workfilelist.md.`);
            continue;
        }
        // Generate Markdown template
        const template = `# ${status}
## Filename
${filename}
## Relative Path
${relativePath}
## Language
${language}
## Summary
${analysis.summary}
## Changes
${analysis.changes}
## Purpose
${analysis.purpose}
`;
        // Save the analysis file
        try {
            await fs.writeFile(saveTo, template);
            console.log(`Saved analysis to ${saveTo}`);
            await moveToDone();
        }
        catch (error) {
            console.error(`Failed to save analysis for ${relativePath}:`, error);
            await fs.appendFile('../skipped_files.log', `${relativePath}: Save failed - ${error}\n`);
            continue;
        }
    }
}
// Main execution
async function main() {
    try {
        console.log('Starting file comparison with Ollama...');
        console.log(`Using model: ${ollamaModel}`);
        console.log(`Max file size: ${maxFileSize} bytes`);
        console.log(`Max content length: ${maxContentLength} characters`);
        await processFiles();
    }
    catch (error) {
        console.error('Fatal error:', error);
        process.exit(1);
    }
}
// Run the script
if (require.main === module) {
    main();
}
//# sourceMappingURL=docproj.js.map