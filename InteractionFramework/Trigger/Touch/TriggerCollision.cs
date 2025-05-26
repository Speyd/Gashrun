using ProtoRender.Object;
using MoveLib.Move;
using TextureLib.Textures.Pair;


namespace InteractionFramework.Trigger.TriggerTouch;
public class TriggerCollision : ITrigger
{
    public bool isTriggered { get; private set; } = false;

    public Action<IUnit>? OnTriggered { get; set; }
    public Action<IUnit>? OnUntriggered { get; set; }
    public DateTime LastCheckTime { get; set; } = DateTime.MinValue;
    public int CooldownMs { get; set; } = 0;


    private ObjectSide? CollisionSide = null;
    private readonly IObject TriggerObject;
    public TriggerCollision(IObject triggerObject, ObjectSide? collisionSide, Action<IUnit>? onTriggered, Action<IUnit>? onUntriggered)
    {
        CollisionSide = collisionSide;
        TriggerObject = triggerObject;

        OnTriggered = onTriggered;
        OnUntriggered = onUntriggered;
    }
    private ObjectSide? lastCollisionSide = null;
    public void CheckTrigger(IUnit unit)
    {
        if (!ITrigger.CheckCooldown(this))
            return;


        var result = Collision.CheckCollisionWithSide(unit, TriggerObject, true);
        if (result.Item1)
            lastCollisionSide = result.Item2;

        if (result.Item1 && isTriggered == false && (CollisionSide is null || result.Item2 == CollisionSide))
        {
            OnTriggered?.Invoke(unit);
            isTriggered = true;
        }
        else if (!result.Item1 && lastCollisionSide == CollisionSide && isTriggered)
        {
            OnUntriggered?.Invoke(unit);
            isTriggered = false;
        }
    }

}