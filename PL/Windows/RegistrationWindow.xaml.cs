using BL;
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
        public string ErrorMessage { get; private set; } = "";

        public RegistrationWindow(BlApi database)
        {
            _bl = database;
            InitializeComponent();
        }

        // When someone registers, the function will check if user exists. If not, it will create a new user
        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            // check if user is already in the system
            var user = _bl.GetUsers().FirstOrDefault(u => u.Username == UserBox.Text);

            if (user != default)
            {
                ErrorMessage = "User already exists";
                return;
            }

            // now check if email is valid
            if (CheckEmail(EmailBox.Text) == false)
            {
                ErrorMessage = "Email not valid";
                return;
            }

            // search for customer with s
            var customer = _bl.GetCustomers().FirstOrDefault(c => c.Phone == PhoneBox.Text && c.Name == UserBox.Text);

            // if customer doesn't exist, create a new one
            if (customer == default)
            {
                _bl.CreateCustomer(NameBox.Text, PhoneBox.Text);
                customer = _bl.GetCustomers().First(c => c.Phone == PhoneBox.Text && c.Name == NameBox.Text);
            }

            _bl.AddUser(new User(ref customer, UserBox.Text, PassBox.Password, EmailBox.Text, AddressBox.Text));
            Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) => DragMove();

        private void returnButton_Click(object sender, RoutedEventArgs e) => Close();

        private void OnEnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                continueButton_Click(sender, e);
            }
        }
    }
}