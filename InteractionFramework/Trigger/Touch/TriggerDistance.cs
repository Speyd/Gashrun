using DataPipes;
using ProtoRender.Object;
using RayTracingLib;


namespace InteractionFramework.Trigger.TriggerTouch;
public class TriggerDistance : ITrigger
{
    public bool isTriggered { get; private set; } = false;
    public Action<IUnit>? OnTriggered { get; set; } = null;
    public bool isWorkTriggered = false;
    public Action<IUnit>? OnUntriggered { get; set; } = null;
    public bool isWorkUntriggered = false;

    public DateTime LastCheckTime { get; set; } = DateTime.MinValue;
    public int CooldownMs { get; set; } = 0;

    public IObject? CurrentTargetObject { get; set; }
    public Type? TriggerObject { get; set; } = null;
    public double MinTriggerDistance { get; set; } = 1;
    public bool isSwap = false;
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
                if (isSwap == false)
                {
                    if (isWorkTriggered == false)
                        OnTriggered?.Invoke(unit);

                    isTriggered = true;
                    isWorkTriggered = true;
                    isWorkUntriggered = false;
                }
                else
                {
                    if (isWorkUntriggered == false)
                        OnUntriggered?.Invoke(unit);

                    isTriggered = false;
                    isWorkTriggered = false;
                    isWorkUntriggered = true;
                }
            }
            else if (isTriggered)
            {
                if (isSwap == false)
                {
                    if (isWorkUntriggered == false)
                        OnUntriggered?.Invoke(unit);

                    isTriggered = false;
                    isWorkTriggered = false;
                    isWorkUntriggered = true;
                }
                else
                {
                    if (isWorkTriggered == false)
                        OnTriggered?.Invoke(unit);

                    isTriggered = true;
                    isWorkTriggered = true;
                    isWorkUntriggered = false;
                }
            }
        }
    }
}