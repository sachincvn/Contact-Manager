using Contact_Manager.ViewModels;

namespace Contact_Manager.Views;

public partial class AddContactDetail : ContentPage
{
	public AddContactDetail(AddContactDetailViewModel viewmodel)
	{
        InitializeComponent();
        BindingContext = viewmodel;
    }
}