using DataPipes;
using ProtoRender.Object;
using RayTracingLib;


namespace InteractionFramework.Trigger;
public class TriggerDistance : ITrigger
{
    public bool isTriggered { get; private set; } = false;
    public Action<IUnit>? OnTriggered { get; set; } = null;
    private bool IsInvokeTriggered = false;

    public Action<IUnit>? OnUntriggered { get; set; } = null;
    private bool IsInvokeUntriggered = true;

    public IObject? CurrentTargetObject { get; set; }
    public double MinTriggerDistance { get; set; } = 1;

    public TriggerDistance(double minTriggerDistance, Action<IUnit>? onTriggered, Action<IUnit>? onUntriggered)
    {
        MinTriggerDistance = minTriggerDistance;

        OnTriggered = onTriggered;
        OnUntriggered = onUntriggered;
    }
    public IObject? GetTarget()
    {
        return CurrentTargetObject;
    }
    public void CheckTrigger(IUnit unit)
    {
        if (unit?.Map is not null)
        {
            var result = Raycast.RaycastFun(unit.Map, unit);
            double distance = MathUtils.CalculateDistance(result.Item2.X, result.Item2.Y, unit.X.Axis, unit.Y.Axis);
            if (distance > 0 && distance <= MinTriggerDistance * ScreenLib.Screen.Setting.Tile)
            {
                CurrentTargetObject = result.Item1;
                if (!IsInvokeTriggered)
                {
                    OnTriggered?.Invoke(unit);
                    IsInvokeTriggered = true;
                    IsInvokeUntriggered = false;
                }
                isTriggered = true;
            }
            else if (isTriggered)
            {
                if (!IsInvokeUntriggered)
                {
                    OnUntriggered?.Invoke(unit);
                    IsInvokeUntriggered = true;
                    IsInvokeTriggered = false;
                }
                isTriggered = false;
            }
        }
    }
}