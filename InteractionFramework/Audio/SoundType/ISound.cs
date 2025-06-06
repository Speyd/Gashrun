using ProtoRender.Map;
using ProtoRender.Object;
using ScreenLib;
using SFML.Audio;
using SFML.System;
using System.Collections.Concurrent;


namespace InteractionFramework.Audio.SoundType;
public interface ISound
{
    List<Sound> ActiveSounds { init; }

    ConcurrentDictionary<IObject, byte> AllowedListeners { init; }
    void SubscribeListener(IObject listener);
    void UnsubscribeListener(IObject listener);


    ConcurrentDictionary<IObject, byte> MonoListeners { init; }
    void SubscribeMonoListener(IObject listener);
    void UnsubscribeMonoListener(IObject listener);


    void CleanupStoppedSounds();
    void Play(IMap map, Vector3f positionSound = new Vector3f());
}
