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

    private RenderOrder _renderOrder = RenderOrder.Indicators;
    public RenderOrder RenderOrder
    {
        get => _renderOrder;
        set
        {
            IUIElement.SetRenderOrder(Owner, _renderOrder, value, this);
            _renderOrder = value;
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
    public virtual SFML.Graphics.Color FillColor{ get; set; }

    public Sight(Drawable drawable, IUnit? owner = null)
    {
        Owner = owner;

        Drawables.Add(drawable);
    }
    public Sight(IUnit? owner = null)
    {
        Owner = owner;
    }

    public abstract void Render();
    public abstract void UpdateInfo();
    public abstract void UpdateScreenInfo();
    public abstract void Hide();
}
