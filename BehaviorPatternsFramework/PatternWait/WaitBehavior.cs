using BehaviorPatternsFramework.Behavior;
using BehaviorPatternsFramework.Enum;
using BehaviorPatternsFramework.PatternWait.TimeProvider;
using FpsLib;
using ProtoRender.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework.PatternWait;
public class WaitBehavior : IAIBehavior
{
    public BehaviorStatus Status { get; private set; } = BehaviorStatus.Failure;
    public Func<AIContext, bool>? IsBlocked { get; set; }
    public Action<AIContext>? FuncSuccess { get; set; }


    private double _elapsedMs = 0;
    private long _currentWaitMs = 0;

    public IWaitTimeProvider WaitTimeProvider { get; set; } = new FixedWaitTime(600);

    public void Update(AIContext context)
    {
        Status = BehaviorStatus.Running;
        if (IsBlocked?.Invoke(context) ?? false)
        {
            Status = BehaviorStatus.Failure;
            return;
        }

        var deltaTimeMs = FPS.GetDeltaTime();
        _elapsedMs += deltaTimeMs;


        if (_elapsedMs >= _currentWaitMs / 1000f)
        {
            _elapsedMs = 0;
            Status = BehaviorStatus.Success;
        }
    }

    public void Enter(AIContext context)
    {
        _currentWaitMs = WaitTimeProvider.GetWaitTimeMs();
    }

    public void Exit(AIContext context)
    {
    }

    public BehaviorStatus GetNextEvent(AIContext context)
    {
        return Status;
    }

    public IAIBehavior GetDeepCopy()
    {
        var tempWaitBehavior = new WaitBehavior();
        tempWaitBehavior.WaitTimeProvider = WaitTimeProvider.GetDeepCopy();

        return tempWaitBehavior;
    }
}
