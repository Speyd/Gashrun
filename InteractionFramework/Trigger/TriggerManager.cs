using System.Collections.Concurrent;
using ProtoRender.Object;


namespace InteractionFramework.Trigger;
public static class TriggerManager
{
    public static readonly ConcurrentDictionary<IUnit, ConcurrentDictionary<ITrigger, byte>> triggers = new();

    private static CancellationTokenSource tokenSource = new();
    public static CancellationToken cancellationToken = new CancellationToken();

    public static void Start()
    {
        _ = CheckTriggersAsync();
    }

    public static void Stop()
    {
        tokenSource.Cancel();
    }
    public static async Task CheckTriggersAsync()
    {
        await Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Parallel.ForEach(triggers, ScreenLib.Screen.Setting.ParallelOptions, pair =>
                {
                    var unit = pair.Key;
                    var triggerDict = pair.Value;

                    Parallel.ForEach(triggerDict.Keys, ScreenLib.Screen.Setting.ParallelOptions, trigger =>
                    {
                        trigger.CheckTrigger(unit);
                    });
                });

                await Task.Delay(100, cancellationToken);
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
