using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.Effects;
using BRIX.Web.Client.Localization;

namespace BRIX.Web.Client.Models.Characters
{
    public class Aspects
    {
        public static Dictionary<Type, AspectViewModel> Collection => new()
        {
            { typeof(TargetSelectionAspect), new AspectViewModel() {
                Name = "",
                PageRoute = ""
            }},
            { typeof(TargetSizeAspect), new AspectViewModel() {
                Name = "",
                PageRoute = ""
            }},
            { typeof(ActivationConditionsAspect), new AspectViewModel() {
                Name = "",
                PageRoute = ""
            }},
            { typeof(DurationAspect), new AspectViewModel() {
                Name = "",
                PageRoute = ""
            }},
            { typeof(AOEAspect), new AspectViewModel() {
                Name = "",
                PageRoute = ""
            }},
            { typeof(VampirismAspect), new AspectViewModel() {
                Name = "",
                PageRoute = ""
            }},
        };
    }

    public class AspectViewModel
    {
        public string Name { get; set; }

        public string PageRoute { get; set; }
    }
}
