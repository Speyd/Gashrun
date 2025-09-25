using ProtoRender.Map;
using ProtoRender.Object;
using SFML.Audio;
using SFML.System;
using System.Collections.Concurrent;
using ObjectFramework;


namespace InteractionFramework.Audio.SoundType;
public abstract class SoundEmitter : ISound
{
    public static readonly Vector3f BaseMonoPosition = new Vector3f(0, 0, 0);

    public List<Sound> ActiveSounds { private get; init; } = new();
    private readonly object _lock = new object();

    private string _soundBufferPath = "";
    public string SoundBufferPath
    {
        get => _soundBufferPath;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Path to sound buffer cannot be null or empty.");

            Sound.SoundBuffer = SoundDataCache.GetOrAdd(value, path => new SoundBuffer(path)).First();
            _soundBufferPath = value;
        }
    }
    public Sound Sound { get; private set; }
    public float HeightAttenuation { get; set; } = 1;

    public bool RequireListeners { get; set; } = false;
    public ConcurrentDictionary<IObject, byte> AllowedListeners { protected get; init; } = new();
    public ConcurrentDictionary<IObject, byte> MonoListeners { protected get; init; } = new();


    public SoundEmitter(string soundBufferPath)
    {
        Sound = new Sound();
        SoundBufferPath = soundBufferPath;

        SoundDataCache.Load(soundBufferPath, new SoundBuffer(soundBufferPath));
        SoundHandler.RegisterISound(this);
    }



    public abstract Sound Play(IMap map, Vector3f positionSound = new Vector3f());
    protected static void StandartPlay(SoundEmitter ISoundObject, Sound sound, IMap map, Vector3f positionSound = new Vector3f())
    {
        if (Camera.CurrentUnit is null || map != Camera.CurrentUnit.Map ||
            ISoundObject.AllowedListeners.Count != 0 && !ISoundObject.AllowedListeners.Keys.Contains(Camera.CurrentUnit))
            return;
        else if(ISoundObject.RequireListeners && ISoundObject.AllowedListeners.Count != 0 &&
            !ISoundObject.AllowedListeners.Keys.Contains(Camera.CurrentUnit))
        {
            return;
        }


        ISoundObject.SelectMethodPlay(sound, positionSound);
    }
    private void SelectMethodPlay(Sound sound, Vector3f positionSound)
    {
        if (Camera.CurrentUnit is null)
            return;

        if (MonoListeners.Keys.Contains(Camera.CurrentUnit))
        {
            sound.Position = BaseMonoPosition;
            sound.RelativeToListener = true;
            sound.Play();

            RegisterSound(sound);
            return;
        }
        else
        {
            sound.RelativeToListener = false;
        }

        sound.Position = new Vector3f(positionSound.X, positionSound.Z, positionSound.Y);
        sound.Play();
    }


    public virtual void SubscribeListener(IObject listener)
    {
        AllowedListeners.TryAdd(listener, 0);
    }
    public virtual void UnsubscribeListener(IObject listener)
    {
        AllowedListeners.TryRemove(listener, out _);
    }
    public virtual void SubscribeMonoListener(IObject listener)
    {
        SubscribeListener(listener);
        MonoListeners.TryAdd(listener, 0);
    }
    public virtual void UnsubscribeMonoListener(IObject listener)
    {
        UnsubscribeListener(listener);
        MonoListeners.TryRemove(listener, out _);
    }



    public virtual void CleanupStoppedSounds()
    {
        lock (_lock)
        {
            ActiveSounds.RemoveAll(s =>
                 s.Status == SoundStatus.Stopped || 
                 s.CPointer == IntPtr.Zero
            );
        }
    }
    public void RegisterSound(Sound sound)
    {
        lock (_lock)
        {
            ActiveSounds.Add(sound);
        }
    }
}
