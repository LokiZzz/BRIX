using BRIX.Library.DiceValue;

Dice dice = new Dice(6, 1);
double avg = dice.Average(explodingDepth: 2);

Console.WriteLine();