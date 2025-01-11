using Microsoft.AspNetCore.Components.Web;

namespace BRIX.Web.Client.Services.UI
{
    public class AlertResult(MouseEventArgs mouseEventArgs, bool isPositive)
    {
        public MouseEventArgs MouseEventArgs { get; set; } = mouseEventArgs;

        public bool IsPositive { get; set; } = isPositive;
    }
}
