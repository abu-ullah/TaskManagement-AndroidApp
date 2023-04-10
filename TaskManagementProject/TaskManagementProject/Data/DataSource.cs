using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskManagementProject.Models;

namespace TaskManagementProject.Data
{
    class DataSource
    {
        public static User currentUser;
        private static string myToken;
        public static List<User> myUsers = new List<User>();
        private static ObservableCollection<TaskModel> myTasksCreatedBy = new ObservableCollection<TaskModel>();
        private static ObservableCollection<TaskModel> myTasksAssignedTo = new ObservableCollection<TaskModel>();

        public class UpdateRequest
        {
            public bool done { get; set; }

            public UpdateRequest(bool done)
            {
                this.done = done;
            }
        }

        public static string GetToken()
        {
            return myToken;
        }

        public static void SetToken(string token)
        {
            myToken = token;
        }

        // Method Adds All Users to list myUsers.
        public static async Task AddUsers()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-access-token", myToken);
            Uri uri = new Uri($"https://taskmanager-project-fall2022-zmoya.ondigitalocean.app/v1/users/all");
            HttpResponseMessage response = await client.GetAsync(uri);

            string content = await response.Content.ReadAsStringAsync();

            var values = JObject.Parse(content);
            foreach (var item in values["allUsers"])
            {
                User newUser = new User(item["email"].ToString(), item["name"].ToString(), item["uid"].ToString());
                myUsers.Add(newUser);
            }
        }

        // Method Adds Tasks CreatedBy The logged user to list myTasksCreatedBy.
        public static async void AddTasksCreatedBy()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-access-token", myToken);
            Uri uri = new Uri($"https://taskmanager-project-fall2022-zmoya.ondigitalocean.app/v1/tasks/createdby/");
            HttpResponseMessage response = await client.GetAsync(uri);

            string content = await response.Content.ReadAsStringAsync();

            var values = JObject.Parse(content);

            foreach (var item in values["allTasks"])
            {
                myTasksCreatedBy.Add(new TaskModel(
                    item["assignedToName"].ToString(),
                    item["assignedToUid"].ToString(), 
                    item["createdByName"].ToString(),
                    item["createdByUid"].ToString(),
                    item["description"].ToString(),
                    (bool)item["done"],
                    item["taskUid"].ToString()
                    ));
            }
        }

        public static void AddSingleTask(string description, string assignedToName, string uid)
        {
            myTasksCreatedBy.Add(new TaskModel
            {
                Description = description,
                AssignedToName = assignedToName,
                TaskUid = uid
            }); ;
        }

        // Method Adds Tasks AssignedTo the logged user to list myTasksAssignedTo.
        public static async void AddTasksAssignedTo()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-access-token", myToken);
            Uri uri = new Uri($"https://taskmanager-project-fall2022-zmoya.ondigitalocean.app/v1/tasks/assignedto/");
            HttpResponseMessage response = await client.GetAsync(uri);

            string content = await response.Content.ReadAsStringAsync();

            var values = JObject.Parse(content);

            foreach (var item in values["allTasks"])
            {
                myTasksAssignedTo.Add(new TaskModel(
                    item["assignedToName"].ToString(),
                    item["assignedToUid"].ToString(),
                    item["createdByName"].ToString(),
                    item["createdByUid"].ToString(),
                    item["description"].ToString(),
                    (bool)item["done"],
                    item["taskUid"].ToString()
                    ));
            }
        }

        public static List<User> GetUsers()
        {
            return myUsers;
        }

        public static ObservableCollection<TaskModel> GetTasksCreatedBy()
        {
            return myTasksCreatedBy;
        }

        public static ObservableCollection<TaskModel> GetTasksAssignedTo()
        {
            return myTasksAssignedTo;
        }

        public static async void DeleteTask(string taskId)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-access-token", myToken);
            Uri uri = new Uri($"https://taskmanager-project-fall2022-zmoya.ondigitalocean.app/v1/tasks/{taskId}");
            HttpResponseMessage response = await client.DeleteAsync(uri);

            string content = await response.Content.ReadAsStringAsync();

            var values = JObject.Parse(content);
        }

        public static async Task UpdateTask(string taskId, bool done)
        {
            var method = new HttpMethod("PATCH");
            UpdateRequest update = new UpdateRequest(done);
            StringContent jsonContent = new StringContent(
                JsonConvert.SerializeObject(update),
                Encoding.UTF8,
                "application/json"
                );

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-access-token", myToken);
            Uri uri = new Uri($"https://taskmanager-project-fall2022-zmoya.ondigitalocean.app/v1/tasks/{taskId}");
            var request = new HttpRequestMessage(method, uri);
            request.Content = jsonContent;
            HttpResponseMessage response = await client.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();

            var values = JObject.Parse(content);
        }

    }
}
