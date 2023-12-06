using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class EditCharacterImagePageVM : ViewModelBase, IQueryAttributable
    {
        private readonly ICharacterService _characterService;

        public EditCharacterImagePageVM(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        private CharacterModel? _character;
        public CharacterModel? Character
        {
            get => _character;
            set => SetProperty(ref _character, value);
        }

        private ImageSource? _image;
        public ImageSource? Image
        {
            get => _image;
            set
            {
                SetProperty(ref _image, value);
                OnPropertyChanged(nameof(ShowPlaceholder));
            }
        }

        public bool ShowPlaceholder => Image == null;


        private double _contentX;
        public double ContentX
        {
            get => _contentX;
            set => SetProperty(ref _contentX, value);
        }

        private double _contentY;
        public double ContentY
        {
            get => _contentY;
            set => SetProperty(ref _contentY, value);
        }

        private double _contentScale = 1;
        public double ContentScale
        {
            get => _contentScale;
            set => SetProperty(ref _contentScale, value);
        }

        private string _imagePath = string.Empty;

        [RelayCommand]
        public async Task Browse()
        {
            PickOptions options = new()
            {
                PickerTitle = Localization.SelectCharacterPortrait,
                FileTypes = FilePickerFileType.Images,
            };

            FileResult? result = await FilePicker.Default.PickAsync(options);

            if (result != null)
            {
                ContentX = 0;
                ContentY = 0;
                ContentScale = 1;
                _imagePath = result.FullPath;
                Image = ImageSource.FromFile(result.FullPath);
            }
        }

        [RelayCommand]
        public async Task Save()
        {
            if(Character == null)
            {
                return;
            }

            Character.InternalModel.Portrait.X = ContentX;
            Character.InternalModel.Portrait.Y = ContentY;
            Character.InternalModel.Portrait.S = ContentScale;
            Character.InternalModel.Portrait.Path = _imagePath;

            await _characterService.UpdateAsync(Character.InternalModel);

            await Navigation.Back();
        }

        [RelayCommand]
        public void Clear()
        {
            Image = null;
            _imagePath = string.Empty;
            ContentX = 0;
            ContentY = 0;
            ContentScale = 1;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Character = query.GetParameterOrDefault<CharacterModel>(NavigationParameters.Character);

            if(Character != null && !string.IsNullOrEmpty(Character.InternalModel.Portrait?.Path))
            {
                _imagePath = Character.InternalModel.Portrait.Path;
                FileResult file = new(_imagePath);
                Image = ImageSource.FromFile(file.FullPath);

                ContentX = Character.InternalModel.Portrait.X;
                ContentY = Character.InternalModel.Portrait.Y;
                ContentScale = Character.InternalModel.Portrait.S;
            }

            query.Clear();
        }
    }
}
