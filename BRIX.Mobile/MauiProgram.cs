using BRIX.Mobile.Services;
using BRIX.Mobile.View.Account;
using BRIX.Mobile.View.Character;
using BRIX.Mobile.ViewModel.Account;
using BRIX.Mobile.ViewModel.Character;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BRIX.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		MauiAppBuilder builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
        builder.RegisterServices();
        builder.RegisterViews();

        return builder.Build();
	}

    public static void RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IAccountService, AccountServiceMoq>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
    }

    public static void RegisterViews(this MauiAppBuilder builder)
	{
        builder.RegisterView<SignInPage, SignInPageVM>();
        builder.RegisterView<CurrentCharacterPage, CurrentCharacterPageVM>();
    }

    private static void RegisterView<TView, TViewModel>(this MauiAppBuilder builder) 
        where TView : ContentPage where TViewModel : ObservableObject
    {
        builder.Services.AddTransient<TView>();
        builder.Services.AddTransient<TViewModel>();
        Routing.RegisterRoute(typeof(TView).Name, typeof(TView));
    }
}
