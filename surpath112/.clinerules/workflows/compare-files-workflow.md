# Compare Files and Generate Change Analysis Workflow

This workflow automates the comparison of files listed in `filelist.md` between the unmodified ASP.NET Zero MVC 11.4 (`./aspnetzeromvc11.4/inzibackend/src`) and modified (`./src`) directories on a Windows system. For each file that differs or is new, it generates a change and purpose analysis Markdown file under `migration-plans` with the same relative path. Identical files are skipped, and large files are marked with an "X" in the list.

## Steps

1. **Read the file list**  
   Use `read_file` to load `filelist.md` and extract the list of files to compare (e.g., `- [ ] ConsoleClient\ConsoleAppModule.cs`).  
   - Input: `filelist.md`  
   - Tool: `read_file`

2. **Iterate through each file**  
   For each file in the list:  
   - Extract the relative path (e.g., `ConsoleClient\ConsoleAppModule.cs`).  
   - Construct the unmodified path (`./aspnetzeromvc11.4/inzibackend/src/<relative_path>`).  
   - Construct the modified path (`./src/<relative_path>`).  
   - Normalize paths to use backslashes (`\`) for Windows compatibility.  
   - Check if the file exists in both directories using `search_files`.  

3. **Check file size**  
   Use a PowerShell command to check the size of the modified file:  
   ```powershell
   (Get-Item ".\src\<relative_path>").Length
   ```  
   If the file size exceeds 1MB (1,000,000 bytes), mark it with an "X" in `filelist.md` (e.g., `- [X] ConsoleClient\ConsoleAppModule.cs (Too large)`) and skip to the next file.  

4. **Compare files**  
   - If the file exists only in `./src` (new file):  
     - Mark as "New" and proceed to analysis.  
   - If the file exists in both:  
     - Use PowerShell’s `Compare-Object` to compare the files:  
       ```powershell
       $unmodified = Get-Content ".\aspnetzeromvc11.4\inzibackend\src\<relative_path>" -Raw
       $modified = Get-Content ".\src\<relative_path>" -Raw
       Compare-Object $unmodified $modified
       ```  
     - If no differences are found, skip to the next file.  
     - If differences exist, mark as "Modified" and proceed to analysis.  
   - If the file exists only in the unmodified directory, skip (not relevant for migration).  

5. **Analyze the file**  
   - Read the modified file’s content using `read_file` (`./src/<relative_path>`).  
   - Determine the language based on the file extension:  
     - `.cs`: C#  
     - `.csproj`: XML  
     - Others: Infer or note as unknown.  
   - Summarize the file’s content without code snippets (e.g., describe classes, methods, or purpose).  
   - For modified files, parse the `Compare-Object` output to summarize changes (e.g., added/removed lines, new methods).  
   - For new files, describe the file’s purpose based on its content and context.  

6. **Generate change analysis file**  
   - Create the output directory under `migration-plans` matching the relative path (e.g., `migration-plans\ConsoleClient`).  
   - Use the provided template to create a Markdown file named `<filename>.md` (e.g., `ConsoleAppModule.cs.md`).  
   - Populate the template with:  
     - Status: "New" or "Modified"  
     - Filename: The file’s name (e.g., `ConsoleAppModule.cs`)  
     - Relative Path: The file’s relative path (e.g., `ConsoleClient\ConsoleAppModule.cs`)  
     - Language: The detected programming language (e.g., C#)  
     - Summary: A concise description of the file’s content  
     - Changes: A summary of differences or “New file” for new files  
     - Purpose: The file’s role in the solution  
   - Write the file using `write_file`.  

7. **Update the file list**  
   - Mark the file as processed in `filelist.md` by updating the checkbox (e.g., `- [x] ConsoleClient\ConsoleAppModule.cs`).  
   - If the file was too large, update to `- [X] ConsoleClient\ConsoleAppModule.cs (Too large)`.  
   - Use `write_file` to update `filelist.md`.  

8. **Manage context**  
   - If context usage exceeds 70%, use the `new_task` tool to start a new task session, preloading the current file list and progress.  
   - Tool: `new_task`  

9. **Repeat**  
   - Continue until all files in the list are processed.  

## Tools Used
- `read_file`: To read `filelist.md` and file contents.  
- `search_files`: To verify file existence.  
- `write_file`: To create analysis files and update `filelist.md`.  
- `new_task`: To manage context for long-running tasks.  
- PowerShell commands: `(Get-Item).Length`, `Get-Content`, `Compare-Object` for file size and comparison.  

## Example Input
**filelist.md**:
```markdown
- [ ] ConsoleClient\ConsoleAppModule.cs
- [ ] ConsoleClient\ConsoleClient.csproj
- [ ] ConsoleClient\Program.cs
- [ ] ConsoleClient\DependencyInjection\DummyServices.cs
- [ ] ConsoleClient\DependencyInjection\ServiceCollectionRegistrar.cs
```

## Example Output
**migration-plans\ConsoleClient\ConsoleAppModule.cs.md**:
```markdown
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
```

**Updated filelist.md**:
```markdown
- [x] ConsoleClient\ConsoleAppModule.cs
- [ ] ConsoleClient\ConsoleClient.csproj
- [ ] ConsoleClient\Program.cs
- [ ] ConsoleClient\DependencyInjection\DummyServices.cs
- [ ] ConsoleClient\DependencyInjection\ServiceCollectionRegistrar.cs
```

## Notes
- Ensure PowerShell is available in the environment (default on Windows).  
- Paths use backslashes (`\`) for Windows compatibility.  
- If a file is too large, the workflow skips analysis but updates `filelist.md`.  
- The workflow assumes `filelist.md` contains valid relative paths as shown in the provided snippet.  
- Use `ask_followup_question` if clarification is needed (e.g., ambiguous file paths or unknown file types).