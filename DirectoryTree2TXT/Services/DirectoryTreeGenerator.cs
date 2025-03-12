using DirectoryTreeViewer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DirectoryTreeViewer.Services
{
    public class DirectoryTreeGenerator
    {
        /// <summary>
        /// Recursively generates a directory tree.
        /// </summary>
        /// <param name="rootPath">The starting directory path.</param>
        /// <param name="indent">Indentation string for formatting.</param>
        /// <param name="isLast">Flag to determine whether the current folder is the last in its branch.</param>
        /// <param name="exclusions">Optional list of exclusions.</param>
        /// <param name="showContents">Flag to indicate whether file contents should be shown.</param>
        /// <returns>A string representing the directory tree.</returns>
        public string GenerateTree(string rootPath, string indent = "", bool isLast = true, List<ExclusionEntry> exclusions = null, bool showContents = false)
        {
            string tree = indent;
            if (!string.IsNullOrEmpty(indent))
            {
                tree += isLast ? "└── " : "├── ";
            }
            tree += "/" + Path.GetFileName(rootPath) + Environment.NewLine;

            List<(bool isDirectory, string path)> children = new List<(bool, string)>();
            try
            {
                // Add directories and files
                children.AddRange(Directory.GetDirectories(rootPath).Select(d => (true, d)));
                children.AddRange(Directory.GetFiles(rootPath).Select(f => (false, f)));
            }
            catch (UnauthorizedAccessException)
            {
                tree += indent + (isLast ? "    " : "│   ") + "└── [Access Denied]" + Environment.NewLine;
                return tree;
            }

            for (int i = 0; i < children.Count; i++)
            {
                bool childIsLast = (i == children.Count - 1);
                var child = children[i];
                string newIndent = indent + (isLast ? "    " : "│   ");

                if (child.isDirectory)
                {
                    string directoryName = Path.GetFileName(child.path);
                    if (exclusions?.Any(e => e.Name.Equals(directoryName, StringComparison.OrdinalIgnoreCase) && e.Ignore) == true)
                        continue;

                    tree += GenerateTree(child.path, newIndent, childIsLast, exclusions, showContents);
                }
                else
                {
                    string fileName = Path.GetFileName(child.path);
                    if (exclusions?.Any(e => e.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase) && e.Ignore) == true)
                        continue;

                    tree += newIndent + (childIsLast ? "└── " : "├── ") + fileName + Environment.NewLine;

                    if (showContents)
                    {
                        try
                        {
                            string fileContent = File.ReadAllText(child.path);
                            string contentIndent = newIndent + (childIsLast ? "    " : "│   ");
                            tree += contentIndent + "    --- Start of " + fileName + " ---" + Environment.NewLine;
                            foreach (var line in fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                            {
                                tree += contentIndent + "    " + line + Environment.NewLine;
                            }
                            tree += contentIndent + "    --- End of " + fileName + " ---" + Environment.NewLine;
                        }
                        catch (UnauthorizedAccessException)
                        {
                            tree += newIndent + "    [Access Denied to " + fileName + "]" + Environment.NewLine;
                        }
                        catch (Exception ex)
                        {
                            tree += newIndent + "    [Error Reading " + fileName + ": " + ex.Message + "]" + Environment.NewLine;
                        }
                    }
                }
            }
            return tree;
        }
    }
}
