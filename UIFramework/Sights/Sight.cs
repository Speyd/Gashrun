using ProtoRender.Object;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UIFramework.Sights;
public abstract class Sight : IUIElement
{
    public virtual Vector2f PositionOnScreen { get; set; }
    public List<Drawable> Drawables { get; init; } = new List<Drawable>();

    public virtual SFML.Graphics.Color FillColor{ get; set; }

    public Sight(Drawable drawable)
    {
        Drawables.Add(drawable);
        IUIElement.RendererUIElement.Add(this);
    }
    public Sight()
    {
        IUIElement.RendererUIElement.Add(this);
    }

    public abstract void UpdateInfo();
    public abstract void UpdateScreenInfo();
    public abstract void Hide();
}
