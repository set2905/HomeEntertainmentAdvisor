namespace HomeEntertainmentAdvisor.Localization
{
    using Microsoft.Extensions.Localization;
    using MudBlazor;
    using MudBlazor.Resources;

    internal class ResXMudLocalizer : MudLocalizer
    {
        private IStringLocalizer _localization;

        public ResXMudLocalizer(IStringLocalizer<LanguageResource> localizer)
        {
            _localization = localizer;
        }

        public override LocalizedString this[string key] => _localization[key];
    }
}
