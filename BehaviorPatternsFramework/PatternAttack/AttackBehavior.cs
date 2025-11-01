using BehaviorPatternsFramework.Enum;
using BehaviorPatternsFramework.Behavior;
using BehaviorPatternsFramework.PatternAttack.Strategy;

namespace BehaviorPatternsFramework.PatternAttack;
public class AttackBehavior : IAIBehavior
{
    public const string ReloadTriggerEvent = "ReloadStarted";
    public const string ReloadCompletedEvent = "ReloadCompleted";

    public BehaviorStatus Status { get; private set; } = BehaviorStatus.Failure;
    public Func<AIContext, bool>? IsBlocked { get; set; }


    InfoGun InfoGun { get; set; }

    private bool lastReloading = false;

    private bool hasSignaledReload = false;
    public float MaxDistanceAttack { get; set; } = -1;

    public List<IAimStrategy> AimStrategies { get; private set; } = new();
    private readonly Random random = new();

    public AttackBehavior(InfoGun infoGun, List<IAimStrategy>? aimStrategies = null)
    {
        InfoGun = infoGun;
        if (aimStrategies is not null)
            AimStrategies.AddRange(aimStrategies);
    }

    public void Update(AIContext context)
    {

        Status = BehaviorStatus.Running;
        if (HandleReload(context) && (IsBlocked?.Invoke(context) ?? false) == false)
        {
            lastReloading = true;
            return;
        }
        if (lastReloading)
        {
            lastReloading = false;
            context.TriggerEvent(ReloadCompletedEvent);
        }

        if (!IsContextValid(context) || (IsBlocked?.Invoke(context) ?? false))
            return;

        PerformAttack(context);


    }

    private bool IsContextValid(AIContext context)
    {
        if (context.TargetObject is null || context.Owner is null || context.Owner.Map is null)
        {
            Status = BehaviorStatus.Failure;
            return false;
        }
        return true;
    }
    private bool HandleReload(AIContext context)
    {
        if (!InfoGun.IsReloading.Invoke())
        {
            hasSignaledReload = false;
            return false;
        }

        if (!hasSignaledReload)
        {
            context.TriggerEvent(ReloadTriggerEvent);
            hasSignaledReload = true;
        }

        Status = BehaviorStatus.Failure;

        return true;
    }

    private void PerformAttack(AIContext context)
    {
        if (AimStrategies.Count == 0)
            return;

        var strategy = AimStrategies[random.Next(AimStrategies.Count)];

        float angle = strategy.GetAimAngle(context, InfoGun);
        context.Owner!.Angle = angle;

        InfoGun.AttackBind.SimulatePress();
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