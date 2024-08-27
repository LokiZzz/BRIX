using BRIX.Library.Abilities;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Characters;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Mobile.Models.NPCs;
using BRIX.Mobile.Resources.Localizations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Security.Cryptography.X509Certificates;

namespace BRIX.Mobile.ViewModel.NPCs
{
    public partial class FastNPCCreationVM : ObservableObject
    {
        public NPCModel PotentialNPC = new ();

        private int _fastHealth = 10;
        public int FastHealth
        {
            get => _fastHealth;
            set
            {
                SetProperty(ref _fastHealth, value);
                UpdatePower();
            }
        }

        private int _fastAttackDistance = 1;
        public int FastAttackDistance
        {
            get => _fastAttackDistance;
            set
            {
                SetProperty(ref _fastAttackDistance, value);
                UpdatePower();
            }
        }

        private string _fastDamage = "1d4";
        public string FastDamage
        {
            get => _fastDamage;
            set
            {
                SetProperty(ref _fastDamage, value);
                UpdatePower();
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
            NPC npc = new()
            {
                Health = 5,
                Speed = new CharacterSpeed()
            };

            Ability damageAbility = new();
            DamageEffect damageEffect = new() { Impact = new DicePool(2) };
            damageEffect.GetAspect<TargetSelectionAspect>().NTAD.DistanceInMeters = _fastAttackDistance;
            damageAbility.AddEffect(damageEffect);
            npc.Abilities.Clear();
            npc.Abilities.Add(damageAbility);

            // Грубая подстройка
            while(npc.Power < _fastPower / 1.2)
            {
                npc.Health += 1;
            }

            while (npc.Power < _fastPower)
            {
                damageEffect.Impact.Modifier += 1;
            }

            // Более тонкая подстройка
            while (npc.Power > _fastPower && npc.Health >= 4)
            {
                npc.Health -= 1;
            }

            if (damageEffect.Impact.Modifier > 2)
            {
                damageEffect.Impact = DicePool.FromValue(damageEffect.Impact.Modifier, 0.5);
            }

            FastHealth = npc.Health;
            FastDamage = damageEffect.Impact.ToString();

            UpdatePower();
        }

        private void UpdatePower()
        {
            NPC npc = new()
            {
                Health = _fastHealth,
                Speed = new CharacterSpeed()
            };

            Ability damageAbility = new()
            {
                Name = $"{Localization.Attack} {_fastDamage}, {_fastAttackDistance} m"
            };

            _ = DicePool.TryParse(_fastDamage, out DicePool? damage);
            DamageEffect damageEffect = new() { Impact = damage ?? new DicePool(0) };
            damageEffect.GetAspect<TargetSelectionAspect>().NTAD.DistanceInMeters = _fastAttackDistance;
            damageAbility.AddEffect(damageEffect);
            npc.Abilities.Clear();
            npc.Abilities.Add(damageAbility);

            FastPower = npc.Power;
            PotentialNPC = new(npc);
        }
    }
}
