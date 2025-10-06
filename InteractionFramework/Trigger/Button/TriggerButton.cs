using ControlLib.Buttons;
using ProtoRender.Object;
using static SFML.Window.Keyboard;


namespace InteractionFramework.Trigger.Button;
public class TriggerButton : ITrigger
{
    public bool isTriggered { get; private set; } = false;
    public bool IsBlocked { get; set; } = false;

    public Action<IUnit>? OnTriggered { get; set; }
    public Action<IUnit>? OnUntriggered { get; set; }

    public DateTime LastCheckTime { get; set; } = DateTime.MinValue;
    public int CooldownMs { get; set; } = 0;

    public ButtonBinding KeyBinding { get; set; }


    public TriggerButton(ButtonBinding key, Action<IUnit>? onTriggered, Action<IUnit>? onUntriggered)
    {
        KeyBinding = key;

        OnTriggered = onTriggered;
        OnUntriggered = onUntriggered;
    }
    public TriggerButton(TriggerButton triggerButton)
    {
        KeyBinding = triggerButton.KeyBinding;

        OnTriggered = triggerButton.OnTriggered;
        OnUntriggered = triggerButton.OnUntriggered;

        CooldownMs = triggerButton.CooldownMs;
    }

    public void CheckTrigger(IUnit unit)
    {
        if (IsBlocked || !ITrigger.CheckCooldown(this))
            return;

        KeyBinding.Listen();
        isTriggered = KeyBinding.IsPress;


        if (isTriggered)
            OnTriggered?.Invoke(unit);
        else
            OnUntriggered?.Invoke(unit);
    }
}
