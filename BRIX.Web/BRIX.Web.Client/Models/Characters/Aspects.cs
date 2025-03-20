using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.Effects;
using BRIX.Web.Client.Localization;

namespace BRIX.Web.Client.Models.Characters
{
    public class Aspects
    {
        public static Dictionary<Type, AspectVM> Collection => new()
        {
            { typeof(TargetSelectionAspect), new AspectVM() {
                Route = "tse", Icon = ""
            }},
            { typeof(TargetSizeAspect), new AspectVM() {
                Route = "size", Icon = ""
            }},
            { typeof(ActivationConditionsAspect), new AspectVM() {
                Route = "cond", Icon = ""
            }},
            { typeof(DurationAspect), new AspectVM() {
                Route = "dur", Icon = ""
            }},
            { typeof(AOEAspect), new AspectVM() {
                Route = "aoe", Icon = ""
            }},
            { typeof(VampirismAspect), new AspectVM() {
                Route = "vamp", Icon = ""
            }},
        };
    }

    public class AspectVM
    {
        public string Title { get; set; } = string.Empty;

        public string LexisDescription { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public string Route { get; set; } = string.Empty;
    }
}
