using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskManagementProject.Controllers;
using TaskManagementProject.Data;
using Xamarin.Forms;

namespace TaskManagementProject
{
    public partial class MainPage : ContentPage
    {
        public class PostRequest
        {
            public string email { get; set; }

            public string password { get; set; }

            public PostRequest(string email, string password)
            {
                this.email = email;
                this.password = password;
            }
        }

        public MainPage()
        {
            InitializeComponent();
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            await Button_ClickedAsync(txtEmail.Text, txtPassword.Text);
        }

        private async Task Button_ClickedAsync(string email, string password)
        {
            PostRequest post = new PostRequest(email, password);

            StringContent jsonContent = new StringContent(
                JsonConvert.SerializeObject(post),
                Encoding.UTF8,
                "application/json"
                );

            HttpClient client = new HttpClient();
            Uri uri = new Uri("https://taskmanager-project-fall2022-zmoya.ondigitalocean.app/v1/users/login");
            HttpResponseMessage response = await client.PostAsync(uri, jsonContent);

            string content = await response.Content.ReadAsStringAsync();

            var values = JObject.Parse(content);

            if (response.IsSuccessStatusCode)
            {
                DataSource.SetToken(values["token"].ToString());
                await Navigation.PushAsync(new UsersTasksPage());
            } else
            {
                await DisplayAlert("Alert", "Invalid username or password!", "Ok");
            }
        }

        private async void btnSignUp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }
    }
}
