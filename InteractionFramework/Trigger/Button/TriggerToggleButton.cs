using ControlLib;
using ProtoRender.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractionFramework.Trigger.Button;
public class TriggerToggleButton : ITrigger
{
    public bool isTriggered { get; private set; } = false;

    public Action<IUnit>? OnTriggered { get; set; }
    public Action<IUnit>? OnUntriggered { get; set; }
    public DateTime LastCheckTime { get; set; } = DateTime.MinValue;
    public int CooldownMs { get; set; } = 0;


    private readonly ButtonBinding binding;
    private bool wasPressedLastCheck = false;

    public TriggerToggleButton(ButtonBinding key, Action<IUnit>? onTriggered, Action<IUnit>? onUntriggered)
    {
        binding = key;
        OnTriggered = onTriggered;
        OnUntriggered = onUntriggered;
    }

    public void CheckTrigger(IUnit unit)
    {
        if (!ITrigger.CheckCooldown(this))
            return;


        binding.Listen();

        bool isPressedNow = binding.IsPress;
        if (isPressedNow && !wasPressedLastCheck)
            isTriggered = !isTriggered;


        if (isTriggered)
            OnTriggered?.Invoke(unit);
        else
            OnUntriggered?.Invoke(unit);


        wasPressedLastCheck = isPressedNow;
    }
}

