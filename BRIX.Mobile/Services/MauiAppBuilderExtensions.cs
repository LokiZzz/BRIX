using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Services
{
    public static class MauiAppBuilderExtensions
    {
        public static void UseServicePool(this MauiApp app)
        {
            Resolver.RegisterServiceProvider(app.Services);
        }
    }
}
