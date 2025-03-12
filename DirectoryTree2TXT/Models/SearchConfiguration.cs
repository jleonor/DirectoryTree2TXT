using System.Collections.Generic;

namespace DirectoryTreeViewer.Models
{
    public class SearchConfiguration
    {
        public string DirectoryPath { get; set; } = string.Empty;
        public List<ExclusionEntry> ExclusionEntries { get; set; } = new List<ExclusionEntry>();
        public bool ShowContents { get; set; }
    }
}
