using BehaviorPatternsFramework.Enum;
using BehaviorPatternsFramework.Behavior;
using BehaviorPatternsFramework.PatternAttack.Strategy;
using SFML.System;
using HitBoxLib.PositionObject;
using HitBoxLib.Segment.SignsTypeSide;
using ImageMagick;
using ProtoRender.Object;

namespace BehaviorPatternsFramework.PatternAttack;
public class AttackBehavior : IAIBehavior
{
    public const string ReloadTriggerEvent = "ReloadStarted";
    public const string ReloadCompletedEvent = "ReloadCompleted";

    private readonly Random random = new();

    public BehaviorStatus Status { get; private set; } = BehaviorStatus.Failure;
    public Func<AIContext, bool>? IsBlocked { get; set; }


    InfoGun InfoGun { get; set; }

    private bool lastReloading = false;

    private bool hasSignaledReload = false;
    public float MaxDistanceAttack { get; set; } = -1;

    public List<IAimStrategy> AimHorizontalStrategies { get; set; } = new();
    public List<IAimStrategy> AimVerticalStrategies { get; set; } = new();


    public AttackBehavior(InfoGun infoGun)
    {
        InfoGun = infoGun;
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
        if (AimHorizontalStrategies.Count == 0 || AimVerticalStrategies.Count == 0)
            return;

        var horizontalStrategy = AimHorizontalStrategies[random.Next(AimHorizontalStrategies.Count)];
        var verticalStrategy = AimVerticalStrategies[random.Next(AimVerticalStrategies.Count)];

        var tempAngle = context.Owner!.Angle;
        var tempVerticalAngle = context.Owner!.VerticalAngle;

        context.Owner!.Angle = horizontalStrategy.GetAimHorizontalAngle(context, InfoGun);
        context.Owner!.VerticalAngle = verticalStrategy.GetAimVerticalAngle(context, InfoGun);

        InfoGun.AttackBind.SimulatePress();

        context.Owner!.Angle = tempAngle;
        context.Owner!.VerticalAngle = tempVerticalAngle;
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

