using BRIX.Library.Abilities;
using BRIX.Library.Aspects.Base;
using BRIX.Library.Effects;
using Microsoft.AspNetCore.Components;

namespace BRIX.Lexica
{
    public static class ShortLexisExtensions
    {
        public static MarkupString ToAggregatedString<T>(
            this IEnumerable<T> collection, 
            Func<T, string> toStringDelegate)
        {
            if(collection.Count() == 0)
            {
                return (MarkupString)"[NONE]";
            }
            else
            {
                string result = '[' 
                    + collection.Select(toStringDelegate).Aggregate((x, y) => x + ',' + y) 
                    + "]";

                return (MarkupString)result;
            }
        }

        public static async Task<string> ToFullShortLexis(this Ability ability)
        {
            string shortAbilityDescription = string.Empty;

            shortAbilityDescription += await ability.Activation.ToShortLexisAsync() + Environment.NewLine;

            foreach (EffectBase effect in ability.Effects)
            {
                shortAbilityDescription += "* " + await effect.ToShortLexisAsync() + Environment.NewLine;

                foreach (AspectBase aspect in effect.Aspects)
                {
                    if (effect.Aspects.IndexOf(aspect) > 0)
                    {
                        shortAbilityDescription += ' ';
                    }

                    shortAbilityDescription += await aspect.ToShortLexisAsync();
                }

                shortAbilityDescription += Environment.NewLine;
            }

            return shortAbilityDescription;
        }
    }
}
