//Code for DemoScreenGum
using GumRuntime;
using System.Linq;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using RIPRUSH.Components.Controls;
using RIPRUSH.Components.Elements;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using RenderingLibrary.Graphics;

using System.Linq;

namespace RIPRUSH.Screens;
partial class DemoScreenGum : MonoGameGum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::MonoGameGum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("DemoScreenGum");
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new DemoScreenGum(visual);
            visual.Width = 0;
            visual.WidthUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        global::MonoGameGum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(DemoScreenGum)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("DemoScreenGum", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public ContainerRuntime DemoSettingsMenu { get; protected set; }
    public NineSliceRuntime Background { get; protected set; }
    public ContainerRuntime MenuTitle { get; protected set; }
    public ContainerRuntime MenuTitle1 { get; protected set; }
    public ContainerRuntime MenuItems { get; protected set; }
    public TextRuntime TitleText { get; protected set; }
    public TextRuntime TitleText1 { get; protected set; }
    public ButtonClose ButtonCloseInstance1 { get; protected set; }
    public DividerHorizontal DividerInstance { get; protected set; }
    public DividerHorizontal DividerInstance4 { get; protected set; }
    public Label ResolutionLabel { get; protected set; }
    public ListBox ResolutionBox { get; protected set; }
    public ButtonStandard DetectResolutionsButton { get; protected set; }
    public CheckBox FullScreenCheckbox { get; protected set; }
    public DividerHorizontal DividerInstance1 { get; protected set; }
    public Label MusicLabel { get; protected set; }
    public Slider MusicSlider { get; protected set; }
    public Label SoundLabel { get; protected set; }
    public Slider SoundSlider { get; protected set; }
    public DividerHorizontal DividerInstance2 { get; protected set; }
    public NineSliceRuntime Background1 { get; protected set; }
    public ContainerRuntime DemoDialog { get; protected set; }
    public ContainerRuntime MarginContainer { get; protected set; }
    public Label LabelInstance { get; protected set; }
    public TextBox TextBoxInstance { get; protected set; }
    public PasswordBox TextBoxInstance1 { get; protected set; }
    public TextBox MultiLineTextBox { get; protected set; }

    public DemoScreenGum(InteractiveGue visual) : base(visual)
    {
    }
    public DemoScreenGum()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        DemoSettingsMenu = this.Visual?.GetGraphicalUiElementByName("DemoSettingsMenu") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        Background = this.Visual?.GetGraphicalUiElementByName("Background") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        MenuTitle = this.Visual?.GetGraphicalUiElementByName("MenuTitle") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        MenuTitle1 = this.Visual?.GetGraphicalUiElementByName("MenuTitle1") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        MenuItems = this.Visual?.GetGraphicalUiElementByName("MenuItems") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        TitleText = this.Visual?.GetGraphicalUiElementByName("TitleText") as global::MonoGameGum.GueDeriving.TextRuntime;
        TitleText1 = this.Visual?.GetGraphicalUiElementByName("TitleText1") as global::MonoGameGum.GueDeriving.TextRuntime;
        ButtonCloseInstance1 = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonClose>(this.Visual,"ButtonCloseInstance1");
        DividerInstance = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<DividerHorizontal>(this.Visual,"DividerInstance");
        DividerInstance4 = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<DividerHorizontal>(this.Visual,"DividerInstance4");
        ResolutionLabel = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"ResolutionLabel");
        ResolutionBox = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ListBox>(this.Visual,"ResolutionBox");
        DetectResolutionsButton = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"DetectResolutionsButton");
        FullScreenCheckbox = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<CheckBox>(this.Visual,"FullScreenCheckbox");
        DividerInstance1 = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<DividerHorizontal>(this.Visual,"DividerInstance1");
        MusicLabel = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"MusicLabel");
        MusicSlider = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Slider>(this.Visual,"MusicSlider");
        SoundLabel = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"SoundLabel");
        SoundSlider = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Slider>(this.Visual,"SoundSlider");
        DividerInstance2 = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<DividerHorizontal>(this.Visual,"DividerInstance2");
        Background1 = this.Visual?.GetGraphicalUiElementByName("Background1") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        DemoDialog = this.Visual?.GetGraphicalUiElementByName("DemoDialog") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        MarginContainer = this.Visual?.GetGraphicalUiElementByName("MarginContainer") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        LabelInstance = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"LabelInstance");
        TextBoxInstance = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<TextBox>(this.Visual,"TextBoxInstance");
        TextBoxInstance1 = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<PasswordBox>(this.Visual,"TextBoxInstance1");
        MultiLineTextBox = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<TextBox>(this.Visual,"MultiLineTextBox");
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
