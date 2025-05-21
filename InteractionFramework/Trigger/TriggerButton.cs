using ControlLib;
using ProtoRender.Object;


namespace InteractionFramework.Trigger;
public class TriggerButton : ITrigger
{
    public bool isTriggered { get; private set; } = false;

    public Action<IUnit>? OnTriggered { get; set; }
    public Action<IUnit>? OnUntriggered { get; set; }
    public ITrigger? DependentTrigger {  get; set; }

    private readonly BottomBinding binding;

    private readonly IUnit owner;

    public TriggerButton(IUnit owner, BottomBinding key, ITrigger? dependentTrigger = null)
    {
        this.owner = owner;
        binding = key;

        DependentTrigger = dependentTrigger;
    }

    public void CheckTrigger(IUnit unit)
    {
        binding.Listen();
        isTriggered = binding.IsPress;
    }
}
