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
    /// Логика взаимодействия для PointsWindow.xaml
    /// </summary>
    public partial class PointsWindow : Window
    {
        private Window window;
        private Model db = new Model();
        private int page = 0;
        private int sizePage = 10;
        private int pagePoint = 0;
        private int sizePagePoint = 10;
        private Student student;
        private DAL.Point point;
        private Teacher teacher;
        public PointsWindow(Window window, Teacher teacher)
        {
            InitializeComponent();
            this.window = window;
            this.teacher = teacher;
            initStudents();
            ItemTB.ItemsSource = db.Items.ToList();
            InitComboFilter();
            InitComboSearch();
        }

        private void InitComboFilter()
        {
            FilterCB.Items.Add("Все");
            FilterCB.Items.Add("Оценка 5");
            FilterCB.Items.Add("Оценка 4");
            FilterCB.Items.Add("Оценка 3");
            FilterCB.Items.Add("Оценка 2");
            FilterCB.SelectedItem = FilterCB.Items[0];
        }

        private void InitComboSearch()
        {
            SearchCB.Items.Add("По имени");
            SearchCB.Items.Add("По фамилии");
            SearchCB.Items.Add("По отчеству");
            SearchCB.SelectedItem = SearchCB.Items[0];
        }

        private void initStudents()
        {
            StudentsDG.ItemsSource = db.Students.OrderBy(r => r.Id).Skip(sizePage * page).Take(sizePage).ToList();
        }

        private void InitPoints()
        {
            if (student != null)
            {
                PointDG.ItemsSource = db.Points.Where(item => item.IdStudent == student.Id).OrderBy(r => r.Id).Skip(sizePagePoint * pagePoint).Take(sizePagePoint).ToList();
            }
            
        }

        private void Clear()
        {
            PointTB.Text = "";
            point = null;
        }

        private void BackStudent_Click(object sender, RoutedEventArgs e)
        {
            if(page > 0)
            {
                page--;
                initStudents();
            }
        }

        private void ForwardStudent_Click(object sender, RoutedEventArgs e)
        {
            page++;
            initStudents();
        }

        private void BackPoint_Click(object sender, RoutedEventArgs e)
        {
            if (pagePoint > 0)
            {
                pagePoint--;
                InitPoints();
            }
        }

        private void ForwardPoint_Click(object sender, RoutedEventArgs e)
        {
            pagePoint++;
            InitPoints();
        }

        private void DelPoint_Click(object sender, RoutedEventArgs e)
        {
            if(point != null && student != null)
            {
                db.Points.Remove(point);
                db.SaveChanges();
                InitPoints();
                Clear();
            }
            else
            {
                MessageBox.Show("Выберите запись");
            }
        }

        private void EditPoint_Click(object sender, RoutedEventArgs e)
        {
            string point1 = PointTB.Text;
            int pointParse;
            bool check = int.TryParse(point1, out pointParse);
            Item item = ItemTB.SelectedItem as Item;
            if (check && student != null && item != null)
            {
                point.IdStudent = student.Id;
                point.IdTeacher = teacher.Id;
                point.IdItem = item.Id;
                point.Point1 = pointParse;
                db.SaveChanges();
                InitPoints();
                Clear();
            }
            else
            {
                MessageBox.Show("Выберите запись и заполните поля");
            }
        }

        private void StudentsDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pagePoint = 0;
            student = StudentsDG.SelectedItem as Student;
            if(student != null)
            {
                InitPoints();
            }
        }

        private void PointDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            point = PointDG.SelectedItem as DAL.Point;
            if (point != null)
            {
                PointTB.Text = point.Point1.ToString();

                foreach(var item in ItemTB.ItemsSource)
                {
                    var temp = item as Item;
                    if(temp.Id == point.IdItem)
                    {
                        ItemTB.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void AddPoint_Click(object sender, RoutedEventArgs e)
        {
            string point = PointTB.Text;
            int pointParse;
            bool check = int.TryParse(point, out pointParse);
            Item item = ItemTB.SelectedItem as Item;
            if (check && student != null && item != null)
            {
                DAL.Point point1 = new DAL.Point();
                point1.IdStudent = student.Id;
                point1.IdTeacher = teacher.Id;
                point1.IdItem = item.Id;
                point1.Point1 = pointParse;
                db.Points.Add(point1);
                db.SaveChanges();
                InitPoints();
                Clear();
            }
            else
            {
                MessageBox.Show("Выберите запись и заполните поля");
            }
        }

        private void BackToNav(object sender, RoutedEventArgs e)
        {
            window.Show();
            Close();
        }

        private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(FilterCB.SelectedIndex == 0) // Все
            {
                InitPoints();
            }
            if (FilterCB.SelectedIndex == 1) // 5
            {
                FilterParam(5);
            }
            if (FilterCB.SelectedIndex == 2) // 4
            {
                FilterParam(4);
            }
            if (FilterCB.SelectedIndex == 3) // 3
            {
                FilterParam(3);
            }
            if (FilterCB.SelectedIndex == 4) // 2
            {
                FilterParam(2);
            }
        }

        private void FilterParam(int point1)
        {
            PointDG.ItemsSource = db.Points
                .Where(item => item.IdStudent == student.Id && item.Point1 == point1)
                .OrderBy(r => r.Id)
                .Skip(sizePagePoint * page)
                .Take(sizePagePoint)
                .ToList();
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchTB.Text;
            if(SearchTB.Text == "")
            {
                initStudents();
            }
            if (SearchCB.SelectedIndex == 0) // Все
            {
                StudentsDG.ItemsSource = db.Students
                    .Where(item => item.FirstName.Contains(query))
                    .OrderBy(r => r.Id)
                    .Skip(sizePagePoint * page)
                    .Take(sizePagePoint)
                    .ToList();
            }
            if (SearchCB.SelectedIndex == 1) // 5
            {
                StudentsDG.ItemsSource = db.Students
                    .Where(item => item.LastName.Contains(query))
                    .OrderBy(r => r.Id)
                    .Skip(sizePagePoint * page)
                    .Take(sizePagePoint)
                    .ToList();
            }
            if (SearchCB.SelectedIndex == 2) // 4
            {
                StudentsDG.ItemsSource = db.Students
                    .Where(item => item.Patronymic.Contains(query))
                    .OrderBy(r => r.Id)
                    .Skip(sizePagePoint * page)
                    .Take(sizePagePoint)
                    .ToList();
            }
        }

    }
}
