using ProtoRender.Object;
using System.Collections.Concurrent;
using ObjectFramework;

namespace UIFramework.Render;

public static class UIRender
{
    public static ConcurrentDictionary<IUnit, SortedDictionary<RenderOrder, ConcurrentDictionary<IUIElement, byte>>> TreePriority { get; }
    private static readonly ConcurrentDictionary<IUnit, object> Locks = new();

    static UIRender()
    {
        TreePriority = new();
    }

    private static object GetLock(IUnit owner) => Locks.GetOrAdd(owner, _ => new object());

    public static void AddToPriority(IUnit? owner, RenderOrder objRender, IUIElement uiElement)
    {
        if (owner is null)
            return;

        var priorityTree = TreePriority.GetOrAdd(owner, _ => new SortedDictionary<RenderOrder, ConcurrentDictionary<IUIElement, byte>>());

        lock (GetLock(owner))
        {
            if (!priorityTree.TryGetValue(objRender, out var elementSet))
            {
                elementSet = new ConcurrentDictionary<IUIElement, byte>();
                priorityTree[objRender] = elementSet;
            }
            elementSet.TryAdd(uiElement, 0);
        }
    }

    public static void RemoveFromPriority(IUnit? owner, RenderOrder objRender, IUIElement uiElement)
    {
        if (owner is null)
            return;

        if (TreePriority.TryGetValue(owner, out var priorityTree))
        {
            lock (GetLock(owner))
            {
                if (priorityTree.TryGetValue(objRender, out var elementSet))
                {
                    elementSet.TryRemove(uiElement, out _);
                    if (elementSet.IsEmpty)
                        priorityTree.Remove(objRender);

                    if (priorityTree.Count == 0)
                        TreePriority.TryRemove(owner, out _);
                }
            }
        }
    }

    public static void DrawingByPriority()
    {
        if (Camera.CurrentUnit is null)
            return;

        if (TreePriority.TryGetValue(Camera.CurrentUnit, out var priorityTree))
        {
            lock (GetLock(Camera.CurrentUnit))
            {
                var renderOrders = priorityTree.Keys.ToList();
                foreach (var renderOrder in renderOrders)
                {
                    if (priorityTree.TryGetValue(renderOrder, out var elements))
                    {
                        foreach (var uiElement in elements.Keys)
                        {
                            uiElement.Render();
                        }
                    }
                }
            }
        }
    }
}
