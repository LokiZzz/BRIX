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
TargetSelectionAspect damageTSA = effDamage.GetAspect<TargetSelectionAspect>();
damageTSA.IsTargetSelectionIsRandom = true;
damageTSA.Strategy = ETargetType.Area;
damageTSA.Area.AreaType = AreaSettings.EAreaType.Sphere;

damageTSA.IsConcording = true;

MoveTargetEffect effSmash = new ()
{
    DistanceInMeters = 3,
    TargetPath = EMoveTargetPath.StraightBCaT,
    DirectionRestriction = EMoveTargetDirectionRestriction.OnlyHorizontalSurface
};

chargeAbility.AddEffect(effDamage);
chargeAbility.AddEffect(effSmash);

damageTSA = effDamage.GetAspect<TargetSelectionAspect>();
damageTSA.Area.AreaType = AreaSettings.EAreaType.Brick;
Brick? shape = damageTSA.Area.Shape as Brick;
shape.A = 3;
shape.B = 2;
shape.C = 5;

chargeAbility.Detach(damageTSA);

damageTSA = effDamage.GetAspect<TargetSelectionAspect>();
damageTSA.Area.AreaType = AreaSettings.EAreaType.Cylinder;

Console.ReadLine();

