using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Windows;
using WpfApp1.DAL;
using WpfApp1.Main;

namespace WpfApp1.RegAtho
{
    public partial class AuthoWindow : Window
    {
        private Model db = new Model();
        private int countError;

        public AuthoWindow()
        {
            InitializeComponent();
        }

        private void Autho_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTB.Text;
            string pass = PassTB.Text;
            Random rand = new Random();
            int num1 = rand.Next(0, 20);
            int num2 = rand.Next(0, 20);

            if (countError >= 3)
            {
                string result = Interaction.InputBox($"Введите разность чисел {num1} - {num2}", "CAPTHA", "", -1, -1);
                int resultInt;
                bool check = int.TryParse(result, out resultInt);

                if (check)
                {
                    if ((num1 - num2) == resultInt)
                    {
                        ToLogin(login, pass);
                    }
                    else
                    {
                        MessageBox.Show("Неверно");
                    }
                }
            }
            else
            {
                ToLogin(login, pass);
            }
        }

        private void ToLogin(string login, string pass)
        {
            if (login != "" && pass != "")
            {
                var user = db.Users.FirstOrDefault(item => item.Login == login && item.Password == pass);
                if (user != null)
                {
                    var teacher = db.Teachers.FirstOrDefault(item => item.IdUser == user.Id);
                    Window window = new NavigationWindow(user, teacher);
                    window.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Логин или пароль не верны");
                    countError++;
                }
            }
            else
            {
                MessageBox.Show("Введите данные полностью");
                countError++;
            }

        }

        private void ToReg_Click(object sender, RoutedEventArgs e)
        {
            Window window = new RegWindow();
            window.Show();
            Close();
        }
    }
}
