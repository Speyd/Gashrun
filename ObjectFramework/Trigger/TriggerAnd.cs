using ProtoRender.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ObjectFramework.Trigger;
public class TriggerAnd : ITrigger
{
    public bool isTriggered { get; private set; } = false;
    public Action<IUnit>? OnTriggered { get; set; }
    public Action<IUnit>? OnUntriggered { get; set; }

    private readonly List<ITrigger> triggers;

    public TriggerAnd(params ITrigger[] triggers)
    {
        this.triggers = triggers.ToList();
    }

    public void CheckTrigger(IUnit unit)
    {
        foreach (var trigger in triggers)
            trigger.CheckTrigger(unit);

        bool all = triggers.All(t => t.isTriggered);
        if (all && !isTriggered)
        {
            isTriggered = true;
            OnTriggered?.Invoke(unit);
        }
        else if (!all && isTriggered)
        {
            isTriggered = false;
            OnUntriggered?.Invoke(unit);
        }
    }
}
