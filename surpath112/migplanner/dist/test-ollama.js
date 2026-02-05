#!/usr/bin/env tsx
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const ollama_1 = require("ollama");
async function testOllamaConnection() {
    console.log('Testing Ollama connection...');
    const ollama = new ollama_1.Ollama({ host: 'http://localhost:11434' });
    try {
        // Test if Ollama is running
        const models = await ollama.list();
        console.log('✅ Ollama is running');
        console.log('Available models:', models.models.map(m => m.name));
        // Check if deepseek-r1:7b is available
        const hasDeepSeek = models.models.some(m => m.name.includes('deepseek-r1'));
        if (hasDeepSeek) {
            console.log('✅ DeepSeek R1 model is available');
            // Test a simple generation
            console.log('Testing simple generation...');
            const response = await ollama.generate({
                model: 'deepseek-r1:7b',
                prompt: 'Respond with just "Hello World" in JSON format: {"message": "Hello World"}',
                stream: false,
                options: {
                    temperature: 0.1,
                }
            });
            console.log('✅ Generation test successful');
            console.log('Response:', response.response.substring(0, 200) + '...');
        }
        else {
            console.log('❌ DeepSeek R1 model not found. Please run: ollama pull deepseek-r1:7b');
        }
    }
    catch (error) {
        console.error('❌ Error connecting to Ollama:', error);
        console.log('Make sure Ollama is running with: ollama serve');
    }
}
if (require.main === module) {
    testOllamaConnection();
}
//# sourceMappingURL=test-ollama.js.map