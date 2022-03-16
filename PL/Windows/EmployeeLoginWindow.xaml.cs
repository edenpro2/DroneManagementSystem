using BLAPI;
using DalFacade.DO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PL.Windows
{
    public partial class EmployeeLoginWindow
    {
        private readonly BlApi _bl;
        private User _user;

        public EmployeeLoginWindow(BlApi ibl)
        {
            _bl = ibl;
            InitializeComponent();
            DataContext = this;
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameBox.Text;
            var password = PassBox.Password;

            if (username == "")
            {
                ErrorTextBlock.Text = "Please enter username";
            }
            else if (password == "")
            {
                ErrorTextBlock.Text = "Please enter password";
            }
            else
            {
                var user = _bl.GetUsers(u => u.active).FirstOrDefault(u => u.username == username);
                if (user.password == password)
                {
                    _user = user;
                    DialogResult = true;
                    Close();
                }
                else
                {
                    ErrorTextBlock.Text = "Incorrect username or password";
                }
            }

        }

        internal User GetValue()
        {
            return _user;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
