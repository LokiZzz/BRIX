using BRIX.Lexis;
using BRIX.Library.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Lexica
{
    /// <summary>
    /// Простейший перевод с модели на обычный человеческий язык.
    /// Учитывая, что аспекты и эффекты ограниченый небольшим числом, 
    /// заморачиваться со сложными инструментами морфологии нет смысла.
    /// </summary>
    public static class AspectLexis
    {
        public static string GetAspectDescription(AspectBase aspectBase, ELexisLanguage language)
        {
            switch (aspectBase)
            {
                case ActionPointAspect apa:
                    return GetActionPointAspectLexis(apa, language);
                default:
                    return string.Empty;
            }
        }

        private static string GetActionPointAspectLexis(ActionPointAspect aspect, ELexisLanguage language)
        {
            switch (language)
            {
                case ELexisLanguage.English:
                    return $"To use this ability you have to spend " +
                        $"{aspect.ActionPoints} action {NumberDeclension.ENGDeclension(aspect.ActionPoints, "point")}.";
                case ELexisLanguage.Russian:
                    return $"Чтобы использовать эту способность вы должны потратить " +
                        $"{NumberDeclension.RUSDeclension(aspect.ActionPoints, "очко")} действий.";
                default:
                    return string.Empty;
            }
        }

        
    }
}
