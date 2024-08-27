using BRIX.Library.Characters;
using BRIX.Library.DiceValue;
using BRIX.Library.Items;
using BRIX.Mobile.Models.Abilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Inventory
{
    public partial class InventoryItemVM : ObservableObject
    {
        public InventoryItemVM(Item model)
        {
            InternalModel = model;

            if (model is Artifact artifact)
            {
                Features = new(artifact.Features.Select(x => new ArtifactFeatureModel(x)));
            }
        }

        private Item _internalModel = new();
        public Item InternalModel
        {
            get => _internalModel;
            set
            {
                _internalModel = value;

                switch(value)
                {
                    case Container:
                        Type = EInventoryItemType.Container;
                        break;
                    case Artifact:
                        Type = EInventoryItemType.Artifact;
                        break;
                    case Item:
                        Type = EInventoryItemType.Thing;
                        break;
                }
            }
        }

        private ObservableCollection<ArtifactFeatureModel> _features = [];
        public ObservableCollection<ArtifactFeatureModel> Features
        {
            get => _features;
            set => SetProperty(ref _features, value);
        }

        public bool ShowFeatures => InternalModel is Artifact && Type == EInventoryItemType.Artifact;

        public string Name
        {
            get => InternalModel.Name;
            set => SetProperty(
                InternalModel.Name, value, InternalModel, (model, prop) => model.Name = prop
            );
        }

        public string Description
        {
            get => InternalModel.Description;
            set => SetProperty(
                InternalModel.Description, value, InternalModel, (model, prop) => model.Description = prop
            );
        }

        public int Count
        {
            get => InternalModel.Count;
            set
            {
                SetProperty(
                    InternalModel.Count, value, InternalModel, (model, prop) => model.Count = prop
                );
                OnPropertyChanged(nameof(ShowCount));
                OnPropertyChanged(nameof(PriceString));
                OnPropertyChanged(nameof(FullPrice));

                if (Type == EInventoryItemType.Artifact)
                {
                    OnFullPriceChanged?.Invoke(this, FullPrice);
                }
            }
        }

        public string WeaponDice
        {
            get => InternalModel is Artifact artifact ? artifact.Damage.ToString() : "0";
            set
            {
                if (DicePool.TryParse(value, out DicePool? dice) && dice != null && InternalModel is Artifact artifact)
                {
                    artifact.Damage = dice;
                    UpdatePrice();
                }
            }
        }

        public int Distance
        {
            get => InternalModel is Artifact artifact ? artifact.Distance : 1;
            set
            {
                if(InternalModel is Artifact artifact)
                {
                    SetProperty(
                        artifact.Distance, value, artifact, (model, prop) => model.Distance = prop
                    );
                    UpdatePrice();
                }
            }
        }

        public string ArmorDice
        {
            get => InternalModel is Artifact artifact ? artifact.Defense.ToString() : "0";
            set
            {
                if (DicePool.TryParse(value, out DicePool? dice) && dice != null && InternalModel is Artifact artifact)
                {
                    artifact.Defense = dice;
                    UpdatePrice();
                }
            }
        }

        public string ArtifactLevel => InternalModel is Artifact artifact ? artifact.Level.ToString() : "none";

        public event EventHandler<int>? OnFullPriceChanged;

        public int Price
        {
            get
            {
                if(Type == EInventoryItemType.Artifact)
                {
                    return (InternalModel as Artifact)?.Price ?? 0;
                }

                return 0;
            }
        }

        public int FullPrice => Price * Count;

        public string PriceString => Count > 1 ? $"{Price * Count} ({Price}x{Count})" : Price.ToString();

        private EInventoryItemType? _type;
        public EInventoryItemType? Type
        {
            get => _type;
            set
            {
                bool initialized = _type != null;

                // Если установлен новый тип для инициализированного предмета.
                if (SetProperty(ref _type, value) && initialized)
                {
                    UpdateInternalByType(value);
                }
            }
        }

        private ObservableCollection<InventoryItemVM> _payload = [];
        public ObservableCollection<InventoryItemVM> Payload
        {
            get => _payload;
            set => SetProperty(ref _payload, value);
        }

        public bool ShowPayload => Type == EInventoryItemType.Container;     
        public bool ShowCount => Count != 1 || Type == EInventoryItemType.Artifact;
        public bool IsArtifact => Type == EInventoryItemType.Artifact;

        private RelayCommand? _descriptionCommand;
        public RelayCommand? DescriptionCommand
        {
            get => _descriptionCommand;
            set => SetProperty(ref _descriptionCommand, value);
        }

        private void UpdateInternalByType(EInventoryItemType? type)
        {
            if (type == null)
            {
                return;
            }

            Item newItem = InventoryItemConverter.CreateItemByType(type.Value, InternalModel);
            newItem.Id = InternalModel.Id;

            InternalModel = newItem;

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(ShowCount));
            OnPropertyChanged(nameof(ShowPayload));
            OnPropertyChanged(nameof(ShowFeatures));
            OnPropertyChanged(nameof(IsArtifact));
            UpdatePrice();
            UpdateArtifactProperties();
        }

        private void UpdateArtifactProperties()
        {
            OnPropertyChanged(nameof(WeaponDice));
            OnPropertyChanged(nameof(Distance));
            OnPropertyChanged(nameof(ArmorDice));
        }

        private void UpdatePrice()
        {
            OnPropertyChanged(nameof(Price));
            OnPropertyChanged(nameof(PriceString));
            OnPropertyChanged(nameof(FullPrice));
            OnPropertyChanged(nameof(ArtifactLevel));
            OnFullPriceChanged?.Invoke(this, FullPrice);
        }

        public void AddFeature(ArtifactFeatureModel feature)
        {
            if (InternalModel is Artifact artifact && feature.Internal is ArtifactFeature featureInternal)
            {
                artifact.AddFeature(featureInternal);
                Features.Add(feature);
                UpdatePrice();
            }
        }

        public void RemoveFeature(ArtifactFeatureModel feature)
        {
            if (InternalModel is Artifact artifact && feature.Internal is ArtifactFeature featureInternal)
            {
                artifact.RemoveFeature(featureInternal);
                Features.Remove(Features.First(x => x.Internal.Id == feature.Internal.Id));
                UpdatePrice();
            }
        }

        /// <summary>
        /// Заменяет переданной способностью другую, с таким же Guid-ом
        /// </summary>
        public void UpdateFeature(ArtifactFeatureModel feature)
        {
            if (InternalModel is Artifact artifact && feature.Internal is ArtifactFeature featureInternal)
            {
                int indexOfOldAbility = Features.IndexOf(
                    Features.First(x => x.Internal.Id == feature.Internal.Id)
                );
                Features[indexOfOldAbility] = feature;
                artifact.UpdateFeature(featureInternal);
                UpdatePrice();
            }
        }

        public override string ToString() => Name;
    }

    public class InventoryItemNodeVM(Item model) : InventoryItemVM(model)
    {
        public ImageSource? Icon { get; set; }
        public Color? BackgroundColor { get; set; }
    }

    public enum EInventoryItemType
    {
        Thing,
        Container,
        Artifact
    }
}
