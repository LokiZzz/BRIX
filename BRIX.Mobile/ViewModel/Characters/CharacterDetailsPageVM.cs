using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Messaging;

namespace BRIX.Mobile.ViewModel.Characters
{
    public class CharacterDetailsPageVM : ViewModelBase
    {
        public CharacterDetailsPageVM()
        {
            WeakReferenceMessenger.Default.Register<CurrentCharacterChanged>(
                this,
                async (r, m) => await Initialize(true)
            );
        }

        private Task Initialize(bool force = false) 
        {
            return Task.CompletedTask;
        }
    }
}
