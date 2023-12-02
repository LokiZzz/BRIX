namespace BRIX.Mobile.Services
{
    /// <summary>
    /// Статический репозиторий для ServiceProvider-а.
    /// </summary>
    public static class Resolver
    {
        private static IServiceProvider? _serviceProvider;

        public static IServiceProvider ServiceProvider => _serviceProvider 
            ?? throw new Exception("Service provider has not been initialized");

        public static void RegisterServiceProvider(IServiceProvider services)
        {
            if(_serviceProvider != null)
            {
                throw new Exception(
                    "The service provider must be registered only once, at the app start."
                );
            }

            _serviceProvider = services;
        }

        public static T Resolve<T>() where T : class
            => ServiceProvider.GetRequiredService<T>();
    }
}
