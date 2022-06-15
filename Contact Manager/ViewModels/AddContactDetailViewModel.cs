using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Contact_Manager.ViewModels.Items;
using Contact_Manager.Models;
using Contact_Manager.Realm;
using Contact_Manager.Views;
using Realms;

namespace Contact_Manager.ViewModels
{
    [INotifyPropertyChanged]
    [QueryProperty(nameof(Contact), "contact")]
    public partial class AddContactDetailViewModel
    {
        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(AddContactDetailCommand))]
        private ContactModel _contact;

        public AddContactDetailViewModel()
        {
            Contact = new ContactModel();
        }

        [ICommand]
        private async Task AddContactDetailAsync()
        {
            if (string.IsNullOrEmpty(Contact.Name) || string.IsNullOrEmpty(Contact.SurName) || string.IsNullOrEmpty(Contact.Number))
            {
                await Toast.Make("Please fill the above details!").Show();
            }
            else
            {
                Contact.FullName = $"{Contact.Name} {Contact.SurName}";
                Contact.Image = "icon_person";
                await Shell.Current.GoToAsync("..", new Dictionary<string, object>()
                {
                    { "wrapper", new WrapperModel<ContactModel>(Contact, Operation.Add) }
                });
            }

        }
    }
}
