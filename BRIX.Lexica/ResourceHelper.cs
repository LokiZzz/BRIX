using System.Resources;

namespace BRIX.Lexica
{
    public static class ResourceHelper
    {
        public static string GetResourceString(string resourceName)
        {
            try
            {
                ResourceManager resources = new ResourceManager(
                    "BRIX.Lexica.Resources", 
                    typeof(LexisProvider).Assembly
                );

                return resources.GetString(resourceName) ?? string.Empty;
            }
            catch(MissingManifestResourceException)
            {
                return string.Empty;
            }
        }
    }
}
