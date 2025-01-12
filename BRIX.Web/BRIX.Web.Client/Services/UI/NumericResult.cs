using Microsoft.AspNetCore.Components.Web;

namespace BRIX.Web.Client.Services.UI
{
    public class NumericResult(MouseEventArgs mea, int value = 0, ENumericAction action = ENumericAction.None)
    {
        public MouseEventArgs MouseEventArgs { get; set; } = mea;

        public int Value { get; set; } = value;

        public ENumericAction Action { get; set; } = action;

        public void ApplyAction(ref int initialValue)
        {
            initialValue = Action switch
            {
                ENumericAction.Add => initialValue + Value,
                ENumericAction.Substract => initialValue - Value,
                ENumericAction.Set => Value,
                _ => initialValue
            };
        }
    }

    public enum ENumericAction
    {
        None = 0,
        Add = 1,
        Substract = 2,
        Set = 3
    }
}
