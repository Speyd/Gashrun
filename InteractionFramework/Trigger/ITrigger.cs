using ProtoRender.Object;


namespace InteractionFramework.Trigger;
public interface ITrigger
{
    DateTime LastCheckTime { get; set; }
    int CooldownMs { get; set; }

    bool isTriggered {  get; }
    public Action<IUnit>? OnTriggered { get; set; }
    public Action<IUnit>? OnUntriggered { get; set; }
    void CheckTrigger(IUnit unit);
    public static bool CheckCooldown(ITrigger trigger)
    {
        var now = DateTime.UtcNow;
        if (trigger.CooldownMs != 0 && (now - trigger.LastCheckTime).TotalMilliseconds < trigger.CooldownMs)
            return false;

        trigger.LastCheckTime = now;
        return true;
    }
}
