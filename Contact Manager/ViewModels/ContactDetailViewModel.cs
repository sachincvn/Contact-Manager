using Contact_Manager.Models;
using Contact_Manager.Views;
using Contact_Manager.Realm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Input;
using Realms;
using AutoMapper;

namespace Contact_Manager.ViewModels
{  
    [INotifyPropertyChanged]
    [QueryProperty(nameof(Contact), "contact")]
    public partial class ContactDetailViewModel
    {
        [ObservableProperty]
        private ContactModel _contact;


        public ContactDetailViewModel()
        {
            Contact = new ContactModel();
        }

        [ICommand]
        private async Task CallContactAsync()
        {
            if (PhoneDialer.Default.IsSupported)
            {
                PhoneDialer.Default.Open(Contact.Number);
            }
            else
            {
                await Toast.Make("Unable to open Phone Dialer").Show();
            }
        }

        [ICommand]
        private async Task MessageContactAsync()
        {
            if (Sms.Default.IsComposeSupported)
            {
                string[] recipients = new[] { Contact.Number };
                string msgText = "Hello, From Contact Manager App.";

                var message = new SmsMessage(msgText, recipients);

                await Sms.Default.ComposeAsync(message);
            }
            else
            {
                await Toast.Make("Unable to open Message Application").Show();
            }
        }


        [ICommand]
        private async Task EditContactAsync()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(AddContactDetail), new Dictionary<string, object>()
                {
                    { "contact", Contact}
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [ICommand]
        private async Task DeleteContactAsync()
        {
            bool deleteContact = await Shell.Current.DisplayAlert("Delete Contact ?", $"Are you sure you want to remove {Contact.Name} from your contacts?", "Yes", "No");
            if (deleteContact)
            {
                await Shell.Current.GoToAsync("..", new Dictionary<string, object>()
                {
                    { "contact", new WrapperModel<ContactModel>(Contact, Operation.Delete)}
                });
            }
        }

        [ICommand]
        private async Task SelectProfilePictureAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.StorageRead>();
            if (status == PermissionStatus.Granted)
            {
                try
                {
                    var result = await FilePicker.Default.PickAsync();
                    if (result != null)
                    {
                        if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                            result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase) ||
                            result.FileName.EndsWith("webp", StringComparison.OrdinalIgnoreCase))
                        {
                            Realms.Realm realmDb = Realms.Realm.GetInstance();
                            var contactFromDb = realmDb.All<ContactObject>().FirstOrDefault(i => i.Id == Contact.Id);
                            if (contactFromDb != null)
                            {
                                using (var db = realmDb.BeginWrite())
                                {
                                    Contact.Image = result.FullPath;
                                    contactFromDb.Image = result.FullPath;
                                    db.Commit();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }
            else
            {
                await Toast.Make("Permission denied to read external storage").Show();
            }
        }
    }
}
