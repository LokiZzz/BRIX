﻿using BRIX.Library.Effects.HealDamage;
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
        private HealOrDamageEffect _effect;

        public HealDamageEffectVM()
        {
            _effect = new HealOrDamageEffect();
            _effect.GetAspect<TargetSelectionAspect>().Strategy = ETargetType.NTargetsAtDistanсeL;
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
            }
        }

        private int _maxTargetDistance;
        public int MaxTargetDistance
        {
            get => _maxTargetDistance;
            set
            {
                SetProperty(ref _maxTargetDistance, value);
                _effect.GetAspect<TargetSelectionAspect>().NTAD.DistanceInMeters = value;
                OnPropertyChanged(nameof(ExperienceCost));
            }
        }

        private int _targetCount;
        public int TargetCount
        {
            get => _targetCount;
            set
            {
                SetProperty(ref _targetCount, value);
                _effect.GetAspect<TargetSelectionAspect>().NTAD.TargetsCount = value;
                OnPropertyChanged(nameof(ExperienceCost));
            }
        }

        public int ExperienceCost => _effect.GetExpCost();
    }
}