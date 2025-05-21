using ObstacleLib.SpriteLib;
using ProtoRender.Object;
using ProtoRender.RenderAlgorithm;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectFramework.VisualImpact.Data;
public class VisualImpactData : IBeyoundData
{
    public readonly long LifetimeMilliseconds;
    internal readonly Stopwatch Stopwatch = new();

    internal SpriteObstacle VisualImpact;

    public int Id { get; set; }
    public IUnit? Owner { get; set; }

    public VisualImpactData(SpriteObstacle visualImpact, long lifetimeMilliseconds, bool isCreate = true)
    {
        if (isCreate)
            VisualImpact = new SpriteObstacle(visualImpact);
        else
            VisualImpact = visualImpact;

        LifetimeMilliseconds = lifetimeMilliseconds;
    }
    public VisualImpactData(VisualImpactData visualImpactData, bool isCreate = true)
    {
        if (isCreate)
            VisualImpact = new SpriteObstacle(visualImpactData.VisualImpact);
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
