using BehaviorPatternsFramework.Behavior;
using BehaviorPatternsFramework.Enum;
using ProtoRender.Physics;

namespace BehaviorPatternsFramework.PatternMove.Dodge;
public class DodgeBehavior : IAIBehavior
{
    public BehaviorStatus Status { get; private set; } = BehaviorStatus.Failure;
    public Func<AIContext, bool>? IsBlocked { get; set; }


    private double _elapsedMs = 0;
    public List<DodgeStep> Steps { get; private set; } = new List<DodgeStep>();

    private int _currentStepIndex = 0;
    public DodgeStep? CurrentStep { get; private set; }


    public DodgeBehavior(List<DodgeStep> steps)
    {
        AddDodgeStep(steps);
        CurrentStep = steps.FirstOrDefault();
    }
    public DodgeBehavior(DodgeStep step)
        :this(new List<DodgeStep> {step})
    {}

    public void Update(AIContext context)
    {
        if (context.Owner is null || context.Owner.Map is null || (IsBlocked?.Invoke(context) ?? false) || CurrentStep is null)
        {
            Status = BehaviorStatus.Failure;
            return;
        }

        var step = CurrentStep.Value;
        _elapsedMs += FpsLib.FPS.GetDeltaTime();

        MoveLib.Move.MovePositions.Move(context.Owner, step.VectorDirection.X, step.VectorDirection.Y);

        if (_elapsedMs >= step.DurationMs / 1000f)
        {
            _elapsedMs = 0;
            MoveToNextStep();
        }

        Status = BehaviorStatus.Success;
    }

    public void AddDodgeStep(DodgeStep step) => Steps.Add(step);
    public void AddDodgeStep(List<DodgeStep> steps) => Steps.AddRange(steps);

    public void MoveToNextStep()
    {
        if (Steps.Count == 0)
            return;

        _currentStepIndex++;

        if (_currentStepIndex >= Steps.Count)
            _currentStepIndex = 0;

        CurrentStep = Steps[_currentStepIndex];
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
