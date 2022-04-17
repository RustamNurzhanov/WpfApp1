using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfApp1.DAL;

namespace WpfApp1.Main
{
    public partial class StudentsWindow : Window
    {
        private Window window;
        private Model db = new Model();
        private int page = 0;
        private int sizePage = 10;
        private Student student;
        private string urlPhoto = "";

        public StudentsWindow(Window window)
        {
            InitializeComponent();
            this.window = window;
            Init();
            ClassCB.ItemsSource = db.Classes.ToList();
        }

        private void Init()
        {
            StudentDG.ItemsSource = db.Students.OrderBy(r => r.Id).Skip(sizePage * page).Take(sizePage).ToList();
        }

        private void Clear()
        {
            urlPhoto = "";
            student = null;
            NameTB.Text = "";
            SurnameTB.Text = "";
            PatronymicTB.Text = "";
            AgeTB.Text = "";
            PhotoImage.Source = null;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (page > 0)
            {
                page--;
                Init();
            }
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            page++;
            Init();
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTB.Text;
            string surname = SurnameTB.Text;
            string patrinymic = PatronymicTB.Text;
            string age = AgeTB.Text;

            int ageParse;

            bool checkAge = int.TryParse(age, out ageParse);
            Class @class = ClassCB.SelectedItem as Class;

            if(name != "" && surname != "" && patrinymic != "" && checkAge && @class != null)
            {
                Student student = new Student()
                {
                    FirstName = name,
                    LastName = surname,
                    Patronymic = patrinymic,
                    Age = ageParse,
                    IdClass = @class.Id,
                };
                if(urlPhoto != "")
                {
                    student.Photo = File.ReadAllBytes(urlPhoto);
                }
                db.Students.Add(student);
                db.SaveChanges();
                Init();
                Clear();
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }

        private void EditStudent_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTB.Text;
            string surname = SurnameTB.Text;
            string patrinymic = PatronymicTB.Text;
            string age = AgeTB.Text;

            int ageParse;

            bool checkAge = int.TryParse(age, out ageParse);
            Class @class = ClassCB.SelectedItem as Class;

            if (name != "" && surname != "" && patrinymic != "" && checkAge && @class != null && student != null)
            {

                student.FirstName = name;
                student.LastName = surname;
                student.Patronymic = patrinymic;
                student.Age = ageParse;
                student.IdClass = @class.Id;

                if (urlPhoto != "")
                {
                    student.Photo = File.ReadAllBytes(urlPhoto);
                }
                db.SaveChanges();
                Init();
                Clear();
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }

        private void DelStudent_Click(object sender, RoutedEventArgs e)
        {
            if(student != null)
            {
                db.Students.Remove(student);
                db.SaveChanges();
                Init();
                Clear();
            }
            else
            {
                MessageBox.Show("Выберите запись");
            }
        }

        private void BackToNav_Click(object sender, RoutedEventArgs e)
        {
            window.Show();
            Close();
        }

        private void StudentsDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Clear();
            student = StudentDG.SelectedItem as Student;

            if(student != null)
            {
                NameTB.Text = student.FirstName;
                SurnameTB.Text = student.LastName;
                PatronymicTB.Text = student.Patronymic;
                AgeTB.Text = student.Age.ToString();

                foreach(var item in ClassCB.ItemsSource)
                {
                    var temp = item as Class;
                    if(temp.Id == student.IdClass)
                    {
                        ClassCB.SelectedItem = item;
                        break;
                    }
                }
            }
            if(student.Photo != null)
            {
                MemoryStream ms = new MemoryStream(student.Photo);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();
                PhotoImage.Source = bitmapImage;
            }
        }

        private void AddPhoto_Click(object sender, RoutedEventArgs e)
        {
            string filter = "Image files ( *.PNG )|*.bmp;*.jpg;*.gif; *.tif; *.png; *.ico; *.emf; *.wmf";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;
            openFileDialog.ShowDialog();
            urlPhoto = openFileDialog.FileName;
            UrlTB.Content = "Фото добавлено";
        }
    }
}
