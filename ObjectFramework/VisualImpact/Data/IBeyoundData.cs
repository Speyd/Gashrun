using ObstacleLib.SpriteLib;
using ProtoRender.Object;
using ProtoRender.RenderAlgorithm;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectFramework.VisualImpact.Data;
public interface IBeyoundData
{
    protected static readonly Result jammerResult = new Result();

    IUnit? Owner { get; set; }
    int Id { get; set; }
    void Render(IUnit observer);
    void UpdateBeforeAdd(double x, double y, double z);
    void UpdateBeforeAdd();
    void UpdateCheckRemove(ConcurrentDictionary<int, IBeyoundData> toRemove);

    IBeyoundData GetCopy();
}
