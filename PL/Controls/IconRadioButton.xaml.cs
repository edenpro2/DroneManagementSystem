using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace PL.Controls
{
    public partial class IconRadioButton
    {
        public string IconSource { get; set; } = "...\\Icons\\settings.png";

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(IconRadioButton));

        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set
            {
                SetValue(IsCheckedProperty, value);
                OnPropertyChanged("IsCheckedProperty");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }


}


