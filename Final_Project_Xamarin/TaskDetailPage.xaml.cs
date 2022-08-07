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
    public partial class TaskDetailPage : ContentPage
    {
        Task globalTask;
        internal TaskDetailPage(Task tsk)
        {

            InitializeComponent();
            dpDate.Date = Convert.ToDateTime(tsk.DeadLine);
            globalTask = tsk;
            txtTaskId.Text = tsk.TaskId;
            txtTaskId.IsEnabled = false;

        }
        protected override async void OnAppearing()
        {
            List<User> tempList = await App.Database.GetUsersAsync();

            pickerUser.ItemsSource = tempList;
            int index = tempList.FindIndex(item => item.Username == globalTask.Assigned);
            pickerUser.SelectedIndex = index;
        }

        private async void btnEdit_Clicked(object sender, EventArgs e)
        {
            bool fieldempty = txtDescription.Text == "";
            if (fieldempty)
            {
                await DisplayAlert("Alert", "Fields cannot be empty!", "OK");
            }
            else
            {
                User temp = (User)pickerUser.SelectedItem;
                if (await EditTaskAsync(txtTaskId.Text, txtDescription.Text, temp.Username, dpDate.Date.ToString()))
                {
                    await DisplayAlert("Alert", $"Task {txtTaskId.Text} was Edited!", "OK");

                }
                else
                {
                    await DisplayAlert("Alert", $"Error on trying to edit task {txtTaskId.Text} !", "OK");

                }
                await Navigation.PopAsync();

            }
        }
        private async Task<bool> EditTaskAsync(string taskId, string description, string assigned, string deadline)
        {

            await App.Database.EditTaskAsync(
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
        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Alert", "Are you sure? Delete cannot be undo!", "Yes", "No");
            if (answer)
            {
                await App.Database.DeleteTaskAsync(globalTask);

                await Navigation.PopAsync();
            }
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}