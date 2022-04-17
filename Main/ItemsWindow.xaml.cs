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
    /// <summary>
    /// Логика взаимодействия для ItemsWindow.xaml
    /// </summary>
    public partial class ItemsWindow : Window
    {
        private Window window;
        private Model db = new Model();
        private int page = 0;
        private int sizePage = 10;
        private Item item;

        public ItemsWindow(Window window)
        {
            InitializeComponent();
            this.window = window;
            Init();
        }

        private void Init()
        {
            ItemDG.ItemsSource = db.Items.OrderBy(r => r.Id).Skip(sizePage * page).Take(sizePage).ToList();
        }

        private void Clear()
        {
            item = null;
            TitleTB.Text = "";
        }

        private void ItemDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            item = ItemDG.SelectedItem as Item;

            if (item != null)
            {
                TitleTB.Text = item.Title;
            }
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

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTB.Text;

            if (title != "")
            {
                Item item = new Item()
                {
                    Title = title
                };
                db.Items.Add(item);
                db.SaveChanges();
                Init();
                Clear();
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }

        private void EditItem_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTB.Text;

            if (title != "" && item != null)
            {
                item.Title = title;
                db.SaveChanges();
                Init();
                Clear();
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }

        private void DelItem_Click(object sender, RoutedEventArgs e)
        {
            if (item != null)
            {
                db.Items.Remove(item);
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
    }
}
