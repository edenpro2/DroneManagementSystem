using BLAPI;
using DalFacade.DO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using static BL.BO.UserVerification;

namespace PL.Windows
{
    public partial class RegistrationWindow
    {
        private readonly BlApi _bl;

        public RegistrationWindow(BlApi database)
        {
            _bl = database;
            InitializeComponent();
        }

        /// <summary>
        /// When someone registers, the function will check if user exists. If not, it will create a new user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            // check if user is already in the system
            var user = _bl.GetUsers().FirstOrDefault(u => u.username == UserBox.Text);

            if (user.username != default)
            {
                ErrorBox.Text = "User already exists";
                return;
            }

            // now check if email is valid
            if (CheckEmail(EmailBox.Text) == false)
            {
                ErrorBox.Text = "Email not valid";
                return;
            }

            // search for customer with s
            var customer = _bl.GetCustomers().FirstOrDefault(c => c.phone == PhoneBox.Text && c.name == UserBox.Text);

            // if customer doesn't exist, create a new one
            if (customer.phone == default)
            {
                _bl.CreateCustomer(NameBox.Text, PhoneBox.Text);
            }

            customer = _bl.GetCustomers().FirstOrDefault(c => c.phone == PhoneBox.Text && c.name == UserBox.Text);
            user = new User(customer.id, UserBox.Text, PassBox.Password, EmailBox.Text, AddressBox.Text);

            _bl.AddUser(user);
            Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnEnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                continueButton_Click(sender, e);
            }
        }
    }
}