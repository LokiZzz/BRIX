using BRIX.Lexica;
using BRIX.Library.Aspects;
using BRIX.Library.Effects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Lexis
{
    public class EffectLexis
    {
        public static string GetEffectDescription(EffectBase effectBase, ELexisLanguage language)
        {
            switch (effectBase)
            {
                case DamageEffect de:
                    return GetDamageEffectLexis(de, language);
                default:
                    return string.Empty;
            }
        }

        private static string GetDamageEffectLexis(DamageEffect effect, ELexisLanguage language)
        {
            switch (language)
            {
                case ELexisLanguage.English:
                    return $"Due to this effect, the ability deals " +
                        $"{Numbers.ENGDeclension(effect.Impact, "point")} " +
                        $"of damage to targets.";
                case ELexisLanguage.Russian:
                    //
                    return $"Благодаря этому эффекту способность наносит целям " +
                        $"{Numbers.RUSDeclension(effect.Impact, "очко")} " +
                        $"урона.";
                default:
                    return string.Empty;
            }
        }
    }
}
