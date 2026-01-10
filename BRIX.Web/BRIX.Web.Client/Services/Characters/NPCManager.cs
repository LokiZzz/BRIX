using BRIX.Library.Abilities;
using BRIX.Library.Characters;
using BRIX.Library.Effects;
using BRIX.Utility.Extensions;
using BRIX.Web.Client.Extensions;
using BRIX.Web.Client.Models.Common;
using BRIX.Web.Client.Options;
using BRIX.Web.Client.Services.Http;
using BRIX.Web.Client.Services.UI;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;

namespace BRIX.Web.Client.Services.Characters
{
    public class NPCManager(
        HttpClientBuilder httpClientBuilder,
        IOptions<GameServiceOptions> gameServiceOptions,
        ModalService modalService,
        NavigationManager navigation)
    {
        private readonly string _baseAddress = gameServiceOptions.Value.ServiceAddress;

        /// <summary>
        /// Временное хранилище для изменяемого неигрового персонажа.
        /// </summary>
        public NPC? EditingNPC { get; private set; }

        public SummonDescriptor? Summon { get; private set; }

        public async Task<List<NPC>> GetAllAsync()
        {
            modalService.IsBusy = true;
            using HttpClient http = await httpClientBuilder.CreateAsync(_baseAddress);
            JsonResponse<List<NPC>> response = await http.GetJsonAsync<List<NPC>>("api/npc");
            modalService.IsBusy = false;

            OperationResult result = response.ToOperationResult();

            if(result.Successfull)
            {
                return response.Payload ?? [];
            }

            modalService.PushErrors(result.Errors);

            return [];
        }

        public async Task<NPC?> GetAsync(Guid id)
        {
            modalService.IsBusy = true;
            string uri = QueryHelpers.AddQueryString("api/npc", "id", id.ToString());
            using HttpClient http = await httpClientBuilder.CreateAsync(_baseAddress);
            JsonResponse<List<NPC>> response = await http.GetJsonAsync<List<NPC>>(uri);
            modalService.IsBusy = false;

            OperationResult result = response.ToOperationResult();

            if (result.Successfull)
            {
                return response.Payload?.FirstOrDefault();
            }

            modalService.PushErrors(result.Errors);

            return null;
        }

        public async Task<OperationResult> DeleteAsync(NPC npc)
        {
            modalService.IsBusy = true;

            using HttpClient http = await httpClientBuilder.CreateAsync(_baseAddress);
            JsonResponse response = await http.DeleteJsonAsync($"api/npc?id={npc.Id}");

            OperationResult result = response.ToOperationResult();

            modalService.PushErrors(result.Errors);
            modalService.IsBusy = false;

            return result;
        }

        public async Task<OperationResult> SaveAsync(NPC npc)
        {
            modalService.IsBusy = true;
            using HttpClient http = await httpClientBuilder.CreateAsync(_baseAddress);
            JsonResponse response = await http.PutJsonAsync("api/npc", npc);
            OperationResult result = response.ToOperationResult();

            if (result.Successfull)
            {
                Reset();
            }    

            modalService.PushErrors(result.Errors);
            modalService.IsBusy = false;

            return result;
        }

        /// <summary>
        /// Сохранить изменения, сделанные в EditingCharacter.
        /// </summary>
        public async Task<OperationResult> SaveAsync() => 
            await SaveAsync(EditingNPC ?? throw new Exception("No editing NPC."));

        public void SaveSummon()
        {
            Summon?.UpdateSummon(EditingNPC ?? throw new Exception("No editing NPC to update summon."));
            Reset();
        }

        /// <summary>
        /// Забрать персонажа с сервера и войти в режим редактирования. Если персонаж на сервере не найден — создать.
        /// </summary>
        public async Task EditNPCAsync(Guid id)
        {
            NPC? npc = await GetAsync(id);

            if (npc is null)
            {
                npc = new();
            }

            EditNPC(npc);
        }

        /// <summary>
        /// Войти в режим редактирования. Персонаж копируется в свойство EditingCharacter и в любой момент
        /// изменения можно будет сбросить методов Reset, обнулив свойство и заново забрав персонажа с сервера.
        /// </summary>
        public void EditNPC(NPC? npc = null)
        {
            if(npc is not null)
            {
                Reset();
                EditingNPC = npc.Copy();
            }
            else
            {
                EditingNPC = new NPC();
            }

            navigation.LocationChanged += ResetIfExitEditing;
        }

        public void EditSummon(SummonDescriptor summoning)
        {
            Summon = summoning;
            EditNPC(summoning.Summon);
        }

        public void Reset()
        {
            EditingNPC = null;
            navigation.LocationChanged -= ResetIfExitEditing;
        }

        /// <summary>
        /// Инициализация процесса редактирования для способности. Позволяет идемпотентно инциализировать нужные
        /// свойства. Инициалзиация произойдёт корректно, даже если пользователь перейдёт по адресу напрямую, например,
        /// по ссылке. Если порядковый номер способности не указан, то метод добавит персонажу новую способность.
        /// </summary>
        /// <returns>
        /// Порядковый номер способности, необходимый при навигации, если что-то пошло не так, то вернёт null
        /// </returns>
        public async Task<int?> AddOrEditAbility(Guid npcId, int? abilityNumber = null)
        {
            // Если персонаж для редактирования не выбран или не совпадает, то выбрать.
            if (EditingNPC is null || EditingNPC.Id != npcId)
            {
                await EditNPCAsync(npcId);
            }

            if (EditingNPC is null)
            {
                throw new Exception("Не удалось войти в режим редактирования персонажа.");
            }

            // Если айди способности в параметрах не указан, значит её нужно создать и добавить персонажу,
            // а потом установить её айди.
            if (abilityNumber is null)
            {
                Ability newAbility = new();
                EditingNPC?.AddAbility(newAbility);
                abilityNumber = EditingNPC!.Abilities.IndexOf(newAbility);
            }

            if (abilityNumber is null || EditingNPC?.Abilities.ElementAtOrDefault(abilityNumber.Value) is null)
            {
                throw new Exception("Не удалось войти в режим редактирования способности.");
            }

            return abilityNumber;
        }

        private void ResetIfExitEditing(object? sender, LocationChangedEventArgs e)
        {
            if (!e.Location.Contains("/abilities/"))
            {
                Reset();
            }
        }
    }

    public class SummonDescriptor
    {
        // Позже можно выделить интерфейс для типов, имеющих способности и здесь ссылаться через него
        public required Character EditingCharacter { get; set; }

        public required Guid CreatureId { get; set; }

        public NPC? Summon => EditingCharacter.FindSummon(CreatureId, out _, out _, out _)
            ?? throw new Exception("Summoning creature not found");

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
            NPC? summon = EditingCharacter.FindSummon(CreatureId, out int? abilityIndex, out int? effectIndex);

            if (summon is not null && abilityIndex is not null  && effectIndex is not null)
            {
                return $"/character/{EditingCharacter.Id}/abilities/{abilityIndex}/effects/smn/{effectIndex}";
            }

            throw new Exception("Cannot find save summon callback route.");
        }
    }
}