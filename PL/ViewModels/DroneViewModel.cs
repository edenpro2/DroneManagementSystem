using DalFacade.DO;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using static DalFacade.DO.DegreeConverter;

namespace PL.ViewModels
{
    public class DroneViewModel : INotifyPropertyChanged
    {
        private string _dms;
        public string Dms => _dms;

        private Drone _drone;

        public Drone Drone
        {
            get => _drone;
            set
            {
                _drone = value;
                if (_drone.location is not null)
                {
                    var loc = (Location)_drone.location;
                    _dms = CoordinatesToSexagesimal(loc.longitude, loc.latitude);
                }
                OnPropertyChanged();
            }
        }

        private string _modelImg;

        public string ModelImg
        {
            get => _modelImg;
            set
            {
                _modelImg = value;
                OnPropertyChanged();
            }
        }

        public DroneViewModel(Drone drone)
        {
            _drone = drone;
            _dms = "";
            var resources1 = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + @"\Resources\Models\";
            var resources2 = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Resources\Models\";
            string dir;

            if (Directory.Exists(resources1))
                dir = resources1;
            else dir = resources2;

            var fileinfo = new DirectoryInfo(dir)
                .GetFiles()
                .ToArray()
                .FirstOrDefault(file => file.Name.Contains(drone.model))
                .ToString();

            _modelImg = Path.GetFileNameWithoutExtension(fileinfo);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}