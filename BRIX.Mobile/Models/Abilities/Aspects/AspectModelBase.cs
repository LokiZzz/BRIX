using BRIX.Lexica;
using BRIX.Library.Aspects;
using BRIX.Library.Effects;
using BRIX.Mobile.Services;
using BRIX.Mobile.ViewModel.Abilities.Aspects;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public abstract partial class AspectModelBase : ObservableObject 
    {
        public AspectBase InternalModel { get; set; }

        public T GetSpecificAspect<T>() where T : AspectBase
        {
            return InternalModel as T;
        }

        public string Name => AspectsDictionary.Collection[GetType()].Name;

        public string Description 
        {
            get
            {
                ILocalizationResourceManager localization = ServicePool.GetService<ILocalizationResourceManager>();
                ELexisLanguage language = localization.LexisLanguage;

                return AspectLexis.GetAspectDescription(InternalModel, language);
            } 
        }
    }
}
