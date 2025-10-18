using BehaviorPatternsFramework.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework;
public interface IAIBehavior
{
    virtual static GameEventType SuccessfulUpdate { get; } = GameEventType.None;
    virtual static GameEventType ErrorUpdate { get; } = GameEventType.None;

    AIBehaviorType AIBehaviorType { get; }

    void Enter(AIContext context);
    void Update(AIContext context);
    void Exit(AIContext context);

    GameEventType GetNextEvent(AIContext context);
}
