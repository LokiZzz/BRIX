using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Settings
{
    public static class Account
    {
        public const string RememberMe = nameof(RememberMe);
        public const string Login = nameof(Login);
        public const string Password = nameof(Password);
        public const string Culture = nameof(Culture);
    }

    public static class Help
    {
        public const string ShowCharactersListHelp = nameof(ShowCharactersListHelp);
        public const string ShowAbilitiesListHelp = nameof(ShowAbilitiesListHelp);
    }
}
