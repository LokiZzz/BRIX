using BRIX.Library.Abilities;
using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Library.Mathematics;
using BRIX.Utility.Extensions;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

//while(true)
//{
//    int ap = int.Parse(Console.ReadLine());
//    Console.WriteLine($"Coef: {GetActionPointsCoefficient(ap)}");
//}

//double GetActionPointsCoefficient(int actionPoints)
//{
//    double coef = actionPoints switch
//    {
//        1 => 3,
//        2 => 1.5,
//        3 => 1,
//        4 => 0.75,
//        5 => 0.65,
//        >= 6 and <=50 => GetThrasholdedCoeficient(),
//        _ => 0.05
//    };

//    return coef;

//    // 6-10 по -3%; 11-15 по -2%; 16-50 по -1%; 
//    double GetThrasholdedCoeficient()
//    {
//        return (-new ThrasholdCostConverter((1, 0), (5, 35), (6, 3), (11, 2), (16, 1))
//            .Convert(actionPoints))
//            .ToCoeficient();
//    }
//}