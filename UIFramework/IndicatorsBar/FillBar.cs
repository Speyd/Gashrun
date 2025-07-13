using SFML.Graphics;
using SFML.System;
using ScreenLib;
using UIFramework.IndicatorsBar.Content;
using UIFramework.IndicatorsBar.Filler;
using ProtoRender.Object;
using UIFramework.Text.AlignEnums;
using InteractionFramework;


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


    public Stat Stat { get; set; }

    public FillSegment Forward { get; init; }
    public FillSegment Backward { get; init; }


    private float GetOriginAxisOffset(float main, float side)
    {
        float originOffset = -BorderThickness / 2;

        if (main > side)
            originOffset = BorderThickness / 2;
        else if (main < side)
            originOffset = -BorderThickness / 2;

        return originOffset;
    }
    public override HorizontalAlign HorizontalAlignment
    {
        get => _horizontalAlignment;
        set
        {
            _horizontalAlignment = value;
            Border.Origin = new Vector2f(GetHorizontalBounds(Border.GetLocalBounds()), Border.Origin.Y);

            float originXOffset = GetOriginAxisOffset(Border.Origin.X, Forward.Fill.Origin.X);
            Forward.Fill.Origin = new Vector2f(GetHorizontalBounds(Forward.Fill.GetLocalBounds()) + originXOffset, Forward.Fill.Origin.Y);
            Backward.Fill.Origin = new Vector2f(GetHorizontalBounds(Backward.Fill.GetLocalBounds()) + originXOffset, Backward.Fill.Origin.Y);
        }
    }
    public override VerticalAlign VerticalAlignment
    {
        get => _verticalAlignment;
        set
        {
            _verticalAlignment = value;
            Border.Origin = new Vector2f(Border.Origin.X, GetVerticalBounds(Border.GetLocalBounds()));

            float originYOffset = GetOriginAxisOffset(Border.Origin.Y, Forward.Fill.Origin.Y);
            Forward.Fill.Origin = new Vector2f(Forward.Fill.Origin.X, GetVerticalBounds(Forward.Fill.GetLocalBounds()) + originYOffset);
            Backward.Fill.Origin = new Vector2f(Backward.Fill.Origin.X, GetVerticalBounds(Backward.Fill.GetLocalBounds()) + originYOffset);
        }
    }


    public FillBar(RectangleShape border, IBarContent forwardFillContent, IBarContent backwardFillContent, Stat stat, IUnit? owner = null)
        : base(border, owner)
    {
        FillColor = Color.Transparent;
        Stat = stat;
        Stat.OnChanged += UpdateValues;

        Forward = new(forwardFillContent);
        Forward.ValueProgress.SetValue(Stat.Value, Stat.Max);

        Backward = new(backwardFillContent);
        Backward.ValueProgress.SetValue(Stat.Max - Stat.Value, Stat.Max);

        Drawables.Add(Forward.Fill);
        Drawables.Add(Backward.Fill);
        UpdatePosition();
    }
    public FillBar(IBarContent forwardFillContent, IBarContent backwardFillContent, Stat stat, IUnit? owner = null)
        :this(new RectangleShape(), forwardFillContent, backwardFillContent, stat, owner)
    {}


    private void UpdateValues()
    {
        Forward.SetValue(this, Stat.Value);
        Backward.SetValue(this, Stat.Max - Stat.Value, Forward.Fill.Size.X);
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
    public override void ToggleVisibilityObject()
    {
        if (IsHide && Drawables.Count > 0)
            Drawables.Clear();
        else
        {
            if (!Drawables.Contains(Border))
                Drawables.Add(Border);
            if (!Drawables.Contains(Forward.Fill))
                Drawables.Add(Forward.Fill);
            if (!Drawables.Contains(Backward.Fill))
                Drawables.Add(Backward.Fill);

        }
    }
}
