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
                    ToggleEllipse.Fill = new SolidColorBrush(Colors.Green);
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
                    ToggleEllipse.Fill = new SolidColorBrush(Colors.Green); // Change to indicate "normal" mode
                }
                else
                {
                    window.WindowState = WindowState.Maximized;
                    ToggleEllipse.Fill = new SolidColorBrush(Colors.Orange); // Change to indicate "maximized" mode
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
