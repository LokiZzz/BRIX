namespace BRIX.Mobile.Services.Navigation
{
    public static class NavigationExtensions
    {
        public static T GetParameterOrDefault<T>(this IDictionary<string, object> query, string key)
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
    }
}
