using BRIX.Library.Abilities;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Characters;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Library.Extensions;
using BRIX.Mobile.Models.NPCs;
using BRIX.Mobile.Resources.Localizations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;

namespace BRIX.Mobile.ViewModel.NPCs
{
    public partial class FastNPCCreationVM : ObservableObject
    {
        public FastNPCCreationVM()
        {
            UpdatePotentialNPC();
        }

        public NPCModel PotentialNPC = new ();

        private int _fastHealth = 10;
        public int FastHealth
        {
            get => _fastHealth;
            set
            {
                if (SetProperty(ref _fastHealth, value))
                {
                    UpdatePotentialNPC();
                }
            }
        }

        private int _fastSpeed = 1;
        public int FastSpeed
        {
            get => _fastSpeed;
            set
            {
                if (SetProperty(ref _fastSpeed, value))
                {
                    UpdatePotentialNPC();
                }
            }
        }

        private int _fastAttackDistance = 1;
        public int FastAttackDistance
        {
            get => _fastAttackDistance;
            set
            {
                if (SetProperty(ref _fastAttackDistance, value))
                {
                    UpdatePotentialNPC();
                }
            }
        }

        private string _fastDamage = "1d4";
        public string FastDamage
        {
            get => _fastDamage;
            set
            {
                if (SetProperty(ref _fastDamage, value))
                {
                    UpdatePotentialNPC();
                }
            }
        }

        private int _fastPower;
        public int FastPower
        {
            get => _fastPower;
            set => SetProperty(ref _fastPower, value);
        }

        [RelayCommand]
        public void UpdateByDesiredPower()
        {
            int fastPower = FastPower;
            int healthPower = (fastPower * 1.5).Round();
            int attackPower = (fastPower * 0.5).Round();

            _fastHealth = CharacterCalculator.ExpToHealth(healthPower);

            if (_fastSpeed > 1)
            {
                int speedCost = new MoveCharacterEffect { DistancePerActionPoint = _fastSpeed }.GetExpCost();

                if (attackPower - speedCost >= 0)
                {
                    attackPower -= speedCost;
                }
                else
                {
                    _fastSpeed = 1;
                }
            }

            double distanceCoef = TargetSelectionAspect.GetDistanceCoeficient(_fastAttackDistance);
            int averageDamage = Math.Sqrt(attackPower / distanceCoef).Round();
            _fastDamage = DicePool.FromValue(averageDamage, 0.5).ToString();

            OnPropertyChanged(nameof(FastHealth));
            OnPropertyChanged(nameof(FastDamage));
            OnPropertyChanged(nameof(FastAttackDistance));
            OnPropertyChanged(nameof(FastSpeed));

            UpdatePotentialNPC();
        }

        private void UpdatePotentialNPC()
        {
            PotentialNPC.Abilities.Clear();
            PotentialNPC.Internal.Abilities.Clear();

            PotentialNPC.Health = FastHealth == 0 ? 10 : FastHealth;

            if (FastSpeed > 1)
            {
                Ability speedAbility = new();
                speedAbility.Name = $"{Localization.Speed} {FastSpeed}";
                speedAbility.AddEffect(new MoveCharacterEffect { DistancePerActionPoint = FastSpeed });
                PotentialNPC.AddAbility(new(speedAbility));
            }

            if (!string.IsNullOrEmpty(FastDamage))
            {
                if (!DicePool.TryParse(FastDamage, out DicePool? damage))
                {
                    damage = new DicePool((1, 4));
                }

                int attackDistance = FastAttackDistance > 1 ? FastAttackDistance : 1;

                Ability attackAbility = new()
                {
                    Name = $"{Localization.Attack} {damage?.ToString()}, {attackDistance} m"
                };
                DamageEffect dmgEffect = new() { Impact = damage ?? new DicePool((1, 4)) };
                dmgEffect.GetAspect<TargetSelectionAspect>().Strategy = ETargetSelectionStrategy.NTargetsAtDistanсeL;
                dmgEffect.GetAspect<TargetSelectionAspect>().NTAD.DistanceInMeters = FastAttackDistance;
                attackAbility.AddEffect(dmgEffect);
                PotentialNPC.AddAbility(new(attackAbility));
            }

            FastPower = PotentialNPC.Power;
        }
    }
}
