using BehaviorPatternsFramework.Behavior;
using BehaviorPatternsFramework.Enum;
using FpsLib;
using ProtoRender.Object;
using ProtoRender.Physics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework;
public class ZoneRestrictionBehavior : IAIBehavior
{
    public BehaviorStatus Status { get; private set; } = BehaviorStatus.Failure;
    public Func<AIContext, bool>? IsBlocked { get; set; }

    public Vector2f CenterPoint { get; set; }

    private Vector2f _targetCoordinate = new Vector2f();
    public Vector2f TargetCoordinate
    { 
        get => _targetCoordinate;
        set
        {
            SetTargetCoordinate(value);
        }
    }

    public int Radius { get; set; }

    public bool _isReturning = false;

    public ZoneRestrictionBehavior(Vector2f centerPoint)
    {
        this.CenterPoint = centerPoint;
        this.TargetCoordinate = centerPoint;
    }

    public void Update(AIContext context)
    {
        var owner = context.Owner;
        if (owner is null || owner.Map is null || (IsBlocked?.Invoke(context) ?? false))
        {
            Status = BehaviorStatus.Failure;
            return;
        }

        var dx = owner.CellX - CenterPoint.X;
        var dy = owner.CellY - CenterPoint.Y;
        if (Math.Abs(dx) > Radius || Math.Abs(dy) > Radius)
        {
            _isReturning = true;
        }

        if (_isReturning)
        {
            HandleReturn(owner);
            return;
        }

        Status = BehaviorStatus.Failure;
    }
    private void MoveTowardsCenter(IUnit owner)
    {
        float deltaTimeMs = FPS.GetDeltaTime();
        float moveDistance = owner.MoveSpeed * deltaTimeMs;

        owner.Angle = DataPipes.MathUtils.CalculateAngleToTarget(TargetCoordinate, owner.OriginPosition);
        Vector2f velocity = moveDistance * owner.LookDirection;

        MoveLib.Move.Collision.TryMoveWithCollision(
            owner,
            velocity.X,
            velocity.Y,
            owner.IgnoreCollisionObjects.Keys.ToList()
        );
    }
    private void HandleReturn(IUnit owner)
    {
        int dx = owner.CellX - (int)CenterPoint.X;
        int dy = owner.CellY - (int)CenterPoint.Y;

        if (dx == 0 && dy == 0)
        {
            _isReturning = false;
            Status = BehaviorStatus.Failure;
            return;
        }

        MoveTowardsCenter(owner);

        Status = BehaviorStatus.Success;
    }


    public void SetTargetCoordinate(Vector2f coordinate)
    {
        var dx = coordinate.X - CenterPoint.X;
        var dy = coordinate.Y - CenterPoint.Y;
        if (Math.Abs(dx) > Radius || Math.Abs(dy) > Radius)
           return;

        _targetCoordinate = coordinate;
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
