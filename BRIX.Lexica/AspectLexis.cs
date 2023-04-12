using BRIX.Lexis;
using BRIX.Library.Aspects;
using BRIX.Library.Mathematics;

namespace BRIX.Lexica
{
    /// <summary>
    /// Простейший перевод с модели на обычный человеческий язык.
    /// Учитывая, что аспекты и эффекты ограниченый небольшим числом, 
    /// заморачиваться со сложными инструментами морфологии нет смысла.
    /// </summary>
    public static class AspectLexis
    {
        public static string GetLexis(AspectBase aspectBase, ELexisLanguage language)
        {
            try
            {
                switch (aspectBase)
                {
                    case ActionPointAspect apa:
                        return ForActionPoints(apa, language);
                    case TargetSelectionAspect tsa:
                        return ForTargetSelection(tsa, language);
                    default:
                        return string.Empty;
                }
            }
            catch(NullReferenceException)
            {
                return string.Empty;
            }
        }

        private static string ForActionPoints(ActionPointAspect aspect, ELexisLanguage language)
        {
            switch (language)
            {
                case ELexisLanguage.English:
                    return $"To use this ability you have to spend " +
                        $"{aspect.ActionPoints} action {Numbers.ENGDeclension(aspect.ActionPoints, "point", false)}.";
                case ELexisLanguage.Russian:
                    return $"Чтобы использовать эту способность вы должны потратить " +
                        $"{Numbers.RUSDeclension(aspect.ActionPoints, "очко")} действий.";
                default:
                    return string.Empty;
            }
        }

        private static string ForTargetSelection(TargetSelectionAspect aspect, ELexisLanguage language)
        {
            switch (language)
            {
                case ELexisLanguage.English:
                    return ForTargetSelectionENG(aspect);
                case ELexisLanguage.Russian:
                    return ForTargetSelectionRUS(aspect);
                default:
                    return string.Empty;
            }
        }

        private static string ForTargetSelectionRUS(TargetSelectionAspect aspect)
        {
            if(aspect.Strategy == ETargetSelectionStrategy.NTargetsAtDistanсeL)
            {
                string result = $"Вы можете выбрать до " +
                    $"{Numbers.RUSDeclension(aspect.NTAD.TargetsCount, "цель")}, " +
                    $"расположенных не далее, чем на расстоянии " +
                    $"{Numbers.RUSDeclension(aspect.NTAD.DistanceInMeters, "метр")} " +
                    $"от персонажа.";

                if(aspect.NTAD.IsTargetSelectionIsRandom)
                {
                    result += " Цель должна выбираться случайным образом.";
                }

                return result;
            }
            else
            {
                string result = $"Эффект будет применён ко всем целям в области. Область — это " +
                    $"{ShapeLexis.GetLexis(aspect.Area.Shape, ELexisLanguage.Russian)}. " +
                    $"Область может быть размещена под любым углом. " +
                    $"Максимальное расстояние между персонажем и ближайшей к нему точкой области: " +
                    $"{Numbers.RUSDeclension(aspect.Area.DistanceToAreaInMeters, "метр")}.";

                if(aspect.Area.ExcludedTargetsCount > 0)
                {
                    result += $" При использовании способности персонаж по желанию может исключить " +
                        $"{Numbers.RUSDeclension(aspect.Area.ExcludedTargetsCount, "цель")}.";
                }

                return result;
            }
        }

        private static string ForTargetSelectionENG(TargetSelectionAspect aspect)
        {
            if (aspect.Strategy == ETargetSelectionStrategy.NTargetsAtDistanсeL)
            {
                //You can select up to 5 targets located within 3 meters of the character.
                string result = $"You can select up to " +
                    $"{Numbers.ENGDeclension(aspect.NTAD.TargetsCount, "target")} " +
                    $"located within {Numbers.RUSDeclension(aspect.NTAD.DistanceInMeters, "метр")} " +
                    $"of the character.";

                if (aspect.NTAD.IsTargetSelectionIsRandom)
                {
                    result += " The target must be chosen randomly.";
                }

                return result;
            }
            else
            {
                string result = $"The effect will be applied to all targets in the area. " +
                    $"The area is a {ShapeLexis.GetLexis(aspect.Area.Shape, ELexisLanguage.English)}. " +
                    $"The area can be placed at any angle. " +
                    $"The maximum distance between the character and the closest point of the area to him: " +
                    $"{Numbers.ENGDeclension(aspect.Area.DistanceToAreaInMeters, "meter")}.";

                if (aspect.Area.ExcludedTargetsCount > 0)
                {
                    result += $" When using the ability, the character can optionally exclude " +
                        $"{Numbers.RUSDeclension(aspect.Area.ExcludedTargetsCount, "target")}.";
                }

                return result;
            }
        }
    }
}