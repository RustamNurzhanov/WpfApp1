using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfApp1.DAL;

namespace WpfApp1.Main
{
    public partial class NavigationWindow : Window
    {
        private Model db = new Model();
        private User user;
        private Teacher teacher;
        private DateTime dateInput;
        private DateTime dateOutput;

        public NavigationWindow(User user, Teacher teacher)
        {
            InitializeComponent();
            this.user = user;
            this.teacher = teacher;
            Init();
        }

        private void Init()
        {
            dateInput = DateTime.Now;
            LastNameLab.Content = $"Фамилия: {teacher.LastName}";
            FirstNameLab.Content = $"Имя: {teacher.FirstName}";
            PatronymicLab.Content = $"Отчество: {teacher.Patronymic}";
            AgeLab.Content = $"Возраст: {teacher.Age.ToString()}";
            var list = db.TimeTeachers.Where(item =>
                item.IdUser == user.Id
                && item.TimeInput.Year == DateTime.Now.Year
                && item.TimeInput.Month == DateTime.Now.Month
                && item.TimeInput.Day == DateTime.Now.Day);
            if (list.Count() != 0)
            {
                ActiveNowLab.Content = $"За сегодня: {Math.Round(list.Sum(item => item.TimeWork) / 60.0, 3)} часов";
            }

            var list2 = db.TimeTeachers.Where(item =>
                item.IdUser == user.Id
                && item.TimeInput.Year == DateTime.Now.Year
                && item.TimeInput.Month == DateTime.Now.Month);
            if (list2.Count() != 0)
            {
                ActiveMounthLab.Content = $"За месяц: {Math.Round(list2.Sum(item => item.TimeWork) / 60.0, 3)} часов";
            }

            TimeUserDG.ItemsSource = db.TimeTeachers.Where(item => item.IdUser == user.Id).ToList();

            MemoryStream ms = new MemoryStream(teacher.Photo);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = ms;
            image.EndInit();
            UserImage.Source = image;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dateOutput = DateTime.Now;
            TimeSpan timeSpan = dateOutput - dateInput;
            if (timeSpan.Minutes != 0 || timeSpan.Hours != 0)
            {
                TimeTeacher timeTeacher = new TimeTeacher();
                timeTeacher.TimeInput = dateInput;
                timeTeacher.TimeOutput = dateOutput;
                timeTeacher.IdUser = user.Id;
                if (timeSpan.Hours != 0)
                {
                    timeTeacher.TimeWork = (timeSpan.Hours * 60) + timeSpan.Minutes;
                }
                else
                {
                    timeTeacher.TimeWork = timeSpan.Minutes;
                }
                db.TimeTeachers.Add(timeTeacher);
                db.SaveChanges();
            }
        }

        private void ToClass_Click(object sender, RoutedEventArgs e)
        {
            Window window = new ClassesWindow(this);
            window.Show();
            Hide();
        }

        private void ToItem_Click(object sender, RoutedEventArgs e)
        {
            Window window = new ItemsWindow(this);
            window.Show();
            Hide();
        }

        private void ToStudent_Click(object sender, RoutedEventArgs e)
        {
            Window window = new StudentsWindow(this);
            window.Show();
            Hide();
        }

        private void ToPoint_Click(object sender, RoutedEventArgs e)
        {
            Window window = new PointsWindow(this, teacher);
            window.Show();
            Hide();
        }

    }
}
