using BRIX.Library.Aspects;
using BRIX.Library.Enums;
using BRIX.Mobile.Services;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public partial class DurationAspectModel : SpecificAspectModelBase<DurationAspect>
    {
        public DurationAspectModel(DurationAspect model) : base(model) 
        {
            ILocalizationResourceManager localization = ServicePool.GetService<ILocalizationResourceManager>();

            UnitOptions = new(Enum.GetValues<ETimeUnit>().Select(x => new TimeUnitOption
            {
                Name = localization[x.ToString("G")].ToString(),
                Unit = x
            }));

            SelectedUnit = UnitOptions.FirstOrDefault(x => x.Unit == Internal.Unit);
        }

        public ObservableCollection<TimeUnitOption> UnitOptions { get; set; }

        public TimeUnitOption SelectedUnit
        {
            get => UnitOptions.FirstOrDefault(x => x.Unit == Internal.Unit);
            set
            {
                SetProperty(Internal.Unit, value.Unit, Internal,
                    (model, prop) => model.Unit = prop);
            }
        }

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
        public string Name { get; set; }

        public ETimeUnit Unit { get; set; }

        public override string ToString() => Name;
    }
}
