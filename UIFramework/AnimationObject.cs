using SFML.Graphics;
using SFML.System;
using System.Numerics;
using AnimationLib;
using ProtoRender.RenderAlgorithm;
using ScreenLib;
using ScreenLib.Output;
using TextureLib;
using ControlLib;
using static System.Net.Mime.MediaTypeNames;
namespace UIFramework;

public abstract class AnimationObject : IUIElement
{
    private Vector2f _positionOnScreen;
    public Vector2f PositionOnScreen 
    {
        get => _positionOnScreen;
        set
        {
            _positionOnScreen = value;
            RenderSprite.Position = value;
        }
    }
    public Sprite RenderSprite { get; set; } = new Sprite();
    public List<Drawable> Drawables { get; init; } = new List<Drawable>();


    public BottomBinding BottomBinding { get; set; }
    protected AnimationState AnimationState { get; init; } = new AnimationState();


    public float _scaleX = 1.0f;
    private float _originScaleX = 0;
    public float ScaleX 
    {
        get => _scaleX;
        set
        {
            _scaleX = value / Screen.MultWidth;
            _originScaleX = value;

            RenderSprite.Scale = new Vector2f(_scaleX, _scaleY);
            SetPositionCenter();
        } 
    }
    public float _scaleY = 1.0f;
    private float _originScaleY = 0;
    public float ScaleY
    {
        get => _scaleY;
        set
        {
            _scaleY = value / Screen.MultHeight;
            _originScaleY = value;

            RenderSprite.Scale = new Vector2f(_scaleX, _scaleY);
            SetPositionCenter();
        }
    }


    private int _speedAnimation = 100;
    public int SpeedAnimation 
    {
        get => _speedAnimation;
        set
        {
            _speedAnimation = value;
            AnimationState.Speed = value;
        }
    }
     

    private float _percentShiftX = 0;
    private float _originPercentShiftX = 0;

    public float PercentShiftX 
    {
        get => _percentShiftX;
        set
        {
            _percentShiftX =  -value / Screen.MultWidth;
            _originPercentShiftX = -value;
            SetPositionCenter();
        }
    }

    private float _percentShiftY = 0;
    private float _originPercentShiftY = 0;
    public float PercentShiftY
    {
        get => _percentShiftY;
        set
        {
            _percentShiftY = value / Screen.MultHeight;
            _originPercentShiftY = value;
            SetPositionCenter();
        }
    }


    public bool _isAnimation = false;
    public bool IsAnimation 
    {
        get => _isAnimation;
        set
        {
            _isAnimation = value;
            AnimationState.IsAnimation = value;
        }
    }
    public bool IsAnimatingOnPress { get; protected set; } = false;


    public AnimationObject(BottomBinding bottomBinding)
    {
        BottomBinding = bottomBinding;
        Drawables.Add(RenderSprite);

        Screen.WidthChangesFun += UpdateScreenInfo;
        Screen.HeightChangesFun += UpdateScreenInfo;

        IUIElement.RendererUIElement.Add(this);
    }


    public abstract void UpdateInfo();
    public virtual void UpdateScreenInfo()
    {
        PercentShiftX = _originPercentShiftX;
        PercentShiftY = _originPercentShiftY;

        ScaleX = _originScaleX;
        ScaleY = _originScaleY;
    }
    public virtual void Hide()
    {
        if (Drawables.Count > 0)
            Drawables.Clear();
        else
            Drawables.Add(RenderSprite);
    }

    public void SetPositionCenter()
    {
        Vector2u? textureSize = AnimationState.GetFrame(0)?.Texture.Size;
        if (textureSize is null)
            return;

        Vector2f texture = new Vector2f(textureSize.Value.X, textureSize.Value.Y);

        Vector2f screenSize = new Vector2f(Screen.Setting.HalfWidth, Screen.Setting.HalfHeight);
        PositionOnScreen = SetShiftCoordination(screenSize, texture);
    }
    Vector2f SetShiftCoordination(Vector2f value, Vector2f textureSize)
    {
        RenderSprite.Origin = textureSize / 2;


        Vector2f newCoordination;
        float percentSizeX = ((textureSize.X * PercentShiftX) / 100);
        float percentSizeY = ((textureSize.Y * PercentShiftY) / 100);

        newCoordination.X = (value.X - percentSizeX);
        newCoordination.Y = (value.Y - percentSizeY);

        return newCoordination;
    }
    public void SetScale(float scale)
    {
        ScaleX = scale;
        ScaleY = scale;
    }
}
