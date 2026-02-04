import os
import shutil
import subprocess
import tempfile
import json
import argparse

def run_git_command(args, cwd):
    result = subprocess.run(
        args,
        cwd=cwd,
        capture_output=True,
        text=True,                # ← keep this
        encoding='utf-8',         # ← add this
        errors='replace'          # optional: turns undecodable bytes →   (prevents crash)
    )
    if result.returncode != 0:
        raise RuntimeError(f"Git command failed:\n{result.stderr}")
    return result.stdout

def get_changes(base_dir, modified_dir, output_file):
    with tempfile.TemporaryDirectory() as temp_dir:
        repo_dir = os.path.join(temp_dir, 'repo')
        shutil.copytree(base_dir, repo_dir)
        
        # Initialize git repo
        run_git_command(['git', 'init'], repo_dir)
        run_git_command(['git', 'add', '.'], repo_dir)
        run_git_command(['git', 'commit', '-m', 'base'], repo_dir)
        
        # Overlay modified_dir on top of repo_dir
        shutil.copytree(modified_dir, repo_dir, dirs_exist_ok=True)
        
        # Get git status
        status_output = run_git_command(['git', 'status', '--porcelain'], repo_dir)
        
        changes = []
        for line in status_output.splitlines():
            if not line.strip():
                continue
            status, path = line.split(maxsplit=1)
            path = path.strip('"')  # Handle paths with spaces
            
            # Filter for .cs, .js, .ts files
            if not path.endswith(('.cs', '.js', '.ts')):
                continue
            
            if status == 'M' or status == 'MM':
                change_type = 'modified'
                diff = run_git_command(['git', 'diff', 'HEAD', '--', path], repo_dir)
            elif status == '??':
                change_type = 'new'
                # For new files, create a diff from empty file
                with open(os.path.join(repo_dir, path), 'r', encoding='utf-8') as f:
                    content = f.read()
                diff_lines = ['--- /dev/null', f'+++ b/{path}']
                diff_lines.extend([f'+{l}' for l in content.splitlines()])
                diff = '\n'.join(diff_lines) + '\n'
            else:
                continue  # Ignore other statuses like deletions (D)
            
            changes.append({
                'file': path,
                'type': change_type,
                'diff': diff
            })
        
        # Write to jsonl
        with open(output_file, 'w', encoding='utf-8') as f:
            for change in changes:
                f.write(json.dumps(change) + '\n')

if __name__ == '__main__':
    parser = argparse.ArgumentParser(description='Compare two project folders and list changes in .cs, .js, .ts files, honoring .gitignore.')
    parser.add_argument('base_dir', help='Path to the base source code directory')
    parser.add_argument('modified_dir', help='Path to the modified project directory')
    parser.add_argument('output_file', help='Path to the output jsonl file')
    args = parser.parse_args()
    
    get_changes(args.base_dir, args.modified_dir, args.output_file)
    print(f'Changes saved to {args.output_file}')