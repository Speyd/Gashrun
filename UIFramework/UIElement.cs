using ProtoRender.Object;
using ScreenLib;
using SFML.Graphics;
using SFML.System;
using UIFramework.Render;
using UIFramework.Text.AlignEnums;


namespace UIFramework;
public abstract class UIElement : IUIElement
{

    public virtual float PreviousScreenWidth { get; protected set; } = Screen.ScreenWidth;
    public virtual float PreviousScreenHeight { get; protected set; } = Screen.ScreenHeight;

    protected bool _isHide = false;
    public virtual bool IsHide
    {
        get => _isHide;
        set
        {
            if (_isHide != value)
            {
                _isHide = value;
                ToggleVisibilityObject();
            }

        }
    }

    protected  Vector2f _positionOnScreen;
    public virtual Vector2f PositionOnScreen
    {
        get => _positionOnScreen;
        set
        {
            _positionOnScreen = value;
            HorizontalAlignment = HorizontalAlignment;
            VerticalAlignment = VerticalAlignment;
        }
    }
    public virtual List<Drawable> Drawables { get; init; } = new();

    protected RenderOrder _renderOrder = RenderOrder.Indicators;
    public virtual RenderOrder RenderOrder
    {
        get => _renderOrder;
        set
        {
            IUIElement.SetRenderOrder(Owner, _renderOrder, value, this);
            _renderOrder = value;
        }
    }

    protected IUnit? _owner = null;
    public virtual IUnit? Owner
    {
        get => _owner;
        set
        {
            if (_owner is null && value is null)
                return;

            IUIElement.SetOwner(_owner, value, this);
            _owner = value;
        }
    }

    protected HorizontalAlign _horizontalAlignment = HorizontalAlign.None;
    public virtual HorizontalAlign HorizontalAlignment { get; set; } = HorizontalAlign.None;
    protected VerticalAlign _verticalAlignment = VerticalAlign.None;
    public virtual VerticalAlign VerticalAlignment { get; set; } = VerticalAlign.None;


    public UIElement(IUnit? owner = null)
    {
        Owner = owner;

        Screen.WidthChangesFun += UpdateScreenInfo;
        Screen.HeightChangesFun += UpdateScreenInfo;
    }



    public virtual float GetHorizontalBounds(FloatRect bounds)
    {
        float width = bounds.Width;
        float offsetX = bounds.Left;

        float boundsX = HorizontalAlignment switch
        {
            HorizontalAlign.Right => bounds.Left,
            HorizontalAlign.Center => bounds.Left + bounds.Width / 2f,
            HorizontalAlign.Left => bounds.Left + bounds.Width,
            _ => bounds.Left
        };

        return boundsX;
    }
    public virtual float GetVerticalBounds(FloatRect bounds)
    {
        float height = bounds.Height;
        float offsetY = bounds.Top;

        float boundsY = VerticalAlignment switch
        {
            VerticalAlign.Bottom => bounds.Top,
            VerticalAlign.Center => bounds.Top + bounds.Height / 2f,
            VerticalAlign.Top => bounds.Top + bounds.Height,
            _ => bounds.Top
        };

        return boundsY;
    }

    public virtual void UpdateWidth()
    {
        if (Screen.ScreenWidth == PreviousScreenWidth)
            return;

        float widthScale = Screen.ScreenWidth / PreviousScreenWidth;
        PositionOnScreen = new Vector2f(PositionOnScreen.X * widthScale, PositionOnScreen.Y);

        PreviousScreenWidth = Screen.ScreenWidth;
    }
    public virtual void UpdateHeight()
    {
        if (Screen.ScreenHeight == PreviousScreenHeight)
            return;

        float heightScale = Screen.ScreenHeight / PreviousScreenHeight;
        PositionOnScreen = new Vector2f(PositionOnScreen.X, PositionOnScreen.Y * heightScale);

        PreviousScreenHeight = Screen.ScreenHeight;
    }

    public virtual void Render()
    {
        if(IsHide) return;

        UpdateInfo();
        foreach (var draw in Drawables)
            Screen.OutputPriority?.AddToPriority(IUIElement.OutputPriorityType, draw);
    }
    public virtual void UpdateInfo()
    {

    }
    public virtual void UpdateScreenInfo()
    {
        UpdateWidth();
        UpdateHeight();
    }
    public virtual void ToggleVisibility()
    {
        IsHide = !IsHide;
    }
    public abstract void ToggleVisibilityObject();
}
