using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.View.Popups
{
    public partial class ParametrizedPopup<TParameters> : Popup
    {
        public TParameters Parameters { get; set; }
    }
}
