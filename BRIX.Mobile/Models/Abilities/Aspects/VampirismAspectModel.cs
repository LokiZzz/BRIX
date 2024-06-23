using BRIX.Library.Aspects;
using BRIX.Library.Enums;
using BRIX.Mobile.Services;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public partial class VampirismAspectModel : SpecificAspectModelBase<VampirismAspect>
    {
        public VampirismAspectModel(VampirismAspect model) : base(model) { }

        public int VampirismPercent
        {
            get => Internal.VampirismPercent;
            set
            {
                SetProperty(Internal.VampirismPercent, value, Internal,
                    (model, prop) => model.VampirismPercent = prop);
            }
        }
    }
}
