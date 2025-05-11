using ScreenLib.Output;
using ScreenLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using ProtoRender.Object;
using UIFramework.Render;


namespace UIFramework;
public interface IUIElement
{
    float PreviousScreenWidth { get; }
    float PreviousScreenHeight { get; }

    Vector2f PositionOnScreen { get; set; }
    List<Drawable> Drawables { get; init; }
    RenderOrder RenderOrder { get; set; }
    IUnit? Owner { get; set; }


    void Render();
    void UpdateInfo();
    void UpdateScreenInfo();
    void Hide();

    public static void SetRenderOrder(IUnit? unit,
    RenderOrder fromOrder, RenderOrder toOrder,
    IUIElement element)
    {
        if (unit is null || !UIRender.TreePriority.TryGetValue(unit, out var renderDict))
            return;

        if (renderDict.TryGetValue(fromOrder, out var fromList))
        {
            fromList.Remove(element);
            if (fromList.Count == 0)
            {
                renderDict.Remove(fromOrder);
            }
        }

        if (!renderDict.TryGetValue(toOrder, out var toList))
        {
            toList = new List<IUIElement>();
            renderDict[toOrder] = toList;
        }

        toList.Add(element);
    }

    public static void SetOwner(IUnit? fromUnit, IUnit? toUnit, IUIElement element)
    {
        if (fromUnit is not null)
        {
            if (UIRender.TreePriority.TryRemove(fromUnit, out var sortedDict))
            {
                if (toUnit is not null)
                {
                    UIRender.AddToPriority(toUnit, RenderOrder.Hands, element);
                }
            }
        }
        else
            UIRender.AddToPriority(toUnit, RenderOrder.Hands, element);
    }

}
