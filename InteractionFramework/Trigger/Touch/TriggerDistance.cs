using DataPipes;
using ProtoRender.Object;
using RayTracingLib;


namespace InteractionFramework.Trigger.TriggerTouch;
public class TriggerDistance : ITrigger
{
    public bool isTriggered { get; private set; } = false;
    public Action<IUnit>? OnTriggered { get; set; } = null;
    public Action<IUnit>? OnUntriggered { get; set; } = null;
    public DateTime LastCheckTime { get; set; } = DateTime.MinValue;
    public int CooldownMs { get; set; } = 0;

    public IObject? CurrentTargetObject { get; set; }
    public Type? TriggerObject { get; set; } = null;
    public double MinTriggerDistance { get; set; } = 1;

    public TriggerDistance(double minTriggerDistance, Action<IUnit>? onTriggered, Action<IUnit>? onUntriggered, Type? triggerObj = null)
    {
        MinTriggerDistance = minTriggerDistance;
        TriggerObject = triggerObj;

        OnTriggered = onTriggered;
        OnUntriggered = onUntriggered;
    }
    public IObject? GetTarget()
    {
        return CurrentTargetObject;
    }
    public void CheckTrigger(IUnit unit)
    {
        if (!ITrigger.CheckCooldown(this) || unit?.Map is not null)
        {
            var result = Raycast.RaycastFun(unit);
            double distance = MathUtils.CalculateDistance(result.Item2.X, result.Item2.Y, unit.X.Axis, unit.Y.Axis);
            if (distance > 0 && distance <= MinTriggerDistance * ScreenLib.Screen.Setting.Tile &&
                (TriggerObject is null || TriggerObject is  not null && TriggerObject.IsAssignableFrom(result.Item1?.GetType())))
            {
                CurrentTargetObject = result.Item1;
                OnTriggered?.Invoke(unit);

                isTriggered = true;
            }
            else if (isTriggered)
            {
                OnUntriggered?.Invoke(unit);

                isTriggered = false;
            }
        }
    }
}