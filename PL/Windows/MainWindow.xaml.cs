#pragma warning disable CS8629  
using BL;
using BLAPI;
using DalFacade.DO;
using PL.Controls;
using PL.Windows.Tracking;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PL.Windows
{
    public partial class MainWindow
    {
        private static readonly BlApi bl = BlFactory.GetBl();
        private BackgroundWorker? _bgThread;
        private static User? _user;
        public static double MinScreenHeight => PLMethods.MinScreenHeight(0.9);
        public static double MinScreenWidth => PLMethods.MinScreenWidth(0.9);

        public MainWindow()
        {
            InitializeComponent();
            CustomButtons = new WindowControls(this);
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            _bgThread = new BackgroundWorker();

            // Disable login 
            ChangeButtonState();
            _bgThread.WorkerReportsProgress = true;
            _bgThread.WorkerSupportsCancellation = true;
            _bgThread.DoWork += BackgroundWorker_DoWork;
            _bgThread.ProgressChanged += BackgroundWorker_ProgressChanged;
            _bgThread.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;

            // Start the background worker.
            _bgThread.RunWorkerAsync();
        }

        private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            _bgThread?.ReportProgress(1);
            Dispatcher.Invoke(() => _user = bl.GetUsers(u => u.username == UsernameBox.Text).FirstOrDefault());
        }

        private void BackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            Gif.Visibility = Visibility.Visible;
        }

        private void BackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            Task.Delay(1500).ContinueWith(_ => { Dispatcher.Invoke(Login); });
            _bgThread?.CancelAsync();
        }

        private void Login()
        {
            if (_user == null || _user.username == null)
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
                new CustomerUi(bl, _user).Show();
                Close();
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
            var empLoginWindow = new EmployeeLoginWindow(bl);

            if (!(bool)empLoginWindow.ShowDialog())
                return;

            _user = empLoginWindow.GetValue();

            if (_user == null)
                return;

            new EmployeeUi(bl, _user).Show();
            Close();
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            ErrorTextBlock.Text = "";
            RegistrationWindow regWindow = new(bl);
            regWindow.ShowDialog();
        }

        private void DevBtn_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Expansion
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) => DragMove();
    }
}