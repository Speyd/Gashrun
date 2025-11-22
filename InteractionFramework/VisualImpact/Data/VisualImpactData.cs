using ObstacleLib.SpriteLib;
using ProtoRender.Map;
using ProtoRender.Object;
using System.Collections.Concurrent;
using System.Diagnostics;


namespace InteractionFramework.VisualImpact.Data;
public class VisualImpactData : IBeyoundData
{
    public readonly long LifetimeMilliseconds;
    internal readonly Stopwatch Stopwatch = new();

    public SpriteObstacle VisualImpact;

    public int Id { get; set; }
    public IUnit? Owner { get; set; }
    public IMap? Map { get; set; }

    public VisualImpactData(SpriteObstacle visualImpact, long lifetimeMilliseconds, bool isCreate = true)
    {
        Map = null;

        if (isCreate)
            VisualImpact = new SpriteObstacle(visualImpact);
        else
            VisualImpact = visualImpact;

        LifetimeMilliseconds = lifetimeMilliseconds;
    }
    public VisualImpactData(VisualImpactData visualImpactData, bool isCreate = true)
    {
        Map = visualImpactData.Map;

        if (isCreate)
            VisualImpact = new SpriteObstacle(visualImpactData.VisualImpact, new TextureLib.Loader.ImageLoadOptions() { CreateNew = true});
        else
            VisualImpact = visualImpactData.VisualImpact;

        LifetimeMilliseconds = visualImpactData.LifetimeMilliseconds;
    }

    public void Render(IUnit observer)
    {
        VisualImpact?.Render(IBeyoundData.jammerResult, observer);
    }
    public void UpdateBeforeAdd(double x, double y, double z)
    {
        VisualImpact.X.Axis = x;
        VisualImpact.Y.Axis = y;
        VisualImpact.Z.Axis = z;

        Stopwatch.Restart();
    }
    public void UpdateBeforeAdd()
    {
        Stopwatch.Restart();
    }
    public void UpdateCheckRemove(ConcurrentDictionary<int, IBeyoundData> toRemove)
    {
        if (LifetimeMilliseconds != -1 &&
                Stopwatch.ElapsedMilliseconds >= LifetimeMilliseconds &&
                VisualImpact is not null)
        {
            BeyondRenderManager.Remove(Id);
        }
    }

    public IBeyoundData GetCopy() => new VisualImpactData(this);
}
