using BehaviorPatternsFramework.Enum;
using ProtoRender.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework;
public class AIController
{
    private IUnit Owner;
    private readonly Dictionary<AIBehaviorType, AIStateMachine> _machines = new();
    public AIContext Context { get; set; } = new AIContext();

    public AIController(IUnit owner)
    {
        Owner = owner;
        Context.Owner = Owner;
    }
    public void AddStateMachine(AIBehaviorType type, AIStateMachine machine)
    {
        machine.SetAIContext(Context);
        _machines[type] = machine;
    }

    public AIStateMachine? GetMachine(AIBehaviorType type)
    {
        _machines.TryGetValue(type, out var machine);
        return machine;
    }

    public void Start()
    {
        foreach(var m in _machines.Values)
            BehaviorHandler.Add(m);
    }
}
