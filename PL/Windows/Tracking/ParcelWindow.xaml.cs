﻿using System;
using System.Windows.Input;
using BLAPI;
using DalFacade.DO;
using PL.Controls;
using PL.ViewModels;

namespace PL.Windows.Tracking
{
    public partial class ParcelWindow
    {
        public ParcelViewModel ViewModel { get; }
        public MapViewModel MapUri { get; } = new();

        public ParcelWindow(BlApi bl, ParcelViewModel pvm)
        {
            ViewModel = pvm;
            InitializeComponent();
            DataContext = this;
            CustomButtons = new WindowControls(this);
            var loc = bl.Location(bl.SearchForDrone(d => pvm.Id == d.id));
            MapUri.Uri = NewMapUri(new Location(loc.latitude, loc.longitude));
        }

        private static Uri NewMapUri(Location location)
        {
            var lat = location.latitude - location.latitude % 0.0001;
            var lon = location.longitude - location.longitude % 0.0001;
            return new Uri($"https://www.openstreetmap.org/?mlat={lat}&amp;mlon={lon}#map=10/{lat}/{lon}&amp;layers=N");
        }

        private void Window_MouseLeftBtnDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}