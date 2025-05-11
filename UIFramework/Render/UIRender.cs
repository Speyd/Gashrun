using ScreenLib.Output;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScreenLib;
using ProtoRender.Object;
using System.Collections.Concurrent;
using ObjectFramework;

namespace UIFramework.Render;

public static class UIRender
{
    public static ConcurrentDictionary<IUnit, SortedDictionary<RenderOrder, List<IUIElement>>> TreePriority { get; }
    static UIRender()
    {
        TreePriority = new();
    }

    public static void AddToPriority(IUnit? owner, RenderOrder objRender, IUIElement uiElement)
    {
        if (owner is null)
            return;

        var priorityTree = TreePriority.GetOrAdd(owner, _ => new SortedDictionary<RenderOrder, List<IUIElement>>());
        if (!priorityTree.TryGetValue(objRender, out List<IUIElement>? value))
        {
            value = new List<IUIElement>();
            priorityTree[objRender] = value;
        }

        value.Add(uiElement);
    }

    public static void DrawingByPriority()
    {
        if (Camera.CurrentUnit is null)
            return;

        if (TreePriority.TryGetValue(Camera.CurrentUnit, out var priorityTree))
        {
            foreach (KeyValuePair<RenderOrder, List<IUIElement>> item in priorityTree)
            {
                foreach (var value in item.Value)
                    value.Render();
            }
        }
    }
}
