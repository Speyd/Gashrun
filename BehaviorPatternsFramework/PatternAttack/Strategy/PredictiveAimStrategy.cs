using BehaviorPatternsFramework.Behavior;
using DataPipes;
using FpsLib;
using HitBoxLib.PositionObject;
using HitBoxLib.Segment.SignsTypeSide;
using MoveLib.Move;
using NGenerics.Sorting;
using ProtoRender.Object;
using ProtoRender.Physics;
using SFML.System;
using System.Collections.Generic;

namespace BehaviorPatternsFramework.PatternAttack.Strategy;
public class PredictiveAimStrategy : IAimStrategy
{
    public float GetAimHorizontalAngle(AIContext context, InfoGun infoGun)
    {
        float deltaTime = FPS.GetDeltaTime();
        var owner = context.Owner!;
        var target = context.TargetObject!;

        var move = target as IMovable;
        if (move == null)
            return MathUtils.CalculateAngleToTarget(new Vector2f((float)target.X.Axis, (float)target.Y.Axis), owner.OriginPosition);

        var targetPos = new Vector2f((float)target.X.Axis, (float)target.Y.Axis);
        var targetVelocity = move.MoveDirection * move.MoveSpeed * deltaTime;

        var dist = MathUtils.CalculateDistance(
            targetPos.X,
            targetPos.Y,
            owner.X.Axis,
            owner.Y.Axis
        );

        var timeToHit = dist / Math.Ceiling(infoGun.GetHorizontalBulletSpeed.Invoke() / (infoGun.SleepBulletHandler / 2));

        var predictedPos = new Vector2f(
            (float)(targetPos.X + targetVelocity.X * timeToHit),
            (float)(targetPos.Y + targetVelocity.Y * timeToHit)
        );

        return MathUtils.CalculateAngleToTarget(predictedPos, owner.OriginPosition);
    }
    public float GetAimVerticalAngle(AIContext context, InfoGun infoGun)
    {
        float deltaTime = FPS.GetDeltaTime();
        var owner = context.Owner!;
        var target = context.TargetObject!;

        var ownerCenter = owner.GetHitboxCenter();
        var targetCenter = target.GetHitboxCenter();

        float dz = targetCenter.Z - ownerCenter.Z;

        float horizontalDistance = (float)MathUtils.CalculateDistance(
            target.X.Axis,
            target.Y.Axis,
            owner.X.Axis,
            owner.Y.Axis
        );

        var timeToHit = horizontalDistance / Math.Ceiling(infoGun.GetVerticalBulletSpeed.Invoke() / infoGun.SleepBulletHandler / 2);

        if (target is IJumper jumper && jumper.CurrentJumpForce != 0)
        {
            float verticalVelocity = jumper.CurrentJumpForce - (jumper.Gravity * deltaTime);
            dz = targetCenter.Z + verticalVelocity * (float)timeToHit * deltaTime - ownerCenter.Z;

            if (dz < jumper.GroundLevel)
                dz = jumper.GroundLevel;

            var maxHeight = (jumper.CurrentJumpForce * jumper.CurrentJumpForce) / (2 * jumper.Gravity);
            float maxJumpZ = (float)target.Z.Axis + maxHeight;

            if (target is IMovable move && move.MoveDirection.Z > 0 && dz > maxJumpZ)
                dz = targetCenter.Z;
        }

        return -MathF.Atan2(dz, horizontalDistance);
    }
}