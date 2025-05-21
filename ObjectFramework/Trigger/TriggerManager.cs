using DataPipes;
using RayTracingLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoRender.Object;
using System.Threading;

namespace ObjectFramework.Trigger;
public static class TriggerManager
{
    private static readonly ConcurrentDictionary<IUnit, ConcurrentDictionary<ITrigger, byte>> triggers = new();
    public static CancellationToken cancellationToken = new CancellationToken();

    public static async Task CheckTriggersAsync()
    {
        await Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Parallel.ForEach(triggers.Keys, ScreenLib.Screen.Setting.ParallelOptions, triggerObj =>
                {
                    Parallel.ForEach(triggers[triggerObj].Keys, ScreenLib.Screen.Setting.ParallelOptions, trigger =>
                    {
                        trigger.CheckTrigger(triggerObj);
                    });
                });

                await Task.Delay(30, cancellationToken);
            }
        }, cancellationToken);
    }

    public static void AddTriger(IUnit obj, ITrigger trigger)
    {
        var triggerDict = triggers.GetOrAdd(obj, _ => new ConcurrentDictionary<ITrigger, byte>());
        triggerDict.TryAdd(trigger, 0);
    }
    public static void RemoveTriger(IUnit obj, ITrigger trigger)
    {
        if (triggers.TryGetValue(obj, out var triggerDict))
        {
            triggerDict.TryRemove(trigger, out _);
            if (triggerDict.IsEmpty)
                triggers.TryRemove(obj, out _);
        }
    }
}
