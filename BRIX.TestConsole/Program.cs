// See https://aka.ms/new-console-template for more information
using BRIX.Library;
using BRIX.Library.Character;
using BRIX.Library.DiceValue;
using BRIX.Library.Mathematics;


new ThrasholdCoefConverter((1, 0), (2, 100), (6, 50), (11, 10), (101, 5), (101, 1)).Convert(6);

int exp = ExperienceCalculator.GetExpForLevel(1);
exp = ExperienceCalculator.GetExpForLevel(2);
exp = ExperienceCalculator.GetExpForLevel(3);
exp = ExperienceCalculator.GetExpForLevel(4);
exp = ExperienceCalculator.GetExpForLevel(5);
exp = ExperienceCalculator.GetExpForLevel(25);

int expToLevelUp = ExperienceCalculator.GetExpToLevelUp(47000);

Console.WriteLine("Hello, World!");

