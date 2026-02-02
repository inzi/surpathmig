#!/usr/bin/env tsx

import { Ollama } from 'ollama';
import Anthropic from '@anthropic-ai/sdk';
import OpenAI from 'openai';
import * as fs from 'fs/promises';
import * as path from 'path';
import { existsSync } from 'fs';
import dotenv from 'dotenv';

// Load environment variables from .env file
dotenv.config();

// Configuration
// Set modelProvider to "ollama", "anthropic", or "openai" to choose your AI provider
const modelProvider: "ollama" | "anthropic" | "openai" = "anthropic";

// Model configurations from environment variables with fallbacks
const ollamaModel = process.env.OLLAMA_MODEL || 'devstral:24b';
const ollamaHost = process.env.OLLAMA_BASE_URL || 'http://localhost:11434';
const anthropicModel = process.env.ANTHROPIC_MODEL || 'claude-3-5-sonnet-20241022';
const openaiModel = process.env.OPENAI_MODEL || 'gpt-4o';

// Note: Set environment variables in .env file or system environment:
// ANTHROPIC_API_KEY, OPENAI_API_KEY, ANTHROPIC_MODEL, OPENAI_MODEL, OLLAMA_MODEL, OLLAMA_BASE_URL
const maxFileSize = 1000000; // 1MB limit for processing
const maxContentLength = 100000; // Leverage 128k context window
const retryCount = 5; // Number of retries for API failures
const retryDelayBase = 1000; // Base delay for retries (ms)

// Initialize clients
const ollama = new Ollama({ host: ollamaHost });
const anthropic = new Anthropic({
  apiKey: process.env.ANTHROPIC_API_KEY,
});
const openai = new OpenAI({
  apiKey: process.env.OPENAI_API_KEY,
});

interface AnalysisResult {
  summary: string;
  changes: string;
  purpose: string;
}

// Function to move the last file from workfilelist.md to donefilelist.md
async function moveToDone(): Promise<void> {
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
  } catch (error) {
    console.error('Error in moveToDone:', error);
    throw error;
  }
}



// Function to sanitize content (replace triple backticks and special characters)
function sanitizeContent(content: string): string {
  let sanitized = content.replace(/```/g, '[CODE_BLOCK]');
  sanitized = sanitized.replace(/[\x00-\x1F\x7F]/g, '');
  sanitized = sanitized.replace(/"/g, '\\"');
  sanitized = sanitized.replace(/\r\n/g, '\n');
  return sanitized;
}

// Function to truncate XML content at tag boundaries
function truncateXmlContent(content: string, maxLength: number): string {
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
function getLanguage(filePath: string): string {
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
async function invokeOllamaAnalysis(
  modifiedContent: string,
  unmodifiedContent: string,
  status: string,
  language: string,
  relativePath: string
): Promise<AnalysisResult | null> {
  console.log(`Calling Ollama API with model: ${ollamaModel} for ${relativePath}`);

  // Sanitize content
  let sanitizedModified = sanitizeContent(modifiedContent);
  let sanitizedUnmodified = sanitizeContent(unmodifiedContent);

  // Truncate content, using XML-aware truncation for .csproj files
  if (language === 'XML') {
    sanitizedModified = truncateXmlContent(sanitizedModified, maxContentLength);
    sanitizedUnmodified = truncateXmlContent(sanitizedUnmodified, maxContentLength);
  } else {
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

[[[[RETRY_MSG]]]]

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

[[[[RETRY_MSG]]]]

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
  let retry_msg = ''; // Will be populated with specific error instructions on retry attempts
  while (attempt < retryCount) {
    try {
      let this_prompt = prompt;
      // Replace [[[[RETRY_MSG]]]] with retry_msg (empty string on first attempt, specific instructions on retries)
      this_prompt = this_prompt.replace('[[[[RETRY_MSG]]]]', retry_msg);

      const response = await ollama.generate({
        model: ollamaModel,
        prompt: this_prompt,
        stream: false,
        options: {
          temperature: 0.1,
          top_p: 0.9,
        }
      });

      const responseContent = response.response;
      const responseLog = `Attempt ${attempt + 1} for ${relativePath}: Content=${responseContent}\n`;
      await fs.appendFile('../ollama_responses.log', responseLog);

      // Extract JSON from response
      let jsonContent = responseContent.trim();
      
      // Try to extract JSON if wrapped in markdown
      const jsonRegex = /```json\s*([\s\S]*?)\s*```/;
      const match = responseContent.match(jsonRegex);
      if (match) {
        jsonContent = match[1].trim();
      }

      // Additional cleanup - remove any leading/trailing text that might not be JSON
      const jsonStartIndex = jsonContent.indexOf('{');
      const jsonEndIndex = jsonContent.lastIndexOf('}');
      if (jsonStartIndex !== -1 && jsonEndIndex !== -1 && jsonEndIndex > jsonStartIndex) {
        jsonContent = jsonContent.substring(jsonStartIndex, jsonEndIndex + 1);
      }

      // Try to parse the JSON
      let analysis: AnalysisResult;
      try {
        analysis = JSON.parse(jsonContent) as AnalysisResult;
             } catch (jsonError) {
         const truncatedResponse = jsonContent.length > 200 ? jsonContent.substring(0, 200) + '...' : jsonContent;
         retry_msg = `IMPORTANT! - Your previous response was not valid JSON. Please respond with ONLY a valid JSON object in the exact format requested. Do not include any markdown, backticks, explanatory text, or multiple JSON objects. Previous invalid response: "${truncatedResponse}"`;
         throw new Error(`Invalid JSON response: ${jsonError instanceof Error ? jsonError.message : String(jsonError)}`);
       }
      
      // Validate required fields
      if (!analysis.summary || !analysis.changes || !analysis.purpose) {
        retry_msg = `IMPORTANT! - Your previous response was missing required fields. All three fields (summary, changes, purpose) must be present and non-empty. Missing: ${!analysis.summary ? 'summary ' : ''}${!analysis.changes ? 'changes ' : ''}${!analysis.purpose ? 'purpose' : ''}. Please provide a complete JSON response.`;
        throw new Error(`Incomplete analysis response - missing fields: ${JSON.stringify(analysis)}`);
      }

      // Validate field types and content
      if (typeof analysis.summary !== 'string' || typeof analysis.changes !== 'string' || typeof analysis.purpose !== 'string') {
        retry_msg = `IMPORTANT! - All fields must be strings. Please ensure summary, changes, and purpose are all string values, not other data types.`;
        throw new Error(`Invalid field types in analysis response`);
      }

      // Check for empty strings
      if (analysis.summary.trim() === '' || analysis.changes.trim() === '' || analysis.purpose.trim() === '') {
        retry_msg = `IMPORTANT! - All fields must contain meaningful content, not empty strings. Please provide descriptive text for summary, changes, and purpose fields.`;
        throw new Error(`Empty field values in analysis response`);
      }
      
      return analysis;
    } catch (error) {
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

// Function to call Anthropic API for analysis
async function invokeClaudeAnalysis(
  modifiedContent: string,
  unmodifiedContent: string,
  status: string,
  language: string,
  relativePath: string
): Promise<AnalysisResult | null> {
  console.log(`Calling Anthropic API with model: ${anthropicModel} for ${relativePath}`);

  // Sanitize content
  let sanitizedModified = sanitizeContent(modifiedContent);
  let sanitizedUnmodified = sanitizeContent(unmodifiedContent);

  // Truncate content, using XML-aware truncation for .csproj files
  if (language === 'XML') {
    sanitizedModified = truncateXmlContent(sanitizedModified, maxContentLength);
    sanitizedUnmodified = truncateXmlContent(sanitizedUnmodified, maxContentLength);
  } else {
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
  const requestLog = `Request for ${relativePath}: Model=${anthropicModel}, PromptLength=${prompt.length}, Language=${language}\n`;
  await fs.appendFile('../anthropic_requests.log', requestLog);

  let attempt = 0;
  while (attempt < retryCount) {
    try {
      const response = await anthropic.messages.create({
        model: anthropicModel,
        max_tokens: 4000,
        temperature: 0.1,
        messages: [
          {
            role: 'user',
            content: prompt
          }
        ]
      });

      const responseContent = response.content[0].type === 'text' ? response.content[0].text : '';
      const responseLog = `Attempt ${attempt + 1} for ${relativePath}: Content=${responseContent}\n`;
      await fs.appendFile('../anthropic_responses.log', responseLog);

      // Extract JSON from response
      let jsonContent = responseContent.trim();
      
      // Try to extract JSON if wrapped in markdown
      const jsonRegex = /```json\s*([\s\S]*?)\s*```/;
      const match = responseContent.match(jsonRegex);
      if (match) {
        jsonContent = match[1].trim();
      }

      // Additional cleanup - remove any leading/trailing text that might not be JSON
      const jsonStartIndex = jsonContent.indexOf('{');
      const jsonEndIndex = jsonContent.lastIndexOf('}');
      if (jsonStartIndex !== -1 && jsonEndIndex !== -1 && jsonEndIndex > jsonStartIndex) {
        jsonContent = jsonContent.substring(jsonStartIndex, jsonEndIndex + 1);
      }

      // Try to parse the JSON
      let analysis: AnalysisResult;
      try {
        analysis = JSON.parse(jsonContent) as AnalysisResult;
      } catch (jsonError) {
        throw new Error(`Invalid JSON response: ${jsonError instanceof Error ? jsonError.message : String(jsonError)}`);
      }
      
      // Validate required fields
      if (!analysis.summary || !analysis.changes || !analysis.purpose) {
        throw new Error(`Incomplete analysis response - missing fields: ${JSON.stringify(analysis)}`);
      }

      // Validate field types and content
      if (typeof analysis.summary !== 'string' || typeof analysis.changes !== 'string' || typeof analysis.purpose !== 'string') {
        throw new Error(`Invalid field types in analysis response`);
      }

      // Check for empty strings
      if (analysis.summary.trim() === '' || analysis.changes.trim() === '' || analysis.purpose.trim() === '') {
        throw new Error(`Empty field values in analysis response`);
      }
      
      return analysis;
    } catch (error) {
      attempt++;
      const errorMessage = error instanceof Error ? error.message : String(error);
      const errorLog = `Attempt ${attempt} error for ${relativePath}: ${errorMessage}\n`;
      await fs.appendFile('../anthropic_responses.log', errorLog);
      
      console.log(`Attempt ${attempt} failed for ${relativePath}: ${errorMessage}`);
      
      if (attempt === retryCount) {
        console.error(`Anthropic analysis failed for ${relativePath} after ${retryCount} attempts: ${errorMessage}`);
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

// Function to call OpenAI API for analysis
async function invokeOpenAIAnalysis(
  modifiedContent: string,
  unmodifiedContent: string,
  status: string,
  language: string,
  relativePath: string
): Promise<AnalysisResult | null> {
  console.log(`Calling OpenAI API with model: ${openaiModel} for ${relativePath}`);

  // Sanitize content
  let sanitizedModified = sanitizeContent(modifiedContent);
  let sanitizedUnmodified = sanitizeContent(unmodifiedContent);

  // Truncate content, using XML-aware truncation for .csproj files
  if (language === 'XML') {
    sanitizedModified = truncateXmlContent(sanitizedModified, maxContentLength);
    sanitizedUnmodified = truncateXmlContent(sanitizedUnmodified, maxContentLength);
  } else {
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
  const requestLog = `Request for ${relativePath}: Model=${openaiModel}, PromptLength=${prompt.length}, Language=${language}\n`;
  await fs.appendFile('../openai_requests.log', requestLog);

  let attempt = 0;
  while (attempt < retryCount) {
    try {
      const response = await openai.chat.completions.create({
        model: openaiModel,
        messages: [
          {
            role: 'user',
            content: prompt
          }
        ],
        max_tokens: 4000,
        temperature: 0.1,
        top_p: 0.9,
      });

      const responseContent = response.choices[0]?.message?.content || '';
      const responseLog = `Attempt ${attempt + 1} for ${relativePath}: Content=${responseContent}\n`;
      await fs.appendFile('../openai_responses.log', responseLog);

      // Extract JSON from response
      let jsonContent = responseContent.trim();
      
      // Try to extract JSON if wrapped in markdown
      const jsonRegex = /```json\s*([\s\S]*?)\s*```/;
      const match = responseContent.match(jsonRegex);
      if (match) {
        jsonContent = match[1].trim();
      }

      // Additional cleanup - remove any leading/trailing text that might not be JSON
      const jsonStartIndex = jsonContent.indexOf('{');
      const jsonEndIndex = jsonContent.lastIndexOf('}');
      if (jsonStartIndex !== -1 && jsonEndIndex !== -1 && jsonEndIndex > jsonStartIndex) {
        jsonContent = jsonContent.substring(jsonStartIndex, jsonEndIndex + 1);
      }

      // Try to parse the JSON
      let analysis: AnalysisResult;
      try {
        analysis = JSON.parse(jsonContent) as AnalysisResult;
      } catch (jsonError) {
        throw new Error(`Invalid JSON response: ${jsonError instanceof Error ? jsonError.message : String(jsonError)}`);
      }
      
      // Validate required fields
      if (!analysis.summary || !analysis.changes || !analysis.purpose) {
        throw new Error(`Incomplete analysis response - missing fields: ${JSON.stringify(analysis)}`);
      }

      // Validate field types and content
      if (typeof analysis.summary !== 'string' || typeof analysis.changes !== 'string' || typeof analysis.purpose !== 'string') {
        throw new Error(`Invalid field types in analysis response`);
      }

      // Check for empty strings
      if (analysis.summary.trim() === '' || analysis.changes.trim() === '' || analysis.purpose.trim() === '') {
        throw new Error(`Empty field values in analysis response`);
      }
      
      return analysis;
    } catch (error) {
      attempt++;
      const errorMessage = error instanceof Error ? error.message : String(error);
      const errorLog = `Attempt ${attempt} error for ${relativePath}: ${errorMessage}\n`;
      await fs.appendFile('../openai_responses.log', errorLog);
      
      console.log(`Attempt ${attempt} failed for ${relativePath}: ${errorMessage}`);
      
      if (attempt === retryCount) {
        console.error(`OpenAI analysis failed for ${relativePath} after ${retryCount} attempts: ${errorMessage}`);
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

// Unified function to perform AI analysis based on the selected provider
async function performAIAnalysis(
  modifiedContent: string,
  unmodifiedContent: string,
  status: string,
  language: string,
  relativePath: string
): Promise<AnalysisResult | null> {
  switch (modelProvider) {
    case 'ollama':
      return await invokeOllamaAnalysis(modifiedContent, unmodifiedContent, status, language, relativePath);
    case 'anthropic':
      return await invokeClaudeAnalysis(modifiedContent, unmodifiedContent, status, language, relativePath);
    case 'openai':
      return await invokeOpenAIAnalysis(modifiedContent, unmodifiedContent, status, language, relativePath);
    default:
      throw new Error(`Unsupported model provider: ${modelProvider}`);
  }
}

// Function to check if files are identical
async function areFilesIdentical(path1: string, path2: string): Promise<boolean> {
  try {
    const content1 = await fs.readFile(path1, 'utf-8');
    const content2 = await fs.readFile(path2, 'utf-8');
    return content1 === content2;
  } catch {
    return false;
  }
}

// Main processing function
async function processFiles(): Promise<void> {
  while (true) {
    // Ensure filelist.md exists
    if (!existsSync('filelist.md')) {
      console.error('filelist.md not found!');
      process.exit(1);
    }

    // Get the first line from filelist.md
    let filelistContent: string;
    try {
      filelistContent = await fs.readFile('filelist.md', 'utf-8');
    } catch (error) {
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

    const modifiedExists = existsSync(modifiedPath);
    const unmodifiedExists = existsSync(unmodifiedPath);

    if (!modifiedExists) {
      console.log(`File ${relativePath} not found in ./src, skipping.`);
      await fs.appendFile('../skipped_files.log', `${relativePath}: Missing in src\n`);
      await moveToDone();
      continue;
    }

    // Check if the path is a directory instead of a file
    try {
      const stats = await fs.stat(modifiedPath);
      if (stats.isDirectory()) {
        console.log(`Path ${relativePath} is a directory, skipping.`);
        await fs.appendFile('../skipped_files.log', `${relativePath}: Is a directory\n`);
        await moveToDone();
        continue;
      }

      // Check file size
      if (stats.size > maxFileSize) {
        console.log(`File ${relativePath} is too large (${stats.size} bytes), skipping.`);
        await fs.appendFile('../skipped_files.log', `${relativePath}: Too large (${stats.size} bytes)\n`);
        await moveToDone();
        continue;
      }
    } catch (error) {
      console.log(`Error checking file stats for ${relativePath}: ${error}, skipping.`);
      await fs.appendFile('../skipped_files.log', `${relativePath}: Stat error - ${error}\n`);
      await moveToDone();
      continue;
    }

    // Compare files
    let status = '';
    let modifiedContent = '';
    let unmodifiedContent = '';

    try {
      modifiedContent = await fs.readFile(modifiedPath, 'utf-8');
    } catch (error) {
      console.log(`Error reading modified file ${relativePath}: ${error}, skipping.`);
      await fs.appendFile('../skipped_files.log', `${relativePath}: Read error - ${error}\n`);
      await moveToDone();
      continue;
    }

    if (modifiedExists && !unmodifiedExists) {
      status = 'New';
    } else if (modifiedExists && unmodifiedExists) {
      // Check if unmodified path is also a directory
      try {
        const unmodifiedStats = await fs.stat(unmodifiedPath);
        if (unmodifiedStats.isDirectory()) {
          console.log(`Unmodified path ${relativePath} is a directory, treating as new file.`);
          status = 'New';
        } else {
          const identical = await areFilesIdentical(modifiedPath, unmodifiedPath);
          if (identical) {
            console.log(`File ${relativePath} is identical, skipping analysis.`);
            await moveToDone();
            continue;
          } else {
            status = 'Modified';
            try {
              unmodifiedContent = await fs.readFile(unmodifiedPath, 'utf-8');
            } catch (error) {
              console.log(`Error reading unmodified file ${relativePath}: ${error}, treating as new file.`);
              status = 'New';
            }
          }
        }
      } catch (error) {
        // If we can't stat the unmodified file, treat as new
        status = 'New';
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

    // Generate analysis with selected AI provider
    const analysis = await performAIAnalysis(modifiedContent, unmodifiedContent, status, language, relativePath);

    if (!analysis) {
      console.log(`Analysis failed for ${relativePath}, leaving in workfilelist.md for manual reprocessing.`);
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
    } catch (error) {
      console.error(`Failed to save analysis for ${relativePath}:`, error);
      await fs.appendFile('../skipped_files.log', `${relativePath}: Save failed - ${error}\n`);
      console.log(`File save failed for ${relativePath}, leaving in workfilelist.md for manual reprocessing.`);
      continue;
    }
  }
}

// Main execution
async function main(): Promise<void> {
  try {
    console.log(`Starting file comparison with ${modelProvider}...`);
    switch (modelProvider) {
      case 'ollama':
        console.log(`Using Ollama model: ${ollamaModel}`);
        break;
      case 'anthropic':
        console.log(`Using Anthropic model: ${anthropicModel}`);
        break;
      case 'openai':
        console.log(`Using OpenAI model: ${openaiModel}`);
        break;
    }
    console.log(`Max file size: ${maxFileSize} bytes`);
    console.log(`Max content length: ${maxContentLength} characters`);
    
    await processFiles();
  } catch (error) {
    console.error('Fatal error:', error);
    process.exit(1);
  }
}

// Run the script
if (require.main === module) {
  main();
} 