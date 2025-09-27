using ProtoRender.Map;
using SFML.Audio;
using SFML.System;


namespace InteractionFramework.Audio.SoundType;
public class SoundDynamic : SoundEmitter
{
    public SoundDynamic(string soundBufferPath)
       : base(soundBufferPath)
    {

    }

    public override Sound Play(IMap? map, Vector3f positionSound = new Vector3f(), ListenerType? listenerType = null)
    {   
        var sound = new Sound(Sound);

        StandartPlay(this, sound, map, positionSound, listenerType);

        return sound;
    }
}
