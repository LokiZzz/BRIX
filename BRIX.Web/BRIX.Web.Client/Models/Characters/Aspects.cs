using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.Aspects.Base;

namespace BRIX.Web.Client.Models.Characters
{
    public class Aspects
    {
        public static Dictionary<Type, AspectVM> Collection => new()
        {
            { typeof(TargetSelectionAspect), new AspectVM() {
                Route = "tse", Icon = "ra ra-archery-target"
            }},
            { typeof(TargetSizeAspect), new AspectVM() {
                Route = "size", Icon = "ra ra-double-team"
            }},
            { typeof(ActivationConditionsAspect), new AspectVM() {
                Route = "cond", Icon = "ra ra-key-basic"
            }},
            { typeof(DurationAspect), new AspectVM() {
                Route = "dur", Icon = "ra ra-stopwatch"
            }},
            { typeof(AOEAspect), new AspectVM() {
                Route = "aoe", Icon = "ra ra-fire-ring"
            }},
            { typeof(VampirismAspect), new AspectVM() {
                Route = "vamp", Icon = "ra ra-bat-sword"
            }},
        };
    }

    public class AspectVM
    {
        public string Title { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public string Route { get; set; } = string.Empty;

        public AspectBase? Model { get; set; }
    }
}
