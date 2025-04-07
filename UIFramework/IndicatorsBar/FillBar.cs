using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using AnimationLib;
using NGenerics.DataStructures.General;
using ScreenLib;
using UIFramework.Render;
using UIFramework.IndicatorsBar.Content;
using UIFramework.IndicatorsBar.Filler;


namespace UIFramework.IndicatorsBar;
public class FillBar : Bar
{
    public override Vector2f PositionOnScreen
    {
        get => _positionOnScreen;
        set
        {
            _positionOnScreen = value;
            Border.Position = value;

            UpdatePosition();
        }
    }

    private void UpdateBordersSize()
    {
        UpdateBorderSize();
        UpdatePosition();
    }
    public override float Width
    {
        get => _width;
        set
        {
            OriginWidth = value;
            _width = value / Screen.MultWidth;

            UpdateBordersSize();
        }
    }
    public override float Height
    {
        get => _height;
        set
        {
            OriginHeight = value;
            _height = value / Screen.MultHeight;
            UpdateBordersSize();
        }
    }

    public override float BorderThickness
    {
        get => _borderThickness;
        set
        {
            _originBorderThickness = value;
            _borderThickness = Screen.ScreenRatio >= 1 ?
                value / Screen.ScreenRatio :
                value * Screen.ScreenRatio;

            Border.OutlineThickness = _borderThickness;

            UpdateBorderSize();
            UpdatePosition();
        }
    }


    public float _maxValue = 100;
    public float MaxValue
    {
        get => _maxValue;
        set
        {
            _maxValue = value;
            UpdateMaxValues();
        }
    }
    public float _minValue = 0;
    public float MinValue
    {
        get => _minValue;
        set
        {
            _minValue = value;
            UpdateMaxValues();
        }
    }


    public FillSegment Forward { get; init; }
    public FillSegment Backward { get; init; }

    public FillBar(IBarContent forwardFillContent, IBarContent backwardFillContent,
    float forwardValue, float backwardValue)
    {
        FillColor = Color.Transparent;

        Forward = new(forwardFillContent);
        Forward.ValueProgress.SetValue(forwardValue, MaxValue, MinValue);

        Backward = new(backwardFillContent);
        Backward.ValueProgress.SetValue(backwardValue, MaxValue, MinValue);

        Drawables.Add(Forward.Fill);
        Drawables.Add(Backward.Fill);
        UpdatePosition();
    }
    public FillBar(IBarContent forwardFillContent, IBarContent backwardFillContent)
        : this(forwardFillContent, backwardFillContent, 100, 0)
    { }

    private void UpdateMaxValues()
    {
        if (_minValue > _maxValue)
        {
            var value = _minValue;
            _minValue = _maxValue;
            _maxValue = value;
        }

        UpdateValues();
    }
    private void UpdateValues()
    {
        Forward.SetValue(this, Forward.ValueProgress.Value);
        Backward.SetValue(this, Backward.ValueProgress.Value, Forward.Fill.Size.X);
    }
    private void UpdatePosition()
    {
        Forward.SetPositionBar(this);
        Backward.SetPositionBar(this, Forward.Fill.Size.X);
    }
    public override void UpdateInfo()
    {
        Forward.Content.UpdateContent(Forward.Fill);
        Backward.Content.UpdateContent(Backward.Fill);
    }
    public override void Hide()
    {
        if (Drawables.Count > 0)
            Drawables.Clear();
        else
        {
            Drawables.Add(Border);
            Drawables.Add(Forward.Fill);
            Drawables.Add(Backward.Fill);

        }
    }
}
