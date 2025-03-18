using ProtoRender.Object;
using ScreenLib.Output;
using ScreenLib;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIFramework.Render;


namespace UIFramework.Sights;
public abstract class Sight : IUIElement
{
    public virtual float PreviousScreenWidth { get; protected set; } = Screen.ScreenWidth;
    public virtual float PreviousScreenHeight { get; protected set; } = Screen.ScreenHeight;
    public virtual Vector2f PositionOnScreen { get; set; }
    public List<Drawable> Drawables { get; init; } = new List<Drawable>();

    public virtual SFML.Graphics.Color FillColor{ get; set; }

    public Sight(Drawable drawable)
    {
        Drawables.Add(drawable);
        UIRender.AddToPriority(RenderOrder.Indicators, this);
    }
    public Sight()
    {
        UIRender.AddToPriority(RenderOrder.Indicators, this);
    }

    public abstract void Render();
    public abstract void UpdateInfo();
    public abstract void UpdateScreenInfo();
    public abstract void Hide();
}
