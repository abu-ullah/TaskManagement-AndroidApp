using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementProject.Data;
using TaskManagementProject.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskManagementProject.Controllers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TasksAssignedToPage : ContentPage
    {
        public TasksAssignedToPage()
        {
            InitializeComponent();

            DataSource.AddTasksAssignedTo();
            listTasks.ItemsSource = DataSource.GetTasksAssignedTo();
        }

        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void listTasks_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var editTask = new TaskDetailPage((TaskModel)e.SelectedItem);
            editTask.BindingContext = (TaskModel)e.SelectedItem;
            await Navigation.PushAsync(editTask);
        }
    }
}