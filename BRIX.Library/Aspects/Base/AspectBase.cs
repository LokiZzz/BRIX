using System;
using System.Runtime.CompilerServices;

namespace BRIX.Library.Aspects
{
    public abstract class AspectBase
    {
        //public virtual bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Показывает, синхронизируется ли аспект с другими аспектами того же типа в других эффектах способности.
        /// </summary>
        public bool IsConcording { get; set; }

        public abstract double GetCoefficient();
    }
}
