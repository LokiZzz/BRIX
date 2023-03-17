using BRIX.Library.DiceValue;
using BRIX.Library;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.Models.Abilities.Effects;
using CommunityToolkit.Mvvm.ComponentModel;
using BRIX.Library.Aspects;
using BRIX.Mobile.Models.Abilities.Aspects;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public partial class ActionPointAspectPageVM : ViewModelBase
    {
        [ObservableProperty]
        private DamageEffectModel _damage = new();

        [ObservableProperty]
        private AbilityModel _ability = new();

        [ObservableProperty]
        private ActionPointsAspectModel _aspect;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Ability = query.GetParameterOrDefault<AbilityModel>(NavigationParameters.Ability) ?? new();
            Damage = query.GetParameterOrDefault<DamageEffectModel>(NavigationParameters.Effect) ?? new();
            Aspect = query.GetParameterOrDefault<ActionPointsAspectModel>(NavigationParameters.Aspect) ?? new();

            Damage.UpdateAspect(Aspect);
            Ability.UpdateEffect(Damage);

            query.Clear();
        }
    }
}
