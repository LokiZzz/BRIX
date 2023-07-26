using BRIX.Lexica;
using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
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
                case HealEffect he:
                    return string.Empty;
                case DamageEffect de:
                    return GetDamageEffectLexis(de, language);
                default:
                    return string.Empty;
            }
        }

        private static string GetDamageEffectLexis(DamageEffect effect, ELexisLanguage language)
        {
            bool damageHimself = effect.GetAspect<TargetSelectionAspect>()?.Strategy == ETargetSelectionStrategy.CharacterHimself;

            switch (language)
            {
                case ELexisLanguage.English:
                    string target = damageHimself ? "character" : "targets";
                    return $"Due to this effect, the ability deals " +
                        $"{Numbers.ENGDeclension(effect.Impact, "point")} " +
                        $"of damage to {target}.";
                case ELexisLanguage.Russian:
                    target = damageHimself ? "персонажу" : "целям";
                    return $"Благодаря этому эффекту способность наносит {target} " +
                        $"{Numbers.RUSDeclension(effect.Impact, "очко")} " +
                        $"урона.";
                default:
                    return string.Empty;
            }
        }
    }
}
