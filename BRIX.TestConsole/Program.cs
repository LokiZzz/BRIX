// See https://aka.ms/new-console-template for more information
using BRIX.Library;
using BRIX.Library.Aspects;
using BRIX.Library.Character;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Library.Mathematics;

Ability chargeAbility = new Ability();

HealDamageEffect effDamage = new ()
{
    IsDamage = true,
    Impact = new DicePool((3, 6))
};

ActionPointAspect damageAPA = effDamage.GetAspect<ActionPointAspect>();
damageAPA.ActionPoints = 5;

MoveTargetEffect effSmash = new ()
{
    DistanceInMeters = 3,
    TargetPath = EMoveTargetPath.StraightBCaT,
    DirectionRestriction = EMoveTargetDirectionRestriction.OnlyHorizontalSurface
};



Console.ReadLine();

