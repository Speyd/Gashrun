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

public abstract class UIElement
{
    static List<UIElement> RendererUIElement { get; } = new List<UIElement>();

    public BottomBinding BottomBinding { get; set; }

    private Vector2f _screenCoordination;
    public Vector2f ScreenCoordination 
    {
        get => _screenCoordination;
        set
        {
            _screenCoordination = value;
            RenderSprite.Position = value;
        }
    }

    public Sprite RenderSprite { get; set; } = new Sprite();
    protected AnimationState AnimationState { get; init; } = new AnimationState();

    public float _scaleX = 1.0f;
    public float ScaleX 
    {
        get => _scaleX;
        set
        {
            _scaleX = value / Screen.MultWidth;
            RenderSprite.Scale = new Vector2f(_scaleX, _scaleY);
            SetPositionCenter();
        } 
    }
    public float _scaleY = 1.0f;
    public float ScaleY
    {
        get => _scaleY;
        set
        {
            _scaleY = value / Screen.MultHeight;
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
    public float PercentShiftX 
    {
        get => _percentShiftX;
        set
        {
            _percentShiftX =  -value / Screen.MultWidth;
            SetPositionCenter();
        }
    }

    private float _percentShiftY = 0;
    public float PercentShiftY
    {
        get => _percentShiftY;
        set
        {
            _percentShiftY = value / Screen.MultHeight;
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

    public UIElement(BottomBinding bottomBinding)
    {
        BottomBinding = bottomBinding;

        RendererUIElement.Add(this);
    }

    public abstract TextureObstacle? Render();
    public static void RenderUIs()
    {
        foreach (var ui in RendererUIElement)
        {
            var uiFrame = ui.Render();
            if (uiFrame is not null)
            {
                ui.RenderSprite.Texture = uiFrame.Texture;
                Screen.OutputPriority?.AddToPriority(OutputPriorityType.Interface, ui.RenderSprite);
            }
        }
    }

    public void SetPositionCenter()
    {
        Vector2u? textureSize = AnimationState.GetFrame(0)?.Texture.Size;
        if (textureSize is null)
            return;

        Vector2f texture = new Vector2f(textureSize.Value.X, textureSize.Value.Y);

        Vector2f screenSize = new Vector2f(Screen.Setting.HalfWidth, Screen.Setting.HalfHeight);
        ScreenCoordination = SetShiftCoordination(screenSize, texture);
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
