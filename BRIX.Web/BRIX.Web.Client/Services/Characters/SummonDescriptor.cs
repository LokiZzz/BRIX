using BRIX.Library.Characters;
using BRIX.Library.Effects;

namespace BRIX.Web.Client.Services.Characters
{
    public class SummonDescriptor
    {
        // Позже можно выделить интерфейс для типов, имеющих способности и здесь ссылаться через него
        public required Character EditingCharacter { get; init; }

        public required Guid CreatureId { get; init; }

        public NPC? Summon => EditingCharacter.FindSummon(CreatureId, out _, out _, out _)
            ?? throw new Exception("Summoning creature not found");

        /// <summary>
        /// Заменяет призываемое существо в эффекте новым.
        /// </summary>
        public void UpdateSummon(NPC newSummon)
        {
            NPC? summon = EditingCharacter
                .FindSummon(CreatureId, out int? abilityIndex, out int? effectIndex, out int? creatureIndex);

            if (summon is not null && abilityIndex is not null && effectIndex is not null && creatureIndex is not null)
            {
                newSummon.Id = CreatureId;
                EditingCharacter
                    .Abilities[abilityIndex.Value]
                    .GetEffectByIndex<SummonCreatureEffect>(effectIndex.Value)
                    .Creatures[creatureIndex.Value]
                    .Creature = newSummon;
            }
            else
            {
                throw new Exception("Existing summon to update is not found.");
            }
        }

        public string GetSaveCallbackRoute()
        {
            // Если саммонер — персонаж
            NPC? summon = EditingCharacter.FindSummon(CreatureId, out int? abilityIndex, out int? effectIndex, out _);

            if (summon is not null && abilityIndex is not null  && effectIndex is not null)
            {
                return $"/character/{EditingCharacter.Id}/abilities/{abilityIndex}/effects/smn/{effectIndex}";
            }

            throw new Exception("Cannot find save summon callback route.");
        }
    }
}