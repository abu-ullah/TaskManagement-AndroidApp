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
    public partial class DeleteTaskPage : ContentPage
    {
        private static string taskId;

        public DeleteTaskPage(TaskModel tsk)
        {
            InitializeComponent();
            taskId = tsk.TaskUid;
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            DataSource.DeleteTask(taskId);
            await Navigation.PopAsync();
        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}