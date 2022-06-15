using CommunityToolkit.Mvvm.ComponentModel;
using Contact_Manager.Models;
using Contact_Manager.ViewModels;

namespace Contact_Manager.Views;


public partial class ContactDetail : ContentPage
{
    public ContactDetail(ContactDetailViewModel viewmodel)
	{
		InitializeComponent();
		BindingContext = viewmodel;
	}
}