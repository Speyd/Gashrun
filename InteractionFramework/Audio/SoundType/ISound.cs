using ProtoRender.Map;
using ProtoRender.Object;
using ScreenLib;
using SFML.Audio;
using SFML.System;
using System.Collections.Concurrent;


namespace InteractionFramework.Audio.SoundType;
public interface ISound
{
    ConcurrentDictionary<IMap, List<Sound>> ActiveSounds { init; }
    /// <summary>
    /// Coefficient for sound attenuation based on the height (Z-axis difference) between the sound source and the listener.
    /// Higher values reduce the effect of height difference on the sound volume.
    /// Default value is 1 (no attenuation).
    /// </summary>
    float HeightAttenuation {  get; set; }

    public ConcurrentDictionary<IObject, ListenerType> Listener { init; }

    void SubscribeListener(IObject listener);
    void UnsubscribeListener(IObject listener);

    void SubscribeMonoListener(IObject listener);
    void UnsubscribeMonoListener(IObject listener);


    void CleanupStoppedSounds();
    Sound Play(IMap? map, Vector3f positionSound = new Vector3f(), ListenerType? listenerType = null);

}
