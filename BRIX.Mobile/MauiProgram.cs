using BRIX.Library.Aspects;
using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.Resources.Handlers;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Abilities;
using BRIX.Mobile.View.Abilities.Aspects;
using BRIX.Mobile.View.Abilities.Effects;
using BRIX.Mobile.View.Account;
using BRIX.Mobile.View.Characters;
using BRIX.Mobile.View.Details;
using BRIX.Mobile.View.Inventory;
using BRIX.Mobile.View.NPCs;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.View.Settings;
using BRIX.Mobile.ViewModel;
using BRIX.Mobile.ViewModel.Abilities;
using BRIX.Mobile.ViewModel.Abilities.Aspects;
using BRIX.Mobile.ViewModel.Abilities.Effects;
using BRIX.Mobile.ViewModel.Account;
using BRIX.Mobile.ViewModel.Characters;
using BRIX.Mobile.ViewModel.Details;
using BRIX.Mobile.ViewModel.Inventory;
using BRIX.Mobile.ViewModel.NPCs;
using BRIX.Mobile.ViewModel.Popups;
using BRIX.Mobile.ViewModel.Settings;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BRIX.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        MauiAppBuilder builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("EBGaramond-Regular.ttf", "Garamond");
				fonts.AddFont("SourceSansPro-Regular.ttf", "SourceSansPro");
				fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "Awesome");
				fonts.AddFont("Font Awesome 6 Brands-Regular-400.otf", "AwesomeBrands");
				fonts.AddFont("rpgawesome-webfont.ttf", "AwesomeRPG");
            })
            .ConfigureEssentials(essentials => essentials.UseVersionTracking());

        builder.RegisterServices();
        builder.RegisterViews();

        MauiApp app = builder.Build();
        app.UseServicePool();

        ILocalStorage localStorage = Resolver.Resolve<ILocalStorage>();

        return app;
	}

    public static void RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IAccountService, AccountServiceMoq>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<ICharacterService, JsonCharacterService>();
        builder.Services.AddSingleton<IAssetsService, JsonAssetsService>();
        builder.Services.AddSingleton<ILocalStorage, PreferencesStorage>();
        builder.Services.AddSingleton<ILocalizationResourceManager, LocalizationResourceManager>();
    }

    public static void RegisterViews(this MauiAppBuilder builder)
    {
        builder.RegisterView<AppShell, AppShellVM>();

        builder.RegisterView<SignInPage, SignInPageVM>(false);

        builder.RegisterView<CharacterPage, CharacterPageVM>(false);
        builder.RegisterView<CharacterAbilitiesPage, CharacterAbilitiesPageVM>(false);
        builder.RegisterView<CharacterInventoryPage, CharacterInventoryPageVM>(false);
        builder.RegisterView<CharacterDetailsPage, CharacterDetailsPageVM>(false);
        builder.RegisterView<CharacterListPage, CharacterListPageVM>();
        builder.RegisterView<AOECharacterPage, AOECharacterPageVM>();
        builder.RegisterView<EditCharacterImagePage, EditCharacterImagePageVM>();
        builder.RegisterView<AddOrEditProjectPage, AddOrEditProjectPageVM>();
        builder.RegisterView<AddHealthPage, AddHealthPageVM>();
        builder.RegisterView<EditSpeedPage, EditSpeedPageVM>();
        builder.RegisterView<AddOrEditInventoryItemPage, AddOrEditInventoryItemPageVM>();
        builder.RegisterView<AOEAbilityPage, AOEAbilityPageVM>();
        builder.RegisterView<AbilityActivationSettingsPage, AbilityActivationSettingsPageVM>();
        builder.RegisterView<CharacterStatusesPage, CharacterStatusesPageVM>();
        builder.RegisterView<AOEStatusPage, AOEStatusPageVM>();

        AddEffectsAndAspects(builder);

        builder.RegisterView<NPCsPage, NPCsPageVM>(false);
        builder.RegisterView<AOENPCsPage, AOENPCsPageVM>();
        builder.RegisterView<EncounterCalculatorPage, EncounterCalculatorPageVM>(false);

        builder.RegisterView<SettingsPage, SettingsPageVM>(false);
        builder.RegisterView<LogPage, LogPageVM>();

        AddPopups(builder);
    }

    private static void AddPopups(MauiAppBuilder builder)
    {
        builder.RegisterPopup<NumericEditorPopup, NumericEditorPopupVM>();
        builder.RegisterPopup<AlertPopup, AlertPopupVM>();
        builder.RegisterPopup<DiceValuePopup, DiceValuePopupVM>();
        builder.RegisterPopup<PickerPopup, PickerPopupVM>();
        builder.RegisterPopup<EntryPopup, EntryPopupVM>();
    }

    private static void AddEffectsAndAspects(MauiAppBuilder builder)
    {
        builder.RegisterView<ChooseEffectPage, ChooseEffectPageVM>();

        builder.RegisterView<DamageEffectPage, DamageEffectPageVM>();
        builder.RegisterView<HealEffectPage, DiceImpactEffectPageVMBase<HealEffect>>();
        builder.RegisterView<FortifyEffectPage, DiceImpactEffectPageVMBase<FortifyEffect>>();
        builder.RegisterView<ExhaustionEffectPage, DiceImpactEffectPageVMBase<ExhaustionEffect>>();
        builder.RegisterView<AccelerationEffectPage, DiceImpactEffectPageVMBase<AccelerationEffect>>();
        builder.RegisterView<DecelerationEffectPage, DiceImpactEffectPageVMBase<DecelerationEffect>>();
        builder.RegisterView<DefenseEffectPage, DiceImpactEffectPageVMBase<DefenseEffect>>();
        builder.RegisterView<VulnerabilityEffectPage, DiceImpactEffectPageVMBase<VulnerabilityEffect>>();
        builder.RegisterView<AmplificationEffectPage, DiceImpactEffectPageVMBase<AmplificationEffect>>();
        builder.RegisterView<ReductionEffectPage, DiceImpactEffectPageVMBase<ReductionEffect>>();
        builder.RegisterView<CleanseEffectPage, EffectPageVMBase<CleanseEffectModel>>();
        builder.RegisterView<CancelationEffectPage, EffectPageVMBase<CancelationEffectModel>>();
        builder.RegisterView<MoveTargetEffectPage, MoveTargetEffectPageVM>();
        builder.RegisterView<MoveAreaEffectPage, EffectPageVMBase<MoveAreaEffectModel>>();
        builder.RegisterView<ShieldEffectPage, EffectPageVMBase<ShieldEffectModel>>();
        builder.RegisterView<PeriodicDamageEffectPage, DiceImpactEffectPageVMBase<PeriodicDamageEffect>>();
        builder.RegisterView<DifficultTerrainEffectPage, SinglePropEffectPageVMBase<DifficultTerrainEffect>>();
        builder.RegisterView<DangerousTerrainEffectPage, DangerousTerrainEffectPageVM>();
        builder.RegisterView<InvisibilityEffectPage, EffectPageVMBase<EffectGenericModelBase<InvisibiltyEffect>>>();
        builder.RegisterView<SummonCreatureEffectPage, SummonCreatureEffectPageVM>();
        builder.RegisterView<RevengeEffectPage, SinglePropEffectPageVMBase<RevengeEffect>>();
        
        builder.RegisterView<WinEffectPage, EffectPageVMBase<EffectGenericModelBase<WinTheGameEffect>>>();

        builder.RegisterView<TargetSelectionAspectPage, TargetSelectionAspectPageVM>();
        builder.RegisterView<ActivationConditionsAspectPage, ActivationConditionsAspectPageVM>();
        builder.RegisterView<DurationAspectPage, DurationAspectPageVM>();
        builder.RegisterView<AOEAspectPage, AOEAspectPageVM>();
        builder.RegisterView<VampirismAspectPage, VampirismAspectPageVM>();
    }

    /// <param name="registerRoute"> Set false if view already registered in AppShell.xaml </param>
    private static void RegisterView<TView, TViewModel>(this MauiAppBuilder builder, bool registerRoute = true)
        where TView : Page where TViewModel : ObservableObject
    {
        builder.Services.AddTransient<TView>();
        builder.Services.AddTransient<TViewModel>();

        if (registerRoute)
        {
            Routing.RegisterRoute(typeof(TView).Name, typeof(TView));
        }
    }

    private static void RegisterPopup<TView, TViewModel>(this MauiAppBuilder builder)
        where TView : Popup where TViewModel : ObservableObject
    {
        builder.Services.AddTransient<TView>();
        builder.Services.AddTransient<TViewModel>();
    }
}
