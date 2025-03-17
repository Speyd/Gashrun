using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;


namespace UIFramework.IndicatorsBar.Content;
public class ColorContent : IBarContent
{
    public SFML.Graphics.Color FillColor { get; set; }
    public ColorContent(Color color)
    {
        FillColor = color;
    }

    public void UpdateContent(RectangleShape bar)
    {
        if (bar.FillColor != FillColor)
            bar.FillColor = FillColor;
    }
}
