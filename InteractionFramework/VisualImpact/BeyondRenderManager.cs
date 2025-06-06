using ProtoRender.Object;
using System.Collections.Concurrent;
using InteractionFramework.VisualImpact.Data;
using ProtoRender.Map;


namespace InteractionFramework.VisualImpact;
public static class BeyondRenderManager
{
    public static ConcurrentDictionary<int, IBeyoundData> ListUpdateEffect = new();
    private static ConcurrentQueue<int> FreeIds = new();

    public static void Render(IUnit observer)
    {
        ConcurrentDictionary<int, IBeyoundData> toRemove = new();
        List<VisualImpactData> effectsSnapshot = new();

        Parallel.ForEach(ListUpdateEffect, effect =>
        {
            effect.Value.UpdateCheckRemove(toRemove);

            if(effect.Value.Map == Camera.CurrentUnit?.Map)
                effect.Value.Render(observer);
        });

        foreach (var item in toRemove.Keys)
        {
            Remove(item);
        }
    }


    private static int _nextId = 0;
    private static int GetNextId()
    {
        if (FreeIds.TryDequeue(out int id))
            return id;

        return Interlocked.Increment(ref _nextId) - 1;
    }


    public static void Create(IUnit owner, IBeyoundData visualImpact, double x, double y, double z)
    {
        var newVisualImpact = visualImpact;
        newVisualImpact.Map = Camera.CurrentUnit?.Map;
        newVisualImpact.Owner = owner;
        newVisualImpact.Id = GetNextId();
        newVisualImpact.UpdateBeforeAdd(x, y, z);

        ListUpdateEffect.AddOrUpdate(newVisualImpact.Id, newVisualImpact, (key, oldValue) => newVisualImpact);
    }
    public static int Create(IUnit owner, IBeyoundData visualImpact)
    {
        var newVisualImpact = visualImpact;
        newVisualImpact.Map = Camera.CurrentUnit?.Map;
        newVisualImpact.Owner = owner;
        newVisualImpact.Id = GetNextId();
        newVisualImpact.UpdateBeforeAdd();

        ListUpdateEffect.AddOrUpdate(newVisualImpact.Id, newVisualImpact, (key, oldValue) => newVisualImpact);
        return newVisualImpact.Id;
    }
    public static void Remove(int id)
    {
        if (ListUpdateEffect.TryRemove(id, out _))
        {
            FreeIds.Enqueue(id);
        }
    }
}
