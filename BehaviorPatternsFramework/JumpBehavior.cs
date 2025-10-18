using BehaviorPatternsFramework.Enum;
using FpsLib;
using ObjectFramework;
using ProtoRender.Object;
using ProtoRender.Physics;
using RayTracingLib;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework;
public class JumpBehavior : IAIBehavior
{
    public static GameEventType SuccessfulUpdate { get; } = GameEventType.JumpCompleted;
    public static GameEventType ErrorUpdate { get; } = GameEventType.JumpError;


    public AIBehaviorType AIBehaviorType { get; } = AIBehaviorType.Movement;
    public GameEventType CurrentEventType { get; private set; } = GameEventType.JumpError;


    private double _elapsedMs = 0;
    public long MovementDurationMs { get; set; }


    public void Update(AIContext context)
    {
        if (context.Owner is null || context.Owner.Map is null)
        {
            SetError(context);
            return;
        }

        var deltaTimeMs = FPS.GetDeltaTime();
        _elapsedMs += deltaTimeMs;

        if(_elapsedMs >= MovementDurationMs / 1000)
        {
            _elapsedMs = 0;
            if (context.Owner is IJumper jumper)
                jumper.Jump();
        }

        SetSuccess(context);
    }

    public void Enter(AIContext context)
    {
       // Console.WriteLine("Entering JumpBehavior");
    }

    public void Exit(AIContext context)
    {
        //Console.WriteLine("Exiting JumpBehavior");
    }

    public GameEventType GetNextEvent(AIContext context)
    {
        return CurrentEventType;
    }

    private void SetSuccess(AIContext context)
    {
        context.EventTypes[AIBehaviorType] = SuccessfulUpdate;
        CurrentEventType = SuccessfulUpdate;
    }
    private void SetError(AIContext context)
    {
        context.EventTypes[AIBehaviorType] = ErrorUpdate;
        CurrentEventType = ErrorUpdate;
    }
}
