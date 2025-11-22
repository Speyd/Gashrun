


using ControlLib;
using ProtoRender.Object;
using RayTracingLib;

namespace InteractionFramework.Trigger.TriggerTouch;
public class TriggerRay : ITrigger
{
    public bool isTriggered { get; private set; } = false;
    public bool IsBlocked { get; set; } = false;

    public Action<IUnit>? OnTriggered { get; set; }
    public Action<IUnit>? OnUntriggered { get; set; }

    public DateTime LastCheckTime { get; set; } = DateTime.MinValue;
    public int CooldownMs { get; set; } = 0;

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
        if (IsBlocked || !ITrigger.CheckCooldown(this) || unit.Map is null)
            return;


        var result = Raycast.RaycastFun(unit);
        CurrentTargetObject = result.Item1;
        isTriggered = result.Item1 is not null ? true : false;

        if (isTriggered)
            OnTriggered?.Invoke(unit);
        else
            OnUntriggered?.Invoke(unit);
    }
}
