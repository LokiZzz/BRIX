using BRIX.Library.DiceValue;

Dice dice = new Dice(10, 3);
double avg = dice.Average(explodingDepth: 4);

Console.WriteLine();