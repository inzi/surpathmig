import * as fs from 'fs';
import * as path from 'path';
import { FolderInfo, FileInfo, CodeAnalysis } from './folder-analyzer';
import { ComponentUsage, CrossReference } from './cross-reference-analyzer';

export interface SubfolderSummary {
    name: string;
    path: string;
    summary: string;
}

export interface DocumentationContent {
    folderPath: string;
    folderName: string;
    overview: string;
    files: FileInfo[];
    keyComponents: string[];
    dependencies: string[];
    subfolderSummaries: SubfolderSummary[];
    architectureNotes: string[];
    businessLogic: string[];
    patterns: string[];
    crossReferences?: ComponentUsage[];
    totalUsages?: number;
    impactRadius?: number;
    testCoverage?: string[];
}

export class ClaudeMdWriter {
    private rootPath: string;

    constructor(rootPath: string) {
        this.rootPath = rootPath;
    }

    /**
     * Generate CLAUDE.md content for a folder
     */
    public generateDocumentation(
        folderInfo: FolderInfo,
        analysis: CodeAnalysis,
        subfolderSummaries: SubfolderSummary[]
    ): string {
        const content: DocumentationContent = {
            folderPath: folderInfo.path,
            folderName: folderInfo.name,
            overview: analysis.purpose || this.generateOverview(folderInfo, analysis),
            files: folderInfo.files,
            keyComponents: analysis.keyComponents,
            dependencies: analysis.dependencies,
            subfolderSummaries: subfolderSummaries,
            architectureNotes: this.generateArchitectureNotes(folderInfo, analysis),
            businessLogic: analysis.businessLogic,
            patterns: analysis.patterns,
            crossReferences: analysis.crossReferences,
            totalUsages: analysis.totalUsages,
            impactRadius: analysis.impactRadius,
            testCoverage: analysis.testCoverage
        };

        return this.formatDocumentation(content);
    }

    /**
     * Format documentation content into markdown
     */
    private formatDocumentation(content: DocumentationContent): string {
        const lines: string[] = [];

        // Header
        lines.push(`# ${content.folderName} Documentation`);
        lines.push('');
        lines.push(`> Auto-generated documentation for ${path.relative(this.rootPath, content.folderPath)}`);
        lines.push('');

        // Overview
        lines.push('## Overview');
        lines.push(content.overview);
        lines.push('');

        // Contents section
        if (content.files.length > 0 || content.keyComponents.length > 0) {
            lines.push('## Contents');
            lines.push('');

            // Files subsection
            if (content.files.length > 0) {
                lines.push('### Files');
                for (const file of content.files) {
                    const desc = file.description || 'Code file';
                    lines.push(`- **${file.name}**: ${desc}`);
                    if (file.keyComponents && file.keyComponents.length > 0) {
                        lines.push(`  - Components: ${file.keyComponents.slice(0, 5).join(', ')}`);
                    }
                }
                lines.push('');
            }

            // Key Components subsection
            if (content.keyComponents.length > 0) {
                lines.push('### Key Components');
                const uniqueComponents = this.getUniqueComponents(content.keyComponents);
                for (const component of uniqueComponents.slice(0, 15)) {
                    lines.push(`- ${component}`);
                }
                lines.push('');
            }

            // Dependencies subsection
            if (content.dependencies.length > 0) {
                lines.push('### Dependencies');
                lines.push('');
                lines.push('External libraries and frameworks:');
                for (const dep of content.dependencies.slice(0, 10)) {
                    lines.push(`- ${dep}`);
                }
                lines.push('');
            }
        }

        // Subfolders section
        if (content.subfolderSummaries.length > 0) {
            lines.push('## Subfolders');
            lines.push('');
            for (const subfolder of content.subfolderSummaries) {
                lines.push(`### ${subfolder.name}/`);
                lines.push(`> **Summary from ${subfolder.name}/CLAUDE.md**`);
                lines.push(`> ${subfolder.summary}`);
                lines.push('');
            }
        }

        // Architecture Notes section
        if (content.architectureNotes.length > 0) {
            lines.push('## Architecture Notes');
            lines.push('');
            for (const note of content.architectureNotes) {
                lines.push(`- ${note}`);
            }
            lines.push('');
        }

        // Business Logic section
        if (content.businessLogic.length > 0) {
            lines.push('## Business Logic');
            lines.push('');
            lines.push('Key business rules and domain logic found in this folder:');
            lines.push('');
            for (const logic of content.businessLogic.slice(0, 10)) {
                // Clean up the logic string
                const cleaned = logic.replace(/\s+/g, ' ').trim();
                if (cleaned.length > 20) {
                    lines.push(`- ${cleaned}`);
                }
            }
            lines.push('');
        }

        // Patterns section
        if (content.patterns.length > 0) {
            lines.push('## Design Patterns');
            lines.push('');
            for (const pattern of content.patterns) {
                lines.push(`- ${pattern}`);
            }
            lines.push('');
        }

        // Cross-Reference section (if available)
        if (content.crossReferences && content.crossReferences.length > 0) {
            lines.push('## Usage Across Codebase');
            lines.push('');

            // Summary metrics
            if (content.totalUsages && content.totalUsages > 0) {
                lines.push('### Summary');
                lines.push(`- **Total Usages**: ${content.totalUsages} references found across the codebase`);
                if (content.impactRadius && content.impactRadius > 0) {
                    lines.push(`- **Impact Radius**: Changes may affect up to ${content.impactRadius} files`);
                }
                if (content.testCoverage && content.testCoverage.length > 0) {
                    lines.push(`- **Test Coverage**: Referenced by ${content.testCoverage.length} test file${content.testCoverage.length !== 1 ? 's' : ''}`);
                }
                lines.push('');
            }

            // Direct consumers by component
            const componentGroups = this.groupCrossReferencesByComponent(content.crossReferences);
            if (componentGroups.size > 0) {
                lines.push('### Direct Consumers');
                lines.push('');

                for (const [component, refs] of componentGroups) {
                    if (refs.directConsumers.length > 0) {
                        lines.push(`#### ${component}`);
                        const topConsumers = refs.directConsumers.slice(0, 10);
                        for (const consumer of topConsumers) {
                            const relPath = path.relative(this.rootPath, consumer).replace(/\\/g, '/');
                            lines.push(`- \`${relPath}\``);
                        }
                        if (refs.directConsumers.length > 10) {
                            lines.push(`- ... and ${refs.directConsumers.length - 10} more files`);
                        }
                        lines.push('');
                    }
                }
            }

            // Test coverage details
            if (content.testCoverage && content.testCoverage.length > 0) {
                lines.push('### Test Coverage');
                lines.push('');
                lines.push('This code is covered by the following test files:');
                for (const testFile of content.testCoverage.slice(0, 5)) {
                    const relPath = path.relative(this.rootPath, testFile).replace(/\\/g, '/');
                    lines.push(`- \`${relPath}\``);
                }
                if (content.testCoverage.length > 5) {
                    lines.push(`- ... and ${content.testCoverage.length - 5} more test files`);
                }
                lines.push('');
            }

            // Impact analysis
            if (content.impactRadius && content.impactRadius > 5) {
                lines.push('### Impact Analysis');
                lines.push('');
                if (content.impactRadius > 20) {
                    lines.push('**⚠️ High Impact Component**: Changes to this folder may have wide-reaching effects.');
                } else {
                    lines.push('**Moderate Impact**: Changes to this folder affect multiple downstream components.');
                }
                lines.push('');
            }
        }

        // Footer
        lines.push('---');
        lines.push(`*Generated on ${new Date().toISOString().split('T')[0]}*`);

        return lines.join('\n');
    }

    /**
     * Generate overview if not provided
     */
    private generateOverview(folderInfo: FolderInfo, analysis: CodeAnalysis): string {
        const fileCount = folderInfo.files.length;
        const subfolderCount = folderInfo.subfolders.length;

        let overview = `This folder contains ${fileCount} file${fileCount !== 1 ? 's' : ''}`;
        if (subfolderCount > 0) {
            overview += ` and ${subfolderCount} subfolder${subfolderCount !== 1 ? 's' : ''}`;
        }
        overview += '.';

        if (analysis.purpose) {
            overview += ` Its primary purpose is ${analysis.purpose}.`;
        }

        if (analysis.patterns.length > 0) {
            overview += ` It implements ${analysis.patterns.join(', ')}.`;
        }

        return overview;
    }

    /**
     * Generate architecture notes
     */
    private generateArchitectureNotes(folderInfo: FolderInfo, analysis: CodeAnalysis): string[] {
        const notes: string[] = [];

        // File organization
        const extensions = [...new Set(folderInfo.files.map(f => f.extension))];
        if (extensions.length > 0) {
            notes.push(`File types: ${extensions.join(', ')}`);
        }

        // Naming conventions
        const hasInterfaces = folderInfo.files.some(f =>
            f.name.startsWith('I') && f.name[1] === f.name[1].toUpperCase()
        );
        if (hasInterfaces) {
            notes.push('Follows interface naming convention (I prefix)');
        }

        // Test files
        const hasTests = folderInfo.files.some(f =>
            f.name.includes('.test.') || f.name.includes('.spec.')
        );
        if (hasTests) {
            notes.push('Contains unit tests');
        }

        // Configuration files
        const hasConfig = folderInfo.files.some(f =>
            f.name.includes('config') || f.name.includes('settings')
        );
        if (hasConfig) {
            notes.push('Contains configuration files');
        }

        // Framework-specific notes
        if (analysis.dependencies.includes('Microsoft.AspNetCore')) {
            notes.push('ASP.NET Core implementation');
        }
        if (analysis.dependencies.includes('EntityFramework')) {
            notes.push('Uses Entity Framework for data access');
        }

        return notes;
    }

    /**
     * Get unique components from the list
     */
    private getUniqueComponents(components: string[]): string[] {
        const seen = new Set<string>();
        const unique: string[] = [];

        for (const component of components) {
            // Extract just the component name without the file
            const parts = component.split(':');
            const componentName = parts.length > 1 ? parts[1].trim() : component;

            if (!seen.has(componentName)) {
                seen.add(componentName);
                unique.push(component);
            }
        }

        return unique;
    }

    /**
     * Write documentation to CLAUDE.md file
     */
    public async writeDocumentation(folderPath: string, content: string): Promise<void> {
        const claudeMdPath = path.join(folderPath, 'CLAUDE.md');

        try {
            fs.writeFileSync(claudeMdPath, content, 'utf-8');
            console.log(`Created CLAUDE.md in ${path.relative(this.rootPath, folderPath)}`);
        } catch (error) {
            throw new Error(`Failed to write CLAUDE.md: ${error}`);
        }
    }

    /**
     * Read existing CLAUDE.md file
     */
    public async readClaudeMd(folderPath: string): Promise<string | null> {
        const claudeMdPath = path.join(folderPath, 'CLAUDE.md');

        if (!fs.existsSync(claudeMdPath)) {
            return null;
        }

        try {
            return fs.readFileSync(claudeMdPath, 'utf-8');
        } catch (error) {
            console.warn(`Could not read CLAUDE.md from ${folderPath}: ${error}`);
            return null;
        }
    }

    /**
     * Extract summary from CLAUDE.md content
     */
    public extractSummary(claudeMdContent: string): string {
        if (!claudeMdContent) {
            return 'No documentation available';
        }

        // Try to extract the overview section
        const overviewMatch = claudeMdContent.match(/## Overview\s*\n([\s\S]*?)(?=\n##|$)/);
        if (overviewMatch && overviewMatch[1]) {
            const overview = overviewMatch[1].trim();
            // Return first paragraph or first 200 characters
            const firstParagraph = overview.split('\n\n')[0];
            if (firstParagraph.length <= 200) {
                return firstParagraph;
            }
            return firstParagraph.substring(0, 197) + '...';
        }

        // Fallback: return first non-header line
        const lines = claudeMdContent.split('\n');
        for (const line of lines) {
            const trimmed = line.trim();
            if (trimmed && !trimmed.startsWith('#') && !trimmed.startsWith('>')) {
                if (trimmed.length <= 200) {
                    return trimmed;
                }
                return trimmed.substring(0, 197) + '...';
            }
        }

        return 'Documentation available';
    }

    /**
     * Update root CLAUDE.md with documentation structure
     */
    public async updateRootDocumentation(processedFolders: string[]): Promise<void> {
        const rootClaudeMdPath = path.join(this.rootPath, 'CLAUDE.md');
        let existingContent = '';

        // Read existing content
        if (fs.existsSync(rootClaudeMdPath)) {
            existingContent = fs.readFileSync(rootClaudeMdPath, 'utf-8');
        }

        // Remove existing documentation structure section if present
        const structureStart = existingContent.indexOf('## Documentation Structure');
        if (structureStart !== -1) {
            existingContent = existingContent.substring(0, structureStart).trim();
        }

        // Generate new documentation structure section
        const structureSection = this.generateDocumentationStructure(processedFolders);

        // Combine content
        const newContent = existingContent + '\n\n' + structureSection;

        // Write updated content
        fs.writeFileSync(rootClaudeMdPath, newContent, 'utf-8');
        console.log('Updated root CLAUDE.md with documentation structure');
    }

    /**
     * Group cross-references by component
     */
    private groupCrossReferencesByComponent(crossReferences: ComponentUsage[]): Map<string, ComponentUsage> {
        const grouped = new Map<string, ComponentUsage>();

        for (const ref of crossReferences) {
            if (!grouped.has(ref.component)) {
                grouped.set(ref.component, ref);
            }
        }

        return grouped;
    }

    /**
     * Generate documentation structure section
     */
    private generateDocumentationStructure(processedFolders: string[]): string {
        const lines: string[] = [];

        lines.push('## Documentation Structure');
        lines.push('');
        lines.push('The following folders have been documented with CLAUDE.md files:');
        lines.push('');

        // Group folders by top-level directory
        const grouped = new Map<string, string[]>();

        for (const folder of processedFolders) {
            const relative = path.relative(this.rootPath, folder).replace(/\\/g, '/');
            const parts = relative.split('/');
            const topLevel = parts[0] || '.';

            if (!grouped.has(topLevel)) {
                grouped.set(topLevel, []);
            }
            grouped.get(topLevel)!.push(relative);
        }

        // Output grouped structure
        for (const [topLevel, folders] of grouped) {
            lines.push(`### ${topLevel}/`);
            for (const folder of folders.sort()) {
                lines.push(`- [${folder}](${folder}/CLAUDE.md)`);
            }
            lines.push('');
        }

        lines.push('---');
        lines.push(`*Documentation structure updated on ${new Date().toISOString().split('T')[0]}*`);

        return lines.join('\n');
    }
}