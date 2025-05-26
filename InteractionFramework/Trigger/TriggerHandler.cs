using System.Collections.Concurrent;
using ProtoRender.Object;


namespace InteractionFramework.Trigger;
public static class TriggerHandler
{
    public static readonly ConcurrentDictionary<IUnit, ConcurrentDictionary<ITrigger, byte>> triggers = new();

    private static readonly CancellationTokenSource _cts = new();
    private static readonly Thread StartThread;

    public static CancellationToken cancellationToken = new CancellationToken();
    static TriggerHandler()
    {
        StartThread = new Thread(CheckTriggersAsync)
        {
            IsBackground = true,
            Name = "Trigger Handler Thread"
        };
        StartThread.Start();
    }
    public static void StopProcess()
    {
        _cts.Cancel();
        StartThread.Join();
    }
    private static void CheckTriggersAsync()
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

            Thread.Sleep(50);
        }
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
