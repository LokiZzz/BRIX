using BRIX.Library.Enums;
using BRIX.Mobile.Services;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public static class ObstacleOptionHelper
    {
        public static ObservableCollection<ObstacleOptionVM> GetOptions(ILocalizationResourceManager localization)
        {
            IEnumerable<ObstacleOptionVM> options = Enum.GetValues<EObstacleEquivalent>()
                .Where(x => x != EObstacleEquivalent.None)
                .Select(x => new ObstacleOptionVM { 
                    LocalizedName = localization[x.ToString("G")].ToString() ?? string.Empty, 
                    Equivalent = x 
                });

            return new(options);
        }
    }

    public class ObstacleOptionVM
    {
        public string LocalizedName { get; set; } = string.Empty;

        public EObstacleEquivalent Equivalent { get; set; }

        public override string ToString() => LocalizedName;
    }
}
