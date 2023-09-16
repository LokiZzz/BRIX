using BRIX.Library.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public partial class DurationAspectModel : SpecificAspectModelBase<DurationAspect>
    {
        public DurationAspectModel(DurationAspect model) : base(model) { }

        public int Rounds
        {
            get => Internal.Rounds;
            set
            {
                SetProperty(Internal.Rounds, value, Internal,
                    (model, prop) => model.Rounds = prop);
            }
        }

        public bool CanDisableStatus
        {
            get => Internal.CanDisableStatus;
            set
            {
                SetProperty(Internal.CanDisableStatus, value, Internal, 
                    (model, prop) => model.CanDisableStatus = prop);
            }
        }
    }
}
