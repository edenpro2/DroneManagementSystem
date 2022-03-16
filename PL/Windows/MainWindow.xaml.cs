#pragma warning disable CS8629  
using BL;
using BLAPI;
using DO;
using PL.Controls;
using PL.Pages;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PL.Windows
{
    public partial class MainWindow
    {
        private static readonly BlApi _bl = BlFactory.GetBl();
        private Image background { get; } = new();
        private readonly BackgroundWorker _backgroundWorker;
        private User _user;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            CustomButtons = new WindowControls(this);
            background.Source = new BitmapImage(new Uri("../Resources/Wallpaper.jpg", UriKind.Relative));
            _backgroundWorker = new BackgroundWorker();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            // Disable login and make create button a cancel.
            ChangeButtonState();

            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.DoWork += BackgroundWorker_DoWork;
            _backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            _backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;

            // Start the background worker.
            _backgroundWorker.RunWorkerAsync();

        }

        private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            _backgroundWorker.ReportProgress(1);
            Dispatcher.Invoke(() => _user = _bl.GetUsers(u => u.username == UsernameBox.Text).FirstOrDefault());
        }

        private void BackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e) => Gif.Visibility = Visibility.Visible;

        private void BackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e) => Task.Delay(2000).ContinueWith(_ => { Dispatcher.Invoke(Login); });

        private void Login()
        {
            if (_user.username == null)
            {
                ErrorTextBlock.Text = "User not found. If you are new, try registering";
                ChangeButtonState();
            }
            else if (_user.password == null || !_user.password.Equals(PassBox.Password))
            {
                ErrorTextBlock.Text = "Wrong username or password";
                ChangeButtonState();
            }
            else
            {
                EnableCustomerUi(_user);
            }
        }

        private void ChangeButtonState()
        {
            if (LoginButton.IsEnabled)
            {
                LoginButton.IsEnabled = false;
                Gif.Visibility = Visibility.Visible;
                LoginButton.Content = "";
            }
            else
            {
                Gif.Visibility = Visibility.Hidden;
                LoginButton.Content = "Login";
                LoginButton.IsEnabled = true;
            }
        }

        private void EmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            var empLoginWindow = new EmployeeLoginWindow(_bl);

            if ((bool)empLoginWindow.ShowDialog())
            {
                EnableEmployeeUi(empLoginWindow.GetValue());
            }
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            ErrorTextBlock.Text = "";
            RegistrationWindow regWindow = new(_bl);
            regWindow.ShowDialog();
        }

        private void DevBtn_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Not required by project
        }

        private void EnableCustomerUi(User user)
        {
            var userInterface = new CustomerUserInterfacePage(_bl, user, this);
            SetPageTo(userInterface);
        }

        private void EnableEmployeeUi(User user)
        {
            var employeeInterface = new EmployeeUserInterface(_bl, user, this);
            SetPageTo(employeeInterface);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) => DragMove();

        private void SetPageTo(FrameworkElement page)
        {
            Height = page.Height;
            Width = page.Width;
            Content = page;
        }


    }
}