namespace BRIX.Mobile.Services.Navigation
{
    public static class NavigationExtensions
    {
        public static T? GetParameterOrDefault<T>(this IDictionary<string, object> query, string key)
        {
            if (query.ContainsKey(key))
            {
                return (T)query[key];
            }
            else
            {
                return default;
            }
        }

        public static T? GetParameterOrNull<T>(this IDictionary<string, object> query, string key) where T : class
        {
            if (query.ContainsKey(key))
            {
                return query[key] as T;
            }
            else
            {
                return null;
            }
        }
    }
}
