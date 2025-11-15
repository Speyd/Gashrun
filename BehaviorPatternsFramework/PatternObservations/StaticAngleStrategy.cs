using BehaviorPatternsFramework.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework.PatternObservations;
public class StaticAngleStrategy : IAngleStrategy
{
    public float Angle { get; set; }

    public StaticAngleStrategy(float angle)
    {
        Angle = angle;
    }

    public float GetNextAngle(AIContext context)
    {
        return Angle;
    }
}
