using BehaviorPatternsFramework.Enum;
using ControlLib.Buttons;
using DataPipes;
using FpsLib;
using SFML.System;
using ProtoRender.Object;
using BehaviorPatternsFramework.Behavior;
using ControlLib.Mouse;

namespace BehaviorPatternsFramework.PatternAttack;
public class AttackBehavior : IAIBehavior
{
    public const string ReloadTriggerEvent = "ReloadStarted";
    public const string ReloadCompletedEvent = "ReloadCompleted";

    public BehaviorStatus Status { get; private set; } = BehaviorStatus.Failure;
    public Func<AIContext, bool>? IsBlocked { get; set; }


    public ButtonBinding attackBind;
    private bool lastReloading = false;
    public Func<bool> isReloading;
    public Func<float> getbulletSpeed;
    int sleepBulletHandler;

    private bool hasSignaledReload = false;
    public float MaxDistanceAttack { get; set; } = -1;


    public AttackBehavior(ButtonBinding attackBind, Func<bool> isReloading, Func<float> getbulletSpeed, int sleepBulletHandler)
    {
        this.attackBind = attackBind;
        this.isReloading = isReloading;
        this.getbulletSpeed = getbulletSpeed;
        this.sleepBulletHandler = sleepBulletHandler;
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
        if (!isReloading.Invoke())
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
        float deltaTimeMs = FPS.GetDeltaTime();
        var targetObject = context.TargetObject!;

        var obs = targetObject as IObserver;
        var move = targetObject as IMovable;
        if (obs == null || move == null)
        {
            Status = BehaviorStatus.Failure;
            return;
        }





        var targetPos = new Vector2f((float)targetObject.X.Axis, (float)targetObject.Y.Axis);
        var targetVelocity = move.MoveDirection * move.MoveSpeed * deltaTimeMs;
        targetVelocity = new Vector2f((float)(targetVelocity.X + targetObject.X.Axis), (float)(targetVelocity.Y + targetObject.Y.Axis));

        var dist = MathUtils.CalculateDistance(
            targetVelocity.X,
            targetVelocity.Y,
            context.Owner.X.Axis,
            context.Owner.Y.Axis
        );

        if (dist > MaxDistanceAttack && MaxDistanceAttack > 0)
        {
            Status = BehaviorStatus.Success;
            return;
        }
 
        var timeToHit = dist / Math.Ceiling(getbulletSpeed.Invoke() / (sleepBulletHandler / 2));
        targetVelocity = move.MoveDirection * move.MoveSpeed * deltaTimeMs;


        var newPosition = new Vector2f(
        (float)(targetPos.X + targetVelocity.X * timeToHit),
        (float)(targetPos.Y + targetVelocity.Y * timeToHit)
    );



        context.Owner.Angle = MathUtils.CalculateAngleToTarget(newPosition, context.Owner!.OriginPosition);

        attackBind.SimulatePress();
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