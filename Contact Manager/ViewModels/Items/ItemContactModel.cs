using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Contact_Manager.Models;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact_Manager.ViewModels.Items
{
    [INotifyPropertyChanged]
    [QueryProperty(nameof(Contact), "contact")]
    public partial class ItemContactModel
    {
        [ObservableProperty]
        private ContactModel _contact;

        public ItemContactModel(ContactModel contact)
        {
            this.Contact = contact;
        }

        [ICommand]
        private async Task CallContact()
        {
            if (PhoneDialer.Default.IsSupported)
            {
                PhoneDialer.Default.Open(Contact.Number);
            }
            else
            {
                await Toast.Make("Could't open dialer").Show();
            }
        }

        [ICommand]
        private async Task MessageContact()
        {
            if (Sms.Default.IsComposeSupported)
            {
                string[] recipients = new[] { Contact.Number };
                string text = "Hello, From Contact Manager App.";

                var message = new SmsMessage(text, recipients);

                await Sms.Default.ComposeAsync(message);
            }
            else
            {
                await Toast.Make("Something Went Wrong").Show();
            }
        }

        [ICommand]
        private async Task ContactDetail()
        {
            await Shell.Current.GoToAsync(nameof(ContactDetail), new Dictionary<string, object>
            {
                { "contact", Contact }
            });
        }


    }
}
