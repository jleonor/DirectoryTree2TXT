﻿namespace DirectoryTreeViewer.Models
{
    public class ExclusionEntry
    {
        public string Name { get; set; } = string.Empty;
        public bool Ignore { get; set; }
        public bool Hide { get; set; } = true;
    }
}
