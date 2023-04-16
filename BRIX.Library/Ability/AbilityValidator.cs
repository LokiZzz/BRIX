using BRIX.Library.Aspects;
using BRIX.Library.Effects;

namespace BRIX.Library
{
    /// <summary>
    /// У способности есть своя логика валидации, по которой можно проверить целостность логики,
    /// по которой способность собрана.
    /// </summary>
    public static class AbilityValidator
    {
        public static List<EAbilityValidationErrors> Validate(this Ability ability)
        {
            List<EAbilityValidationErrors> errors = new();

            CheckObstacleConformity(ability, errors);

            return errors;
        }

        private static void CheckObstacleConformity(Ability ability, List<EAbilityValidationErrors> errors)
        {
            foreach(EffectBase effect in ability.Effects)
            {
                TargetSelectionAspect tsaAspect = effect.GetAspect<TargetSelectionAspect>();
                ObstacleAspect obstacleAspect = effect.GetAspect<ObstacleAspect>();
                TargetChainAspect chainAspect = effect.GetAspect<TargetChainAspect>();

                if (obstacleAspect?.BetweenCharacterAndTarget != EObstacleEquivalent.None)
                {
                    if(tsaAspect?.Strategy != ETargetSelectionStrategy.NTargetsAtDistanсeL)
                    {
                        errors.Add(EAbilityValidationErrors.ObstacleConformity);

                        return;
                    }
                }

                if (obstacleAspect?.BetweenCharacterAndArea != EObstacleEquivalent.None
                    || obstacleAspect?.BetweenEpicenterAndTarget != EObstacleEquivalent.None)
                {
                    if (tsaAspect?.Strategy != ETargetSelectionStrategy.Area)
                    {
                        errors.Add(EAbilityValidationErrors.ObstacleConformity);

                        return;
                    }
                }

                if (obstacleAspect?.BetweenTargetsInChain != EObstacleEquivalent.None)
                {
                    if (chainAspect?.IsChainEnabled == false)
                    {
                        errors.Add(EAbilityValidationErrors.ObstacleConformity);

                        return;
                    }
                }

                if (obstacleAspect?.BetweenTargetAndDestinationPoint != EObstacleEquivalent.None)
                {
                    if (!ability.Effects.Any(x => x.GetType() == typeof(MoveTargetEffect)))
                    {
                        errors.Add(EAbilityValidationErrors.ObstacleConformity);

                        return;
                    }
                }
            }
        }
    }

    public enum EAbilityValidationErrors
    {
        ObstacleConformity = 1,
    }
}