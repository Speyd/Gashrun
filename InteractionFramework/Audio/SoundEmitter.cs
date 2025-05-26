using SFML.Audio;
using SFML.System;
using System.Collections.Concurrent;


namespace InteractionFramework.Audio;
public class SoundEmitter
{
    private static readonly ConcurrentBag<SoundEmitter> Instances = new();
    private static readonly CancellationTokenSource _cts = new();
    private static readonly Thread CleanupThread;

    private readonly ConcurrentQueue<Sound> activeSounds = new();


    private string _soundBufferPath = "";
    public string SoundBufferPath
    {
        get => _soundBufferPath;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Path to sound buffer cannot be null or empty.");

            Sound.SoundBuffer = SoundDataCache.GetOrAdd(value, path => new SoundBuffer(path));
            _soundBufferPath = value;
        }
    }
    public Sound Sound { get; private set; }


    static SoundEmitter()
    {
        CleanupThread = new Thread(SoundCleanupLoop)
        {
            IsBackground = true,
            Name = "SFML Sound Cleanup Thread"
        };
        CleanupThread.Start();
    }

    public SoundEmitter(string soundBufferPath)
    {
        Sound = new Sound();
        SoundBufferPath = soundBufferPath;

        SoundDataCache.Load(soundBufferPath, new SoundBuffer(soundBufferPath));

        Instances.Add(this);
    }


    public void Play(Vector3f position)
    {
        var sound = new Sound(Sound);
        sound.Position = position;

        sound.Play();
        activeSounds.Enqueue(sound);
    }


    private static void SoundCleanupLoop()
    {
        while (!_cts.Token.IsCancellationRequested)
        {
            foreach (var instance in Instances)
            {
                while (instance.activeSounds.TryPeek(out var sound))
                {
                    if (sound.Status == SoundStatus.Stopped)
                        instance.activeSounds.TryDequeue(out _);
                    else
                        break;
                }
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
