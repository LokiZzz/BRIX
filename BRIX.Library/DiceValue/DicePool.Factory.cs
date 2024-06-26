﻿using BRIX.Library.Extensions;

namespace BRIX.Library.DiceValue
{
    public partial class DicePool
    {
        /// <summary>
        /// Раскладывает числовое значение на набор костей с заданным разбросом. Разброс задаётся в процентах от исходного 
        /// значения. Таким образом 10 с разбросом 0.2 дадут диапазон 8-12. Любого заданного разброса метод будет 
        /// пытаться достичь при помощи стандартных костей (1к4, 1к6, 1к8, 1к10, 1к12, 1к20), стараясь не выходить за 20 штук.
        /// </summary>
        /// <param name="value">Значение, раскладываемое на кости.</param>
        /// <param name="desiredSpreadPercent">Желаемый разброс значений в диапазоне от 0 до 1.</param>
        public static DicePool FromRange(int lower, int upper, bool includeD2 = false)
        {
            if (lower < 1 || lower > upper)
            {
                throw new ArgumentException("Lower must be >= 1, and lower must be > upper.");
            }

            DicePool resultDicePool = new();

            if (lower != upper)
            {
                int spreadSize = upper - lower;
                int diceCount = 0;
                int diceFaces = 0;

                List<int> standartDiceSet = [.. Enum.GetValues<EStandartDice>()
                    .Select(x => (int)x)
                    .Where(x => x > 2 || includeD2)
                    .OrderByDescending(x => x)];

                (int Dice, int Count, int Remainder) nearestNonPerfect = (default, default, default);
                bool perfectFound = false;

                foreach (int dice in standartDiceSet)
                {
                    int spreadRemainder = spreadSize % (dice - 1);

                    if (spreadRemainder == 0 && spreadSize / (dice - 1) <= lower)
                    {
                        diceCount = spreadSize / (dice - 1);
                        diceFaces = dice;
                        resultDicePool.Add(new Dice(diceFaces, diceCount));
                        resultDicePool.Modifier = lower - diceCount;
                        perfectFound = true;

                        break;
                    }
                    else
                    {
                        bool noNearest = nearestNonPerfect.Dice == default;
                        bool reminderIsLess = spreadRemainder < nearestNonPerfect.Remainder;
                        bool diffBtwDiceAndReminderIsLess = Math.Abs(spreadRemainder - dice)
                            < Math.Abs(nearestNonPerfect.Remainder - nearestNonPerfect.Dice);

                        if (noNearest || reminderIsLess || diffBtwDiceAndReminderIsLess)
                        {
                            nearestNonPerfect = (dice, Math.Max(spreadSize / (dice - 1), 1), spreadRemainder);
                        }
                    }
                }

                if (!perfectFound)
                {
                    resultDicePool.Add(new Dice(nearestNonPerfect.Dice, nearestNonPerfect.Count));
                    resultDicePool.Modifier = lower - nearestNonPerfect.Count;
                }
            }
            else
            {
                resultDicePool.Modifier = lower;
            }

            return resultDicePool;
        }

        public static DicePool FromValue(int value, double spread)
        {
            if (value <= 0 || spread < 0 || spread >= 1)
            {
                throw new ArgumentException("Оба края диапазона должны быть больше нуля, а левый край должен быть меньше правого.");
            }

            int from = (value - value * spread).Round();
            int to = (value + value * spread).Round();

            return FromRange(from, to);
        }

        public static DicePool FromAdjusted(DicePool dicePoolToAdjust, int percent)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(percent, -100);

            int average = dicePoolToAdjust.Average();
            double spread = (double)(average - dicePoolToAdjust.Min()) / average;

            int newAverage = (average + average * ((double)percent / 100)).Round();

            DicePool dicePool = FromValue(newAverage, spread);
            dicePool.RollOptions.RerollValues = dicePoolToAdjust.RollOptions.RerollValues;
            dicePool.RollOptions.CriticalPercent = dicePoolToAdjust.RollOptions.CriticalPercent;
            dicePool.RollOptions.CriticalModifier = dicePoolToAdjust.RollOptions.CriticalModifier;
            dicePool.RollOptions.ExplodingDepth = dicePoolToAdjust.RollOptions.ExplodingDepth;

            return dicePool;
        }
    }
}
