using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realms;

namespace Contact_Manager.Models
{
    [INotifyPropertyChanged]
    public partial class ContactModel 
    {
        [ObservableProperty]
        private Guid _id;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _surName;

        [ObservableProperty]
        private string _fullName;

        [ObservableProperty]
        private string _number;

        [ObservableProperty]
        private string _image;

        public ContactModel()
        {

        }
        public ContactModel(string name, string surName, string number)
        {
            Name = name;
            SurName = surName;
            Number = number;
        }
    }
}
