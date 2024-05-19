using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.Services;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Abilities.Effects
{
    public partial class MoveTargetEffectPageVM(ILocalizationResourceManager localization) 
        : EffectPageVMBase<MoveTargetEffectModel>
    {
        private readonly ILocalizationResourceManager _localization = localization;

        private TargetPathVM _selectedPath = new();
        public TargetPathVM SelectedPath
        {
            get => _selectedPath;
            set
            {
                SetProperty(ref _selectedPath, value);

                if (Effect != null)
                {
                    Effect.Path = value.Path;
                }
            }
        }

        public ObservableCollection<TargetPathVM> _paths = [];
        public ObservableCollection<TargetPathVM> Paths
        {
            get => _paths;
            set => SetProperty(ref _paths, value);
        }

        protected override void HandleInitial(IDictionary<string, object> query)
        {
            base.HandleInitial(query);

            Paths = new(
                Enum.GetValues<EMoveTargetPath>().Select(x =>
                    new TargetPathVM { Path = x, Text = GetMovePathText(x) }
                )
            );

            SelectedPath = Paths.FirstOrDefault(x => x.Path == Effect?.Path)
                ?? throw new Exception("Путь не инициализирован.");
        }

        private string GetMovePathText(EMoveTargetPath path)
        {
            return _localization[path.ToString()].ToString() 
                ?? "Ошибка: не найден строковый ресурс.";
        }
    }

    public class TargetPathVM
    {
        public EMoveTargetPath Path { get; set; }

        public string Text { get; set; } = string.Empty;

        public override string ToString() => Text;
    }
}
