using BehaviorPatternsFramework.Behavior;
using BehaviorPatternsFramework.Enum;
using DataPipes;


namespace BehaviorPatternsFramework.PatternObservations;
public class MoveCamera : IAIBehavior
{
    public BehaviorStatus Status { get; private set; } = BehaviorStatus.Failure;
    public Func<AIContext, bool>? IsBlocked { get; set; }

    public IAngleStrategy AngleStrategy { get; set; } = new StaticAngleStrategy(MathF.PI);

    public void Update(AIContext context)
    {
        Status = BehaviorStatus.Running;

        if (context.Owner is null || context.Owner.Map is null || (IsBlocked?.Invoke(context) ?? false))
        {
            Status = BehaviorStatus.Failure;
            return;
        }

        var nextAngle = AngleStrategy.GetNextAngle(context);
        context.Owner.Angle = MathUtils.NormalizeAngleDifference(context.Owner.Angle + nextAngle, 0);

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
