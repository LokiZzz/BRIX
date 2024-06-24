using BRIX.Library.Effects;
namespace BRIX.Mobile.Models.Abilities.Effects
{
    public class DangerousTerrainEffectModel(DangerousTerrainEffect effect) 
        : EffectGenericModelBase<DangerousTerrainEffect>(effect)
    {
        public DangerousTerrainEffectModel() : this(new DangerousTerrainEffect()) { }

        public bool IsAreaDisposable
        {
            get => Internal.IsAreaDisposable;
            set
            {
                SetEffectProperty(Internal.IsAreaDisposable, value, Internal, (model, prop) => {
                    model.IsAreaDisposable = prop;
                });
            }
        }
    }
}
