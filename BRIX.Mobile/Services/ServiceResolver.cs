namespace BRIX.Mobile.Services
{
    /// <summary>
    /// Статический репозиторий для ServiceProvider-а.
    /// </summary>
    public static class Resolver
    {
        private static IServiceProvider? _serviceProvider;

        public static IServiceProvider ServiceProvider => _serviceProvider 
            ?? throw new Exception("Service provider не инициализирован");

        public static void RegisterServiceProvider(IServiceProvider services)
        {
            if(_serviceProvider != null)
            {
                throw new Exception(
                    "Service provider должен быть инициализирован только единожды, во время старта приложения."
                );
            }

            _serviceProvider = services;
        }

        public static T Resolve<T>() where T : class
            => ServiceProvider.GetRequiredService<T>();
    }
}
