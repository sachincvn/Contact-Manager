using Contact_Manager.Views;

namespace Contact_Manager;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(AddContactDetail), typeof(AddContactDetail));
        Routing.RegisterRoute(nameof(ContactDetail), typeof(ContactDetail));
    }
}
