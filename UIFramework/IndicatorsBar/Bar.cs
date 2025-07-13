using ScreenLib.Output;
using ScreenLib;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIFramework.Sights.Crosses;
using UIFramework.Render;
using ProtoRender.Object;
using NGenerics.DataStructures.General;
using UIFramework.Text.AlignEnums;


namespace UIFramework.IndicatorsBar;
public class Bar: UIElement
{
    public override Vector2f PositionOnScreen
    {
        get => _positionOnScreen;
        set
        {
            _positionOnScreen = value;
            Border.Position = value;

            HorizontalAlignment = HorizontalAlignment;
            VerticalAlignment = VerticalAlignment;
        }
    }

    protected virtual void UpdateBorderSize()
    {
        Border.Size = new Vector2f(_width, _height);
        Border.Origin = new Vector2f(-_borderThickness, _height + _borderThickness);
    }

    protected float _width = 0;
    public float OriginWidth { get; protected set; } = 0;
    public virtual float Width
    {
        get => _width;
        set
        {
            OriginWidth = value;
            _width = value / Screen.MultWidth;
            UpdateBorderSize();
        }
    }
    protected float _height = 0;
    public float OriginHeight { get; protected set; } = 0;
    public virtual float Height
    {
        get => _height;
        set
        {
            OriginHeight = value;
            _height = value / Screen.MultHeight;
            UpdateBorderSize();
        }
    }



    private SFML.Graphics.Color _borderFillColor = Color.Black;
    public SFML.Graphics.Color BorderFillColor 
    {
        get => _borderFillColor;
        set
        {
            _borderFillColor = value;
            Border.OutlineColor = value;
        }
    }

    private SFML.Graphics.Color _fillColor = Color.Transparent;
    public SFML.Graphics.Color FillColor
    {
        get => _fillColor;
        set
        {
            _fillColor = value;
            Border.FillColor = value;
        }
    }

    private SFML.Graphics.Texture? _fillTexture = null;
    public SFML.Graphics.Texture? FillTexture
    {
        get => _fillTexture;
        set
        {
            _fillTexture = value;
            Border.Texture = value;
           
            if (value is not null)
                SetTextureRect();
        }
    }

    public override HorizontalAlign HorizontalAlignment 
    {
        get => _horizontalAlignment;
        set
        {
            _horizontalAlignment = value;
            Border.Origin = new Vector2f(GetHorizontalBounds(Border.GetLocalBounds()), Border.Origin.Y);
        }
    }
    public override VerticalAlign VerticalAlignment 
    {
        get => _verticalAlignment;
        set
        {
            _verticalAlignment = value;
            Border.Origin = new Vector2f(Border.Origin.X, GetVerticalBounds(Border.GetLocalBounds()));
        }
    }


    private void SetTextureRect()
    {
        if (FillTexture is null)
            return;

        int rectWidth = (int)FillTexture.Size.X;
        int rectHeight = (int)FillTexture.Size.Y;

        TextureRect = new IntRect(0, 0, rectWidth, rectHeight);
    }
    private IntRect _textureRect = new IntRect();
    public IntRect TextureRect
    {
        get => _textureRect;
        set
        {
            _textureRect = value;
            Border.TextureRect = value;
        }
    }



    protected float _borderThickness = 5;
    public float _originBorderThickness { get; protected set; } = 0;
    public virtual float BorderThickness 
    {
        get => _borderThickness;
        set
        {
            _originBorderThickness = value;
            _borderThickness = Screen.ScreenRatio >= 1?
                value / Screen.ScreenRatio:
                value * Screen.ScreenRatio;

            Border.OutlineThickness = _borderThickness;
            UpdateBorderSize();
        }
    }
    public virtual RectangleShape Border { get; set; }


    public Bar(IUnit? owner = null)
        :base(owner)
    {
        Owner = owner;
        Border = new RectangleShape();

        Drawables.Add(Border);
    }
    public Bar(RectangleShape border, IUnit? owner = null)
        : base(owner)
    {
        Owner = owner;
        Border = new RectangleShape(border);

        Drawables.Add(Border);
    }

    public override void UpdateScreenInfo()
    {
        UpdateWidth();
        UpdateHeight();

        Width = OriginWidth;
        Height = OriginHeight;
        BorderThickness = _originBorderThickness;
    }
    public override void ToggleVisibilityObject()
    {
        if (IsHide && Drawables.Count > 0)
            Drawables.Clear();
        else
        {
            if (!Drawables.Contains(Border))
                Drawables.Add(Border);
        }
    }

}
