using System;

namespace BRIX.Library.Aspects
{
    public abstract class AspectBase
    {
        public virtual bool IsEnabled { get; set; }

        public abstract double GetCoefficient();

        /// <summary>
        /// Согласование одинаковых аспектов в разных эффектах.
        /// Со временем может выродится в метод по месту, то есть
        /// реализация будет прямо в базовом классе. Это произойдёт
        /// если окажется, что все аспекты согласуются одинаково — сихронизацией.
        /// Т.е. если все аспекты просто копируются от ведущего аспекта-инициатора.
        /// В методе конкорд необходимо убедиться, что в коллекции лежит
        /// новый экземпляр аспекта, а не ссылка на один из аспектов одного из эффектов.
        /// </summary>
        public abstract AspectBase Concord(List<AspectBase> sameAspects);
    }
}
