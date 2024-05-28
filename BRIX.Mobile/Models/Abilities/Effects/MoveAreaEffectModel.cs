using BRIX.Library.Effects;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public class MoveAreaEffectModel(MoveAreaEffect effect) : EffectGenericModelBase<MoveAreaEffect>(effect)
    {
        public MoveAreaEffectModel() : this(new MoveAreaEffect()) { }

        public int MaxAreaCount
        {
            get => Internal.MaxAreaCount;
            set
            {
                SetEffectProperty(Internal.MaxAreaCount, value, Internal, (model, prop) => {
                    model.MaxAreaCount = prop;
                });
            }
        }

        public int DistanceToArea
        {
            get => Internal.DistanceToArea;
            set
            {
                SetEffectProperty(Internal.DistanceToArea, value, Internal, (model, prop) => {
                    model.DistanceToArea = prop;
                });
            }
        }

        public int MovingDistance
        {
            get => Internal.MovingDistance;
            set
            {
                SetEffectProperty(Internal.MovingDistance, value, Internal, (model, prop) => {
                    model.MovingDistance = prop;
                });
            }
        }

        public int MaxAbilityPower
        {
            get => Internal.MaxAbilityPower;
            set
            {
                SetEffectProperty(Internal.MaxAbilityPower, value, Internal, (model, prop) => {
                    model.MaxAbilityPower = prop;
                });
            }
        }

        public bool OnlyYourAbilities
        {
            get => Internal.OnlyYourAbilities;
            set
            {
                SetEffectProperty(Internal.OnlyYourAbilities, value, Internal, (model, prop) => {
                    model.OnlyYourAbilities = prop;
                });
            }
        }
    }
}
