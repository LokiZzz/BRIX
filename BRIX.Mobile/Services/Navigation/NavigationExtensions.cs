namespace BRIX.Mobile.Services.Navigation
{
    public static class NavigationExtensions
    {
        public static T? GetParameterOrDefault<T>(this IDictionary<string, object> query, string key)
        {
            if (query.TryGetValue(key, out object? value) && value != null)
            {
                return (T)value;
            }
            else
            {
                return default;
            }
        }
    }
}
