using SFML.Graphics;
using SFML.System;
using System.Numerics;
using AnimationLib;
using ProtoRender.RenderAlgorithm;
using ScreenLib;
using ScreenLib.Output;
using TextureLib;
using ControlLib;
using UIFramework.Render;
using ObjectFramework;
using ProtoRender.Object;

namespace UIFramework.Animation;
public class UIAnimation : AnimationHandler, IUIElement
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
            RenderSprite.Position = value;
        }
    }


    public SFML.Graphics.Sprite RenderSprite { get; set; } = new SFML.Graphics.Sprite();
    public List<Drawable> Drawables { get; init; } = new List<Drawable>();

    private RenderOrder _renderOrder = RenderOrder.Hands;
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


    public ControlLib.BottomBinding? BottomBinding { get; set; } = null;

    public float _scaleX = 1.0f;
    private float _originScaleX = 0;
    public float ScaleX
    {
        get => _scaleX;
        set
        {
            ResetPosition();
            _scaleX = value / Screen.MultWidth;
            _originScaleX = value;

            RenderSprite.Scale = new Vector2f(_scaleX, _scaleY);
            SetPosition();
        }
    }


    public float _scaleY = 1.0f;
    private float _originScaleY = 0;
    public float ScaleY
    {
        get => _scaleY;
        set
        {
            ResetPosition();
            _scaleY = value / Screen.MultHeight;
            _originScaleY = value;

            RenderSprite.Scale = new Vector2f(_scaleX, _scaleY);
            SetPosition();
        }
    }


    private float _percentShiftX = 0;
    private float _originPercentShiftX = 0;
    public float PercentShiftX
    {
        get => _percentShiftX;
        set
        {
            ResetPosition();
            _percentShiftX = -value / Screen.MultWidth;
            _originPercentShiftX = -value;
            SetPosition();
        }
    }


    private float _percentShiftY = 0;
    private float _originPercentShiftY = 0;
    public float PercentShiftY
    {
        get => _percentShiftY;
        set
        {
            ResetPosition();
            _percentShiftY = value / Screen.MultHeight;
            _originPercentShiftY = value;
            SetPosition();
        }
    }
    public bool IsAnimatingOnPress { get; protected set; } = false;


    #region Constructor 
    public UIAnimation(Vector2f position, IUnit? owner = null, ControlLib.BottomBinding? bottomBinding = null, params string[] paths)
    {
        Owner = owner;

        BottomBinding = bottomBinding;
        PositionOnScreen = position;

        AnimationState.AddFrames(ImageLoader.TexturesLoad(paths));

        Drawables.Add(RenderSprite);
        Screen.WidthChangesFun += UpdateScreenInfo;
        Screen.HeightChangesFun += UpdateScreenInfo;
    }
    public UIAnimation(IUnit? owner = null, ControlLib.BottomBinding? bottomBinding = null, params string[] paths)
        : this(new Vector2f(), owner, bottomBinding, paths)
    {
        SetPositionCenter();
    }
    #endregion



    protected virtual void AnimationPress()
    {
        if (AnimationState.Index == AnimationState.AmountFrame - 1)
        {
            UpdateFrame();
            IsAnimatingOnPress = false;
        }
        if (AnimationState.IsAnimation && BottomBinding?.IsPress == true || IsAnimatingOnPress == true)
        {
            IsAnimatingOnPress = true;
            UpdateFrame();
        }
        else if (AnimationState.IsAnimation && BottomBinding?.IsPress == false)
        {
            AnimationState.IsAnimation = false;
            UpdateFrame();
            AnimationState.IsAnimation = true;
        }
    }



    public virtual void Render()
    {
        UpdateInfo();
        foreach (var draw in Drawables)
            Screen.OutputPriority?.AddToPriority(IUIElement.OutputPriorityType, draw);
    }
    public virtual void UpdateInfo()
    {
        if (BottomBinding is null)
            UpdateFrame();
        else
            AnimationPress();

        if (AnimationState.CurrentFrame is not null)
            RenderSprite.Texture = AnimationState.CurrentFrame.Texture;
    }
    public virtual void UpdateScreenInfo()
    {
        ResetPosition();
        UpdateWidth();
        UpdateHeight();
        SetPosition();

        PercentShiftX = _originPercentShiftX;
        PercentShiftY = _originPercentShiftY;
        
      
        ScaleX = _originScaleX;
        ScaleY = _originScaleY;
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
            Drawables.Add(RenderSprite);
    }



    public void SetPositionCenter()
    {
        Vector2f sizeTexture = GetFirstSizeFrame();
        Vector2f screenSize = new Vector2f(Screen.Setting.HalfWidth, Screen.Setting.HalfHeight);
        PositionOnScreen = SetShiftCoordination(screenSize, sizeTexture);
    }
    private void ResetPosition()
    {
        Vector2f sizeTexture = GetFirstSizeFrame();
        PositionOnScreen = SetShiftCoordination(PositionOnScreen, -sizeTexture);
    }
    private void SetPosition()
    {
        Vector2f sizeTexture = GetFirstSizeFrame();
        PositionOnScreen = SetShiftCoordination(PositionOnScreen, sizeTexture);
    }



    private Vector2f GetFirstSizeFrame()
    {
        Vector2u? textureSize = AnimationState.GetFrame(0)?.Texture.Size;
        if (textureSize is null)
            throw new NullReferenceException("Frame UIAnimation is null(GetFirstSizeFrame)");

        return new Vector2f(textureSize.Value.X, textureSize.Value.Y);
    }
    Vector2f SetShiftCoordination(Vector2f value, Vector2f textureSize)
    {
        RenderSprite.Origin = textureSize / 2;


        Vector2f newCoordination;
        float percentSizeX = textureSize.X * PercentShiftX / 100;
        float percentSizeY = textureSize.Y * PercentShiftY / 100;

        newCoordination.X = value.X - percentSizeX;
        newCoordination.Y = value.Y - percentSizeY;

        return newCoordination;
    }
    public void SetScale(float scale)
    {
        ScaleX = scale;
        ScaleY = scale;
    }
}
