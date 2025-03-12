using System;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace DirectoryTreeViewer.ViewModels
{
    public class ConfigurationFileViewModel : INotifyPropertyChanged
    {
        private FileInfo _file;
        public FileInfo File
        {
            get => _file;
            set
            {
                _file = value;
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        // Constructor now requires a FileInfo.
        public ConfigurationFileViewModel(FileInfo file)
        {
            _file = file ?? throw new ArgumentNullException(nameof(file));
        }

        public string DisplayName
        {
            get => Path.GetFileNameWithoutExtension(File.Name);
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && value != DisplayName)
                {
                    string newFileName = value + ".json";
                    // Use null-coalescing in case DirectoryName is null.
                    string newPath = Path.Combine(File.DirectoryName ?? "", newFileName);
                    try
                    {
                        File.MoveTo(newPath);
                        File = new FileInfo(newPath);
                        OnPropertyChanged(nameof(DisplayName));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error renaming file: {ex.Message}");
                    }
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }
    }
}
