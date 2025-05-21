using ProtoRender.Object;
using ProtoRender.RenderAlgorithm;
using System.Collections.Concurrent;


namespace InteractionFramework.VisualImpact.Data;
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
