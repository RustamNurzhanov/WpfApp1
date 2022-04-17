using System.Linq;
using System.Windows;
using WpfApp1.DAL;

namespace WpfApp1.RegAtho
{
    public partial class RegWindow : Window
    {
        private Model db = new Model();

        public RegWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTB.Text;
            string pass = PassTB.Text;
            string checkPass = CheckPassTB.Text;

            if (login != "" && pass != "" && checkPass != "")
            {
                if (pass == checkPass)
                {
                    bool checkLogin = db.Users.Any(item => item.Login == login);
                    if (!checkLogin)
                    {
                        User user = new User();
                        user.Login = login;
                        user.Password = pass;
                        Window window = new RegFIOWindow(user);
                        window.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Логин занят");
                    }
                }
                else
                {
                    MessageBox.Show("Пароли не совпадают");
                }
            }
            else
            {
                MessageBox.Show("Введите данные полностью");
            }
        }

        private void ToAutho_Click(object sender, RoutedEventArgs e)
        {
            Window window = new AuthoWindow();
            window.Show();
            Close();
        }
    }
}
