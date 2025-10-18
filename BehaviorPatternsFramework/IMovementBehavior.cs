using ProtoRender.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework;
public interface IMovementBehavior : IBehavior
{
    public long MovementDurationMs { get; set; }
}
