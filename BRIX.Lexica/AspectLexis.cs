using BRIX.Lexis;
using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
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

        private static string ForTargetChain(TargetChainSettings aspect, ELexisLanguage language)
        {
            switch (language)
            {
                case ELexisLanguage.English:
                    if(aspect.IsChainEnabled)
                    {
                        
                        return $"The effect is applied to targets in a chain, with no more than " +
                            $"{Numbers.ENGDeclension(aspect.MaxDistanceBetweenTargets, "meter")} " +
                            $"between targets. Maximum number of targets in a chain: {aspect.MaxTargetsCount}";
                    }
                    else
                    {
                        return "Target chain is not turned on.";
                    }
                case ELexisLanguage.Russian:
                    if (aspect.IsChainEnabled)
                    {
                        return $"Эффект применяется к целям по цепочке, в которой между целями должно быть не более, чем " +
                            $"{Numbers.RUSDeclension(aspect.MaxDistanceBetweenTargets, "метр")}. " +
                            $"Максимальное количество целей в цепи: {aspect.MaxTargetsCount}.";
                    }
                    else
                    {
                        return "Цепи целей не включены.";
                    }
                default:
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
            string result = string.Empty;

            if(aspect.Strategy == ETargetSelectionStrategy.Area)
            {
                result = "Эффект применяется к самому персонажу.";
            }
            else if (aspect.Strategy == ETargetSelectionStrategy.NTargetsAtDistanсeL)
            {
                string located = aspect.NTAD.TargetsCount > 1 ? "расположенных" : "расположенную";
                result = $"Вы можете выбрать " +
                    $"{Numbers.RUSDeclension(aspect.NTAD.TargetsCount, "цель")}, " +
                    $"{located} не далее, чем {Numbers.RUSDeclension(aspect.NTAD.DistanceInMeters, "метр")} " +
                    $"от персонажа.";

                if(aspect.NTAD.IsTargetSelectionIsRandom)
                {
                    result += " Цель должна выбираться случайным образом.";
                }
            }
            else if (aspect.Strategy == ETargetSelectionStrategy.Area)
            {
                result = $"Эффект будет применён ко всем целям в области. Область — это " +
                    $"{ShapeLexis.GetLexis(aspect.Area.Shape, ELexisLanguage.Russian)}. " +
                    $"Область может быть размещена под любым углом. " +
                    $"Максимальное расстояние между персонажем и ближайшей к нему точкой области: " +
                    $"{Numbers.RUSDeclension(aspect.Area.DistanceToAreaInMeters, "метр")}.";

                if(aspect.Area.ExcludedTargetsCount > 0)
                {
                    result += $" При использовании способности персонаж по желанию может исключить из области " +
                        $"{Numbers.RUSDeclension(aspect.Area.ExcludedTargetsCount, "цель")}.";
                }
            }

            if(aspect.TargetChain.IsChainEnabled)
            {
                result += $" Цели поражаются по цепи (" +
                    $"{Numbers.RUSDeclension(aspect.TargetChain.MaxTargetsCount, "цель")}, " +
                    $"звено: {Numbers.RUSDeclension(aspect.TargetChain.MaxDistanceBetweenTargets, "метр")}).";
            }

            return result;
        }

        private static string ForTargetSelectionENG(TargetSelectionAspect aspect)
        {
            string result = string.Empty;

            if (aspect.Strategy == ETargetSelectionStrategy.Area)
            {
                result = "The effect is applied to the character himself.";
            }
            else if (aspect.Strategy == ETargetSelectionStrategy.NTargetsAtDistanсeL)
            {
                result = $"You can select up to " +
                    $"{Numbers.ENGDeclension(aspect.NTAD.TargetsCount, "target")} " +
                    $"located within {Numbers.RUSDeclension(aspect.NTAD.DistanceInMeters, "meter")} " +
                    $"of the character.";

                if (aspect.NTAD.IsTargetSelectionIsRandom)
                {
                    result += " The target must be chosen randomly.";
                }
            }
            else if (aspect.Strategy == ETargetSelectionStrategy.Area)
            {
                result = $"The effect will be applied to all targets in the area. " +
                    $"The area is a {ShapeLexis.GetLexis(aspect.Area.Shape, ELexisLanguage.English)}. " +
                    $"The area can be placed at any angle. " +
                    $"The maximum distance between the character and the closest point of the area to him: " +
                    $"{Numbers.ENGDeclension(aspect.Area.DistanceToAreaInMeters, "meter")}.";

                if (aspect.Area.ExcludedTargetsCount > 0)
                {
                    result += $" When using the ability, the character can optionally exclude " +
                        $"{Numbers.RUSDeclension(aspect.Area.ExcludedTargetsCount, "target")} from the area.";
                }
            }

            if (aspect.TargetChain.IsChainEnabled)
            {
                result += $"Targets are hit in a chain (" +
                    $"{Numbers.ENGDeclension(aspect.TargetChain.MaxTargetsCount, "target")}, " +
                    $"link distance: {Numbers.ENGDeclension(aspect.TargetChain.MaxDistanceBetweenTargets, "meter")}).";
            }

            return result;
        }
    }
}