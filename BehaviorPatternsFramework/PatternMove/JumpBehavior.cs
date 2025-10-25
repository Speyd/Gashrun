using BehaviorPatternsFramework.Behavior;
using BehaviorPatternsFramework.Enum;
using ProtoRender.Physics;


namespace BehaviorPatternsFramework.PatternMove;
public class JumpBehavior : IAIBehavior
{
    public BehaviorStatus Status { get; private set; } = BehaviorStatus.Failure;
    public Func<AIContext, bool>? IsBlocked { get; set; }


    private double _elapsedMs = 0;
    public long MovementDurationMs { get; set; }


    public void Update(AIContext context)
    {
        if (context.Owner is null || context.Owner.Map is null || (IsBlocked?.Invoke(context) ?? false))
        {
            Status = BehaviorStatus.Failure;
            return;
        }

        if (context.Owner is IJumper jumper)
            _ = Task.Run(() => { jumper.Jump(); });
        // var deltaTimeMs = FPS.GetDeltaTime();
        //_elapsedMs += deltaTimeMs;

        //if(_elapsedMs >= MovementDurationMs / 1000)
        // {
        // _elapsedMs = 0;

        //}

        Status = BehaviorStatus.Success;
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
}
