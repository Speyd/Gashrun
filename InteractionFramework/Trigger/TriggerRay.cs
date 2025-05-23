


using ControlLib;
using ProtoRender.Object;
using RayTracingLib;

namespace InteractionFramework.Trigger;
public class TriggerRay : ITrigger
{
    public bool isTriggered { get; private set; } = false;

    public Action<IUnit>? OnTriggered { get; set; }
    public Action<IUnit>? OnUntriggered { get; set; }

    private IObject? CurrentTargetObject { get; set; } = null;

    public TriggerRay(Action<IUnit>? onTriggered, Action<IUnit>? onUntriggered)
    {
        OnTriggered = onTriggered;
        OnUntriggered = onUntriggered;
    }
    public IObject? GetTarget()
    {
        return CurrentTargetObject;
    }
    public void CheckTrigger(IUnit unit)
    {
        if (unit.Map is null)
            return;

        var result = Raycast.RaycastFun(unit.Map, unit, false);
        CurrentTargetObject = result.Item1;
        isTriggered = result.Item1 is not null? true: false;

        if (isTriggered)
            OnTriggered?.Invoke(unit);
        else
            OnUntriggered?.Invoke(unit);
    }
}
