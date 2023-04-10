using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskManagementProject.Data;
using TaskManagementProject.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskManagementProject.Controllers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskDetailPage : ContentPage
    {
        private static string taskId;

        public TaskDetailPage(TaskModel tsk)
        {
            InitializeComponent();
            taskId = tsk.TaskUid; 

            switchDone.IsToggled = tsk.Done;
        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void btnEdit_Clicked(object sender, EventArgs e)
        {
            await DataSource.UpdateTask(taskId, switchDone.IsToggled);
            await Navigation.PopAsync();
        }
    }
}