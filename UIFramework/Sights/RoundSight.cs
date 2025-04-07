using ScreenLib;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScreenLib.Output;
using UIFramework.Render;

namespace UIFramework.Sights;
public class RoundSight : Sight
{
    private Vector2f _positionOnScreen;
    public override Vector2f PositionOnScreen 
    {
        get => _positionOnScreen;
        set
        {
            _positionOnScreen = value;
            Circle.Position = value;
        }
    }

    private SFML.Graphics.Color _fillColor = Color.White;
    public override SFML.Graphics.Color FillColor 
    {
        get => _fillColor;
        set
        {
            _fillColor = value;
            SetCircleShape(_radius, value);
        }
    }

    private float _radius = 1f;
    public float Radius
    {
        get => _radius;
        set
        {
            _radius = value == 0? 1: value < 0? -value: value / Screen.ScreenRatio;
            SetCircleShape(_radius, FillColor);
        }
    }
    public CircleShape Circle { get; private set; }

    public RoundSight(Vector2f positionOnScreen, SFML.Graphics.Color color, float radius)
    {
        if (_radius <= 0)
            throw new ArgumentException("Radius of RoundSight must be greater than zero.", nameof(_radius));

        _fillColor = color;
        _radius = radius;

        Circle = new CircleShape(radius)
        {
            FillColor = this.FillColor,
        };
        PositionOnScreen = positionOnScreen;

        Screen.WidthChangesFun += UpdateScreenInfo;
        Screen.HeightChangesFun += UpdateScreenInfo;

        Drawables.Add(Circle);
        UIRender.AddToPriority(RenderOrder.Indicators, this);
    }
    public RoundSight(SFML.Graphics.Color color, float radius)
        :this(new Vector2f(Screen.Setting.HalfWidth, Screen.Setting.HalfHeight), color, radius)
    {}
    public RoundSight(float radius)
       : this(new Vector2f(Screen.Setting.HalfWidth, Screen.Setting.HalfHeight), Color.White, radius)
    { }


    public override void Render()
    {
        UpdateInfo();
        foreach (var draw in Drawables)
            Screen.OutputPriority?.AddToPriority(OutputPriorityType.Interface, draw);
    }
    public override void UpdateInfo() { }
    public override void UpdateScreenInfo()
    {
        UpdateWidth();
        UpdateHeight();
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
    public override void Hide()
    {
        if (Drawables.Count > 0)
            Drawables.Clear();
        else
            Drawables.Add(Circle);
    }


    public void SetCircleShape(float radius, Color color)
    {
        Circle = new CircleShape(radius)
        {
            FillColor = color,
        };

        Circle.Position = PositionOnScreen;
    }
}
