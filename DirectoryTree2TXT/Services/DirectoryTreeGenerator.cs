using DirectoryTreeViewer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
        public string GenerateTree(string rootPath, string indent = "", bool isLast = true, List<ExclusionEntry>? exclusions = null, bool showContents = false, bool showOnlyFolders = false)
        {
            string tree = indent;
            if (!string.IsNullOrEmpty(indent))
            {
                tree += isLast ? "└── " : "├── ";
            }

            string currentName = "/" + Path.GetFileName(rootPath);
            tree += currentName + Environment.NewLine;

            List<(bool isDirectory, string path)> children = new List<(bool, string)>();
            try
            {
                children.AddRange(Directory.GetDirectories(rootPath).Select(d => (true, d)));
                if (!showOnlyFolders)
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
                string childName = Path.GetFileName(child.path);

                // Check if the entry should be hidden or ignored using wildcard matching
                bool isHidden = exclusions?.Any(e => MatchesExclusion(e.Name, childName) && e.Hide) == true;
                bool isIgnored = exclusions?.Any(e => MatchesExclusion(e.Name, childName) && e.Ignore) == true;

                // Skip the entry entirely if hidden.
                if (isHidden)
                {
                    continue;
                }

                if (child.isDirectory)
                {
                    if (isIgnored)
                    {
                        tree += newIndent + (childIsLast ? "└── " : "├── ") + childName + Environment.NewLine;
                    }
                    else
                    {
                        tree += GenerateTree(child.path, newIndent, childIsLast, exclusions, showContents, showOnlyFolders);
                    }
                }
                else
                {
                    tree += newIndent + (childIsLast ? "└── " : "├── ") + childName + Environment.NewLine;

                    if (showContents && !isIgnored)
                    {
                        try
                        {
                            string fileContent = File.ReadAllText(child.path);
                            string contentIndent = newIndent + (childIsLast ? "    " : "│   ");
                            tree += contentIndent + "    --- Start of " + childName + " ---" + Environment.NewLine;
                            foreach (var line in fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                            {
                                tree += contentIndent + "    " + line + Environment.NewLine;
                            }
                            tree += contentIndent + "    --- End of " + childName + " ---" + Environment.NewLine;
                        }
                        catch (Exception ex)
                        {
                            tree += newIndent + "    [Error Reading " + childName + ": " + ex.Message + "]" + Environment.NewLine;
                        }
                    }
                }
            }
            return tree;
        }

        private bool MatchesExclusion(string exclusionPattern, string fileName)
        {
            if (!exclusionPattern.Contains("*") && !exclusionPattern.Contains("?"))
            {
                return string.Equals(exclusionPattern, fileName, StringComparison.OrdinalIgnoreCase);
            }

            // Convert wildcard pattern to regex
            string pattern = "^" + Regex.Escape(exclusionPattern)
                                    .Replace("\\*", ".*")   // * -> match any number of characters
                                    .Replace("\\?", ".")    // ? -> match any single character
                                    + "$";

            return Regex.IsMatch(fileName, pattern, RegexOptions.IgnoreCase);
        }

    }
}
