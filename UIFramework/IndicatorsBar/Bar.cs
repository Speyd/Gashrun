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


namespace UIFramework.IndicatorsBar;
public class Bar: IUIElement
{
    public float PreviousScreenWidth { get; protected set; } = Screen.ScreenWidth;
    public float PreviousScreenHeight { get; protected set; } = Screen.ScreenHeight;

    protected Vector2f _positionOnScreen;
    public virtual Vector2f PositionOnScreen
    {
        get => _positionOnScreen;
        set
        {
            _positionOnScreen = value;
            Border.Position = value;
        }
    }
    public virtual List<Drawable> Drawables { get; init; } = new List<Drawable>();


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
    {
        Owner = owner;
        Border = new RectangleShape();

        Screen.WidthChangesFun += UpdateScreenInfo;
        Screen.HeightChangesFun += UpdateScreenInfo;

        Drawables.Add(Border);
    }

    public virtual void Render()
    {
        UpdateInfo();
        foreach (var draw in Drawables)
            Screen.OutputPriority?.AddToPriority(IUIElement.OutputPriorityType, draw);
    }
    public virtual void UpdateInfo() 
    { }
    public virtual void UpdateScreenInfo()
    {
        UpdateWidth();
        UpdateHeight();

        Width = OriginWidth;
        Height = OriginHeight;
        BorderThickness = _originBorderThickness;
    }
    public void UpdateWidth()
    {
        float widthScale = Screen.ScreenWidth / PreviousScreenWidth;
        PositionOnScreen = new Vector2f(PositionOnScreen.X * widthScale, PositionOnScreen.Y);

        PreviousScreenWidth = Screen.ScreenWidth;
    }
    public void UpdateHeight()
    {
        float heightScale = Screen.ScreenHeight / PreviousScreenHeight;
        PositionOnScreen = new Vector2f(PositionOnScreen.X, PositionOnScreen.Y * heightScale);

        PreviousScreenHeight = Screen.ScreenHeight;
    }
    public virtual void Hide()
    {
        if (Drawables.Count > 0)
            Drawables.Clear();
        else
            Drawables.Add(Border);
    }

}
