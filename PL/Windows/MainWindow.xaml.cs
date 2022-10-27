using System;
using BL;
using DalFacade.DO;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace PL.Windows
{
    public partial class MainWindow
    {
        #region Properties
        public static double MinScreenWidth => PLMethods.MinScreenWidth(0.9);
        public static double MinScreenHeight => PLMethods.MinScreenHeight(0.9);
        private static readonly BlApi Bl = BlFactory.GetBl();
        private static User? _user;
        private BackgroundWorker? _altThread;
        public string ErrorMessage { get; private set; } = "";
        #endregion

        public MainWindow() => InitializeComponent();

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {

            // Disable login 
            ChangeButtonState();
            
            _altThread = new BackgroundWorker();
            _altThread.WorkerSupportsCancellation = true;
            _altThread.DoWork += BackgroundWorker_DoWork;
            _altThread.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            _altThread.RunWorkerAsync();
        }

        private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(() => _user = Bl.GetUsers(u => u.Username == UsernameBox.Text).FirstOrDefault());
        }

        private void BackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            Task.Delay(1500).ContinueWith(_ => { Dispatcher.Invoke(Login); });
            _altThread?.CancelAsync();
        }

        private void Login()
        {
            ChangeButtonState();

            if (_user?.Username == null)
            {
                ErrorMessage = "User not found. If you are new, try registering";
                return;
            }

            if (_user.Password == null || !_user.Password.Equals(PassBox.Password))
            {
                ErrorMessage = "Wrong username or password";
                return;
            }

            new CustomerUi(Bl, _user).Show();
            Close();
        }

        private void EmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            var empLoginWindow = new EmployeeLoginWindow(Bl);

            empLoginWindow.ShowDialog();

            _user = empLoginWindow.GetValue();

            if (_user == null)
                return;

            new EmployeeUi(Bl, _user).Show();
            Close();
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage = "";
            new RegistrationWindow(Bl).ShowDialog();
        }
        private void DevBtn_Click(object sender, RoutedEventArgs e) { }
        private void ChangeButtonState() => LoginButton.IsEnabled = !LoginButton.IsEnabled;
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) => DragMove();

    }
}