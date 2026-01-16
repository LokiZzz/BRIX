using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace BRIX.Web.Client.Services.UI
{
    public static class NavigationExtensions
    {
        public static void NavigateToRelative(
            this NavigationManager navigation,
            [StringSyntax(StringSyntaxAttribute.Uri)] string relativeUri,
            bool forceLoad = false,
            bool replace = false)
        {
            if (relativeUri.StartsWith('/'))
            {
                relativeUri = relativeUri[1..];
            }

            string queryParameters = string.Empty;
            string currentUri = navigation.Uri;

            if (currentUri.Contains('?'))
            {
                string[] splitted = currentUri.Split();
                currentUri = splitted[0];
                queryParameters = '?' + splitted[1];
            }

            navigation.NavigateTo($"{currentUri}/{relativeUri}{queryParameters}", forceLoad, replace);
        }
    }
}
