using BehaviorPatternsFramework.Enum;
using ProtoRender.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorPatternsFramework.Behavior;
public class AIController
{
    private IUnit? _owner = null;
    public IUnit? Owner 
    {
        get => _owner;
        set
        {
            _owner = value;
            Context.Owner = value;
        }
    }

    private readonly Dictionary<AIBehaviorType, AIStateMachine> _machines = new();
    public AIContext Context { get; set; } = new AIContext();

    public AIController(IUnit owner)
    {
        Owner = owner;
        Context.Owner = Owner;
        Context.Controller = this;
    }
    public void AddStateMachine(AIBehaviorType type, AIStateMachine machine)
    {
        machine.SetAIContext(Context);
        _machines[type] = machine;
    }

    public AIStateMachine? GetMachine(AIBehaviorType type)
          => _machines.TryGetValue(type, out var machine) ? machine : null;

    public void Start()
    {
        foreach (var m in _machines.Values)
            BehaviorHandler.Add(m);
    }
    public void Signal(AIBehaviorType type)
    {
        if (_machines.TryGetValue(type, out var machine) && machine.IsPassive)
            machine.Signal();
    }
}
