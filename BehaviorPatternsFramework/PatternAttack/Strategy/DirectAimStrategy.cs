using BehaviorPatternsFramework.Behavior;
using DataPipes;
using SFML.System;


namespace BehaviorPatternsFramework.PatternAttack.Strategy;
public class DirectAimStrategy : IAimStrategy
{
    public float GetAimAngle(AIContext context, InfoGun infoGun)
    {
        var owner = context.Owner!;
        var target = context.TargetObject!;
        var targetPos = new Vector2f((float)target.X.Axis, (float)target.Y.Axis);

        return MathUtils.CalculateAngleToTarget(targetPos, owner.OriginPosition);
    }
}
