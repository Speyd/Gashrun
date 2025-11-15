using BehaviorPatternsFramework.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework.PatternObservations;
public class RandomAngleStrategy : IAngleStrategy
{
    private static readonly Random _rand = new Random();

    public float MinAngle { get; set; } = 0f;
    public float MaxAngle { get; set; } = 2 * MathF.PI; 

    public RandomAngleStrategy(float minAngle, float maxAngle)
    {
        MinAngle = minAngle;
        MaxAngle = maxAngle;
    }
    public RandomAngleStrategy()
    {}

    public float GetNextAngle(AIContext context)
    {
        double range = MaxAngle - MinAngle;
        return MinAngle + (float)(_rand.NextDouble() * range);
    }
}
