using BehaviorPatternsFramework.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework.PatternObservations;
public interface IAngleStrategy
{
    /// <summary>
    /// Returns the angle delta to apply.
    /// </summary>
    float GetNextAngle(AIContext context);
}