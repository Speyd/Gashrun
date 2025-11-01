using BehaviorPatternsFramework.Behavior;
using DataPipes;
using FpsLib;
using ProtoRender.Object;
using SFML.System;

namespace BehaviorPatternsFramework.PatternAttack.Strategy;
public class PredictiveAimStrategy : IAimStrategy
{
    public float GetAimAngle(AIContext context, InfoGun infoGun)
    {
        float deltaTimeMs = FPS.GetDeltaTime();
        var owner = context.Owner!;
        var target = context.TargetObject!;

        var move = target as IMovable;
        if (move == null)
            return MathUtils.CalculateAngleToTarget(new Vector2f((float)target.X.Axis, (float)target.Y.Axis), owner.OriginPosition);

        var targetPos = new Vector2f((float)target.X.Axis, (float)target.Y.Axis);
        var targetVelocity = move.MoveDirection * move.MoveSpeed * deltaTimeMs;

        var dist = MathUtils.CalculateDistance(
            targetPos.X,
            targetPos.Y,
            owner.X.Axis,
            owner.Y.Axis
        );

        var timeToHit = dist / Math.Ceiling(infoGun.GetBulletSpeed.Invoke() / (infoGun.SleepBulletHandler / 2));

        var predictedPos = new Vector2f(
            (float)(targetPos.X + targetVelocity.X * timeToHit),
            (float)(targetPos.Y + targetVelocity.Y * timeToHit)
        );

        return MathUtils.CalculateAngleToTarget(predictedPos, owner.OriginPosition);
    }
}
