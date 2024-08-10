using BRIX.Library.Aspects;
using BRIX.Library.Enums;
using BRIX.Mobile.Services;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public partial class DurationAspectModel(DurationAspect model) : SpecificAspectModelBase<DurationAspect>(model)
    {
        public int Duration
        {
            get => Internal.Duration;
            set
            {
                SetProperty(Internal.Duration, value, Internal,
                    (model, prop) => model.Duration = prop);
            }
        }

        public bool CanDisableStatus
        {
            get => Internal.CanDisableStatus;
            set
            {
                SetProperty(Internal.CanDisableStatus, value, Internal, 
                    (model, prop) => model.CanDisableStatus = prop);
            }
        }
    }

    public class TimeUnitOption
    {
        public string Name { get; set; } = string.Empty;

        public override string ToString() => Name;
    }
}