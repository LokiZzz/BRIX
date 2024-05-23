using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.Services;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Abilities.Effects
{
    public partial class MoveCharacterEffectPageVM(ILocalizationResourceManager localization) 
        : EffectPageVMBase<MoveCharacterEffectModel>
    {
        private readonly ILocalizationResourceManager _localization = localization;

        private MovingTypeVM _selectedPath = new();
        public MovingTypeVM SelectedMovingType
        {
            get => _selectedPath;
            set
            {
                SetProperty(ref _selectedPath, value);

                if (Effect != null)
                {
                    Effect.MovingType = value.MovingType;
                }
            }
        }

        public ObservableCollection<MovingTypeVM> _movingTypes = [];
        public ObservableCollection<MovingTypeVM> MovingTypes
        {
            get => _movingTypes;
            set => SetProperty(ref _movingTypes, value);
        }

        protected override void HandleInitial(IDictionary<string, object> query)
        {
            base.HandleInitial(query);

            MovingTypes = new(
                Enum.GetValues<ECharacterMovingType>().Select(x =>
                    new MovingTypeVM { MovingType = x, Text = GetMovePathText(x) }
                )
            );

            SelectedMovingType = MovingTypes.FirstOrDefault(x => x.MovingType == Effect?.MovingType)
                ?? throw new Exception("Путь не инициализирован.");
        }

        private string GetMovePathText(ECharacterMovingType path)
        {
            return _localization[path.ToString()].ToString() 
                ?? "Ошибка: не найден строковый ресурс.";
        }
    }

    public class MovingTypeVM
    {
        public ECharacterMovingType MovingType { get; set; }

        public string Text { get; set; } = string.Empty;

        public override string ToString() => Text;
    }
}
