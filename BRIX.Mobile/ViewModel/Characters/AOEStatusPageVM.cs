using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class AOEStatusPageVM : ViewModelBase, IQueryAttributable
    {
        EEditingMode _mode;

        private string _title;
		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

        private StatusItemVM _status;
        public StatusItemVM Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        [RelayCommand]
        public async Task Save()
        {
            await Navigation.Back(
                stepsBack: 1,
                (NavigationParameters.EditMode, _mode),
                (NavigationParameters.Status, Status)
            );
        }

        [RelayCommand]
        public async Task AddEffect()
        {

        }

        [RelayCommand]
        public async Task EditEffect(EffectModelBase effect)
        {

        }

        [RelayCommand]
        public async Task DeleteEffect(EffectModelBase effect)
        {
            AlertPopupResult result = await Ask(Localization.DeleteEffectQuestion);

            if (result?.Answer == EAlertPopupResult.Yes)
            {
                Status.RemoveEffect(effect);
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);
            Status = query.GetParameterOrDefault<StatusItemVM>(NavigationParameters.Status)
                ?? new StatusItemVM(new Library.Ability.Status());

            InitializeTitle();
        }

        private void InitializeTitle()
        {
            switch(_mode)
            {
                case EEditingMode.Add:
                    Title = Localization.AddStatus;
                    break;
                case EEditingMode.Edit:
                    Title = Localization.EditStatus;
                    break;
            }
        }
    }
}
