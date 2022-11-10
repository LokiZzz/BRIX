using BRIX.Mobile.Services;
using BRIX.Mobile.View.Account;
using BRIX.Mobile.ViewModel.Account;

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
    }

    public static void RegisterViews(this MauiAppBuilder builder)
	{
		builder.Services.AddTransient<SignInPage>();
        builder.Services.AddTransient<SignInPageVM>();
    }
}
