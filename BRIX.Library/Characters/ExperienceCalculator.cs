namespace BRIX.Library.Characters
{
    /// <summary>
    /// Персонаж на первом уровне имеет 100 очков опыта.
    /// Для получения каждого следующего уровня ему необходимо получить
    /// дополнительно количество опыта равное уровню умноженному на сто:
    /// 1       2       3       4       5       ...     N
    /// 100     300     600     1000    1500    ...     50 * N * (N + 1)     
    /// </summary>
    public static class CharacterCalculator
    {
        private static int _expPerLevel = 100;
        private static int _experienceModifier = _expPerLevel / 2;

        public static int HealthPerLevel = 10;

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

        public static int HealthToExp(int health)
        {
            if(health == 0)
            {
                return 0;
            }

            int levelEquivalent = health / HealthPerLevel;
            int requiredExp = GetExpForLevel(levelEquivalent);

            int healthRemainder = health % HealthPerLevel;
            int levelsExpDiff = _expPerLevel * (levelEquivalent + 1);
            int expPerHealthPointOnThisLevel = levelsExpDiff / HealthPerLevel;
            requiredExp += healthRemainder * expPerHealthPointOnThisLevel;

            return requiredExp;
        }

        public static int ExpToHealth(int exp)
        {
            if (exp == 0)
            {
                return 0;
            }

            int levelEquivalent = GetLevelFromExp(exp);
            int health = levelEquivalent * HealthPerLevel;
            int levelsExpDiff = _expPerLevel * (levelEquivalent + 1);
            int expPerHealthPointOnThisLevel = levelsExpDiff / HealthPerLevel;
            health += (exp - GetExpForLevel(levelEquivalent)) / expPerHealthPointOnThisLevel;

            return health;
        }
    }
}
