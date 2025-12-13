//Code for CreditScreen
using GumRuntime;
using System.Linq;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using RIPRUSH.Components.Joelements;
using RIPRUSH.Components.Controls;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using RenderingLibrary.Graphics;

using System.Linq;

namespace RIPRUSH.Screens;
partial class CreditScreen : MonoGameGum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::MonoGameGum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("CreditScreen");
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new CreditScreen(visual);
            visual.Width = 0;
            visual.WidthUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        global::MonoGameGum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(CreditScreen)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("CreditScreen", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public ContainerRuntime ControlsPanel { get; protected set; }
    public NineSliceRuntime Background1 { get; protected set; }
    public ContainerRuntime MenuTitle1 { get; protected set; }
    public ContainerRuntime MenuItems1 { get; protected set; }
    public TextRuntime TitleText2 { get; protected set; }
    public TextRuntime CreditsText { get; protected set; }
    public MainMenuButton TurnBackButton { get; protected set; }
    public ScrollViewer CreditScroll { get; protected set; }

    public CreditScreen(InteractiveGue visual) : base(visual)
    {
    }
    public CreditScreen()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        ControlsPanel = this.Visual?.GetGraphicalUiElementByName("ControlsPanel") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        Background1 = this.Visual?.GetGraphicalUiElementByName("Background1") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        MenuTitle1 = this.Visual?.GetGraphicalUiElementByName("MenuTitle1") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        MenuItems1 = this.Visual?.GetGraphicalUiElementByName("MenuItems1") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        TitleText2 = this.Visual?.GetGraphicalUiElementByName("TitleText2") as global::MonoGameGum.GueDeriving.TextRuntime;
        CreditsText = this.Visual?.GetGraphicalUiElementByName("CreditsText") as global::MonoGameGum.GueDeriving.TextRuntime;
        TurnBackButton = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<MainMenuButton>(this.Visual,"TurnBackButton");
        CreditScroll = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ScrollViewer>(this.Visual,"CreditScroll");
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
