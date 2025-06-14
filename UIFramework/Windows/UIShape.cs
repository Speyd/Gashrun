using ProtoRender.Object;
using ProtoRender.WindowInterface;
using ScreenLib;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIFramework.Render;
using UIFramework.Sprite;
using UIFramework.Text.AlignEnums;


namespace UIFramework.Windows;
public class UIShape : UIElement
{
    public override Vector2f PositionOnScreen
    {
        get => _positionOnScreen;
        set
        {
            _positionOnScreen = value;
            RectangleShape.Position = value;
        }
    }
    public override RenderOrder RenderOrder { get; set; } = RenderOrder.Hands;
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

    public UIShape(RectangleShape rectangleShape, IUnit? owner = null)
        :base(owner)
    {
        RectangleShape = new RectangleShape(rectangleShape);
        PositionOnScreen = rectangleShape.Position;

        Drawables.Add(RectangleShape);
    }
    public UIShape(UIShape uIShape, IUnit? owner = null)
        : base(owner)
    {
        RectangleShape = new RectangleShape(uIShape.RectangleShape);

        RenderOrder = uIShape.RenderOrder;
        PreviousScreenHeight = uIShape.PreviousScreenHeight;
        PreviousScreenWidth = uIShape.PreviousScreenWidth;

        HorizontalAlignment = uIShape.HorizontalAlignment;
        VerticalAlignment = uIShape.VerticalAlignment;

        Drawables.Add(RectangleShape);
    }

    #region IUIElement
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

        HorizontalAlignment = HorizontalAlignment;
        VerticalAlignment = VerticalAlignment;

        PreviousScreenWidth = Screen.ScreenWidth;
    }
    public override void UpdateHeight()
    {
        if (Screen.ScreenHeight == PreviousScreenHeight)
            return;

        float heightScale = Screen.ScreenHeight / PreviousScreenHeight;

        RectangleShape.Scale = new Vector2f(RectangleShape.Scale.X, RectangleShape.Scale.Y * heightScale);
        PositionOnScreen = new Vector2f(PositionOnScreen.X, PositionOnScreen.Y * heightScale);

        HorizontalAlignment = HorizontalAlignment;
        VerticalAlignment = VerticalAlignment;

        PreviousScreenHeight = Screen.ScreenHeight;     
    }
    #endregion
}
