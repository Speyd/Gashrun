using ProtoRender.Object;
using ScreenLib;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using UIFramework.Animation;
using UIFramework.Render;
using UIFramework.Text;
using UIFramework.Windows;

namespace UIFramework.Windows.Button;
public class UIButton : UIElement
{
    public override Vector2f PositionOnScreen
    {
        get => _positionOnScreen;
        set
        {
            _positionOnScreen = value;

            Shape.PositionOnScreen = value;
            CenterTextInShape();
        }
    }
    public override RenderOrder RenderOrder
    {
        get => _renderOrder;
        set
        {
            IUIElement.SetRenderOrder(Owner, _renderOrder, value, this);
            Text.RenderOrder = value;
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
            Text.Owner = value;
            Shape.Owner = value;
        }
    }
    public override bool IsHide
    {
        get => _isHide;
        set
        {
            Text.IsHide = value;
            Shape.IsHide = value;

            if (_isHide != value)
            {
                _isHide = value;
                ToggleVisibilityObject();
            }

        }
    }

    public UIShape Shape { get; private set; }
    public UIText Text { get; private set; }
    public Color DefaultColorShape { get; set; } = new Color(255, 255, 255, 0);
    public Color DefaultColorText { get; set; } = Color.White;

    public Color HoverColor { get; set; } = Color.Cyan;

    public Action? OnClick { get; set; }
    private bool _wasLeftPressedLastFrame = false;
    public Action? OnHold { get; set; }

    public ButtonClickMode ClickMode { get; set; } = ButtonClickMode.ClickOnly;




    public UIButton(Vector2f position, Vector2f size, string label, string font, IUnit? owner = null)
        : base(owner)
    {
        Shape = new UIShape(new RectangleShape(size)
        {
            Position = position,
            FillColor = DefaultColorShape
        });

        Text = new UIText(label, 32, position, font, Color.White);
        Text.RenderOrder = RenderOrder;

        PositionOnScreen = position;
    }
    public UIButton(Vector2f position, UIShape uIShape, UIText uIText, IUnit? owner = null)
       : base(owner)
    {
        Shape = new UIShape(uIShape);
        Shape.RectangleShape.FillColor = DefaultColorShape;

        Text = new UIText(uIText);
        Text.RenderOrder = RenderOrder;

        PositionOnScreen = position;
    }

    private void HandleClickMode(bool isLeftPressed)
    {
        switch (ClickMode)
        {
            case ButtonClickMode.ClickOnly:
                if (isLeftPressed && !_wasLeftPressedLastFrame)
                    OnClick?.Invoke();
                break;

            case ButtonClickMode.HoldOnly:
                if (isLeftPressed)
                    OnHold?.Invoke();
                break;

            case ButtonClickMode.ClickAndHold:
                if (isLeftPressed && !_wasLeftPressedLastFrame)
                    OnClick?.Invoke();
                if (isLeftPressed)
                    OnHold?.Invoke();
                break;
        }
    }
    public void CenterTextInShape()
    {
        var shapeBounds = Shape.RectangleShape.GetGlobalBounds();

        Text.PositionOnScreen = new Vector2f(
            shapeBounds.Left + shapeBounds.Width / 2f,
            shapeBounds.Top + shapeBounds.Height / 2f);
    }

    #region IUIElement
    public override void UpdateInfo()
    {
        var window = Screen.Window;

        var mousePos = Mouse.GetPosition(window);
        var mouseWorld = window.MapPixelToCoords(mousePos);
        var bounds = Shape.RectangleShape.GetGlobalBounds();

        bool isHovered = bounds.Contains(mouseWorld.X, mouseWorld.Y);
        bool isLeftPressed = Mouse.IsButtonPressed(Mouse.Button.Left);

        if (isHovered)
        {
            Text.RenderText.Text.FillColor = HoverColor;
            HandleClickMode(isLeftPressed);
        }
        else
        {
            Shape.RectangleShape.FillColor = DefaultColorShape;
            Text.RenderText.Text.FillColor = DefaultColorText;
        }

        _wasLeftPressedLastFrame = isLeftPressed;
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

