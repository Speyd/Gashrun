using SFML.Graphics;
using SFML.System;
using AnimationLib;
using ScreenLib;
using ControlLib.Buttons;
using UIFramework.Render;
using ProtoRender.Object;
using TextureLib.Textures;
using TextureLib.Loader.ImageProcessing;


namespace UIFramework.Animation;
public class UIAnimation : AnimationState, IUIElement
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
    public bool _isHide = false;
    public bool IsHide
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

    public ButtonBinding? _bottomBinding = null;
    public ButtonBinding? BottomBinding 
    {
        get => _bottomBinding;
        set
        {
            if (Owner is not null)
            {
                if (_bottomBinding is not null)
                    Owner.Control.DeleteBottomBind(_bottomBinding);
                if (value is not null)
                    Owner.Control.AddBottomBind(value);
            }
            _bottomBinding = value;
        }
    }

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
            _originPercentShiftX = value;
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

    private Vector2f PreviousSize { get; set; }

    #region Constructor 
    public UIAnimation(Vector2f position, ImageLoadOptions? options = null, bool loadAsync = true, ButtonBinding? bottomBinding = null, params string[] paths)
        : base(options, loadAsync, paths)
    {
        BottomBinding = bottomBinding;
        PositionOnScreen = position;

        if (loadAsync)
            CheckAddTexture();

        Drawables.Add(RenderSprite);
        Screen.WidthChangesFun += UpdateScreenInfo;
        Screen.HeightChangesFun += UpdateScreenInfo;

    }
    public UIAnimation(ImageLoadOptions? options = null, bool loadAsync = true, ButtonBinding ? bottomBinding = null, params string[] paths)
        : this(new Vector2f(), options, loadAsync, bottomBinding, paths)
    {
        SetPositionCenter();
    }
    public UIAnimation(ImageLoadOptions? options = null, bool loadAsync = true, params string[] paths)
        : this(new Vector2f(), options, loadAsync, null, paths)
    {
        SetPositionCenter();
    }
    #endregion

    private void CheckAddTexture()
    {
        _ = Task.Run(async () => 
        {

            while (true)
            {
                if (IsLoaded)
                {
                    RenderSprite.TextureRect = MaxFrameRect;
                    ResetSetting();
                    return;
                }
                await Task.Delay(500);
            }
        });
    }

    public virtual void UpdateFrame()
    {
        AnimationManager.DefiningDesiredSprite(this, Owner?.Angle ?? 0);
    }
    protected virtual void AnimationPress()
    {
        if (Index == CountFrame - 1)
        {
            SetCurrentFrame(0);
            IsAnimatingOnPress = false;
        }
        if (AnimationMode == AnimationMode.Animated && BottomBinding?.IsPress == true || IsAnimatingOnPress == true)
        {
            IsAnimatingOnPress = true;
            UpdateFrame();
        }
        else if (AnimationMode == AnimationMode.Animated && BottomBinding?.IsPress == false)
        {
            AnimationMode = AnimationMode.Static;
            SetCurrentFrame(0);
            AnimationMode = AnimationMode.Animated;
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

        if (CurrentFrame is not null && GetFrame(0) is not null)
            RenderSprite.Texture = CurrentFrame.Texture;
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
    public virtual void UpdateWidth()
    {
        float widthScale = Screen.ScreenWidth / PreviousScreenWidth;
        PositionOnScreen = new Vector2f(PositionOnScreen.X * widthScale, PositionOnScreen.Y);

        PreviousScreenWidth = Screen.ScreenWidth;
    }
    public virtual void UpdateHeight()
    {
        float heightScale = Screen.ScreenHeight / PreviousScreenHeight;
        PositionOnScreen = new Vector2f(PositionOnScreen.X, PositionOnScreen.Y * heightScale);

        PreviousScreenHeight = Screen.ScreenHeight;
    }
    public virtual void ToggleVisibility()
    {
        IsHide = !IsHide;
    }
    public virtual void ToggleVisibilityObject()
    {
        if (_isHide && Drawables.Count > 0)
            Drawables.Clear();
        else if (!_isHide)
        {
            if (!Drawables.Contains(RenderSprite))
                Drawables.Add(RenderSprite);
        }
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
        Vector2u? textureSize = GetFrame(0)?.Texture?.Size;

        if (textureSize is null)
            PreviousSize = new Vector2f(TextureWrapper.Placeholder.Texture!.Size.X, TextureWrapper.Placeholder.Texture!.Size.Y);
        else
            PreviousSize = new Vector2f(textureSize.Value.X, textureSize.Value.Y);

        return PreviousSize;
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


    public Vector2f GetOriginalPositionOnScreen(Vector2f value, Vector2f textureSize)
    {
        RenderSprite.Origin = GetFirstSizeFrame() / 2;

        Vector2f newCoordination;
        float percentSizeX = textureSize.X * PercentShiftX / 100;
        float percentSizeY = textureSize.Y * PercentShiftY / 100;

        newCoordination.X = value.X + percentSizeX;
        newCoordination.Y = value.Y + percentSizeY;

        return newCoordination;
    }
    private void ResetSetting()
    {
        PositionOnScreen = GetOriginalPositionOnScreen(PositionOnScreen, PreviousSize);
        PositionOnScreen = SetShiftCoordination(PositionOnScreen, GetFirstSizeFrame());
    }
}
