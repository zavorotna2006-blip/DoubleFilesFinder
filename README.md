
# DoubleFilesFinder

A C# console application for finding duplicate files by content.

## Features
- Searches files in folders and subfolders.
- Groups files by size.
- Uses SHA-256 to identify duplicate content.
- Displays full paths to duplicate files.
- Skips inaccessible files and directories.

## Algorithm complexity
Let n be the number of files and B be the total size of files hashed.

- Directory traversal and grouping: expected O(n).
- Hashing: O(B).
- Auxiliary memory: O(n), excluding file buffers.

## How to run
1. Open the project in Visual Studio.
2. Run the application.
3. Enter the folder path.
4. Review the duplicate groups.
