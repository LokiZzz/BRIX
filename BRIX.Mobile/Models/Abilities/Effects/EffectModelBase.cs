using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.Services;
using BRIX.Mobile.ViewModel.Abilities.Effects;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Runtime.CompilerServices;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public abstract partial class EffectModelBase : ObservableObject
    {
        public EffectBase? InternalModel { get; set; }

        public T GetSpecificEffect<T>() where T : EffectBase
        {
            return InternalModel != null
                ? (T)InternalModel
                : throw new Exception("InternalModel не инициализирован.");
        }

        public string Name => EffectsDictionary.Collection[GetType()].Name;

        public string Description => InternalModel == null ? string.Empty : InternalModel.ToLexis();

        public List<AspectModelBase> Aspects { get; protected set; } = [];

        public AspectModelBase GetAspect(Type aspectType)
        {
            return Aspects.Single(x => x.InternalModel.GetType() == aspectType);
        }

        public void UpdateAspect(AspectModelBase aspect)
        {
            InternalModel?.SetAspect(aspect.InternalModel);
            int indexOfAspect = Aspects.IndexOf(GetAspect(aspect.InternalModel.GetType()));
            Aspects[indexOfAspect] = aspect;
            OnPropertyChanged(nameof(Aspects));
        }

        public void InitializeAspects()
        {
            if(InternalModel == null)
            {
                return;
            }

            Aspects = InternalModel.Aspects
                .Select(AspectModelFactory.GetAspectModel)
                .Where(x => x != null)
                .ToList();
        }

        /// <summary>
        /// Событие означающее, что было изменено свойство, влияющее на стоимость.
        /// </summary>
        public event EventHandler? EffectPropertyChanged;

        /// <summary>
        /// Если устанавливать свойства через этот метод, то EffectPageVM, подписавшийся на EffectPropertyChanged,
        /// будет обновлять монитор стоимости автоматически.
        /// </summary>
        public bool SetEffectProperty<TModel, T>(
            T oldValue,
            T newValue,
            TModel model,
            Action<TModel, T> callback,
            [CallerMemberName] string? propertyName = null) where TModel : class
        {
            bool set = SetProperty(oldValue, newValue, model, callback, propertyName);
            EffectPropertyChanged?.Invoke(this, new EventArgs());

            return set;
        }
    }
}
