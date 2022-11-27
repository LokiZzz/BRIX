using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Base
{
    public partial class PopupViewModelBase<T> : ViewModelBase
    {
        public T PassInParameters { get; set; }
    }
}
