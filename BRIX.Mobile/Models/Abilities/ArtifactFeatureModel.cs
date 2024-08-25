using BRIX.Library.Abilities;
using BRIX.Library.Items;
using BRIX.Mobile.Models.Abilities.Aspects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Models.Abilities
{
    public class ArtifactFeatureModel : CharacterAbilityModel
    {
        public ArtifactFeatureModel() : this(new ArtifactFeature()) { }

        public ArtifactFeatureModel(ArtifactFeature ability)
        {
            Internal = ability;
            Activation = new(ability.Activation);
            InitializeEffects();
            ConcordedAspects = new ObservableCollection<AspectModelBase>(
                ability.ConcordedAspects.Select(AspectModelFactory.GetAspectModel)
            );
            OnPropertyChanged(nameof(ShowStatusName));
        }

        public new ArtifactFeature Internal { get; }

        public bool ConsumesArtifact
        {
            get => Internal.ConsumesArtifact;
            set => SetProperty(
                Internal.ConsumesArtifact, value, Internal, (model, prop) => model.ConsumesArtifact = prop
            );
        }
    }
}
