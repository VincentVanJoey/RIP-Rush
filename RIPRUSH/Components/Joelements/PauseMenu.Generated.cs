//Code for Joelements/PauseMenu (Container)
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

namespace RIPRUSH.Components.Joelements;
partial class PauseMenu : MonoGameGum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::MonoGameGum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("Joelements/PauseMenu");
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new PauseMenu(visual);
            return visual;
        });
        global::MonoGameGum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(PauseMenu)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("Joelements/PauseMenu", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public NineSliceRuntime Background { get; protected set; }
    public Label PauseLabel { get; protected set; }
    public MainMenuButton PauseResumeButton { get; protected set; }
    public MainMenuButton PauseSettingsButton { get; protected set; }
    public MainMenuButton PauseTitleButton { get; protected set; }
    public StackPanel StackPanelInstance { get; protected set; }

    public PauseMenu(InteractiveGue visual) : base(visual)
    {
    }
    public PauseMenu()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        Background = this.Visual?.GetGraphicalUiElementByName("Background") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        PauseLabel = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"PauseLabel");
        PauseResumeButton = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<MainMenuButton>(this.Visual,"PauseResumeButton");
        PauseSettingsButton = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<MainMenuButton>(this.Visual,"PauseSettingsButton");
        PauseTitleButton = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<MainMenuButton>(this.Visual,"PauseTitleButton");
        StackPanelInstance = global::MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<StackPanel>(this.Visual,"StackPanelInstance");
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
