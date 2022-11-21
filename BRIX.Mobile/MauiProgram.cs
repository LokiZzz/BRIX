using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Account;
using BRIX.Mobile.View.Characters;
using BRIX.Mobile.View.Settings;
using BRIX.Mobile.ViewModel.Account;
using BRIX.Mobile.ViewModel.Characters;
using BRIX.Mobile.ViewModel.Settings;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BRIX.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		MauiAppBuilder builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("EBGaramond-Regular.ttf", "Garamond");
				fonts.AddFont("SourceSansPro-Regular.ttf", "SourceSansPro");
				fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "Awesome");
				fonts.AddFont("Font Awesome 6 Brands-Regular-400.otf", "AwesomeBrands");
				fonts.AddFont("rpgawesome-webfont.ttf", "AwesomeRPG");
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
        builder.Services.AddSingleton<ILocalizationResourceManager, LocalizationResourceManager>();
    }

    public static void RegisterViews(this MauiAppBuilder builder)
	{
        builder.RegisterView<SignInPage, SignInPageVM>();

        builder.RegisterView<CurrentCharacterPage, CurrentCharacterPageVM>(false);
        builder.RegisterView<CharacterAbilitiesPage, CharacterAbilitiesPageVM>(false);
        builder.RegisterView<CharacterInventoryPage, CharacterInventoryPageVM>(false);
        builder.RegisterView<CharacterDetailsPage, CharacterDetailsPageVM>(false);
        builder.RegisterView<CharacterListPage, CharacterListPageVM>();
        builder.RegisterView<AddOrEditCharacterPage, AddOrEditCharacterPageVM>();

        builder.RegisterView<SettingsPage, SettingsPageVM>(false);
        builder.RegisterView<SelectLanguagePage, SelectLanguagePageVM>();
    }

    /// <param name="registerRoute"> Set false if view already registered in AppShell.xaml </param>
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
