using System;
using System.Collections.Generic;
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

namespace WpfApp1.Main
{
    public partial class ClassesWindow : Window
    {
        private Window window;
        private Model db = new Model();
        private int page = 0;
        private int sizePage = 10;
        private Class _class;

        public ClassesWindow(Window window)
        {
            InitializeComponent();
            this.window = window;
            Init();
        }

        private void Init()
        {
            ClassDG.ItemsSource = db.Classes.OrderBy(r => r.Id).Skip(sizePage * page).Take(sizePage).ToList();
        }

        private void Clear()
        {
            _class = null;
            TitleTB.Text = "";
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if(page > 0)
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

        private void AddClass_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTB.Text;

            if(title != "")
            {
                Class @class = new Class() 
                { 
                    Title = title 
                };
                db.Classes.Add(@class);
                db.SaveChanges();
                Init();
                Clear();
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }

        private void EditClass_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTB.Text;

            if (title != "" && _class != null)
            {
                _class.Title = title;
                db.SaveChanges();
                Init();
                Clear();
            }
            else
            {
                MessageBox.Show("Заполните все поля и выберите запись");
            }
        }

        private void DelClass_Click(object sender, RoutedEventArgs e)
        {
            if(_class != null)
            {
                db.Classes.Remove(_class);
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

        private void ClassDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _class = ClassDG.SelectedItem as Class;

            if(_class != null)
            {
                TitleTB.Text = _class.Title;
            }
        }
    }
}
