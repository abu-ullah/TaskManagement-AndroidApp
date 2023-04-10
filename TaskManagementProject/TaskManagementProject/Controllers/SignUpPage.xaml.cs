using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskManagementProject.Controllers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public class PostRequest
        {
            public string name { get; set; }

            public string email { get; set; }

            public string password { get; set; }

            public PostRequest(string email, string password, string name)
            {
                this.email = email;
                this.password = password;
                this.name = name;
            }
        }

        public SignUpPage()
        {
            InitializeComponent();
        }

        private async void btnCreate_Clicked(object sender, EventArgs e)
        {
            bool emptyFields = txtName.Text == "" || txtEmail.Text == "" || txtPassword.Text == "" || txtPasswordRep.Text == "";
            bool passwordsMatch = txtPassword.Text == txtPasswordRep.Text;

            if (emptyFields)
            {
                await DisplayAlert("Alert", "One or many fields are empty", "Ok");
            } else
            {
                if (passwordsMatch)
                {
                    PostRequest post = new PostRequest(txtEmail.Text, txtPassword.Text, txtName.Text);

                    StringContent jsonContent = new StringContent(
                        JsonConvert.SerializeObject(post),
                        Encoding.UTF8,
                        "application/json"
                        );

                    HttpClient client = new HttpClient();
                    Uri uri = new Uri("https://taskmanager-project-fall2022-zmoya.ondigitalocean.app/v1/users/signup");
                    HttpResponseMessage response = await client.PostAsync(uri, jsonContent);

                    string content = await response.Content.ReadAsStringAsync();

                    var values = JObject.Parse(content);

                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Success", $"User {txtEmail.Text} was created!", "Ok");
                        await Navigation.PopAsync();
                    } else
                    {
                        await DisplayAlert("Alert", $"User with username {txtEmail.Text} already exists!", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Alert", "Passwords do not match", "Ok");
                }
            }
        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}