using PL.Controls;
using System.Windows;
using System.Windows.Input;
using PL.ViewModels;
using DalFacade.DO;

namespace PL.Windows.Tracking
{
    public partial class DroneTrackingWindow : Window
    {
        public static double MinScreenHeight => PLMethods.MinScreenHeight(0.9);
        public static double MinScreenWidth => PLMethods.MinScreenWidth(0.9);
        public DroneViewModel DroneViewModel { get; }
        public DroneTrackingWindow(Drone drone)
        {
            DroneViewModel = new DroneViewModel(drone);
            InitializeComponent();
            CustomButtons = new WindowControls(this);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) => DragMove();
    }
}
