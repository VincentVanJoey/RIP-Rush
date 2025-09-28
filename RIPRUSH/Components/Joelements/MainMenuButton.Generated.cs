//Code for Joelements/MainMenuButton (Container)
using GumRuntime;
using System.Linq;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using RenderingLibrary.Graphics;

using System.Linq;

namespace RIPRUSH.Components.Joelements;
partial class MainMenuButton : global::MonoGameGum.Forms.Controls.Button
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::MonoGameGum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("Joelements/MainMenuButton");
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new MainMenuButton(visual);
            return visual;
        });
        global::MonoGameGum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(MainMenuButton)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("Joelements/MainMenuButton", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public enum ButtonCategory
    {
        Enabled,
        Disabled,
        Highlighted,
        Pushed,
        HighlightedFocused,
        Focused,
        DisabledFocused,
    }

    ButtonCategory? _buttonCategoryState;
    public ButtonCategory? ButtonCategoryState
    {
        get => _buttonCategoryState;
        set
        {
            _buttonCategoryState = value;
            if(value != null)
            {
                if(Visual.Categories.ContainsKey("ButtonCategory"))
                {
                    var category = Visual.Categories["ButtonCategory"];
                    var state = category.States.Find(item => item.Name == value.ToString());
                    this.Visual.ApplyState(state);
                }
                else
                {
                    var category = ((global::Gum.DataTypes.ElementSave)this.Visual.Tag).Categories.FirstOrDefault(item => item.Name == "ButtonCategory");
                    var state = category.States.Find(item => item.Name == value.ToString());
                    this.Visual.ApplyState(state);
                }
            }
        }
    }
    public SpriteRuntime Background { get; protected set; }
    public TextRuntime ButtonWords { get; protected set; }

    public int MenuButtonTextColorBlue
    {
        get => ButtonWords.Blue;
        set => ButtonWords.Blue = value;
    }

    public int MenuButtonFontSize
    {
        get => ButtonWords.FontSize;
        set => ButtonWords.FontSize = value;
    }

    public int MenuButtonTextColorGreen
    {
        get => ButtonWords.Green;
        set => ButtonWords.Green = value;
    }

    public int MenuButtonTextColorRed
    {
        get => ButtonWords.Red;
        set => ButtonWords.Red = value;
    }

    public string buttonText
    {
        get => ButtonWords.Text;
        set => ButtonWords.Text = value;
    }

    public MainMenuButton(InteractiveGue visual) : base(visual)
    {
    }
    public MainMenuButton()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        Background = this.Visual?.GetGraphicalUiElementByName("Background") as global::MonoGameGum.GueDeriving.SpriteRuntime;
        ButtonWords = this.Visual?.GetGraphicalUiElementByName("ButtonWords") as global::MonoGameGum.GueDeriving.TextRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
