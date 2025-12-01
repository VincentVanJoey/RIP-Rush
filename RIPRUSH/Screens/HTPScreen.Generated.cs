//Code for HTPScreen
using GumRuntime;
using System.Linq;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using RIPRUSH.Components.Controls;
using RIPRUSH.Components.Joelements;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using RenderingLibrary.Graphics;

using System.Linq;

namespace RIPRUSH.Screens;
partial class HTPScreen : MonoGameGum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::MonoGameGum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("HTPScreen");
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new HTPScreen(visual);
            visual.Width = 0;
            visual.WidthUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        global::MonoGameGum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(HTPScreen)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("HTPScreen", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public NineSliceRuntime powerexBack { get; protected set; }
    public NineSliceRuntime enemyBack { get; protected set; }
    public ContainerRuntime ControlsPanel { get; protected set; }
    public ContainerRuntime ObjectivePanel { get; protected set; }
    public ContainerRuntime PowerPanel { get; protected set; }
    public NineSliceRuntime Background1 { get; protected set; }
    public NineSliceRuntime Background2 { get; protected set; }
    public NineSliceRuntime Background3 { get; protected set; }
    public ContainerRuntime MenuTitle1 { get; protected set; }
    public ContainerRuntime MenuTitle2 { get; protected set; }
    public ContainerRuntime MenuTitle3 { get; protected set; }
    public ContainerRuntime MenuItems1 { get; protected set; }
    public ContainerRuntime MenuItems2 { get; protected set; }
    public ContainerRuntime MenuItems3 { get; protected set; }
    public TextRuntime TitleText2 { get; protected set; }
    public TextRuntime TitleText4 { get; protected set; }
    public TextRuntime TitleText6 { get; protected set; }
    public TextRuntime TitleText3 { get; protected set; }
    public TextRuntime TitleText5 { get; protected set; }
    public TextRuntime TitleText7 { get; protected set; }
    public Label CollectLabel { get; protected set; }
    public Label CollectLabel1 { get; protected set; }
    public Label CollectLabel2 { get; protected set; }
    public Label CollectLabel3 { get; protected set; }
    public MainMenuButton TurnBackButton { get; protected set; }
    public UFOpic UFOpic { get; protected set; }
    public Label AvoidLabel { get; protected set; }
    public CHOCOpic CHOCOpicInstance { get; protected set; }
    public POPpic POPpicInstance { get; protected set; }
    public ZAPpic ZAPpicInstance { get; protected set; }
    public eyepic eyepicInstance { get; protected set; }

    public HTPScreen(InteractiveGue visual) : base(visual)
    {
    }
    public HTPScreen()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        powerexBack = this.Visual?.GetGraphicalUiElementByName("powerexBack") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        enemyBack = this.Visual?.GetGraphicalUiElementByName("enemyBack") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        ControlsPanel = this.Visual?.GetGraphicalUiElementByName("ControlsPanel") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        ObjectivePanel = this.Visual?.GetGraphicalUiElementByName("ObjectivePanel") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        PowerPanel = this.Visual?.GetGraphicalUiElementByName("PowerPanel") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        Background1 = this.Visual?.GetGraphicalUiElementByName("Background1") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        Background2 = this.Visual?.GetGraphicalUiElementByName("Background2") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        Background3 = this.Visual?.GetGraphicalUiElementByName("Background3") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        MenuTitle1 = this.Visual?.GetGraphicalUiElementByName("MenuTitle1") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        MenuTitle2 = this.Visual?.GetGraphicalUiElementByName("MenuTitle2") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        MenuTitle3 = this.Visual?.GetGraphicalUiElementByName("MenuTitle3") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        MenuItems1 = this.Visual?.GetGraphicalUiElementByName("MenuItems1") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        MenuItems2 = this.Visual?.GetGraphicalUiElementByName("MenuItems2") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        MenuItems3 = this.Visual?.GetGraphicalUiElementByName("MenuItems3") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        TitleText2 = this.Visual?.GetGraphicalUiElementByName("TitleText2") as global::MonoGameGum.GueDeriving.TextRuntime;
        TitleText4 = this.Visual?.GetGraphicalUiElementByName("TitleText4") as global::MonoGameGum.GueDeriving.TextRuntime;
        TitleText6 = this.Visual?.GetGraphicalUiElementByName("TitleText6") as global::MonoGameGum.GueDeriving.TextRuntime;
        TitleText3 = this.Visual?.GetGraphicalUiElementByName("TitleText3") as global::MonoGameGum.GueDeriving.TextRuntime;
        TitleText5 = this.Visual?.GetGraphicalUiElementByName("TitleText5") as global::MonoGameGum.GueDeriving.TextRuntime;
        TitleText7 = this.Visual?.GetGraphicalUiElementByName("TitleText7") as global::MonoGameGum.GueDeriving.TextRuntime;
        CollectLabel = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"CollectLabel");
        CollectLabel1 = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"CollectLabel1");
        CollectLabel2 = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"CollectLabel2");
        CollectLabel3 = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"CollectLabel3");
        TurnBackButton = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<MainMenuButton>(this.Visual,"TurnBackButton");
        UFOpic = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<UFOpic>(this.Visual,"UFOpic");
        AvoidLabel = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"AvoidLabel");
        CHOCOpicInstance = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<CHOCOpic>(this.Visual,"CHOCOpicInstance");
        POPpicInstance = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<POPpic>(this.Visual,"POPpicInstance");
        ZAPpicInstance = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ZAPpic>(this.Visual,"ZAPpicInstance");
        eyepicInstance = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<eyepic>(this.Visual,"eyepicInstance");
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
