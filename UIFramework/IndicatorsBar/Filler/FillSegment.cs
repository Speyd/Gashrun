using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using ScreenLib;
using UIFramework.IndicatorsBar.Content;

namespace UIFramework.IndicatorsBar.Filler;
public class FillSegment
{
    public RectangleShape Fill { get; init; } = new();
    public ValueProgress ValueProgress { get; set; } = new();
    public IBarContent Content { get; set; }

    public FillSegment(IBarContent content)
    {
        Content = content;
        Content.UpdateContent(Fill);
        Fill.OutlineColor = Color.Transparent;
    }
    public void SetPositionBar(Bar mainBar, float addWidth = 0)
    {
        UpdateSize(mainBar);

        float mainThickness = mainBar.BorderThickness / 2;
        UpdateOriginSize(mainBar, mainThickness);
        UpdatePosition(mainBar, mainThickness, addWidth);
    }
    private void UpdateSize(Bar mainBar)
    {
        float width = mainBar.Width * ValueProgress.Percent / 100;
        float height = mainBar.Height;
        Fill.Size = new Vector2f(width, height);
    }
    private void UpdateOriginSize(Bar mainBar, float mainThickness)
    {
        Fill.Origin = new Vector2f(-mainThickness, Fill.Size.Y + mainThickness);
    }
    private void UpdatePosition(Bar mainBar, float mainThickness, float addWidth)
    {
        float x = mainBar.PositionOnScreen.X + mainThickness + addWidth;
        float y = mainBar.PositionOnScreen.Y - mainThickness;
        Fill.Position = new Vector2f(x, y);
    }

    public void SetValue(FillBar mainFilBar, float value, float addWidth = 0)
    {
        ValueProgress.SetValue(value, mainFilBar.MaxValue, mainFilBar.MinValue);
        SetPositionBar(mainFilBar);
    }
}

