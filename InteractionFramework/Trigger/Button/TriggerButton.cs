using ControlLib.Buttons;
using ProtoRender.Object;


namespace InteractionFramework.Trigger.Button;
public class TriggerButton : ITrigger
{
    public bool isTriggered { get; private set; } = false;
    public bool IsBlocked { get; set; } = false;

    public Action<IUnit>? OnTriggered { get; set; }
    public Action<IUnit>? OnUntriggered { get; set; }

    public DateTime LastCheckTime { get; set; } = DateTime.MinValue;
    public int CooldownMs { get; set; } = 0;

    private readonly ButtonBinding binding;


    public TriggerButton(ButtonBinding key, Action<IUnit>? onTriggered, Action<IUnit>? onUntriggered)
    {
        binding = key;

        OnTriggered = onTriggered;
        OnUntriggered = onUntriggered;
    }

    public void CheckTrigger(IUnit unit)
    {
        if (IsBlocked || !ITrigger.CheckCooldown(this))
            return;

        binding.Listen();
        isTriggered = binding.IsPress;


        if (isTriggered)
            OnTriggered?.Invoke(unit);
        else
            OnUntriggered?.Invoke(unit);
    }
}
