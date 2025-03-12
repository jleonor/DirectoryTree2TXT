// File: Views/MainWindow.xaml.cs
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DirectoryTreeViewer.ViewModels;

namespace DirectoryTreeViewer.Views
{
    public partial class MainWindow : Window
    {
        private bool isConfigVisible = true;

        public MainWindow()
        {
            InitializeComponent();
            // The DataContext is already set in the XAML.
        }

        // Title bar events (view-only logic)
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                ToggleMaximize();
            else
                DragMove();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void MaximizeButton_Click(object sender, RoutedEventArgs e) => ToggleMaximize();

        private void ToggleMaximize() =>
            WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();

        private void ToggleConfigButton_Click(object sender, RoutedEventArgs e)
        {
            if (isConfigVisible)
            {
                // Collapse the left panel by setting its column width to 0.
                MainContentGrid.ColumnDefinitions[0].Width = new GridLength(0);
                ToggleTextBlock.Text = "Expand Config Options";
                isConfigVisible = false;
            }
            else
            {
                // Restore the left panel to 300px.
                MainContentGrid.ColumnDefinitions[0].Width = new GridLength(300);
                ToggleTextBlock.Text = "Collapse Config Options";
                isConfigVisible = true;
            }
        }

        private void EditTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CommitEdit(sender as TextBox);
            }
        }

        private void EditTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // If focus is moving to the Rename button, let its handler commit instead.
            if (Keyboard.FocusedElement == RenameButton)
                return;

            CommitEdit(sender as TextBox);
        }

        private void CommitEdit(TextBox? textBox)
        {
            if (textBox is null)
                return;

            // Update the binding explicitly since UpdateSourceTrigger=Explicit is used
            var binding = textBox.GetBindingExpression(TextBox.TextProperty);
            binding?.UpdateSource();

            // Exit edit mode by setting IsEditing to false in the view model
            if (textBox.DataContext is ConfigurationFileViewModel config)
            {
                config.IsEditing = false;
            }
        }

        private void RenameButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConfigurationsListBox.SelectedItem is ConfigurationFileViewModel config)
            {
                if (config.IsEditing)
                {
                    // If already in edit mode, commit the changes.
                    if (ConfigurationsListBox.ItemContainerGenerator.ContainerFromItem(config) is ListBoxItem container)
                    {
                        var textBox = FindVisualChild<TextBox>(container);
                        if (textBox != null)
                        {
                            var binding = textBox.GetBindingExpression(TextBox.TextProperty);
                            binding?.UpdateSource();
                        }
                    }
                    config.IsEditing = false;
                }
                else
                {
                    // Begin editing and then auto-select text.
                    config.IsEditing = true;
                    this.Dispatcher.InvokeAsync(() =>
                    {
                        if (ConfigurationsListBox.ItemContainerGenerator.ContainerFromItem(config) is ListBoxItem container)
                        {
                            var textBox = FindVisualChild<TextBox>(container);
                            if (textBox != null)
                            {
                                textBox.Focus();
                                textBox.SelectAll();
                            }
                        }
                    });
                }
            }
        }


        private T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                    return typedChild;
                T? result = FindVisualChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }

    }
}
