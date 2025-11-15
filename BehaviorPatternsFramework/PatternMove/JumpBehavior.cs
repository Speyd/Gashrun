using BehaviorPatternsFramework.Behavior;
using BehaviorPatternsFramework.Enum;
using ProtoRender.Physics;
using System.Net.Quic;


namespace BehaviorPatternsFramework.PatternMove;
public class JumpBehavior : IAIBehavior
{
    public BehaviorStatus Status { get; private set; } = BehaviorStatus.Failure;
    public Func<AIContext, bool>? IsBlocked { get; set; }
    public Action<AIContext>? FuncSuccess { get; set; }
    public Action<AIContext>? FuncError { get; set; }

    private double _elapsedMs = 0;
    public long MovementDurationMs { get; set; }


    public void Update(AIContext context)
    {
        if (context.Owner is null || context.Owner.Map is null || (IsBlocked?.Invoke(context) ?? false))
        {
            Status = BehaviorStatus.Failure;
            return;
        }

        if (context.Owner is IJumper jumper && jumper.GroundState == GroundState.OnGround)
        {
            Status = BehaviorStatus.Success;
            FuncSuccess?.Invoke(context);
            jumper.Jump();
        }

        FuncError?.Invoke(context);
        Status = BehaviorStatus.Success;

        // var deltaTimeMs = FPS.GetDeltaTime();
        //_elapsedMs += deltaTimeMs;

        //if(_elapsedMs >= MovementDurationMs / 1000f)
        // {
        // _elapsedMs = 0;

        //}     
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
