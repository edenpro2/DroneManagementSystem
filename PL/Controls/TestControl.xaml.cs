using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DalFacade.DO;

namespace PL.Controls
{
    public partial class TestControl : UserControl
    {
        public TestControl() => InitializeComponent();

        public TestControl(Parcel parcel)
        {
            ViewModel = parcel;
            Solid = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff33df");
            InitializeComponent();

            if (parcel.Requested != default)
                Solid = Brushes.Green;
        }

        public Parcel ViewModel
        {
            get => (Parcel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(Parcel), typeof(TestControl),
                new PropertyMetadata(ViewModelPropertyChangedCallBack));

        private static void ViewModelPropertyChangedCallBack(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            var tc = (TestControl)sender;
            tc.ViewModel = (Parcel)e.NewValue;
        }

        public SolidColorBrush Solid
        {
            get => (SolidColorBrush)GetValue(SolidProperty);
            set => SetValue(SolidProperty, value);
        }

        public static readonly DependencyProperty SolidProperty =
            DependencyProperty.Register(nameof(Solid), typeof(SolidColorBrush), typeof(TestControl),
                new PropertyMetadata(SolidPropertyChangedCallBack));

        private static void SolidPropertyChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var tc = (TestControl)sender;
            tc.Solid = (SolidColorBrush)e.NewValue;
        }
    }
}