using FpsLib;
using ProtoRender.Object;
using SFML.System;


namespace InteractionFramework.Trigger.Touch;
public class TriggerTouch : ITrigger
{
    private readonly static Vector2f[] angleOffsets = new Vector2f[]
    {
            new (1, 0),
            new (-1, 0),
            new (0, 1),
            new (0, -1),
    };


    public bool isTriggered { get; private set; } = false;
    public bool IsBlocked { get; set; } = false;

    public Action<IUnit>? OnTriggered { get; set; }
    public Action<IUnit>? OnUntriggered { get; set; }

    public DateTime LastCheckTime { get; set; } = DateTime.MinValue;
    public int CooldownMs { get; set; } = 0;


    public IObject? TriggerObject { get; private set; } = null;
    public Predicate<IObject?>? DelegateDefinition { get; set; } = null;


    public TriggerTouch(Predicate<IObject?>? detect, Action<IUnit>? onTriggered, Action<IUnit>? onUntriggered)
    {
        DelegateDefinition = detect;

        OnTriggered = onTriggered;
        OnUntriggered = onUntriggered;
    }

    public TriggerTouch(TriggerTouch trigetTouch, Predicate<IObject?>? detect = null)
        :this(detect is null ? trigetTouch.DelegateDefinition: detect, trigetTouch.OnTriggered, trigetTouch.OnUntriggered)
    {
        CooldownMs =  trigetTouch.CooldownMs;
    }

    private (IObject? CollisionObject, bool IsCollided) ObjectDefinition(IUnit unit)
    {
        double distancePerFrame = unit.MoveSpeed * FPS.GetDeltaTime();

        IObject? collidedObject = null;
        bool isCollided = false;

        foreach (var offset in angleOffsets)
        {
            float movementOffsetX = offset.X * (float)distancePerFrame;
            float movementOffsetY = offset.Y * (float)distancePerFrame;

            var result = MoveLib.Move.Collision.GetCollision(unit, movementOffsetX, movementOffsetY, unit.IgnoreCollisionObjects.Keys.ToList());

            if (result.CollisionObject is not null && result.CollisionCoordinate.HasValue)
            {
                collidedObject = result.CollisionObject;
                isCollided = true;
                break;
            }
        }

        return (collidedObject, isCollided);
    }

    public void CheckTrigger(IUnit unit)
    {
        if (IsBlocked || !ITrigger.CheckCooldown(this) || unit.Map is null)
            return;

        var resultCollision = ObjectDefinition(unit);
        bool resultDefinition = DelegateDefinition is not null ? DelegateDefinition.Invoke(resultCollision.CollisionObject) : true;
        TriggerObject = resultCollision.CollisionObject;

        if (resultCollision.IsCollided && resultDefinition)
        {
            OnTriggered?.Invoke(unit);
            isTriggered = true;
        }
        else if (!resultDefinition && isTriggered)
        {
            OnUntriggered?.Invoke(unit);
            isTriggered = false;
        }
        else if (isTriggered)
        {
            OnUntriggered?.Invoke(unit);
            isTriggered = false;
        }
    }  
}
