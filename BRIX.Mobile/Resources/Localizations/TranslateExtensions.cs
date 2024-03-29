﻿using BRIX.Mobile.Services;

namespace BRIX.Mobile.Resources.Localizations
{
    [ContentProperty(nameof(Name))]
    public class TranslateExtension : IMarkupExtension<BindingBase>
    {
        public string? Name { get; set; }
        public string? StringFormat { get; set; }

        public BindingBase ProvideValue(IServiceProvider serviceProvider)
        {
            Binding binding = new ()
            {
                Mode = BindingMode.OneWay,
                Path = $"[{Name}]",
                Source = Resolver.Resolve<ILocalizationResourceManager>(),
            };

            if(!string.IsNullOrEmpty(StringFormat))
            {
                binding.StringFormat = StringFormat;
            }

            return binding;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }
    }
}
