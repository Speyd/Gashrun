using BehaviorPatternsFramework.Behavior;
using DataPipes;
using SFML.System;


namespace BehaviorPatternsFramework.PatternAttack.Strategy;
public class DirectAimStrategy : IAimStrategy
{
    public float GetAimHorizontalAngle(AIContext context, InfoGun infoGun)
    {
        var owner = context.Owner!;
        var target = context.TargetObject!;
        var targetPos = new Vector2f((float)target.X.Axis, (float)target.Y.Axis);

        return MathUtils.CalculateAngleToTarget(targetPos, owner.OriginPosition);
    }

    public float GetAimVerticalAngle(AIContext context, InfoGun infoGun)
    {
        var owner = context.Owner!;
        var target = context.TargetObject!;

        var ownerCenter = owner.GetHitboxCenter();
        var targetCenter = target.GetHitboxCenter();

        float dx = targetCenter.X - ownerCenter.X;
        float dy = targetCenter.Y - ownerCenter.Y;
        float dz = targetCenter.Z - ownerCenter.Z;

        float horizontalDist = MathF.Sqrt(dx * dx + dy * dy);

        return -MathF.Atan2(dz, horizontalDist);
    }
}
