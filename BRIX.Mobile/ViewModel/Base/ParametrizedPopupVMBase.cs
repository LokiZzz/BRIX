﻿using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Base
{
    public abstract partial class PopupVMBase : ViewModelBase
    {
        public Popup? View { get; set; }

        [RelayCommand]
        public void Close()
        {
            View?.Close();
        }
    }

    public abstract partial class ParametrizedPopupVMBase<T> : PopupVMBase
    {
        private T? _parameters;
        /// <summary>
        /// Свойство для передачи данных во всплывающее окно. Подхватить данные и распределить по внутренним свойствам 
        /// модели этого окна можно в переопределённом методе HandleParameters. Установить параметры можно только один 
        /// раз. Такое поведение настроено с целью сохранить консистентность поведения всплывающих окон.
        /// </summary>
        public T? Parameters
        {
            get => _parameters;
            set
            {
                if(_parameters == null)
                {
                    _parameters = value;
                    HandleParameters();
                }
            }
        }

        protected abstract void HandleParameters();
    }
}
