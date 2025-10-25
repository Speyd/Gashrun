using BehaviorPatternsFramework.Behavior;
using ProtoRender.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework;
public static class BehaviorHandler
{
    private static readonly List<AIStateMachine> Machines = new();
    private static readonly CancellationTokenSource _cts = new();
    private static readonly Thread _updateThread;

    static BehaviorHandler()
    {
        _updateThread = new Thread(UpdateLoop)
        {
            IsBackground = true,
            Name = "Behavior Update Thread"
        };
        _updateThread.Start();
    }

    public static void Add(AIStateMachine machine)
    {
        lock (Machines)
        {
            Machines.Add(machine);
        }
    }

    public static void Stop()
    {
        _cts.Cancel();
        _updateThread.Join();
    }

    private static void UpdateLoop()
    {
        Thread.Sleep(3000);
        while (!_cts.Token.IsCancellationRequested)
        {
            Update();
            Thread.Sleep(1);
        }
    }

    private static void Update()
    {
        lock (Machines)
        {
            foreach (var behavioral in Machines)
                behavioral.Update();
        }
    }
}
