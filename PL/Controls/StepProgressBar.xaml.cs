using DalFacade.DO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace PL.Controls
{
    /// <summary>
    /// User-control shows the current progress of the drone/parcel
    /// </summary>
    public partial class StepProgressBar : INotifyPropertyChanged
    {
        public SolidColorBrush CompletedFill { get; } = new(Color.FromRgb(85, 201, 112));

        // Default Constructor 
        public StepProgressBar()
        {
            InitializeComponent();
        }

        public Parcel ViewModel
        {
            get => (Parcel)GetValue(ViewModelProperty);
            set
            {
                SetValue(ViewModelProperty, value);
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(Parcel), typeof(StepProgressBar));

        public SolidColorBrush Fill
        {
            get => (SolidColorBrush)GetValue(FillProperty);
            set
            {
                SetValue(FillProperty, value);
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register(nameof(Fill), typeof(SolidColorBrush), typeof(StepProgressBar));


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    //public class DateProgressConverter : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if ((DateTime)values[1] != default)
    //            return new SolidColorBrush(Color.FromRgb(97, 181, 94)); //green

    //        return (DateTime)values[0] != default ? new SolidColorBrush(Color.FromRgb(219, 81, 57)) : new SolidColorBrush(Color.FromRgb(204, 204, 204)); // gray
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotSupportedException();
    //    }
    //}
}