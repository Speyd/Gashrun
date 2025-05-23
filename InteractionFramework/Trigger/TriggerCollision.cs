using ProtoRender.Object;
using MoveLib.Move;
using TextureLib;


namespace InteractionFramework.Trigger;
public class TriggerCollision : ITrigger
{
    public bool isTriggered { get; private set; } = false;

    public Action<IUnit>? OnTriggered { get; set; }
    public Action<IUnit>? OnUntriggered { get; set; }


    private ObjectSide? CollisionSide = null;
    private readonly IObject TrggerObject;
    public TriggerCollision(IObject trggerObject, ObjectSide? collisionSide, Action<IUnit>? onTriggered, Action<IUnit>? onUntriggered)
    {
        CollisionSide = collisionSide;
        TrggerObject = trggerObject;

        OnTriggered = onTriggered;
        OnUntriggered = onUntriggered;
    }
    private ObjectSide? lastCollisionSide = null;
    public void CheckTrigger(IUnit unit)
    {
        var result = Collision.CheckCollisionWithSide(unit, TrggerObject, true);
        if(result.Item1)
            lastCollisionSide = result.Item2;

        if ((result.Item1 && isTriggered == false) && (CollisionSide is null || result.Item2 == CollisionSide))
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