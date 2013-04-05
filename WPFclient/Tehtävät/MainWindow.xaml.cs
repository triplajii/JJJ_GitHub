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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tehtävät.TaskServiceRef;

namespace Tehtävät
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TaskServiceRef.TaskClient proxy;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                proxy = new TaskServiceRef.TaskClient();
                LoadTasks();
            }
            catch (Exception ex)
            {
                txtStatus.Text = ex.Message + " " + ex.StackTrace;
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtTask.Text != "")
                {
                    btnSave.IsEnabled = false;
                    txtStatus.Text = await proxy.AddTaskAsync(txtTask.Text.Trim());
                    txtTask.Text = "";
                    btnSave.IsEnabled = true;
                }
                LoadTasks();
            }
            catch (Exception ex)
            {
                txtStatus.Text = ex.Message + " " + ex.StackTrace;
            }
        }

        private void txtTask_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                txtTask.Text = "";
                txtStatus.Text = "";
            }
            catch (Exception ex)
            {
                txtStatus.Text = ex.Message + " " + ex.StackTrace;
            }
        }

        private async void LoadTasks()
        {
            try
            {
                TaskTodoTask[] tasks = await proxy.GetAllTasksAsync();
                //dgTasks.ItemsSource = tasks;
                //dgTasks.DataContext = tasks;
                //tasks = await proxy.GetAllTasksAsync();            
                dgTasks.DataContext = from t in tasks select new { t.task, t.guid };
            }
            catch (Exception ex)
            {
                txtStatus.Text = ex.Message + " " + ex.StackTrace;
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button b = sender as Button;
                if (b != null)
                {
                    string guid = b.Tag.ToString();
                    await proxy.DeleteTaskAsync(guid);
                    LoadTasks();
                }
            }
            catch (Exception ex)
            {
                txtStatus.Text = ex.Message + " " + ex.StackTrace;
            }
        }
    }
}
