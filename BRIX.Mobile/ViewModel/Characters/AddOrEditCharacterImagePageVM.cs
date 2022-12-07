using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class AddOrEditCharacterImagePageVM : ViewModelBase, IQueryAttributable
    {

        private readonly IMediaPicker _media;
        private readonly ILocalStorage _localStorage;

        public AddOrEditCharacterImagePageVM(IMediaPicker media, ILocalStorage localStorage)
        {
            _media = media;
            _localStorage = localStorage;
        }

        [ObservableProperty]
        private CharacterModel _character = new();

        [RelayCommand]
        public async Task SelectImage()
        {
            if (_media.IsCaptureSupported)
            {
                FileResult photo = await _media.PickPhotoAsync();

                if (photo != null)
                {
                    string path = await _localStorage.CopyFileAsync(photo);
                }
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey(NavigationParameters.Character))
            {
                Character = query[NavigationParameters.Character] as CharacterModel;
            }
        }
    }
}
