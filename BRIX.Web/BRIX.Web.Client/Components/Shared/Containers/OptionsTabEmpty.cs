using Newtonsoft.Json.Linq;

namespace BRIX.Web.Client.Components.Shared.Containers
{
    /// <summary>
    /// Пустой вариант компонента для случаев, когда переход по закладкам не должен присваивать значение какому-либо 
    /// свойству. Создан для чистоты в Blazor-коде, чтобы не указывать TValue="object" и Value="new object()".
    /// </summary>
    public class OptionsTabEmpty : OptionsTab<object>
    {
        public override void AddItem(OptionsTabItem<object> item)
        {
            Items.Add(item);
            ActiveItem ??= item;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Закладка для OptionsTabEmpty, для которой не обязательно указывать свойство Value для определения типа 
    /// обобщения.
    /// </summary>
    public class OptionsTabEmptyItem : OptionsTabItem<object>;
}
