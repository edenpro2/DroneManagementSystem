using PL.Windows;
using PL.Windows.Tracking;
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
            //var window = new MainWindow();
            var window = new DroneTrackingWindow(new DalFacade.DO.Drone(100, "DJI Avata"));
            window.Show();
        }
    }
}