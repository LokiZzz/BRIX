using BRIX.Library.DiceValue;

DicePool dices = new DicePool((3, 6));

int average = dices.Average();

dices.RollOptions.CriticalModifier = 2;
dices.RollOptions.CriticalPercent = 15;

average = dices.Average();

Console.WriteLine();