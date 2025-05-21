using ProtoRender.Object;
using MoveLib.Move;
using TextureLib;


namespace InteractionFramework.Trigger;
public class TriggerHitBoxTouch : ITrigger
{
    public bool isTriggered { get; private set; } = false;

    public Action<IUnit>? OnTriggered { get; set; }
    private bool IsInvokeTriggered = false;

    public Action<IUnit>? OnUntriggered { get; set; }
    private bool IsInvokeUntriggered = false;


    private readonly IObject TrggerObject;

    private readonly IUnit owner;

    public TriggerHitBoxTouch(IUnit owner, IObject trggerObject, Action<IUnit>? onTriggered, Action<IUnit>? onUntriggered)
    {
        this.owner = owner;
        TrggerObject = trggerObject;

        OnTriggered = onTriggered;
        OnUntriggered = onUntriggered;
    }

    public void CheckTrigger(IUnit unit)
    {
        var gg = Collision.CheckCollisionWithSide(unit, TrggerObject, true);
        if (gg.Item1)
        {
            if (!IsInvokeTriggered && isTriggered == false && gg.Item2 is ObjectSide.Top)
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