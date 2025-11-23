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
    private readonly object _lock = new object();
    private IMap? oldOwnerMap = null;

    public ConcurrentDictionary<IMap, List<Sound>> ActiveSounds { private get; init; } = new();
    private readonly List<Sound> _excludedSounds = new();


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

    public ConcurrentDictionary<IObject, ListenerType> Listener { protected get; init; } = new();



    public SoundEmitter(string soundBufferPath)
    {
        Sound = new Sound();
        SoundBufferPath = soundBufferPath;

        SoundDataCache.Load(soundBufferPath, new SoundBuffer(soundBufferPath));
        SoundHandler.RegisterISound(this);
    }


    #region Play Methods
    public abstract Sound Play(IMap? map, Vector3f positionSound = new Vector3f(), ListenerType? listenerType = null);
    public abstract Sound Play(IMap? map, IObject obj, ListenerType? listenerType = null);

    protected static void StandartPlay(SoundEmitter ISoundObject, Sound sound, IMap? map,
        Vector3f positionSound = new Vector3f(), ListenerType? listenerType = null)
    {
        bool noCurrentUnit = Camera.CurrentUnit is null;
        bool notCurrentMap = map is not null && map != Camera.CurrentUnit?.Map;

        if (noCurrentUnit || notCurrentMap)
            return;

        bool noListener = !ISoundObject.Listener.TryGetValue(Camera.CurrentUnit!, out var value);
        bool requireListenerMissing = ISoundObject.RequireListeners && listenerType is null;

        if (noListener && requireListenerMissing)
            return;


        ISoundObject.SelectMethodPlay(map, sound, positionSound, listenerType ?? value);
    }


    private void SelectMethodPlay(IMap? map, Sound sound, Vector3f positionSound, ListenerType listenerType)
    {
        switch (listenerType)
        {
            case ListenerType.Mono:
                StandartMonoPlay(map, sound);
                break;

            case ListenerType.Allowed:
                StandartAllowedPlay(map, sound, positionSound);
                break;

            default:
                StandartAllowedPlay(map, sound, positionSound);
                break;
        }
    }
    private void StandartMonoPlay(IMap? map, Sound sound)
    {
        sound.Position = BaseMonoPosition;
        sound.RelativeToListener = true;
        sound.Play();

        RegisterSound(map, sound);
    }
    private void StandartAllowedPlay(IMap? map, Sound sound, Vector3f positionSound)
    {
        sound.Position = new Vector3f(positionSound.X, positionSound.Z, positionSound.Y);
        sound.RelativeToListener = false;
        sound.Play();

        RegisterSound(map, sound);
    }

    #endregion

    #region Object Tracking
    public async Task TrackObjectPosition(IObject obj, Sound sound)
    {
        while (sound.Status != SoundStatus.Stopped)
        {
            if (obj.Map is null)
            {
                sound.Stop();
                return;
            }
            sound.Position = new Vector3f((float)obj.X.Axis, (float)obj.Z.Axis, (float)obj.Y.Axis);
            await Task.Delay(16);
        }
    }
    #endregion

    #region Listener Management
    public virtual void SubscribeListener(IObject listener)
    {
        Listener.TryAdd(listener, ListenerType.Allowed);
    }
    public virtual void UnsubscribeListener(IObject listener)
    {
        Listener.TryRemove(listener, out _);
    }
    public virtual void SubscribeMonoListener(IObject listener)
    {
        Listener.TryAdd(listener, ListenerType.Mono);
    }
    public virtual void UnsubscribeMonoListener(IObject listener)
    {
        Listener.TryRemove(listener, out _);
    }
    #endregion


    #region Cleanup Stopped Sounds Methods
    public virtual void CleanupStoppedSounds()
    {
        foreach (var kvp in ActiveSounds)
        {
            UpdateMapSounds(kvp.Key, kvp.Value);
        }

        CleanupExcludedSounds();
        oldOwnerMap = Camera.CurrentUnit?.Map;
    }

    private void UpdateMapSounds(IMap map, List<Sound> sounds)
    {
        if (map.ActiveAnchors.Count == 0)
        {
            PauseAllPlayingSounds(sounds);
            return;
        }

        bool mapChanged = oldOwnerMap != Camera.CurrentUnit?.Map;
        bool isCurrentMap = Camera.CurrentUnit?.Map == map;
        lock (_lock)
        {
            for (int i = sounds.Count - 1; i >= 0; i--)
            {
                var s = sounds[i];

                if (IsStoppedOrInvalid(s))
                {
                    sounds.RemoveAt(i);
                    continue;
                }

                if (!mapChanged)
                    continue;

                UpdateSoundForMap(s, isCurrentMap);
            }
        }
    }
    private void PauseAllPlayingSounds(List<Sound> sounds)
    {
        foreach (var s in sounds)
        {
            if (s.Status == SoundStatus.Playing)
                s.Pause();
        }
    }

    private void UpdateSoundForMap(Sound s, bool isCurrentMap)
    {
        s.Volume = isCurrentMap ? Sound.Volume : 0;

        if (isCurrentMap && s.Status == SoundStatus.Paused)
            s.Play();
    }
    private bool IsStoppedOrInvalid(Sound s)
    {
        return s.Status == SoundStatus.Stopped || s.CPointer == IntPtr.Zero;
    }
    private void CleanupExcludedSounds()
    {
        _excludedSounds.RemoveAll(IsStoppedOrInvalid);
    }
    #endregion

    #region Sound Registration
    public void RegisterSound(IMap? map, Sound sound)
    {
        lock (_lock)
        {
            if(map is null && !_excludedSounds.Contains(sound))
            {
                _excludedSounds.Add(sound);
                return;
            }


            if (!ActiveSounds.TryGetValue(map, out var list))
            {
                list = new List<Sound>();
                ActiveSounds[map] = list;
            }
            list.Add(sound);
        }
    }
    #endregion
}