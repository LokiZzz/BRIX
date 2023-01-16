using BRIX.Library.Enums;
using BRIX.Library.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Aspects
{
    public class TargetSizeAspect : AspectBase
    {
        private List<ETargetSize> _allowedTargetSizes = new () { ETargetSize.Small, ETargetSize.Medium, ETargetSize.Big };
        public IReadOnlyList<ETargetSize> AllowedTargetSizes => _allowedTargetSizes.AsReadOnly();

        public void AddSize(ETargetSize size)
        {
            if (!_allowedTargetSizes.Any(x => x == size))
            {
                _allowedTargetSizes.Add(size);
            }
        }

        public void RemoveSize(ETargetSize size)
        {
            if (_allowedTargetSizes.Any(x => x == size))
            {
                _allowedTargetSizes.Remove(
                    _allowedTargetSizes.First(x => x == size)
                );
            }
        }

        public override double GetCoefficient() => 
            SizeCategoriesCountToPercentMap[AllowedTargetSizes.Count()]
            .ToCoeficient();

        private Dictionary<int, int> SizeCategoriesCountToPercentMap => new Dictionary<int, int>()
        {
            { 1, -20 }, { 2, -10 }, { 3, 0 }, { 4, 10 }, { 5, 20 }, { 6, 30 },
            { 7, 40 }, { 8, 50 }, { 9, 60 }, { 10, 70 }, { 11, 80 }
        };
    }
}
