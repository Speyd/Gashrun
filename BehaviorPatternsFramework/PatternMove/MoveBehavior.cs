using BehaviorPatternsFramework.Behavior;
using BehaviorPatternsFramework.Enum;
using BehaviorPatternsFramework.PatternObservations;
using FpsLib;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework.PatternMove;

public class MoveBehavior : IAIBehavior
{
    public BehaviorStatus Status { get; private set; } = BehaviorStatus.Failure;
    public Func<AIContext, bool>? IsBlocked { get; set; }
    public Action<AIContext>? FuncSuccess { get; set; }


    private double _elapsedMs = 0;
    public int WaitDurationMs { get; set; } = 680;


    public void Update(AIContext context)
    {
        Status = BehaviorStatus.Running;
        if (context.Owner is null || context.Owner.Map is null || (IsBlocked?.Invoke(context) ?? false))
        {
            Status = BehaviorStatus.Failure;
            return;
        }

        var deltaTimeMs = FPS.GetDeltaTime();
        _elapsedMs += deltaTimeMs;


        if (_elapsedMs >= WaitDurationMs / 1000f)
        {
            _elapsedMs = 0;
            Status = BehaviorStatus.Success;

            return;
        }

        MoveLib.Move.MovePositions.Move(context.Owner, context.Owner.LookDirection.X, context.Owner.LookDirection.Y);
        Status = BehaviorStatus.Failure;
    }

    public void Enter(AIContext context)
    {
        // Console.WriteLine("Entering JumpBehavior");
    }

    public void Exit(AIContext context)
    {
        //Console.WriteLine("Exiting JumpBehavior");
    }

    public BehaviorStatus GetNextEvent(AIContext context)
    {
        return Status;
    }

    public IAIBehavior GetDeepCopy()
    {
        var tempMoveBehavior = new MoveBehavior();
        tempMoveBehavior.WaitDurationMs = WaitDurationMs;

        return tempMoveBehavior;
    }
}
