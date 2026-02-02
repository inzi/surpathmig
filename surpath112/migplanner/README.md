# SurPath Migration Planner

A TypeScript file comparison tool that uses multiple AI providers (Ollama, Anthropic, OpenAI) to analyze code changes for migration planning in ASP.NET Zero projects.

## Features

- **Multi-Provider AI Support**: Choose between Ollama, Anthropic Claude, or OpenAI GPT models
- **Intelligent File Analysis**: Analyzes modified, new, and unchanged files
- **ASP.NET Zero Focused**: Specialized prompts for ASP.NET Zero solution analysis
- **Batch Processing**: Processes files from a queue with retry logic
- **Comprehensive Logging**: Separate logs for requests, responses, and errors

## Setup

### 1. Install Dependencies

```bash
npm install
```

### 2. Configure AI Provider

Edit `compare-files.ts` and set the `modelProvider` variable:

```typescript
// Set to "ollama", "anthropic", or "openai"
const modelProvider: "ollama" | "anthropic" | "openai" = "ollama";
```

Models are now configured via environment variables (see step 3 below).

### 3. Set Environment Variables

You can set API keys either via environment variables or using a `.env` file.

#### Option A: Using .env file (Recommended)
```bash
# Copy the example file and edit it
cp .env.example .env
# Then edit .env with your actual API keys
```

#### Option B: Using environment variables

##### For Anthropic (Claude)
```bash
export ANTHROPIC_API_KEY=your_anthropic_api_key_here
```

##### For OpenAI (GPT)
```bash
export OPENAI_API_KEY=your_openai_api_key_here
```

##### For Ollama (Local)
No API key required. Ensure Ollama is running on the configured host.

### 4. Configure Models (Optional)

Models are configured via environment variables in your `.env` file:

```env
# Model Configuration
ANTHROPIC_MODEL=claude-3-5-sonnet-20241022
OPENAI_MODEL=gpt-4o
OLLAMA_MODEL=devstral:24b
OLLAMA_BASE_URL=http://localhost:11434
```

If not set, the script will use sensible defaults.

## Usage

### 1. Prepare File Lists

Create a `filelist.md` file with relative paths of files to analyze:

```
inzibackend.Application/MyService.cs
inzibackend.Core/MyEntity.cs
inzibackend.Web.Mvc/Controllers/MyController.cs
```

### 2. Run Analysis

```bash
npm start
```

The tool will:
- Process files from `filelist.md` one by one
- Move processed files to `workfilelist.md` then `donefilelist.md`
- Generate analysis reports in `../migration-plans/src/`
- Log all activities to separate log files

## Output

For each analyzed file, the tool generates a markdown report with:

- **Status**: New, Modified, or Identical
- **Summary**: Description of file content and functionality
- **Changes**: Detailed analysis of modifications
- **Purpose**: Role in the ASP.NET Zero solution

## Log Files

- `../content.log` - File contents being analyzed
- `../ollama_requests.log` - Ollama API requests
- `../ollama_responses.log` - Ollama API responses
- `../anthropic_requests.log` - Anthropic API requests
- `../anthropic_responses.log` - Anthropic API responses
- `../openai_requests.log` - OpenAI API requests
- `../openai_responses.log` - OpenAI API responses
- `../skipped_files.log` - Files that couldn't be processed

## Configuration Options

```typescript
const maxFileSize = 1000000;      // 1MB file size limit
const maxContentLength = 100000;  // Content truncation limit
const retryCount = 5;             // Number of retry attempts
const retryDelayBase = 1000;      // Base retry delay (ms)
```

## Provider Comparison

| Provider | Pros | Cons |
|----------|------|------|
| **Ollama** | Free, local, privacy | Requires local setup, model management |
| **Anthropic** | High quality, good reasoning | Paid API, rate limits |
| **OpenAI** | Widely supported, good performance | Paid API, rate limits |

## Troubleshooting

### Ollama Issues
- Ensure Ollama is running: `ollama serve`
- Check model availability: `ollama list`
- Verify host configuration in script

### API Key Issues
- Verify environment variables are set correctly
- Check API key permissions and quotas
- Ensure keys are not expired

### File Processing Issues
- Check file paths in `filelist.md`
- Verify source directories exist
- Review `skipped_files.log` for specific errors

## Development

### Scripts

- `npm start` - Run the file comparison tool
- `npm run dev` - Run with file watching
- `npm run build` - Compile TypeScript
- `npm test` - Run test script

### Adding New Providers

1. Add new provider type to `modelProvider` union type
2. Create new `invoke[Provider]Analysis` function
3. Add case to `performAIAnalysis` switch statement
4. Update configuration and documentation 