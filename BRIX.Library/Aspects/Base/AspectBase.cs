using System;
using System.Runtime.CompilerServices;

namespace BRIX.Library.Aspects
{
    public abstract class AspectBase
    {
        public virtual bool IsEnabled { get; set; }

        /// <summary>
        /// Показывает, синхронизируется ли аспект с другими аспектами того же типа в других эффектах способности.
        /// </summary>
        public abstract bool IsConcording { get; }

        public abstract double GetCoefficient();
    }

    public abstract class FreeConcordanceAspect : AspectBase
    {
        private bool _isConcording;
        public override bool IsConcording => _isConcording;

        public void SetConcordance(bool isConcording)
        {
            _isConcording = isConcording;
        }
    }
}
