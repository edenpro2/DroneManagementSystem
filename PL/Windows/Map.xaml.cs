using BLAPI;
using PL.Controls;
using System;
using System.IO;
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
            CustomButtons = new WindowControls(this);

            var parentDir1 = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.Parent;
            var parentDir2 = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent;

            string resources;
            string icons;

            if (File.Exists($"{parentDir1}\\Resources"))
            {
                resources = parentDir1 + "\\Resources\\";
                icons = parentDir1 + "\\Icons\\";
            }

            resources = parentDir2 + "\\Resources\\";
            icons = parentDir2 + "\\Icons\\";

            switch (type.ToLower())
            {
                case "drone":
                    var drones = ibl.GetDrones();
                    foreach (var drone in drones)
                    {
                        var icon = new Image
                        {
                            Source = new BitmapImage(new Uri($"{resources}drone.png")),
                            Width = 20,
                            Height = 20
                        };

                        AddIcon(icon, drone);
                    }
                    break;
                case "station":
                    var stations = ibl.GetStations();
                    foreach (var station in stations)
                    {
                        var icon = new Image
                        {
                            Source = new BitmapImage(new Uri($"{resources}warehouse3d.png")),
                            Width = 30,
                            Height = 30
                        };

                        AddIcon(icon, station);
                    }
                    break;
                case "customer":
                    var customers = ibl.GetCustomers();
                    foreach (var customer in customers)
                    {
                        var icon = new Image
                        {
                            Source = new BitmapImage(new Uri($"{resources}account.jpg")),
                            Width = 30,
                            Height = 30
                        };

                        AddIcon(icon, customer);
                    }
                    break;
                case "parcel":
                    var parcels = ibl.GetParcels();
                    foreach (var parcel in parcels)
                    {
                        var icon = new Image
                        {
                            Source = new BitmapImage(new Uri($"{icons}package.png")),
                            Width = 30,
                            Height = 30
                        };

                        AddIcon(icon, parcel);
                    }
                    break;
            }
        }

        private void AddIcon(UIElement icon, object o)
        {
            CanvasMap.Children.Add(icon);

            var loc = _bl.Location(o);
            var latitude = loc.latitude;
            var longitude = loc.longitude;

            var width = pixelX(longitude, MapScrollView.Width);
            var height = pixelY(latitude, MapScrollView.Height);

            Canvas.SetLeft(icon, width);
            Canvas.SetTop(icon, height);
        }

        private double pixelX(double targetLong, double width)
        {
            const double minLong = -87.852252;
            const double maxLong = -79.182916;

            var a = targetLong - minLong;
            var b = maxLong - minLong;

            return ((a) / (b)) * (width - 1);
        }

        private double pixelY(double targetLat, double height)
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
