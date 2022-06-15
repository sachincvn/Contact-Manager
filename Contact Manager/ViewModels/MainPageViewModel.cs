using Contact_Manager.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using Contact_Manager.Models;
using Contact_Manager.Views;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Input;
using Contact_Manager.ViewModels.Items;
using Contact_Manager.Realm;
using AutoMapper;
using Contact_Manager.ApiObjects;
using RestSharp;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace Contact_Manager.ViewModels
{
    [INotifyPropertyChanged]
    [QueryProperty(nameof(Wrapper), "wrapper")]
    [QueryProperty(nameof(Wrapper), "contact")]
    public partial class MainPageViewModel
    {
        private readonly ObservableCollection<ItemContactModel> _allContacts;

        [ObservableProperty]
        private ObservableCollection<ItemContactModel> _itemsCollection;

        private WrapperModel<ContactModel> _wrapper;
        public WrapperModel<ContactModel> Wrapper
        {
            get => _wrapper;
            set
            {
                if (value != _wrapper)
                {
                    _wrapper = value;
                    UpdateListCommand.Execute(null);
                }
            }
        }

        public readonly IMapper _mapper;
        private readonly Realms.Realm realmDb;

        private RestClient _restClient;
        private readonly string _baseUrl = "https://1xiwvjh3rc.execute-api.us-east-1.amazonaws.com/Prod/";
    

        public MainPageViewModel(IMapper mapper)
        {
            ItemsCollection = new ObservableCollection<ItemContactModel>();
            _allContacts = new ObservableCollection<ItemContactModel>();
            _mapper = mapper;
            realmDb = Realms.Realm.GetInstance();

            _restClient = new RestClient(_baseUrl);
            GetAllContacts().AwaitTask();
        }


        private async Task GetAllContacts()
        {
            var contactsFromDB = realmDb.All<ContactObject>();
            try
            {
                var status = await Permissions.RequestAsync<Permissions.ContactsRead>();
                if (status == PermissionStatus.Granted)
                {
                    var phoneContacts = await Contacts.Default.GetAllAsync();

                    if (phoneContacts?.Any() ?? false)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            var contactObject = _mapper.Map<ContactObject>(phoneContacts.ElementAt(i));
                            contactObject.Image = "icon_person";
                            if (!realmDb.All<ContactObject>().Any(i => i.Number == contactObject.Number))
                            {
                                realmDb.Write(() =>
                                {
                                    realmDb.Add(contactObject);
                                });
                            }
                        }
                    }
                }
                else
                {
                    await Toast.Make("Permission denied to read your contact details").Show();
                }
                _allContacts.Clear();
                foreach (var item in contactsFromDB)
                {
                    _allContacts.Add(new ItemContactModel(_mapper.Map<ContactModel>(item)));
                }
                GetContactsData().AwaitTask();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task GetContactsData()
        {
            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                var request = new RestRequest("api/Contacts", Method.Get);
                var response = await _restClient.ExecuteAsync<ObservableCollection<ContactData>>(request);

                if (response.IsSuccessful)
                {
                    var items = response.Data;
                    foreach (var item in items)
                    {
                        var contactDto = _mapper.Map<ContactModel>(item);
                        contactDto.FullName = item.Name;
                        contactDto.Image = "icon_person";
                        var contactObject = _mapper.Map<ContactObject>(contactDto);

                        if (!realmDb.All<ContactObject>().Any(i => i.Number == item.Number))
                        {
                            realmDb.Write(() =>
                            {
                                realmDb.Add(contactObject);
                            });
                        }
                    }
                }
                else
                {
                    await Toast.Make(response.ErrorMessage.ToString()).Show();
                }
            }
            DisplayData();
        }


        [ICommand]
        private async Task AddContactAsync()
        {
             await Shell.Current.GoToAsync(nameof(AddContactDetail));
        }


        [ICommand]
        private void UpdateList()
        {
            switch (Wrapper.Operation)
            {
                case Operation.Add:
                    AddContact().AwaitTask();
                    break;
                case Operation.Update:
                    UpdateContact().AwaitTask();
                    break;
                case Operation.Delete:
                    DeleteContact().AwaitTask();
                    break;
                case Operation.Display:
                default:
                    break;
            }
        }

        private async Task DeleteContact()
        {
            var contactFromDb = realmDb.All<ContactObject>().FirstOrDefault(i => i.Number == Wrapper.Item.Number);
            _allContacts.Remove(_allContacts.FirstOrDefault(i => i.Contact.Number == Wrapper.Item.Number));
            using (var db = realmDb.BeginWrite())
            {
                realmDb.Remove(contactFromDb);
                db.Commit();
            }

            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                var request = new RestRequest($"api/Contacts/{Wrapper.Item.Id}", Method.Delete);
                var response = await _restClient.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    await Toast.Make("Deleted Sucessfully").Show();
                    _allContacts.Remove(_allContacts.FirstOrDefault(i => i.Contact.Id == Wrapper.Item.Id));
                }
                else
                {
                    await Toast.Make(response.ErrorMessage.ToString()).Show();
                }
            }
            
            DisplayData();
        }

        private async Task UpdateContact()
        {
            /*ContactData contactdata = new ContactData();
            contactdata.Id = Wrapper.Item.Id.ToString();
            contactdata.Name = Wrapper.Item.Name;
            contactdata.Number = Wrapper.Item.Number;
            contactdata.Email = null;
            contactdata.Address = null;
            var request = new RestRequest($"api/Contacts/{Wrapper.Item.Id}", Method.Put);
            request.AddJsonBody(contactdata);
            var response = await _restClient.ExecutePutAsync<ContactData>(request);
            if (response.IsSuccessful)
            {
                await Toast.Make("Deleted Sucessfully").Show();
                _allContacts.Remove(_contacts.FirstOrDefault(i => i.Contact.Id == Wrapper.Item.Id));
            }
            else
            {
                await Toast.Make(response.ErrorMessage.ToString()).Show();
            }*/


            var contactFromDb = realmDb.All<ContactObject>().FirstOrDefault(i => i.Id == Wrapper.Item.Id);
            if(contactFromDb != null)
            {
                var mappedContact = _mapper.Map<ContactObject>(Wrapper.Item);
                using (var db = realmDb.BeginWrite())
                {
                    realmDb.Add(mappedContact);
                    db.Commit();
                }
            }
        }

        private async Task AddContact()
        {
            try
            {
                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    ContactData contactdata = new ContactData();
                    contactdata.Id = Guid.NewGuid().ToString();
                    contactdata.Name = Wrapper.Item.Name;
                    contactdata.Number = Wrapper.Item.Number;
                    contactdata.Email = null;
                    contactdata.Address = null;

                    var request = new RestRequest("api/Contacts", Method.Post);
                    request.AddJsonBody(contactdata);
                    var response = await _restClient.ExecutePostAsync<ContactData>(request);
                    if (response.IsSuccessful)
                    {
                        _allContacts.Add(new ItemContactModel(Wrapper.Item));
                    }
                    else
                    {
                        await Toast.Make(response.ErrorMessage.ToString()).Show();
                    }
                }
                else
                {
                    await Toast.Make("Please Chek your Internet Connection!").Show();
                }
            }
            catch (Exception ex)
            {
            }
            GetContactsData().AwaitTask();

        }

        [ICommand]
        private async Task SearchContact(string searchText)
        {
            try
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    DisplayData();
                }
                else
                {
                    var searchResult = _allContacts.Select(i => i).Where(contact => contact.Contact.Name.ToLower().Contains(searchText.ToLower())).ToList();
                    ItemsCollection = new ObservableCollection<ItemContactModel>(searchResult);
                }

                /*var request = new RestRequest($"api/Contacts/{id}");
                var response = await _restClient.ExecuteAsync<ContactData>(request);
                if (response.IsSuccessful)
                {
                    await Toast.Make(response.Content.ToString()).Show();
                }
                else
                {
                    await Toast.Make(response.ErrorMessage.ToString()).Show();
                }*/
            }
            catch (Exception ex)
            {
            }
        }

        private void DisplayData()
        {
            ItemsCollection = new ObservableCollection<ItemContactModel>(_allContacts);
        }

        public int TestFunctionAdd(int a,int b)
        {
            return a + b;
        }

        
    }
}
