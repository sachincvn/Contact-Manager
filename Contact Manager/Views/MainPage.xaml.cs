using Contact_Manager.ViewModels;

namespace Contact_Manager;
public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel viewmodel)
    {
        InitializeComponent();
        BindingContext = viewmodel;
    }
}

