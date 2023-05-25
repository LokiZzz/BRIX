using BRIX.Library.Aspects;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public partial class CooldownAspectModel : AspectModelBase
    {
        public CooldownAspectModel(AspectBase model) : base(model) 
        { 
            ILocalizationResourceManager localization = ServicePool.GetService<ILocalizationResourceManager>();

            Options = new(Enum.GetValues<ECooldownOption>().Select(x => new CooldownOptionVM
            {
                Cooldown = x,
                LocalizedName = localization[x.ToString("G")].ToString()
            }));
        }

        public CooldownAspect Internal => GetSpecificAspect<CooldownAspect>();

        public CooldownOptionVM ObstacleBetweenTargetsInChain
        {
            get
            {
                ECooldownOption cooldown = Internal.Condition;

                return Options.FirstOrDefault(x => x.Cooldown == cooldown);
            }
            set
            {
                ECooldownOption cooldown = value.Cooldown;
                SetProperty(Internal.Condition, cooldown, Internal,
                    (model, prop) => model.Condition = prop);

                if(cooldown == ECooldownOption.NoneCooldown || cooldown == ECooldownOption.CannotReset)
                {
                    UsesCount = 0;
                }

                UpdateCost();
            }
        }

        private ObservableCollection<CooldownOptionVM> _options = new();
        public ObservableCollection<CooldownOptionVM> Options
        {
            get => _options;
            set => SetProperty(ref _options, value);
        }

        public int UsesCount
        {
            get => Internal.UsesCount;
            set
            {
                SetProperty(Internal.UsesCount, value, Internal,
                    (model, prop) => model.UsesCount = prop);
                UpdateCost();
            }
        }
    }

    public class CooldownOptionVM
    {
        public string LocalizedName { get; set; }

        public ECooldownOption Cooldown { get; set; }

        public override string ToString() => LocalizedName;
    }
}
