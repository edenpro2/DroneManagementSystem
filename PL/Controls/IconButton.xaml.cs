using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PL.Controls
{
    public partial class IconButton : INotifyPropertyChanged
    {
        public IconButton() => InitializeComponent();

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            IsChecked = !IsChecked;
            Click?.Invoke(this, e);
        }

        public static readonly DependencyProperty IconSourceProperty =
            DependencyProperty.Register(nameof(IconSource), typeof(string), typeof(IconButton));

        public string IconSource
        {
            get => (string)GetValue(IconSourceProperty);
            set
            {
                SetValue(IconSourceProperty, value);
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(IconButton));

        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set
            {
                SetValue(IsCheckedProperty, value);
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(IconButton));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set
            {
                SetValue(TitleProperty, value);
                OnPropertyChanged();
            }
        }

        public event RoutedEventHandler? Click;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Btn_OnMouseEnter(object sender, MouseEventArgs e)
        {
            var border = (Border)Btn.Template.FindName("Border", Btn);

            border.Background = new SolidColorBrush(Color.FromArgb(255, 171, 218, 255));
            border.BorderBrush = new SolidColorBrush(Color.FromRgb(48, 131, 255));
        }

        private void Btn_OnMouseLeave(object sender, MouseEventArgs e)
        {
            var border = (Border)Btn.Template.FindName("Border", Btn);

            border.Background = new SolidColorBrush(Colors.White);
            border.BorderBrush = new SolidColorBrush(Colors.White);
        }
    }
}


