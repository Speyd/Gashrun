using AnimationLib;
using AnimationLib.Core.Elements;
using ProtoRender.Object;
using ScreenLib;
using SFML.Graphics;
using SFML.System;
using TextureLib.Loader;
using TextureLib.Textures;
using UIFramework.Border;
using UIFramework.Render;
using UIFramework.Text.AlignEnums;


namespace UIFramework.Shape;
public class UIShape : UIElement, ITransformable2D
{
    public override Vector2f PositionOnScreen
    {
        get => _positionOnScreen;
        set
        {
            _positionOnScreen = value;
            RectangleShape.Position = value;

            HorizontalAlignment = HorizontalAlignment;
            VerticalAlignment = VerticalAlignment;
        }
    }
    public override RenderOrder RenderOrder
    {
        get => _renderOrder;
        set
        {
            IUIElement.SetRenderOrder(Owner, _renderOrder, value, this);

            _renderOrder = value;
        }
    }
    public override IUnit? Owner
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
    public override bool IsHide
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
    public override HorizontalAlign HorizontalAlignment
    {
        get => _horizontalAlignment;
        set
        {
            _horizontalAlignment = value;
            RectangleShape.Origin = new Vector2f(GetHorizontalBounds(RectangleShape.GetLocalBounds()), RectangleShape.Origin.Y);

        }
    }
    public override VerticalAlign VerticalAlignment
    {
        get => _verticalAlignment;
        set
        {
            _verticalAlignment = value;
            RectangleShape.Origin = new Vector2f(RectangleShape.Origin.X, GetVerticalBounds(RectangleShape.GetLocalBounds()));
        }
    }
    public RectangleShape RectangleShape { get; set; }
    public Frame Animation{ get; set; } = new();

    public IBorder? Border { get; set; }
    public Vector2f Origin
    {
        get
        {
            return RectangleShape.Origin;
        }
    }
    public Vector2f Size
    {
        get
        {
            return RectangleShape.Size;
        }
    }
    public Vector2f Scale
    {
        get
        {
            return RectangleShape.Scale;
        }
    }
    public float AddSize
    {
        get
        {
            return RectangleShape.OutlineThickness;
        }
    }
    public float Rotation
    {
        get
        {
            return RectangleShape.Rotation;
        }
    }



    public UIShape(RectangleShape rectangleShape, IUnit? owner = null)
        : base(owner)
    {
        RectangleShape = new RectangleShape(rectangleShape);
        PositionOnScreen = rectangleShape.Position;

        Drawables.Add(RectangleShape);
    }
    public UIShape(UIShape uIShape, ImageLoadOptions options = null, IUnit? owner = null)
        : base(owner)
    {
        RectangleShape = new RectangleShape(uIShape.RectangleShape);
        Animation = options is not null && options.CreateNew ? new Frame(uIShape.Animation, options) : uIShape.Animation;

        RenderOrder = uIShape.RenderOrder;
        PreviousScreenHeight = uIShape.PreviousScreenHeight;
        PreviousScreenWidth = uIShape.PreviousScreenWidth;

        HorizontalAlignment = uIShape.HorizontalAlignment;
        VerticalAlignment = uIShape.VerticalAlignment;

        Drawables.Add(RectangleShape);
    }


    #region IUIElement
    public override void UpdateInfo()
    {
        Border?.Update(this);
        Animation.Update((float)(Owner?.Angle ?? 0));

        RectangleShape.Texture = Animation.CurrentElement?.Texture ?? TextureWrapper.Placeholder.Texture;
        if (RectangleShape.TextureRect != Animation.MaxFrameRect)
            RectangleShape.TextureRect = Animation.MaxFrameRect;
    }
    public override void ToggleVisibilityObject()
    {
        if (IsHide && Drawables.Count > 0)
            Drawables.Clear();
        else
        {
            if (!Drawables.Contains(RectangleShape))
                Drawables.Add(RectangleShape);
        }
    }
    public override void UpdateWidth()
    {
        if (Screen.ScreenWidth == PreviousScreenWidth)
            return;

        float widthScale = Screen.ScreenWidth / PreviousScreenWidth;

        RectangleShape.Scale = new Vector2f(RectangleShape.Scale.X * widthScale, RectangleShape.Scale.Y);
        PositionOnScreen = new Vector2f(PositionOnScreen.X * widthScale, PositionOnScreen.Y);

        PreviousScreenWidth = Screen.ScreenWidth;
    }
    public override void UpdateHeight()
    {
        if (Screen.ScreenHeight == PreviousScreenHeight)
            return;

        float heightScale = Screen.ScreenHeight / PreviousScreenHeight;

        RectangleShape.Scale = new Vector2f(RectangleShape.Scale.X, RectangleShape.Scale.Y * heightScale);
        PositionOnScreen = new Vector2f(PositionOnScreen.X, PositionOnScreen.Y * heightScale);

        PreviousScreenHeight = Screen.ScreenHeight;
    }
    #endregion
}
