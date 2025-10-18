using BehaviorPatternsFramework.Enum;
using ProtoRender.Object;
using SFML.Window;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework;
public class AIContext
{
    public ConcurrentDictionary<AIBehaviorType, GameEventType> EventTypes { get; set; } = new();


    public IUnit? Owner { get; set; } = null;

    public IObject? TargetObject { get; set; } = null;
}