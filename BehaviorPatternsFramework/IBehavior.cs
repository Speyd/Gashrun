
using ProtoRender.Object;

namespace BehaviorPatternsFramework;
public interface IBehavior
{
    IUnit? Owner { get; set; }

    void Update(AIContext aIContext);
}
