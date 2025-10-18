using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework;

public enum GameEventType
{
    None,
    PlayerSeen,
    PlayerLost,
    GotHit,
    JumpError,
    JumpCompleted,
    PersecutionCompleted,
    PersecutionError,
    IdleTimeout
}
