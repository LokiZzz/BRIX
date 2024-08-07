﻿using BRIX.Library.Enums;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects.TargetSelection
{
    public class TargetSizeSettings
    {
        private readonly List<ETargetSize> _allowedTargetSizes =
        [
            ETargetSize.Small, ETargetSize.Medium, ETargetSize.Big 
        ];

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

        public double GetCoefficient() =>
            SizeCategoriesCountToPercentMap[AllowedTargetSizes.Count]
            .ToCoeficient();

        private static Dictionary<int, int> SizeCategoriesCountToPercentMap => new()
        {
            { 0, 0 }, { 1, -20 }, { 2, -10 }, { 3, 0 }, { 4, 10 }, { 5, 20 }, { 6, 30 },
            { 7, 40 }, { 8, 50 }, { 9, 60 }, { 10, 70 }, { 11, 80 }
        };
    }
}
