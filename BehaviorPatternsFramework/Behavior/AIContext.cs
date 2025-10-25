using BehaviorPatternsFramework.Enum;
using ProtoRender.Object;
using SFML.Window;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework.Behavior;
public class AIContext
{
    public AIController? Controller { get; internal set; }

    public IUnit? Owner { get; set; } = null;

    public IObject? TargetObject { get; set; } = null;

    public event Action<string>? OnEvent;
    public void TriggerEvent(string evt) => OnEvent?.Invoke(evt);
}