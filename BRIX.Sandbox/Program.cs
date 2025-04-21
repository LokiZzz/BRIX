using BRIX.Lexica;
using BRIX.Library.Characters;
using BRIX.Library.Effects;
using Newtonsoft.Json;

Character character = new();
string json = JsonConvert.SerializeObject(character);
Character? deserialized =  JsonConvert.DeserializeObject<Character>(json);

Guid original = character.Id;
Guid? deserializedGuid = deserialized?.Id;

bool allIsFine = original == deserializedGuid;