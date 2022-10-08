using PL.Windows;
using System.Windows;

namespace PL
{
    public partial class App
    {
        public App()
        {
            Startup += App_Startup;
        }

        private static void App_Startup(object sender, StartupEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
        }
    }
}