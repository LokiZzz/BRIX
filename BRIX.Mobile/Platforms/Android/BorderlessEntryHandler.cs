using AndroidX.AppCompat.Widget;

namespace BRIX.Mobile.Platforms.Android
{
    public class BorderlessEntryHandler : Microsoft.Maui.Handlers.EntryHandler
    {
        protected override void ConnectHandler(AppCompatEditText platformView)
        {
            platformView.Background = null;
            base.ConnectHandler(platformView);
        }
    }
}
