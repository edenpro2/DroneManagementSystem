using System.Windows;

namespace PL.Controls
{
    public partial class WindowControls
    {
        private static DependencyObject _windowObject = new();

        private static Window Window => (Window)_windowObject;

        private static DependencyObject _prevWindow = new();

        public WindowControls(DependencyObject window)
        {
            _prevWindow = _windowObject;
            _windowObject = window;
            InitializeComponent();
        }

        public WindowControls()
        {
            InitializeComponent();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            var window = Window;
            window.Close();
            _windowObject = _prevWindow;
        }

        private void RestoreBtn_Click(object sender, RoutedEventArgs e)
        {
            var window = Window;
            window.WindowState = (window.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            var window = Window;
            window.WindowState = WindowState.Minimized;
        }
    }
}