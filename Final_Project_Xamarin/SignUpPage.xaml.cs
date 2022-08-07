using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Final_Project_Xamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();

        }
        private async void btnCreate_Clicked(object sender, EventArgs e)
        {
            bool fieldsEmpty = txtUsername.Text == "" || txtPassword.Text == "" || txtRepPassword.Text == "";
            bool passMatch = txtPassword.Text == txtRepPassword.Text;
            if (fieldsEmpty)
            {
                await DisplayAlert("Alert", "Fields Cannot be empty!", "OK");
            }
            else
            {
                if (passMatch)
                {
                    if (await CreateUserAsync(txtUsername.Text, txtPassword.Text))
                    {
                        await DisplayAlert("Alert", $"User {txtUsername.Text} was Created!", "OK");
                        await Navigation.PopAsync();

                    }
                    else
                    {
                        await DisplayAlert("Alert", "Username already Exists!!", "OK");
                        clean();
                    }
                }
                else
                {
                    await DisplayAlert("Alert", "Pass do not match!", "OK");
                   
                }


            }
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        private async Task<bool> CreateUserAsync(string username, string password)
        {
            foreach (User user in await App.Database.GetUsersAsync())
            {
                if (user.Username == username)
                {
                    return false;
                }
            }

            await App.Database.SaveUserAsync(
                new User
                {
                    Username = username,
                    Password = password
                }
                );
            return true;

        }


        private async void RequestApi(string user)
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri(string.Format($"https://api.github.com/users/{user}", string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);

            actIndicator.IsRunning = !response.IsSuccessStatusCode;

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);

                txtPassword.IsEnabled = txtRepPassword.IsEnabled = btnCreate.IsEnabled = true;

                myImage.Source = values["avatar_url"];

                txtUsername.IsEnabled = false;
            }
            else
            {
                myImage.Source = "profile.png";
                txtPassword.IsEnabled = txtRepPassword.IsEnabled = btnCreate.IsEnabled = false;
                actIndicator.IsRunning = false;
            }
        }

        private void txtUsername_SearchButtonPressed(object sender, EventArgs e)
        {
            SearchBar sb = (SearchBar)sender;
            RequestApi(sb.Text);
        }

        private void clean()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtRepPassword.Text = "";
            myImage.Source = "profile.png";
            txtUsername.IsEnabled = true;
            txtPassword.IsEnabled = txtRepPassword.IsEnabled = false;
            

        }
    }
}