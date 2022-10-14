using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Character
{
    /// <summary>
    /// Персонаж на первом уровне персонаж имеет 100 очков опыта.
    /// Для получения каждого следующего уровня ему необходимо получить
    /// дополнительно количество опыта равное уровню умноженному на сто:
    /// 1       2       3       4       5       ...     N
    /// 100     300     600     1000    1500    ...     50 * N * (N + 1)     
    /// </summary>
    public static class ExperienceCalculator
    {
        private static int _experienceModifier = 50;

        public static int GetExpForLevel(int level) => _experienceModifier * level * (level + 1);

        public static int GetLevelFromExp(int exp)
        {
            // Опыт, необходимый для достижения уровня считается по формуле:
            // Exp = 50 * Lvl * (Lvl + 1)

            // Таким образом при данном количестве опыта для получения уровня персонажа
            // выводится квадратное уравнение:
            // Lvl^2 + Lvl - Exp/50 = 0, где
            double a = 1;
            double b = 1; 
            double c = -(double)exp / _experienceModifier;

            // Решение квадратного уравнения
            double D = b * b - 4 * a * c;
            double x1 = (Math.Sqrt(D) - b) / 2 * a;
            double x2 = -(Math.Sqrt(D) - a) / 2 * b;
            double continuousLevel = x1 >= 0 ? x1 : x2;

            return (int)Math.Truncate(continuousLevel);
        }

        public static int GetExpToLevelUp(int currentExp)
        {
            int currentLevel = GetLevelFromExp(currentExp);
            int expForNextLevel = GetExpForLevel(currentLevel + 1);

            return expForNextLevel - currentExp;
        }
    }
}
