# Example Usage: Switching Between AI Providers

This document shows how to configure and use different AI providers with the migration planner tool.

## Quick Start Examples

### Using Ollama (Local AI)

1. **Configure the provider:**
```typescript
// In compare-files.ts
const modelProvider: "ollama" | "anthropic" | "openai" = "ollama";
```

2. **Set environment variables (optional):**
```env
# In .env file
OLLAMA_MODEL=devstral:24b
OLLAMA_BASE_URL=http://localhost:11434
```

2. **Start Ollama (if not running):**
```bash
ollama serve
```

3. **Run the analysis:**
```bash
npm start
```

### Using Anthropic Claude

1. **Set your API key:**
```bash
# Windows (PowerShell)
$env:ANTHROPIC_API_KEY="your_anthropic_api_key_here"

# Linux/macOS
export ANTHROPIC_API_KEY="your_anthropic_api_key_here"
```

2. **Configure the provider:**
```typescript
// In compare-files.ts
const modelProvider: "ollama" | "anthropic" | "openai" = "anthropic";
```

3. **Set model (optional):**
```env
# In .env file
ANTHROPIC_MODEL=claude-3-5-sonnet-20241022
```

3. **Run the analysis:**
```bash
npm start
```

### Using OpenAI GPT

1. **Set your API key:**
```bash
# Windows (PowerShell)
$env:OPENAI_API_KEY="your_openai_api_key_here"

# Linux/macOS
export OPENAI_API_KEY="your_openai_api_key_here"
```

2. **Configure the provider:**
```typescript
// In compare-files.ts
const modelProvider: "ollama" | "anthropic" | "openai" = "openai";
```

3. **Set model (optional):**
```env
# In .env file
OPENAI_MODEL=gpt-4o
```

3. **Run the analysis:**
```bash
npm start
```

## Sample Output Comparison

All providers generate the same structured output format, but with different analysis quality and perspectives:

### Example Analysis Output

```markdown
# Modified
## Filename
UserService.cs
## Relative Path
inzibackend.Application/Users/UserService.cs
## Language
C#
## Summary
Application service class that handles user management operations including CRUD operations, role assignments, and user authentication workflows in an ASP.NET Zero application.
## Changes
Added new method GetUsersByRoleAsync() for filtering users by role, updated constructor to inject IRoleManager dependency, and added validation logic for user creation with enhanced error handling.
## Purpose
Core application service in the Application Layer that implements business logic for user management, serving as an intermediary between the presentation layer and domain entities while maintaining separation of concerns in the DDD architecture.
```

## Provider-Specific Considerations

### Ollama
- **Best for**: Privacy-conscious environments, offline work, cost control
- **Setup time**: Longer (requires model download and local setup)
- **Performance**: Depends on local hardware
- **Cost**: Free after initial setup

### Anthropic Claude
- **Best for**: High-quality analysis, complex reasoning tasks
- **Setup time**: Minimal (just API key)
- **Performance**: Fast, consistent
- **Cost**: Pay-per-token (check current pricing)

### OpenAI GPT
- **Best for**: Widely supported, good general performance
- **Setup time**: Minimal (just API key)
- **Performance**: Fast, reliable
- **Cost**: Pay-per-token (check current pricing)

## Switching Providers Mid-Project

You can switch providers at any time during analysis:

1. **Stop the current process** (Ctrl+C)
2. **Update the configuration** in `compare-files.ts`
3. **Set appropriate environment variables** (for cloud providers)
4. **Restart the analysis** with `npm start`

The tool will continue from where it left off using the new provider.

## Environment Variables Setup

### Using .env file (recommended)

Copy the example file and edit it with your API keys:

```bash
cp .env.example .env
# Then edit .env with your actual API keys
```

The `.env` file should contain:

```env
# API Keys
ANTHROPIC_API_KEY=your_anthropic_api_key_here
OPENAI_API_KEY=your_openai_api_key_here

# Model Configuration (optional)
ANTHROPIC_MODEL=claude-3-5-sonnet-20241022
OPENAI_MODEL=gpt-4o
OLLAMA_MODEL=devstral:24b

# Ollama Configuration (optional)
OLLAMA_BASE_URL=http://localhost:11434
```

### Using system environment variables

```bash
# Windows (PowerShell)
$env:ANTHROPIC_API_KEY="your_key_here"
$env:OPENAI_API_KEY="your_key_here"

# Linux/macOS (bash/zsh)
export ANTHROPIC_API_KEY="your_key_here"
export OPENAI_API_KEY="your_key_here"
```

## Troubleshooting Provider Issues

### Ollama Not Responding
```bash
# Check if Ollama is running
curl http://localhost:11434/api/tags

# Start Ollama if not running
ollama serve

# Check available models
ollama list
```

### API Key Issues
```bash
# Test Anthropic API key
curl https://api.anthropic.com/v1/messages \
  -H "x-api-key: $ANTHROPIC_API_KEY" \
  -H "content-type: application/json" \
  -d '{"model":"claude-3-5-sonnet-20241022","max_tokens":10,"messages":[{"role":"user","content":"Hi"}]}'

# Test OpenAI API key
curl https://api.openai.com/v1/models \
  -H "Authorization: Bearer $OPENAI_API_KEY"
```

### Rate Limiting
If you hit rate limits with cloud providers:
1. Reduce processing speed by adding delays
2. Switch to a different provider temporarily
3. Upgrade your API plan
4. Use Ollama for unlimited local processing

## Performance Comparison

Based on typical usage:

| Provider | Speed | Quality | Cost | Setup |
|----------|-------|---------|------|-------|
| Ollama | Medium* | Good | Free | Complex |
| Anthropic | Fast | Excellent | $$ | Simple |
| OpenAI | Fast | Very Good | $ | Simple |

*Depends on local hardware

## Best Practices

1. **Start with Ollama** for testing and development
2. **Use cloud providers** for production analysis requiring high quality
3. **Keep API keys secure** and never commit them to version control
4. **Monitor costs** when using paid APIs
5. **Test with small file sets** before processing large batches
6. **Keep backups** of your analysis results 