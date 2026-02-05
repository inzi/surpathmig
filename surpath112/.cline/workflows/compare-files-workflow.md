Compare Files and Generate Change Analysis Workflow
This workflow automates the comparison of files listed in filelist.md between the unmodified ASP.NET Zero MVC 11.4 (./aspnetzeromvc11.4/inzibackend/src) and modified (./src) directories on a Windows system. For each file that differs or is new, it generates a change and purpose analysis Markdown file under migration-plans with the same relative path. Identical files are skipped, and large files are marked as unprocessed. Files are processed one at a time by "popping" them from filelist.md to workfilelist.md, with successfully processed files moved to donefilelist.md.
Steps

Pop a file to process  

Read the first line of filelist.md using read_file to get a file path (e.g., ConsoleClient\ConsoleAppModule.cs).  
Append this line to workfilelist.md using write_file.  
Read the last line of workfilelist.md to get the file to process.  
If filelist.md is empty, exit the workflow.  
Tools: read_file, write_file  
Note: filelist.md is not modified (no checkbox updates).


Validate the file  

Extract the relative path from the last line of workfilelist.md (e.g., ConsoleClient\ConsoleAppModule.cs).  
Construct the unmodified path (.\aspnetzeromvc11.4\inzibackend\src\<relative_path>).  
Construct the modified path (.\src\<relative_path>).  
Normalize paths to use backslashes (\) for Windows compatibility.  
Check if the file exists in either directory using search_files.  
If the file doesn’t exist in ./src, skip to the next iteration (leave in workfilelist.md).


Check file size  

Use a PowerShell command to check the size of the modified file:  (Get-Item ".\src\<relative_path>").Length


If the file size exceeds 1MB (1,000,000 bytes), skip processing and leave the file in workfilelist.md for reprocessing (e.g., with a larger-context model).


Compare files  

If the file exists only in ./src (new file):  
Mark as "New" and proceed to analysis.


If the file exists in both:  
Use PowerShell’s Compare-Object to compare the files:  $unmodified = Get-Content ".\aspnetzeromvc11.4\inzibackend\src\<relative_path>" -Raw
$modified = Get-Content ".\src\<relative_path>" -Raw
Compare-Object $unmodified $modified


If no differences are found, skip to Step 7 (mark as processed).  
If differences exist, mark as "Modified" and proceed to analysis.


If the file exists only in the unmodified directory, skip to Step 7 (mark as processed, as it’s not relevant for migration).


Analyze the file  

Read the modified file’s content using read_file (./src/<relative_path>).  
Determine the language based on the file extension:  
.cs: C#  
.csproj: XML  
.js: JavaScript  
.ts: TypeScript  
Others: Infer or note as unknown.


Summarize the file’s content without code snippets (e.g., describe classes, methods, or purpose).  
For modified files, parse the Compare-Object output to summarize changes (e.g., added/removed lines, new methods).  
For new files, describe the file’s purpose based on its content and context.


Generate change analysis file  

Create the output directory under migration-plans matching the relative path (e.g., migration-plans\ConsoleClient).  
Use the provided Markdown template to create a file named <filename>.md (e.g., ConsoleAppModule.cs.md).  
Populate the template with:  
Status: "New" or "Modified"  
Filename: The file’s name (e.g., ConsoleAppModule.cs)  
Relative Path: The file’s relative path (e.g., ConsoleClient\ConsoleAppModule.cs)  
Language: The detected programming language (e.g., C#)  
Summary: A concise description of the file’s content  
Changes: A summary of differences or “New file” for new files  
Purpose: The file’s role in the solution


Write the file using write_file.


Mark file as processed  

If the file was successfully processed (new, modified, identical, or only in unmodified directory):  
Append the last line of workfilelist.md to donefilelist.md using write_file.  
Remove the last line from workfilelist.md using a PowerShell command:  $content = Get-Content ".\workfilelist.md"; if ($content) { $content[0..($content.Length - 2)] | Set-Content ".\workfilelist.md" }




If the file was skipped (e.g., too large or missing in ./src), leave it in workfilelist.md for reprocessing.


Manage context  

If context usage exceeds 70%, use the new_task tool to start a new task session, preloading the current state of workfilelist.md and donefilelist.md.  
Tool: new_task


Repeat  

Continue processing files by returning to Step 1 until filelist.md is fully processed (or manually stopped).



Tools Used

read_file: To read filelist.md, workfilelist.md, and file contents.  
search_files: To verify file existence.  
write_file: To append to workfilelist.md and donefilelist.md, and create analysis files.  
new_task: To manage context for long-running tasks.  
PowerShell commands: (Get-Item).Length, Get-Content, Compare-Object, and custom command to remove the last line from workfilelist.md.

Example Input
filelist.md:
ConsoleClient\ConsoleAppModule.cs
ConsoleClient\ConsoleClient.csproj
ConsoleClient\Program.cs
ConsoleClient\DependencyInjection\DummyServices.cs
ConsoleClient\DependencyInjection\ServiceCollectionRegistrar.cs

workfilelist.md (after popping one file):
ConsoleClient\ConsoleAppModule.cs

donefilelist.md (after processing):
ConsoleClient\ConsoleAppModule.cs

Example Output
migration-plans\ConsoleClient\ConsoleAppModule.cs.md:
# Modified
## Filename
ConsoleAppModule.cs
## Relative Path
ConsoleClient\ConsoleAppModule.cs
## Language
C#
## Summary
A module class that configures services and dependencies for the console client application.
## Changes
Added 5 new service registrations. Modified 2 existing service configurations. Added 1 new method for logging setup.
## Purpose
Initializes and configures the console client’s dependency injection and core services within the ASP.NET Zero solution.

workfilelist.md (after processing):
(empty, or contains unprocessed files like large files)

donefilelist.md (updated):
ConsoleClient\ConsoleAppModule.cs

Notes

PowerShell is required (default on Windows).  
Paths use backslashes (\) for Windows compatibility.  
Large files (>1MB) or missing files remain in workfilelist.md for reprocessing (e.g., with a larger-context model).  
filelist.md is not modified, eliminating checkbox updates.  
Use ask_followup_question if clarification is needed (e.g., ambiguous file paths or unknown file types).  
The workflow assumes filelist.md contains valid relative paths without checkboxes.

