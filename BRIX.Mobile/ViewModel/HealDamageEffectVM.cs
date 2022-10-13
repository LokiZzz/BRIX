using BRIX.Library.Aspects;
using BRIX.Library.Effects;
using BRIX.Library.Mathematics;
using BRIX.Mobile.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel
{
    public class HealDamageEffectVM : ViewModelBase
    {
        private HealDamageEffect _effect;

        public HealDamageEffectVM()
        {
            _effect = new HealDamageEffect();
            _effect.GetAspect<TargetSelectionAspect>().Strategy = ETargetType.NTargetsAtDistanсeL;

            Damage = 10;
            ActionPoints = 2;
            MaxTargetDistance = 1;
            TargetCount = 1;
        }

		private int _damage;
		public int Damage
		{
			get => _damage;
            set
            {
                SetProperty(ref _damage, value);
                _effect.Impact = new Library.DiceValue.DicePool(value);
                OnPropertyChanged(nameof(ExperienceCost));
            }
        }

        private int _actionPoints;
        public int ActionPoints
        {
            get => _actionPoints;
            set
            {
                SetProperty(ref _actionPoints, value);
                _effect.GetAspect<ActionPointAspect>().ActionPoints = value;
                OnPropertyChanged(nameof(ExperienceCost));
                OnPropertyChanged(nameof(ActionPointsModifierString));
            }
        }

        public string ActionPointsModifierString => $"{_effect.GetAspect<ActionPointAspect>().GetCoefficient().ToPercent()}%";

        private int _maxTargetDistance;
        public int MaxTargetDistance
        {
            get => _maxTargetDistance;
            set
            {
                SetProperty(ref _maxTargetDistance, value);
                _effect.GetAspect<TargetSelectionAspect>().NTAD.DistanceInMeters = value;
                OnPropertyChanged(nameof(ExperienceCost));
                OnPropertyChanged(nameof(MaxTargetDistanceModifierString));
            }
        }

        public string MaxTargetDistanceModifierString => $"{_effect.GetAspect<TargetSelectionAspect>().GetNTADDistanceCoef().ToPercent()}%";

        private int _targetCount;
        public int TargetCount
        {
            get => _targetCount;
            set
            {
                SetProperty(ref _targetCount, value);
                _effect.GetAspect<TargetSelectionAspect>().NTAD.TargetsCount = value;
                OnPropertyChanged(nameof(ExperienceCost));
                OnPropertyChanged(nameof(MaxTargetCountModifierString));
            }
        }

        public string MaxTargetCountModifierString => $"{_effect.GetAspect<TargetSelectionAspect>().GetNTADCountCoeficient().ToPercent()}%";

        public int ExperienceCost => _effect.GetExpCost();
    }
}
