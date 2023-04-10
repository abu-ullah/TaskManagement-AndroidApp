using Newtonsoft.Json;
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
    public partial class UsersTasksPage : ContentPage
    {
        public class CreateTaskRequest
        {
            public string description { get; set; }

            public string assignedToUid { get; set; }

            public CreateTaskRequest(string desc, string assignedToUid)
            {
                this.description = desc;
                this.assignedToUid = assignedToUid;
            }
        }

        public UsersTasksPage()
        {
            InitializeComponent();
            PopulateViews();
        }

        private async void PopulateViews()
        {
            await DataSource.AddUsers();

            DataSource.AddTasksCreatedBy();
            listTasks.ItemsSource = DataSource.GetTasksCreatedBy();
            pickerUser.ItemsSource = DataSource.myUsers;
            pickerUser.SelectedIndex = 0;
        }

        private async void listTasks_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var deleteTask = new DeleteTaskPage((TaskModel)e.SelectedItem);
            deleteTask.BindingContext = (TaskModel)e.SelectedItem;
            await Navigation.PushAsync(deleteTask);
        }

        private async void btnAddTask_Clicked(object sender, EventArgs e)
        {
            CreateTaskRequest taskRequest = new CreateTaskRequest(
                txtTaskDescription.Text.ToString(), 
                ((User)pickerUser.SelectedItem).Uid
            );

            StringContent jsonContent = new StringContent(
                JsonConvert.SerializeObject(taskRequest),
                Encoding.UTF8,
                "application/json"
            );

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-access-token", DataSource.GetToken());
            Uri uri = new Uri("https://taskmanager-project-fall2022-zmoya.ondigitalocean.app/v1/tasks/");
            HttpResponseMessage response = await client.PostAsync(uri, jsonContent);

            string content = await response.Content.ReadAsStringAsync();
            var values = JObject.Parse(content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Success", "Created Task Successfully!", "Ok");
                listTasks.ItemsSource = null;
                DataSource.AddSingleTask(taskRequest.description, ((User)pickerUser.SelectedItem).Name, values["id"].ToString());
                listTasks.ItemsSource = DataSource.GetTasksCreatedBy();
            }
            else
            {
                await DisplayAlert("Alert", "Something went wrong while creating Task", "Ok");
            }
        }

        private async void btnLogout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void btnSeeTasksAssignedTo_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TasksAssignedToPage());
        }
    }
}