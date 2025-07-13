using ScreenLib.Output;
using SFML.Graphics;
using SFML.System;
using ProtoRender.Object;
using UIFramework.Render;
using System.Collections.Concurrent;


namespace UIFramework;
public interface IUIElement
{
    public static ScreenLib.Output.OutputPriorityType OutputPriorityType { get; } = OutputPriorityType.Interface;

    float PreviousScreenWidth { get; }
    float PreviousScreenHeight { get; }

    Vector2f PositionOnScreen { get; set; }
    List<Drawable> Drawables { get; init; }
    RenderOrder RenderOrder { get; set; }
    IUnit? Owner { get; set; }

    bool IsHide {  get; set; }

    void Render();
    void UpdateInfo();
    void UpdateScreenInfo();
    public void ToggleVisibility();
    internal void ToggleVisibilityObject();


    public static void SetRenderOrder(IUnit? unit, RenderOrder fromOrder, RenderOrder toOrder, IUIElement element)
    {
        if (unit is null || !UIRender.TreePriority.TryGetValue(unit, out var renderDict))
            return;

        if (renderDict.TryGetValue(fromOrder, out var fromSet))
        {
            fromSet.TryRemove(element, out _);

            if (fromSet.IsEmpty)
                renderDict.Remove(fromOrder);
        }

        if (!renderDict.TryGetValue(toOrder, out var toSet))
        {
            toSet = new ConcurrentDictionary<IUIElement, byte>();
            renderDict[toOrder] = toSet;
        }

        toSet.TryAdd(element, 0);
    }


    public static void SetOwner(IUnit? fromUnit, IUnit? toUnit, IUIElement element)
    {
        if (fromUnit is not null)
        {
            if(fromUnit == toUnit)
                UIRender.AddToPriority(toUnit, element.RenderOrder, element);
            else if (UIRender.TreePriority.TryGetValue(fromUnit, out var sortedDict))
            {
                if (sortedDict.TryGetValue(element.RenderOrder, out var concDic))
                    concDic.TryRemove(element, out _);
                if (toUnit is not null)
                    UIRender.AddToPriority(toUnit, element.RenderOrder, element);
            }
        }
        else if(toUnit is not null)
            UIRender.AddToPriority(toUnit, element.RenderOrder, element);
    }

}
