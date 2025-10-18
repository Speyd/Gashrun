using FpsLib;
using ProtoRender.Object;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework;
public class CircularMovement : IMovementBehavior
{
    public float Speed { get; }
    public long MovementDurationMs { get; set; }

    public IUnit? Owner { get; set; } = null;

    public float Diametr { get; set; } = 5;
    public int cuurent = 1;
    public void Update(AIContext aIContext)
    {
        if (Owner == null || Owner.Map == null)
            return;

        float deltaTimeMs = FPS.GetDeltaTime();
       // _elapsedMs += deltaTimeMs;

        float MovementDurationSec = MovementDurationMs / 1000f;

        Owner.Angle += (deltaTimeMs * (MathF.PI * 2 / MovementDurationSec) );
        //Vector2f offset = obj.Direction;
        //        Vector2f offset = new Vector2f(
        //    MathF.Cos((float)-obj.Angle), // <-- минус для против часовой
        //    MathF.Sin((float)-obj.Angle)
        //) ;

        Vector2f offset = new Vector2f(
            MathF.Cos((float)Owner.Angle * cuurent), // <-- плюс для против часовой
            MathF.Sin((float)Owner.Angle * cuurent)
        );

        // offset = -offset  // <-- для отзеркаливания движения

        Vector2f targetPos = (Owner.MoveSpeed / MovementDurationSec * Diametr * deltaTimeMs) * offset;

        var result = MoveLib.Move.Collision.TryMoveWithCollision(Owner, targetPos.X, targetPos.Y, Owner.IgnoreCollisionObjects.Keys.ToList());

        if (result.Result)
            cuurent = -cuurent;
    }
}

