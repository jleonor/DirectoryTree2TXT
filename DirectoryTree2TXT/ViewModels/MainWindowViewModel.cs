using DirectoryTreeViewer.Helpers;
using DirectoryTreeViewer.Models;
using DirectoryTreeViewer.Services;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DirectoryTreeViewer.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private string _directoryPath = string.Empty;
        private string _treeOutput = string.Empty;
        private bool _showContents;
        private bool _showOnlyFolders;
        private string _newEntryName = string.Empty;
        private ConfigurationFileViewModel? _selectedConfiguration; // Backing field added

        public ObservableCollection<ConfigurationFileViewModel> Configurations { get; set; }
        public ObservableCollection<ExclusionEntry> ExclusionEntries { get; set; }

        public string DirectoryPath
        {
            get => _directoryPath;
            set { _directoryPath = value; OnPropertyChanged(); }
        }

        public string TreeOutput
        {
            get => _treeOutput;
            set { _treeOutput = value; OnPropertyChanged(); }
        }

        public bool ShowContents
        {
            get => _showContents;
            set { _showContents = value; OnPropertyChanged(); }
        }

        public bool ShowOnlyFolders
        {
            get => _showOnlyFolders;
            set { _showOnlyFolders = value; OnPropertyChanged(); }
        }

        public string NewEntryName
        {
            get => _newEntryName;
            set { _newEntryName = value; OnPropertyChanged(); }
        }

        public ConfigurationFileViewModel? SelectedConfiguration
        {
            get => _selectedConfiguration;
            set
            {
                _selectedConfiguration = value;
                OnPropertyChanged();
                if (_selectedConfiguration != null)
                {
                    var config = _configurationService.LoadConfiguration(_selectedConfiguration.File);
                    if (config != null)
                    {
                        DirectoryPath = config.DirectoryPath;
                        ShowContents = config.ShowContents;
                        ExclusionEntries.Clear();
                        foreach (var entry in config.ExclusionEntries)
                        {
                            ExclusionEntries.Add(entry);
                        }
                    }
                }
            }
        }

        public ICommand BrowseCommand { get; }
        public ICommand GenerateTreeCommand { get; }
        public ICommand AddEntryCommand { get; }
        public ICommand RemoveEntryCommand { get; }
        public ICommand ClearEntriesCommand { get; }
        public ICommand SaveConfigCommand { get; }
        public ICommand DeleteConfigCommand { get; }
        public ICommand RenameConfigCommand { get; }

        private readonly DirectoryTreeGenerator _directoryTreeGenerator;
        private readonly ConfigurationService _configurationService;

        public MainWindowViewModel()
        {
            ExclusionEntries = new ObservableCollection<ExclusionEntry>();
            Configurations = new ObservableCollection<ConfigurationFileViewModel>();
            _directoryTreeGenerator = new DirectoryTreeGenerator();
            _configurationService = new ConfigurationService();

            BrowseCommand = new RelayCommand(param => BrowseDirectory());
            GenerateTreeCommand = new RelayCommand(param => GenerateTree());
            AddEntryCommand = new RelayCommand(param => AddEntry());
            RemoveEntryCommand = new RelayCommand(param => RemoveEntry(param as ExclusionEntry));
            ClearEntriesCommand = new RelayCommand(param => ClearEntries());
            SaveConfigCommand = new RelayCommand(param => SaveConfiguration());
            DeleteConfigCommand = new RelayCommand(param => DeleteConfiguration(param as ConfigurationFileViewModel));
            RenameConfigCommand = new RelayCommand(param => RenameConfiguration(param as ConfigurationFileViewModel));

            LoadConfigurations();
        }

        private void BrowseDirectory()
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "Select a Directory"
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                DirectoryPath = dialog.FileName;
            }
        }

        private void GenerateTree()
        {
            if (Directory.Exists(DirectoryPath))
            {
                var exclusions = ExclusionEntries?.ToList() ?? new List<ExclusionEntry>(); // Fix null issue
                TreeOutput = _directoryTreeGenerator.GenerateTree(DirectoryPath, "", true, exclusions, ShowContents, ShowOnlyFolders);
            }
            else
            {
                MessageBox.Show("The specified directory does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddEntry()
        {
            if (!string.IsNullOrWhiteSpace(NewEntryName))
            {
                ExclusionEntries.Add(new ExclusionEntry
                {
                    Name = NewEntryName,
                    Ignore = true,
                    Hide = true
                });

                NewEntryName = string.Empty;
            }
        }


        private void RemoveEntry(ExclusionEntry? entry)
        {
            if (entry != null)
            {
                ExclusionEntries.Remove(entry);
            }
        }

        private void ClearEntries()
        {
            ExclusionEntries.Clear();
        }

        private void SaveConfiguration()
        {
            var config = new SearchConfiguration
            {
                DirectoryPath = DirectoryPath,
                ExclusionEntries = ExclusionEntries.ToList(),
                ShowContents = ShowContents
            };

            _configurationService.SaveConfiguration(config);
            LoadConfigurations();
        }

        private void DeleteConfiguration(ConfigurationFileViewModel? configVM)
        {
            if (configVM != null)
            {
                _configurationService.DeleteConfiguration(configVM.File);
                LoadConfigurations();
            }
        }

        private void RenameConfiguration(ConfigurationFileViewModel? configVM)
        {
            if (configVM != null)
            {
                configVM.IsEditing = true;
            }
        }


        private void LoadConfigurations()
        {
            Configurations.Clear();
            var configFiles = _configurationService.LoadConfigurations();
            foreach (var file in configFiles)
            {
                Configurations.Add(new ConfigurationFileViewModel(file));
            }
        }
    }
}
