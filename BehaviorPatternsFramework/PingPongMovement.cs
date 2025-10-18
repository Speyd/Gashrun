using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FpsLib;
using HitBoxLib.Operations;
using ProtoRender.Object;
using ProtoRender.Physics;
using SFML.System;

namespace BehaviorPatternsFramework;
public class PingPongMovement : IMovementBehavior
{
    private double _elapsedMs = 0;

    public long MovementDurationMs { get; set; }

    public IUnit? Owner { get; set; } = null;
    // public float Direction { get; private set; } = 1f; // 1 - вперёд, -1 - назад
    public bool retlt = false;
    public bool Isturn { get; private set; } = false;
    public bool Isturn1 { get; private set; } = false;

    public double startAngle = 0;
    public void Update(AIContext aIContext)
    {
        if (Owner is null || Owner.Map is null)
            return;

        var deltaTimeMs = FPS.GetDeltaTime();
        _elapsedMs += deltaTimeMs;

       // var angle = obj.Direction;

        double halfDuration = (MovementDurationMs / 1000.0) / 2.0;
        if (_elapsedMs >= halfDuration && !Isturn)
        {
            // MoveLib.Angle.MoveAngle.TurnAngle(obj, MathF.PI);
            if (!Isturn1)
                Owner.Angle += Math.PI;
            else
                Isturn1 = false;
            Isturn = true;
        }
        if (_elapsedMs >= MovementDurationMs / 1000.0)
        {
            _elapsedMs = 0;
            Owner.Angle -= Math.PI;
            Isturn = false;
            return;
        }

        float distance = Owner.MoveSpeed * (float)(deltaTimeMs / 1);

        Vector2f direction = Owner.Direction;
        var Velocity = distance * Owner.Direction;
       // Console.WriteLine($"{_elapsedMs} | {halfDuration}");
        var xy = new Vector2f((float)Owner.X.Axis, (float)Owner.Y.Axis);
        var result = MoveLib.Move.Collision.TryMoveWithCollision(Owner, Velocity.X, Velocity.Y, Owner.IgnoreCollisionObjects.Keys.ToList());

        if(result.Result == true)
        {
            Owner.X.Axis = xy.X;
            Owner.Y.Axis = xy.Y;

            if (_elapsedMs < halfDuration && Isturn)
            {
                //Console.WriteLine("ffffffffffffffffffffffffffffffffffffffffffffffffffffffffff");
                _elapsedMs += halfDuration;
                Isturn = false;
            }
            else
            {
                _elapsedMs = MovementDurationMs / 1000.0 - _elapsedMs;

                Owner.Angle += Math.PI;
                Isturn = false;
                Isturn1 = true;
            }
        }
    }
}
