using ProtoRender.Map;
using ProtoRender.Object;
using ScreenLib;
using SFML.Audio;
using SFML.System;
using System.Collections.Concurrent;
using System.Numerics;


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

            Sound.SoundBuffer = SoundDataCache.GetOrAdd(value, path => new SoundBuffer(path));
            _soundBufferPath = value;
        }
    }
    public Sound Sound { get; private set; }


    public ConcurrentDictionary<IObject, byte> AllowedListeners { protected get; init; } = new();
    public ConcurrentDictionary<IObject, byte> MonoListeners { protected get; init; } = new();


    public SoundEmitter(string soundBufferPath)
    {
        Sound = new Sound();
        SoundBufferPath = soundBufferPath;

        SoundDataCache.Load(soundBufferPath, new SoundBuffer(soundBufferPath));
        SoundHandler.RegisterISound(this);
    }



    public abstract void Play(IMap map, Vector3f positionSound = new Vector3f());
    protected static void StandartPlay(SoundEmitter ISoundObject, Sound sound, IMap map, Vector3f positionSound = new Vector3f())
    {
        if (Camera.CurrentUnit is null || map != Camera.CurrentUnit.Map ||
            ISoundObject.AllowedListeners.Count != 0 && !ISoundObject.AllowedListeners.Keys.Contains(Camera.CurrentUnit))
            return;

        if (ISoundObject.MonoListeners.Keys.Contains(Camera.CurrentUnit))
        {
            sound.Position = BaseMonoPosition;
            sound.RelativeToListener = true;
            sound.Play();

            ISoundObject.RegisterSound(sound);
            return;
        }
        else
        {
            sound.RelativeToListener = false;
        }

        Vector3 listenerPos = new Vector3((float)Camera.CurrentUnit.X.Axis, (float)Camera.CurrentUnit.Y.Axis, (float)Camera.CurrentUnit.Z.Axis);
        Vector3 sourcePos = new Vector3(positionSound.X, positionSound.Y, positionSound.Z);

        float z = AudioMath.AdjustZBySoundSide(Screen.Setting.Tile, listenerPos, sourcePos);
        float y = positionSound.Z - (float)Camera.CurrentUnit.Z.Axis;

        sound.Position = new Vector3f(positionSound.X, y, z);
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
            ActiveSounds.RemoveAll(s => s.Status == SoundStatus.Stopped);
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
