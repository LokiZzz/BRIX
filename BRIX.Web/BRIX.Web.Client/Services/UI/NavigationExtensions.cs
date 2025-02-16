using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace BRIX.Web.Client.Services.UI
{
    public static class NavigationExtensions
    {
        public static void NavigateToRelative(
            this NavigationManager navigation,
            [StringSyntax(StringSyntaxAttribute.Uri)] string uri,
            bool forceLoad = false,
            bool replace = false)
        {
            navigation.NavigateTo($"{navigation.Uri}/{uri}", forceLoad, replace);
        }
    }
}
