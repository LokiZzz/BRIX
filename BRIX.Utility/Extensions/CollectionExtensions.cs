namespace BRIX.Utility.Extensions
{
    public static class CollectionExtensions
    {
        public static T Random<T>(this IEnumerable<T> collection)
        {
            int randomIndex = new Random().Next(0, collection.Count() - 1);

            return collection.ElementAt(randomIndex);
        }
    }
}
