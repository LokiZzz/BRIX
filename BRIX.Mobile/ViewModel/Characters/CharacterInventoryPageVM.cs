using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Characters
{
    public class CharacterInventoryPageVM : ViewModelBase
    {
        public CharacterInventoryPageVM()
        {
            WeakReferenceMessenger.Default.Register<CurrentCharacterChanged>(
                this,
                async (r, m) => await Initialize(true)
            );
        }

        private async Task Initialize(bool force = false) { }
    }
}
