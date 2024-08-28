using BRIX.Library.Effects;
using BRIX.Library.Enums;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects
{
    public class TargetSizeAspect : AspectBase
    {
        public override void Initialize()
        {
            base.Initialize();
            _allowedTargetSizes = [ ETargetSize.Small, ETargetSize.Medium, ETargetSize.Big ];
        }

        protected List<ETargetSize> _allowedTargetSizes = [];
        public IReadOnlyCollection<ETargetSize> AllowedTargetSizes => _allowedTargetSizes;

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

        public override double GetCoefficient()
        {
            return SizeCategoriesCountToPercentMap[AllowedTargetSizes.Count].ToCoeficient();
        }

        private static Dictionary<int, int> SizeCategoriesCountToPercentMap => new()
        {
            { 0, 0 }, { 1, -20 }, { 2, -10 }, { 3, 0 }, { 4, 10 }, { 5, 20 }, { 6, 30 },
            { 7, 40 }, { 8, 50 }, { 9, 60 }, { 10, 70 }, { 11, 80 }
        };
    }
}
