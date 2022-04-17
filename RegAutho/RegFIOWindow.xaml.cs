using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.DAL;
using WpfApp1.Main;

namespace WpfApp1.RegAtho
{
    public partial class RegFIOWindow : Window
    {
        private Model db = new Model();
        private User user;
        private string urlPhoto;

        public RegFIOWindow(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void AddPhoto_Click(object sender, RoutedEventArgs e)
        {
            string filter = "Image files ( *.PNG )|*.bmp;*.jpg;*.gif; *.tif; *.png; *.ico; *.emf; *.wmf";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;
            openFileDialog.ShowDialog();
            urlPhoto = openFileDialog.FileName;
            UrlLab.Content = "Фото добавлено";
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTB.Text;
            string lastName = LastNameTB.Text;
            string patronymic = PatronymicTB.Text;
            string age = AgeTB.Text;

            int ageParse;
            bool checkAge = int.TryParse(age, out ageParse);

            if (checkAge && firstName != "" && lastName != "" && patronymic != "" && urlPhoto != "")
            {
                db.Users.Add(this.user);
                db.SaveChanges();
                User user = db.Users.First(item => item.Login == this.user.Login);
                Teacher teacher = new Teacher();
                teacher.LastName = lastName;
                teacher.FirstName = firstName;
                teacher.Patronymic = patronymic;
                teacher.Age = ageParse;
                if(urlPhoto != "" && urlPhoto != null)
                {
                    teacher.Photo = File.ReadAllBytes(urlPhoto);
                    teacher.IdUser = user.Id;
                    db.Teachers.Add(teacher);
                    db.SaveChanges();
                    Window window = new NavigationWindow(user, teacher);
                    window.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Выберите все поля");
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля и выберите фото");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Window window = new RegWindow();
            window.Show();
            Close();
        }
    }
}
