using ProtoRender.Object;
using ScreenLib;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UIFramework.Render;
using UIFramework.Text;

namespace UIFramework.Windows;
public class UIButton : UIElement
{
    public override Vector2f PositionOnScreen
    {
        get => _positionOnScreen;
        set
        {
            _positionOnScreen = value;

            TextButton.PositionOnScreen = value;
            Shape.PositionOnScreen = value;
        }
    }
    public override RenderOrder RenderOrder
    {
        get => _renderOrder;
        set
        {
            IUIElement.SetRenderOrder(Owner, _renderOrder, value, this);
            TextButton.RenderOrder = value;
            Shape.RenderOrder = value;

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
            TextButton.Owner = value;
            Shape.Owner = value;

        }
    }
    public override bool IsHide
    {
        get => _isHide;
        set
        {
            TextButton.IsHide = value;
            Shape.IsHide = value;

            if (_isHide != value)
            {
                _isHide = value;
                ToggleVisibilityObject();
            }

        }
    }

    public UIShape Shape { get; private set; }
    public UIText TextButton { get; private set; }
    public Color DefaultColorShape { get; set; } = new Color(255, 255, 255, 0);
    public Color DefaultColorText { get; set; } = Color.White;

    public Color HoverColor { get; set; } = Color.Cyan;
    public Action? OnClick { get; set; }


    public UIButton(Vector2f position, Vector2f size, string label, string font, IUnit? owner = null)
        :base(owner)
    {
        Shape = new UIShape(new RectangleShape(size)
        {
            Position = position,
            FillColor = DefaultColorShape
        });

        TextButton = new UIText(label, 32, position, font, Color.White);
        TextButton.RenderOrder = RenderOrder;

        PositionOnScreen = position;
    }
    public UIButton(Vector2f position, UIShape uIShape, UIText uIText, IUnit? owner = null)
       : base(owner)
    {
        Shape = new UIShape(uIShape);
        Shape.RectangleShape.FillColor = DefaultColorShape;

        TextButton = new UIText(uIText);
        TextButton.RenderOrder = RenderOrder;

        PositionOnScreen = position;
    }


    #region IUIElement
    public override void UpdateInfo()
    {
        var window = Screen.Window;

        var mousePos = Mouse.GetPosition(window);
        var mouseWorld = window.MapPixelToCoords(mousePos);
        var bounds = Shape.RectangleShape.GetGlobalBounds();

        if (bounds.Contains(mouseWorld.X, mouseWorld.Y))
        {
            TextButton.RenderText.Text.FillColor = HoverColor;

            if (Mouse.IsButtonPressed(Mouse.Button.Left))
                OnClick?.Invoke();
        }
        else
        {
            Shape.RectangleShape.FillColor = DefaultColorShape;
            TextButton.RenderText.Text.FillColor = DefaultColorText;
        }
    }
    public override void ToggleVisibilityObject()
    {
    }
    public override void UpdateWidth()
    {
        if (Screen.ScreenWidth == PreviousScreenWidth)
            return;

        float widthScale = Screen.ScreenWidth / PreviousScreenWidth;
        _positionOnScreen = new Vector2f(PositionOnScreen.X * widthScale, PositionOnScreen.Y);

        PreviousScreenWidth = Screen.ScreenWidth;
    }
    public override void UpdateHeight()
    {
        if (Screen.ScreenHeight == PreviousScreenHeight)
            return;

        float heightScale = Screen.ScreenHeight / PreviousScreenHeight;
        _positionOnScreen = new Vector2f(PositionOnScreen.X, PositionOnScreen.Y * heightScale);

        PreviousScreenHeight = Screen.ScreenHeight;
    }

    #endregion
}

