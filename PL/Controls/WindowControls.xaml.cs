using System.Windows;

namespace PL.Controls
{
    public partial class WindowControls
    {
        private static DependencyObject WindowObject;

        private static DependencyObject Window => (Window)WindowObject;

        public WindowControls(DependencyObject window)
        {
            WindowObject = window;
            InitializeComponent();
        }

        public WindowControls()
        {
            InitializeComponent();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            var window = (Window)Window;
            window.Close();
        }

        private void RestoreBtn_Click(object sender, RoutedEventArgs e)
        {
            var window = (Window)Window;
            window.WindowState = window.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            var window = (Window)Window;
            window.WindowState = WindowState.Minimized;
        }
    }
}