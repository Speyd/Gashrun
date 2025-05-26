using ProtoRender.Object;
using ProtoRender.WindowInterface;
using ScreenLib;
using SFML.Graphics;
using SFML.System;
using UIFramework.Render;
using UIFramework.Text.AlignEnums;


namespace UIFramework.Text;
public class UIText : RenderText, IUIElement
{
    public float PreviousScreenWidth { get; protected set; } = Screen.ScreenWidth;
    public float PreviousScreenHeight { get; protected set; } = Screen.ScreenHeight;

    public bool _isHide = false;
    public bool IsHide
    {  get => _isHide;
       set
        {
            if (_isHide != value)
            {
                _isHide = value;
                Hide();
            }

        }
    }

    private Vector2f _positionOnScreen;
    public Vector2f PositionOnScreen
    {
        get => _positionOnScreen;
        set
        {
            _positionOnScreen = value;

            AdjustTextSize();
            Text.Position = value;

        }
    }
    public List<Drawable> Drawables { get; init; } = new();

    private RenderOrder _renderOrder = RenderOrder.Indicators;
    public RenderOrder RenderOrder
    {
        get => _renderOrder;
        set
        {
            IUIElement.SetRenderOrder(Owner, _renderOrder, value, this);
            _renderOrder = value;
        }
    }


    private IUnit? _owner = null;
    public IUnit? Owner
    {
        get => _owner;
        set
        {
            IUIElement.SetOwner(_owner, value, this);
            _owner = value;
        }
    }
    public uint OriginCharacterSize{ get; protected set; }

    public VerticalAlign VerticalAlignment { get; set; } = VerticalAlign.None;
    public HorizontalAlign HorizontalAlignment { get; set; } = HorizontalAlign.None;


    public UIText(string text, uint size, Vector2f position, string pathToFont, SFML.Graphics.Color color, IUnit? owner = null)
        :base(text, size, position, pathToFont, color)
    {
        Owner = owner;

        OriginCharacterSize = Text.CharacterSize;
        PositionOnScreen = position;

        Screen.WidthChangesFun += UpdateScreenInfo;
        Screen.HeightChangesFun += UpdateScreenInfo;

        Drawables.Add(Text);
    }
    public UIText(RenderText render, IUnit? owner = null)
        : base(render)
    {
        Owner = owner;

        OriginCharacterSize = Text.CharacterSize;
        PositionOnScreen = render.Text.Position;

        Screen.WidthChangesFun += UpdateScreenInfo;
        Screen.HeightChangesFun += UpdateScreenInfo;

        Drawables.Add(Text);
    }
    public UIText(UIText uIText)
        : base(uIText)
    {
        Owner = uIText.Owner;

        OriginCharacterSize = uIText.OriginCharacterSize;
        _positionOnScreen = uIText._positionOnScreen;

        VerticalAlignment = uIText.VerticalAlignment;
        HorizontalAlignment = uIText.HorizontalAlignment;

        Screen.WidthChangesFun += UpdateScreenInfo;
        Screen.HeightChangesFun += UpdateScreenInfo;

        Drawables.Add(Text);
    }


    public virtual void AdjustTextSize()
    {
        uint previousCharacterSize = OriginCharacterSize == Text.CharacterSize? 0: Text.CharacterSize;
        Text.CharacterSize = (uint)(OriginCharacterSize / Screen.MultHeight);
    }
    public float GetHorizontalBounds()
    {
        FloatRect bounds = Text.GetLocalBounds();
        float width = bounds.Width;
        float offsetX = bounds.Left;

        float boundsX = 0;
        switch (HorizontalAlignment)
        {
            case HorizontalAlign.Center:
                boundsX = width / 2f + offsetX;
                break;
            case HorizontalAlign.Right:
                boundsX = width + offsetX;
                break;
            case HorizontalAlign.Left:
                boundsX = offsetX;
                break;
        }

        return boundsX;
    }
    public float GetVerticalBounds()
    {
        FloatRect bounds = Text.GetLocalBounds();
        float height = bounds.Height;
        float offsetY = bounds.Top;

        float boundsY = 0;
        switch (VerticalAlignment)
        {
            case VerticalAlign.Center:
                boundsY = height / 2f + offsetY;
                break;
            case VerticalAlign.Top:
                boundsY = offsetY;
                break;
            case VerticalAlign.Bottom:
                boundsY = height + offsetY;
                break;
        }

        return boundsY;
    }
    internal virtual void SetTextAsync(string text)
    {
        Text.DisplayedString = text;

        Text.Origin = new Vector2f(GetHorizontalBounds(), GetVerticalBounds());
        Text.Position = _positionOnScreen;
    }
    public virtual void SetText(string text)
    {
        if (Text.DisplayedString == text)
            return;

        WriteQueue.EnqueueDraw(this, text);
    }


    public void UpdateWidth()
    {
        if (Screen.ScreenWidth == PreviousScreenWidth)
            return;

        float widthScale = Screen.ScreenWidth / PreviousScreenWidth;
        PositionOnScreen = new Vector2f(PositionOnScreen.X * widthScale, PositionOnScreen.Y);

        PreviousScreenWidth = Screen.ScreenWidth;
    }
    public void UpdateHeight()
    {
        if (Screen.ScreenHeight == PreviousScreenHeight)
            return;

        float heightScale = Screen.ScreenHeight / PreviousScreenHeight;
        PositionOnScreen = new Vector2f(PositionOnScreen.X, PositionOnScreen.Y * heightScale);

        PreviousScreenHeight = Screen.ScreenHeight;
    }


    #region IUIElement
    public void Render()
    {
        UpdateInfo();
        foreach (var draw in Drawables)
            Screen.OutputPriority?.AddToPriority(IUIElement.OutputPriorityType, draw);
    }
    public virtual void UpdateInfo()
    {

    }
    public void UpdateScreenInfo()
    {
        UpdateWidth();
        UpdateHeight();
    }
    public void Hide()
    {
        if (IsHide && Drawables.Count > 0)
            Drawables.Clear();
        else
        {
            if(!Drawables.Contains(Text))
                Drawables.Add(Text);
        }
    }
    #endregion
}
