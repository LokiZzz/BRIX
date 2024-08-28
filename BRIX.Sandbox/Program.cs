using BRIX.Library.Abilities;
using BRIX.Library.Aspects;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Library.Items;
using Newtonsoft.Json;

JsonSerializerSettings settings = new()
{
    Formatting = Formatting.Indented,
    TypeNameHandling = TypeNameHandling.All,
};

Ability ability = new();
ability.AddEffect(new DamageEffect());
ability.AddEffect(new AccelerationEffect());

string abilityJSON = JsonConvert.SerializeObject(ability, settings);

Ability? abilityRessurected = JsonConvert.DeserializeObject<Ability>(abilityJSON, settings);

TargetSizeAspect tsa = new ();
tsa.AddSize(BRIX.Library.Enums.ETargetSize.Gigantic);
tsa.AddSize(BRIX.Library.Enums.ETargetSize.Monstrous);
tsa.AddSize(BRIX.Library.Enums.ETargetSize.Colossal);

string aspectJSON = JsonConvert.SerializeObject(tsa, settings);

TargetSizeAspect? aspectRessurected = JsonConvert.DeserializeObject<TargetSizeAspect>(aspectJSON, settings);

string stop = "";