using ProtoRender.Object;


namespace InteractionFramework.Trigger;
public interface ITrigger
{
    bool isTriggered {  get; }
    public Action<IUnit>? OnTriggered { get; set; }
    public Action<IUnit>? OnUntriggered { get; set; }
    void CheckTrigger(IUnit unit);
}
