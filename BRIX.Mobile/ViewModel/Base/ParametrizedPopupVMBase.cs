using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Base
{
    public abstract partial class ParametrizedPopupVMBase<T> : ViewModelBase
    {
        private T _parameters;

        /// <summary>
        /// Свойство для передачи данных во всплывающее окно. Подхватить данные и распределить по внутренним свойствам 
        /// модели этого окна можно в переопределённом методе HandleParameters. Установить параметры можно только один 
        /// раз. Такое поведение настроено с целью сохранить консистентность поведения всплывающих окон.
        /// </summary>
        public T PassInParameters
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
