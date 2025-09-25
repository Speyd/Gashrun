using ProtoRender.Object;
using MoveLib.Move;
using TextureLib.Textures.Pair;
using HitBoxLib.HitBoxSegment;


namespace InteractionFramework.Trigger.TriggerTouch;
public class TriggerCollision : ITrigger
{
    public bool isTriggered { get; private set; } = false;
    public bool IsBlocked { get; set; } = false;

    public Action<IUnit>? OnTriggered { get; set; }
    public Action<IUnit>? OnUntriggered { get; set; }

    public DateTime LastCheckTime { get; set; } = DateTime.MinValue;
    public int CooldownMs { get; set; } = 0;

    private ObjectSide? TriggerSide = null;
    private ObjectSide? EntrySide = null;

    private readonly IObject TriggerObject;


    public TriggerCollision(IObject triggerObject, ObjectSide? triggerSide, ObjectSide? entrySide, Action<IUnit>? onTriggered, Action<IUnit>? onUntriggered)
    {
        TriggerSide = triggerSide;
        EntrySide = entrySide;

        TriggerObject = triggerObject;

        OnTriggered = onTriggered;
        OnUntriggered = onUntriggered;
    }


    private ObjectSide? lastCollisionSide = null;
    public void CheckTrigger(IUnit unit)
    {
        if (IsBlocked || !ITrigger.CheckCooldown(this))
            return;


        var result = CollisionHelper.CheckCollisionWithSide(unit, unit.HitBox.MainHitBox, TriggerObject, TriggerObject.HitBox.MainHitBox, true);
        if (result.Item1)
            lastCollisionSide = result.CollisionSide;

        if (result.Item1 && isTriggered == false && (TriggerSide is null || result.CollisionSide == TriggerSide))
        {
            OnTriggered?.Invoke(unit);
            isTriggered = true;
        }
        else if (!result.Item1 && lastCollisionSide == EntrySide && isTriggered)
        {
            OnUntriggered?.Invoke(unit);
            isTriggered = false;
        }
    }

}