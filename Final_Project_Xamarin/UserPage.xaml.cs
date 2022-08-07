using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Final_Project_Xamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        private static List<Task> myTasks = new List<Task>();

        public UserPage()
        {
            InitializeComponent();
            dpDate.MinimumDate = DateTime.Now;

        }
        protected override async void OnAppearing()
        {
            listTasks.ItemsSource = await App.Database.GetTasksAsync();

            pickerUser.ItemsSource = await App.Database.GetUsersAsync();


            pickerUser.SelectedIndex = 0;


        }

        private async void btnAddTask_Clicked(object sender, EventArgs e)
        {
            bool fieldsEmpty = txtTaskId.Text == "" || txtDescription.Text == "";
            if (fieldsEmpty)
            {
                await DisplayAlert("Alert", "Fields cannot be Emplty!", "OK");
            }
            else
            {
                User temp = (User)pickerUser.SelectedItem;
                if (await CreateTaskAsync(txtTaskId.Text, txtDescription.Text, temp.Username, dpDate.Date.ToString()))
                {
                    await DisplayAlert("Alert", $"Task {txtTaskId.Text} was created!", "OK");
                    pickerUser.SelectedIndex = 0;
                    txtTaskId.Text = "";
                    txtDescription.Text = "";
                    dpDate.Date = DateTime.Now;
                    listTasks.ItemsSource = await App.Database.GetTasksAsync();


                }
                else
                {
                    await DisplayAlert("Alert", "Task Id already Exists!", "OK");

                }
            }
        }

        private void btnLogout_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        private async Task<bool> CreateTaskAsync(string taskId, string description, string assigned, string deadline)
        {
            foreach (Task tsk in await App.Database.GetTasksAsync())
            {
                if (tsk.TaskId == taskId)
                {
                    return false;
                }
            }

            await App.Database.SaveTasksAsync(
                new Task
                {
                    TaskId = taskId,
                    Description = description,
                    Assigned = assigned,
                    DeadLine = deadline
                }
                );

            return true;

        }

        private async void listTasks_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            myTasks.Clear();
            foreach (Task tsk in await App.Database.GetTasksAsync())
            {
                myTasks.Add(tsk);
            }
            var taskDetail = new TaskDetailPage(myTasks[e.SelectedItemIndex]);
            taskDetail.BindingContext = myTasks[e.SelectedItemIndex];
            await Navigation.PushAsync(taskDetail);
        }
    }
}