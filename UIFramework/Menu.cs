
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

            Buttons.ForEach(b => b.Owner = Owner);
            Texts.ForEach(t => t.Owner = Owner);
            Sprites.ForEach(s => s.Owner = Owner);
            if (Background is not null)
                Background.Owner = value;
        }
    }

    public UIShape? Background { get; private set; } = null;
    public List<UIButton> Buttons { private get; init; } = new();
    public List<UIText> Texts { private get; init; } = new();
    public List<UISprite> Sprites { private get; init; } = new();


    public Menu(IUnit? owner = null)
    {
        Owner = owner;
    }

    public void Run()
    {
        if(Owner is null)
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

    public void AddButton(UIButton button)
    {
        Buttons.Add(button);
        if (IsRun)
            button.Owner = Owner;
    }

    public void AddText(UIText text)
    {
        Texts.Add(text);
        if (IsRun)
            text.Owner = Owner;
    }

    public void AddSprites(UISprite sprite)
    {
        Sprites.Add(sprite);
        if (IsRun)
            sprite.Owner = Owner;
    }
}
