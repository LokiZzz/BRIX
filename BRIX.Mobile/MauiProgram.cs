using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Account;
using BRIX.Mobile.View.Characters;
using BRIX.Mobile.ViewModel.Account;
using BRIX.Mobile.ViewModel.Characters;
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
        builder.Services.AddSingleton<ICharacterService, JsonCharacterService>();
        builder.Services.AddSingleton<ILocalStorage, LocalStorage>();
    }

    public static void RegisterViews(this MauiAppBuilder builder)
	{
        builder.RegisterView<SignInPage, SignInPageVM>();
        builder.RegisterView<CurrentCharacterPage, CurrentCharacterPageVM>(false);
        builder.RegisterView<CharacterListPage, CharacterListPageVM>();
        builder.RegisterView<AddOrEditCharacterPage, AddOrEditCharacterPageVM>();
    }

    private static void RegisterView<TView, TViewModel>(this MauiAppBuilder builder, bool registerRoute = true)
        where TView : ContentPage where TViewModel : ObservableObject
    {
        builder.Services.AddTransient<TView>();
        builder.Services.AddTransient<TViewModel>();

        if (registerRoute)
        {
            Routing.RegisterRoute(typeof(TView).Name, typeof(TView));
        }
    }
}
