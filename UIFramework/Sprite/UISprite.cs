using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using ScreenLib;
using ScreenLib.Output;
using UIFramework.Render;
using SFML.Graphics;
using ProtoRender.Object;


namespace UIFramework.Sprite;
public class UISprite: IUIElement
{
    public float PreviousScreenWidth { get; protected set; } = Screen.ScreenWidth;
    public float PreviousScreenHeight { get; protected set; } = Screen.ScreenHeight;
    private Vector2f _positionOnScreen;
    public Vector2f PositionOnScreen
    {
        get => _positionOnScreen;
        set
        {
            _positionOnScreen = value;
            Sprite.Position = value;
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
    public bool _isHide = false;
    public bool IsHide
    {
        get => _isHide;
        set
        {
            if (_isHide != value)
            {
                _isHide = value;
                Hide();
            }

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

    public SFML.Graphics.Sprite Sprite { get; set; }


    public UISprite(SFML.Graphics.Sprite sprite, IUnit? owner = null)   
    {
        Owner = owner;
        Sprite = new SFML.Graphics.Sprite(sprite);
        FloatRect bounds = Sprite.GetLocalBounds();
        Sprite.Origin = new Vector2f(bounds.Width / 2f, bounds.Height / 2f);

        PositionOnScreen = sprite.Position;

        Screen.WidthChangesFun += UpdateScreenInfo;
        Screen.HeightChangesFun += UpdateScreenInfo;

        Drawables.Add(Sprite);
    }
    public UISprite(SFML.Graphics.Texture texture, IUnit? owner = null)
    {
        Owner = owner;
        Sprite = new SFML.Graphics.Sprite(texture);
        FloatRect bounds = Sprite.GetLocalBounds();
        Sprite.Origin = new Vector2f(bounds.Width / 2f, bounds.Height / 2f);

        PositionOnScreen = Sprite.Position;

        Screen.WidthChangesFun += UpdateScreenInfo;
        Screen.HeightChangesFun += UpdateScreenInfo;

        Drawables.Add(Sprite);
    }



    public void UpdateWidth()
    {
        if (Screen.ScreenWidth == PreviousScreenWidth)
            return;

        float widthScale = Screen.ScreenWidth / PreviousScreenWidth;

        Sprite.Scale = new Vector2f(Sprite.Scale.X * widthScale, Sprite.Scale.Y);
        //FloatRect bounds = Sprite.GetLocalBounds();
        //Sprite.Origin = new Vector2f(bounds.Width / 2f, bounds.Height / 2f);

        PositionOnScreen = new Vector2f(PositionOnScreen.X * widthScale, PositionOnScreen.Y);
        PreviousScreenWidth = Screen.ScreenWidth;
    }
    public void UpdateHeight()
    {
        if (Screen.ScreenHeight == PreviousScreenHeight)
            return;

        float heightScale = Screen.ScreenHeight / PreviousScreenHeight;

        Sprite.Scale = new Vector2f(Sprite.Scale.X, Sprite.Scale.Y * heightScale);
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
            if (!Drawables.Contains(Sprite))
                Drawables.Add(Sprite);
        }
    }
    #endregion
}
