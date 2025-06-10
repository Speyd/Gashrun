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
public abstract class Sight : UIElement
{
    public virtual SFML.Graphics.Color FillColor{ get; set; }

    public Sight(Drawable drawable, IUnit? owner = null)
        :base(owner)
    {
        Drawables.Add(drawable);
    }
    public Sight(IUnit? owner = null)
        : base(owner)
    { }
}
