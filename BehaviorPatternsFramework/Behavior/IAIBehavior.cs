using BehaviorPatternsFramework.Behavior;
using BehaviorPatternsFramework.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework;
public interface IAIBehavior
{
    BehaviorStatus Status { get; }
    Func<AIContext, bool>? IsBlocked { get; }

    void Enter(AIContext context);
    void Update(AIContext context);
    void Exit(AIContext context);

    BehaviorStatus GetNextEvent(AIContext context);
    IAIBehavior GetDeepCopy ();

}
