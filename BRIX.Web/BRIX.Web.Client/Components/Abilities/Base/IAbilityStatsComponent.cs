namespace BRIX.Web.Client.Components.Abilities.Base
{
    /// <summary>
    /// Компонент, показывающий статистику и производящий общие операции над способностью, специфичные для 
    /// определённого владельца — персонажа, NPС или артефакта.
    /// </summary>
    public interface IAbilityStatsComponent
    {
        /// <summary>
        /// Сохранить способность.
        /// </summary>
        /// <returns></returns>
        public Task SaveAsync();

        /// <summary>
        /// Сбросить изменения.
        /// </summary>
        public void Reset();

        /// <summary>
        /// Обновить состояние компонента.
        /// </summary>
        public void Refresh();
    }
}
