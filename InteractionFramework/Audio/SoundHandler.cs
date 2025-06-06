using InteractionFramework.Audio.SoundType;
using System.Collections.Concurrent;


namespace InteractionFramework.Audio;
public static class SoundHandler
{
    private static readonly ConcurrentBag<ISound> Instances = new();
    private static readonly CancellationTokenSource _cts = new();
    private static readonly Thread CleanupThread;


    static SoundHandler()
    {
        CleanupThread = new Thread(SoundCleanupLoop)
        {
            IsBackground = true,
            Name = "SFML Sound Cleanup Thread"
        };
        CleanupThread.Start();
    }

    public static void RegisterISound(ISound sound)
    {
        if(!Instances.Contains(sound))
            Instances.Add(sound);
    }

    private static void SoundCleanupLoop()
    {
        while (!_cts.Token.IsCancellationRequested)
        {
            foreach (var instance in Instances)
            {
                instance.CleanupStoppedSounds();
            }

            Thread.Sleep(100);
        }
    }
    public static void StopCleanup()
    {
        _cts.Cancel();
        CleanupThread.Join();
    }


}
