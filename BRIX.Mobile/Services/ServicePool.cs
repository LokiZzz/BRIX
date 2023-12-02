namespace BRIX.Mobile.Services
{
    public class ServicePool
    {
        public static TService GetService<TService>() => Current.GetService<TService>();

        public static IServiceProvider Current =>
#if WINDOWS10_0_17763_0_OR_GREATER
			MauiWinUIApplication.Current.Services;

#elif ANDROID
            MauiApplication.Current.Services;

#elif IOS || MACCATALYST
            MauiUIApplicationDelegate.Current.Services;
#else
			null;
#endif

        //private readonly IPlatformApplication _platformApplication;

        //public ServicePool(IPlatformApplication platformApplication)
        //{
        //    _platformApplication = platformApplication;
        //}

        //public TService? GetService<TService>()
        //{
        //    return _platformApplication.Services.GetService<TService>();
        //}
    }
}
