using DalFacade.DO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using DalFacade;
using static DalFacade.DO.DegreeConverter;

namespace PL.ViewModels
{
    public class DroneViewModel : INotifyPropertyChanged
    {
        public string Dms { get; private set; }

        private Drone _drone;

        public Drone Drone
        {
            get => _drone;
            set
            {
                _drone = value;
                if (_drone.location != null)
                {
                    var loc = (Location)_drone.location;
                    Dms = CoordinatesToSexagesimal(loc.longitude, loc.latitude);
                }
                OnPropertyChanged();
            }
        }

        public string ModelImg { get; }

        public DroneViewModel(Drone drone)
        {
            _drone = drone;
            Dms = "";
            ModelImg = FileReader.GetFilePath(drone.model, new List<string> {".png", ".jpg"});
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}