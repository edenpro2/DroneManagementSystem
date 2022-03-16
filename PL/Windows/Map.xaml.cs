using BLAPI;
using PL.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DO;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;

namespace PL.Windows
{
    public partial class Map
    {
        private readonly BlApi bl;
        public Map(BlApi ibl, string type)
        {
            bl = ibl;
            InitializeComponent();
            Height = 1000;
            Width = 1300;
            CustomButtons = new WindowControls(this);

            switch (type.ToLower())
            {
                case "drone":
                    var drones = ibl.GetDrones();
                    foreach (var drone in drones)
                    {
                        var icon = new Image
                        {
                            Source = new BitmapImage(new Uri(@"C:\Users\Eden\source\repos\CS_Project_5782_8318\PL\Resources\drone.png")),
                            Width = 30,
                            Height = 30
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
                            Source = new BitmapImage(new Uri(@"C:\Users\Eden\source\repos\CS_Project_5782_8318\PL\Resources\warehouse3d.png")),
                            Width = 30,
                            Height = 15
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
                            Source = new BitmapImage(new Uri(@"C:\Users\Eden\source\repos\CS_Project_5782_8318\PL\Resources\account.jpg")),
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
                            Source = new BitmapImage(new Uri(@"C:\Users\Eden\source\repos\CS_Project_5782_8318\PL\Icons\package.png")),
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

            var loc = bl.Location(o);
            var latitude = loc.latitude;
            var longitude = loc.longitude;

            var width = pixelX(longitude, MapGrid.Width);
            var height = pixelY(latitude, MapGrid.Height);

            Canvas.SetLeft(icon, width);
            Canvas.SetTop(icon, height);
        }

        private double pixelX(double targetLong, double Width)
        {
            const double minLong = -87.852252;
            const double maxLong = -79.182916;

            var a = targetLong - minLong;
            var b = maxLong - minLong;

            return ((a) / (b)) * (Width - 1);
        }

        private double pixelY(double targetLat, double Height)
        {
            const double minLat = 31.089027;
            const double maxLat = 24.614693;

            return ((targetLat - minLat) / (maxLat - minLat)) * (Height - 1);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) => DragMove();

        Point GetMousePos() => PointToScreen(Mouse.GetPosition(this));

        private void Map_OnScrollWheel(object sender, MouseWheelEventArgs e)
        {


        }


    }
}
