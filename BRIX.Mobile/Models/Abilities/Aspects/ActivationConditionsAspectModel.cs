using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Enums;
using BRIX.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public partial class ActivationConditionsAspectModel : SpecificAspectModelBase<ActivationConditionsAspect>
    {
        public ActivationConditionsAspectModel(ActivationConditionsAspect model) : base(model)
        {
            ILocalizationResourceManager localization = Resolver.Resolve<ILocalizationResourceManager>();

            Conditions = new(Enum.GetValues<EActivationCondition>().Select(x => 
                new ActivationConditionOptionVM
                {
                    Condition = x,
                    Text = localization[x.ToString("G")].ToString() ?? string.Empty
                }
            ));
        }

        private ObservableCollection<ActivationConditionOptionVM> _conditions = new();
        public ObservableCollection<ActivationConditionOptionVM> Conditions
        {
            get => _conditions;
            set => SetProperty(ref _conditions, value);
        }

        private ObservableCollection<ActivationConditionOptionVM> _selectedConditions = new();
        public ObservableCollection<ActivationConditionOptionVM> SelectedConditions
        {
            get => _selectedConditions;
            set => SetProperty(ref _selectedConditions, value);
        }
    }

    public class ActivationConditionOptionVM
    {
        public string Text { get; set; }

        public EActivationCondition Condition { get; set; }

        public override string ToString() => Text;
    }
}
