# Compare Files and Generate Change Analysis Workflow

This workflow automates the comparison of files listed in `filelist.md` between the unmodified ASP.NET Zero MVC 11.4 (`./aspnetzeromvc11.4/inzibackend/src`) and modified (`./src`) directories on a Windows system. A PowerShell script (`nextcompare.ps1`) handles file popping, comparison, and skipping of identical files, outputting data for files needing analysis. The LLM processes this output to complete a change analysis Markdown file under `migration-plans`. Large files (>1MB) or missing files remain in `workfilelist.md` for reprocessing.

## Steps

1. **Run the PowerShell script**  
   - Execute `nextcompare.ps1` to process the next file from `filelist.md`.  
   - The script:  
     - Reads the first line from `filelist.md` and appends it to `workfilelist.md` (without modifying `filelist.md`).  
     - Takes the last line from `workfilelist.md` as the file to process.  
     - Validates the file’s existence in `./src` and checks its size (<1MB).  
     - Compares the file between `./src` and `./aspnetzeromvc11.4/inzibackend/src` (if it exists in both).  
     - Outputs one of:  
       - `"identical"`: The file is identical; it’s moved to `donefilelist.md`, and the script loops to the next file.  
       - `"finished"`: No files remain in `filelist.md`.  
       - A JSON object containing:  
         - `save_to`: The output path (e.g., `migration-plans/ConsoleClient/ConsoleAppModule.cs.md`).  
         - `template`: A partial Markdown template with pre-filled fields (status, filename, relative path, language).  
         - `modified_content`: Content of the modified file (`./src/<relative_path>`).  
         - `unmodified_content`: Content of the unmodified file (or empty for new files).  
         - `relative_path`: The file’s relative path (e.g., `ConsoleClient/ConsoleAppModule.cs`).  
         - `status`: "New" or "Modified".  
   - Tool: PowerShell (`nextcompare.ps1`)

2. **Process the script output**  
   - If the output is `"finished"`, exit the workflow.  
   - If the output is `"identical"`, loop back to Step 1 to run `nextcompare.ps1` again.  
   - If the output is a JSON object:  
     - Parse the JSON to extract `save_to`, `template`, `modified_content`, `unmodified_content`, `relative_path`, and `status`.  
     - Complete the template’s `Summary`, `Changes`, and `Purpose` fields:  
       - **Summary**: Analyze `modified_content` to describe the file’s content (e.g., classes, methods, or purpose) without code snippets.  
       - **Changes**: For "Modified" files, compare `modified_content` and `unmodified_content` to summarize differences (e.g., added/removed lines, new methods). For "New" files, use “New file”.  
       - **Purpose**: Describe the file’s role in the solution based on `modified_content` and context.  
     - Save the completed template to the path specified in `save_to` using `write_file`.  
     - Append the `relative_path` to `donefilelist.md` and remove it from `workfilelist.md` using PowerShell:  
       ```powershell
       Add-Content -Path ".\donefilelist.md" -Value "<relative_path>"
       $content = Get-Content ".\workfilelist.md"; if ($content) { $content[0..($content.Length - 2)] | Set-Content ".\workfilelist.md" }
       ```  
   - Tools: `write_file`, PowerShell for file list updates

3. **Manage context**  
   - If context usage exceeds 70%, use the `new_task` tool to start a new task session, preloading the current state of `workfilelist.md` and `donefilelist.md`.  
   - Tool: `new_task`

4. **Repeat**  
   - Loop back to Step 1 until `nextcompare.ps1` outputs `"finished"`.

## Tools Used
- PowerShell: `nextcompare.ps1` for file popping, comparison, and initial processing.  
- `write_file`: To save analysis files and update `donefilelist.md`.  
- `new_task`: To manage context for long-running tasks.  
- PowerShell commands: For updating `workfilelist.md` and `donefilelist.md`.

## Example Input
**filelist.md**:
```markdown
ConsoleClient\ConsoleAppModule.cs
ConsoleClient\ConsoleClient.csproj
ConsoleClient\Program.cs
ConsoleClient\DependencyInjection\DummyServices.cs
ConsoleClient\DependencyInjection\ServiceCollectionRegistrar.cs
```

## Example Script Outputs
- **Identical file**:
  ```
  identical
  ```

- **File needing analysis**:
  ```json
  {
    "save_to": "migration-plans/ConsoleClient/ConsoleAppModule.cs.md",
    "template": "# Modified\n## Filename\nConsoleAppModule.cs\n## Relative Path\nConsoleClient\\ConsoleAppModule.cs\n## Language\nC#\n## Summary\n\n## Changes\n\n## Purpose\n",
    "modified_content": "namespace ConsoleClient { public class ConsoleAppModule { ... } }",
    "unmodified_content": "namespace ConsoleClient { public class ConsoleAppModule { ... } }",
    "relative_path": "ConsoleClient\\ConsoleAppModule.cs",
    "status": "Modified"
  }
  ```

- **Finished**:
  ```
  finished
  ```

## Example Output
**migration-plans/ConsoleClient/ConsoleAppModule.cs.md**:
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

**donefilelist.md**:
```markdown
ConsoleClient\ConsoleAppModule.cs
```

**workfilelist.md**:
```markdown
(empty, or contains unprocessed files like large files)
```

## Notes
- Requires PowerShell (default on Windows).  
- Paths use backslashes (`\`) for Windows compatibility.  
- Large files (>1MB) or missing files remain in `workfilelist.md` for reprocessing.  
- `filelist.md` is not modified.  
- The LLM only analyzes files flagged as "New" or "Modified" by `nextcompare.ps1`.  
- Use `ask_followup_question` if clarification is needed (e.g., ambiguous file content).  
- The workflow assumes `filelist.md` contains valid relative paths without checkboxes.