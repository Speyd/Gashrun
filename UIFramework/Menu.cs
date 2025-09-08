
using ObjectFramework;
using ProtoRender.Object;
using ScreenLib;
using SFML.Audio;
using UIFramework.Border;
using UIFramework.Shape;
using UIFramework.Sprite;
using UIFramework.Text;
using UIFramework.Windows.Button;

namespace UIFramework;
public class Menu
{
    public bool IsRun { get; private set; } = false;

    private IUnit? _owner = null;
    public IUnit? Owner
    {
        get => _owner;
        set
        {
            if (!IsRun)
            {
                _owner = value;
                return;
            }
            _owner = value;

            UIElements.ForEach(b => b.Owner = Owner);
            if (Background is not null)
                Background.Owner = value;
        }
    }

    public UIShape? Background { get; private set; } = null;
    public List<IUIElement> UIElements { private get; init; } = new();

    public Menu(IUnit? owner = null)
    {
        Owner = owner;
    }

    public void Run()
    {
        if (Owner is null)
        {
            IsRun = false;        
            return;
        }

        IsRun = true;
        Owner = _owner;

        Owner!.Control.FreezeControlsKey = true;
        Owner!.Control.FreezeControlsMouse = true;
        Screen.Window.SetMouseCursorVisible(true);
    }
    public void Stop()
    {
        if (Owner is null || !IsRun)
        {
            IsRun = false;
            return;
        }

        Owner.Control.FreezeControlsKey = false;
        Owner.Control.FreezeControlsMouse = false;
        Screen.Window.SetMouseCursorVisible(false);

        Owner = null;
        IsRun = false;
    }

    public void SetBackground(UIShape background)
    {
        Background = background;
        if(IsRun)
            Background.Owner = Owner;
    }

    public void AddUIElements(UIElement element)
    {
        UIElements.Add(element);
        if (IsRun)
            element.Owner = Owner;
    }
}
