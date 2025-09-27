using DataPipes;
using DataPipes.Pool;
using HitBoxLib.HitBoxSegment;
using ProtoRender.Object;
using RayTracingLib;
using SFML.System;


namespace InteractionFramework.Trigger.TriggerTouch;
public class TriggerDistance : ITrigger
{
    public bool isTriggered { get; private set; } = false;
    public bool IsBlocked { get; set; } = false;

    public Action<IUnit>? OnTriggered { get; set; } = null;
    public Action<IUnit>? OnUntriggered { get; set; } = null;

    public DateTime LastCheckTime { get; set; } = DateTime.MinValue;
    public int CooldownMs { get; set; } = 0;

    public (IObject? TargetObject, Vector3f CoordinateTouch, List<Box> TouchBox)? CurrentTargetResult { get; private set; }
    public double MinTriggerDistance { get; set; } = 1;
    public Predicate<IObject?>? DelegateDefinition { get; set; } = null;


    public TriggerDistance(Predicate<IObject?>? delegateDefinition, Action<IUnit>? onTriggered, Action<IUnit>? onUntriggered)
    {
        DelegateDefinition = delegateDefinition;

        OnTriggered = onTriggered;
        OnUntriggered = onUntriggered;
    }

    public void CheckTrigger(IUnit unit)
    {
        if (IsBlocked || !ITrigger.CheckCooldown(this) || unit?.Map is null)
            return;

        var result = Raycast.RaycastFun(unit);
        double distance = MathUtils.CalculateDistance(result.Item2.X, result.Item2.Y, unit.X.Axis, unit.Y.Axis);

        if (distance > 0 && distance <= MinTriggerDistance * ScreenLib.Screen.Setting.Tile &&
            (DelegateDefinition is null || DelegateDefinition.Invoke(result.Item1)))
        {
            CurrentTargetResult = result;
            OnTriggered?.Invoke(unit);

            isTriggered = true;
        }
        else if (isTriggered)
        {
            OnUntriggered?.Invoke(unit);
            
            isTriggered = false;
        }
    }

    public (IObject? TargetObject, Vector3f CoordinateTouch, List<Box> TouchBox)? GetTarget()
    {
        return CurrentTargetResult;
    }

    public void ResetTarget()
    {
        CurrentTargetResult = null;
    }
}