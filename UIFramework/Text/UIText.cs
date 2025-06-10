using ProtoRender.Object;
using ProtoRender.WindowInterface;
using ScreenLib;
using SFML.Graphics;
using SFML.System;
using UIFramework.Render;
using UIFramework.Text.AlignEnums;


namespace UIFramework.Text;
public class UIText : UIElement
{  
    public override Vector2f PositionOnScreen
    {
        get => _positionOnScreen;
        set
        {
            _positionOnScreen = value;

            AdjustTextSize();
            RenderText.Text.Position = value;

        }
    }

    public override HorizontalAlign HorizontalAlignment
    {
        get => _horizontalAlignment;
        set
        {
            _horizontalAlignment = value;
            RenderText.Text.Origin = new Vector2f(GetHorizontalBounds(RenderText.Text.GetLocalBounds()), RenderText.Text.Origin.Y);
        }
    }
    public override VerticalAlign VerticalAlignment
    {
        get => _verticalAlignment;
        set
        {
            _verticalAlignment = value;
            RenderText.Text.Origin = new Vector2f(RenderText.Text.Origin.X, GetVerticalBounds(RenderText.Text.GetLocalBounds()));
        }
    }

    public uint OriginCharacterSize{ get; protected set; }
    public RenderText RenderText { get; set; }


    public UIText(string text, uint size, Vector2f position, string pathToFont, SFML.Graphics.Color color, IUnit? owner = null)
        :base(owner)
    {

        RenderText = new RenderText(text, size, position, pathToFont, color);
        OriginCharacterSize = RenderText.Text.CharacterSize;
        PositionOnScreen = position;

        Drawables.Add(RenderText.Text);
    }
    public UIText(RenderText render, IUnit? owner = null)
        : base(owner)
    {
        RenderText = new RenderText(render);
        OriginCharacterSize = RenderText.Text.CharacterSize;
        PositionOnScreen = render.Text.Position;

        Drawables.Add(RenderText.Text);
    }
    public UIText(UIText uIText)
        :base(uIText.Owner)
    {
        RenderText = new RenderText(uIText.RenderText);
        OriginCharacterSize = uIText.OriginCharacterSize;
        _positionOnScreen = uIText._positionOnScreen;

        VerticalAlignment = uIText.VerticalAlignment;
        HorizontalAlignment = uIText.HorizontalAlignment;

        Drawables.Add(RenderText.Text);
    }


    public virtual void AdjustTextSize()
    {
        uint previousCharacterSize = OriginCharacterSize == RenderText.Text.CharacterSize? 0: RenderText.Text.CharacterSize;
        RenderText.Text.CharacterSize = (uint)(OriginCharacterSize / Screen.MultHeight);
    }
    internal virtual void SetTextAsync(string text)
    {
        RenderText.Text.DisplayedString = text;

        var bounds = RenderText.Text.GetLocalBounds();
        RenderText.Text.Origin = new Vector2f(GetHorizontalBounds(bounds), GetVerticalBounds(bounds));
        RenderText.Text.Position = _positionOnScreen;
    }
    public virtual void SetText(string text)
    {
        if (RenderText.Text.DisplayedString == text)
            return;

        WriteQueue.EnqueueDraw(this, text);
    }


    #region IUIElement
    public override void ToggleVisibilityObject()
    {
        if (IsHide && Drawables.Count > 0)
            Drawables.Clear();
        else
        {
            if(!Drawables.Contains(RenderText.Text))
                Drawables.Add(RenderText.Text);
        }
    }
    #endregion
}
