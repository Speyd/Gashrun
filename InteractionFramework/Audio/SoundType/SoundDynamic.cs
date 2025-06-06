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

    public override void Play(IMap map, Vector3f positionSound = new Vector3f())
    {   
        var sound = new Sound(Sound);

        StandartPlay(this, sound, map, positionSound);

        RegisterSound(sound);
    }
}
