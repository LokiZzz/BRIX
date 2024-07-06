using BRIX.Mobile.Resources.Handlers;
using BRIX.Mobile.Services;
using Microsoft.Maui.Platform;
using System.Runtime.ExceptionServices;

namespace BRIX.Mobile;

public partial class App : Application
{
	public App(IServiceProvider serviceProvider)
	{
		InitializeComponent();

        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
        {
            if (view is BorderlessEntry)
            {
                #if __ANDROID__
                handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
                #elif __IOS__
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
                #elif WINDOWS
                handler.PlatformView.FontWeight = Microsoft.UI.Text.FontWeights.Thin;
                #endif
            }
        });

        Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping(nameof(BorderlessEditor), (handler, view) =>
        {
            if (view is BorderlessEditor)
            {
                #if __ANDROID__
                handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
                #elif __IOS__
                //handler.PlatformView.Layer.BorderWidth = 0;
                //handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
                #elif WINDOWS
                handler.PlatformView.FontWeight = Microsoft.UI.Text.FontWeights.Thin;
                #endif
            }
        });

        MainPage = serviceProvider.GetService<AppShell>();

        AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
    }

    private static readonly List<Type> ExceptionTypesToIgnore = [typeof(FormatException)];

    private void CurrentDomain_FirstChanceException(object? sender, FirstChanceExceptionEventArgs args)
    {
        bool ignoreException = ExceptionTypesToIgnore.Any(x => x.Equals(args.Exception.GetType()));

        if (ignoreException)
        {
            return;
        }

        Logger.LogError(args.Exception);
    }
}
