using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DirectoryTreeViewer.Controls
{
    public partial class CustomTitleBar : UserControl
    {
        public CustomTitleBar()
        {
            InitializeComponent();
        }

        // DependencyProperty so you can specify the application name from XAML.
        public static readonly DependencyProperty ApplicationTitleProperty =
            DependencyProperty.Register("ApplicationTitle", typeof(string), typeof(CustomTitleBar), new PropertyMetadata("Application"));

        public string ApplicationTitle
        {
            get { return (string)GetValue(ApplicationTitleProperty); }
            set { SetValue(ApplicationTitleProperty, value); }
        }

        // When the title bar is clicked and dragged, restore the window (if maximized) and start dragging.
        private void CustomTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window window = Window.GetWindow(this);
            if (window != null)
            {
                if (window.WindowState == WindowState.Maximized)
                {
                    // Restore to normal window state before dragging.
                    window.WindowState = WindowState.Normal;
                    ToggleWindowButton.Tag = "□"; // update to the icon representing normal state
                }
                window.DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            if (window != null)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        private void ToggleWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            if (window != null)
            {
                if (window.WindowState == WindowState.Maximized)
                {
                    window.WindowState = WindowState.Normal;
                    ToggleWindowButton.Tag = "□"; // Icon indicating maximization is available
                }
                else
                {
                    window.WindowState = WindowState.Maximized;
                    ToggleWindowButton.Tag = "❐"; // Icon indicating restore is available
                }
            }
        }



        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
