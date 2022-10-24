using BL;
using DalFacade;
using DalFacade.DO;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace PL.Windows
{
    public partial class Map
    {
        private readonly BlApi _bl;
        public Map(BlApi ibl, string type)
        {
            InitializeComponent();
            _bl = ibl;
            Height = 1000;
            Width = 1300;

            var resources = FileReader.GetFolderPath("\\Resources");
            var icons = FileReader.GetFolderPath("\\Icons");
            

            switch (type)
            {
                case nameof(Drone):
                    var drones = ibl.GetDrones();
                    foreach (var drone in drones)
                    {
                        var icon = new Image
                        {
                            Width = 20,
                            Height = 20,
                            Source = new BitmapImage(new Uri($"{resources}\\drone.png"))
                        };
                        AddIcon(icon, drone);
                    }
                    break;
                case nameof(Station):
                    var stations = ibl.GetStations();
                    foreach (var station in stations)
                    {
                        var icon = new Image
                        {
                            Width = 20,
                            Height = 20,
                            Source = new BitmapImage(new Uri($"{resources}\\warehouse3d.png"))
                        };
                        AddIcon(icon, station);
                    }
                    break;
                case nameof(Customer):
                    var customers = ibl.GetCustomers();
                    foreach (var customer in customers)
                    {
                        var icon = new Image
                        {
                            Width = 20,
                            Height = 20,
                            Source = new BitmapImage(new Uri($"{resources}\\account.jpg"))
                        };
                        AddIcon(icon, customer);
                    }
                    break;
                case nameof(Parcel):
                    var parcels = ibl.GetParcels();
                    foreach (var parcel in parcels)
                    {
                        var icon = new Image
                        {
                            Width = 20,
                            Height = 20,
                            Source = new BitmapImage(new Uri($"{icons}\\package.png"))
                        };
                        AddIcon(icon, parcel);
                    }
                    break;
            }
        }

        private void AddIcon(UIElement icon, object o)
        {
            CanvasMap.Children.Add(icon);

            var loc = _bl.LocationOf(o);
            var latitude = loc.Latitude;
            var longitude = loc.Longitude;

            var width = PixelX(longitude, MapScrollView.Width);
            var height = PixelY(latitude, MapScrollView.Height);

            Canvas.SetLeft(icon, width);
            Canvas.SetTop(icon, height);
        }

        private static double PixelX(double targetLong, double width)
        {
            const double minLong = -87.852252;
            const double maxLong = -79.182916;

            var a = targetLong - minLong;
            const double b = maxLong - minLong;

            return ((a) / (b)) * (width - 1);
        }

        private static double PixelY(double targetLat, double height)
        {
            const double minLat = 31.089027;
            const double maxLat = 24.614693;

            return ((targetLat - minLat) / (maxLat - minLat)) * (height - 1);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            DragMove();
        }

        //Point GetMousePos()
        //{
        //    return PointToScreen(Mouse.GetPosition(this));
        //}

        private void Map_OnScrollWheel(object sender, MouseWheelEventArgs e)
        {
            //no-op
        }


    }
}
