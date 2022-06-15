using Contact_Manager.ViewModels;
using Contact_Manager.Views;
using AutoMapper;

namespace Contact_Manager;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<ContactDetailViewModel>();
        builder.Services.AddTransient<AddContactDetailViewModel>();

		 
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<ContactDetail>();
        builder.Services.AddTransient<AddContactDetail>();

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return builder.Build();
	}
}
