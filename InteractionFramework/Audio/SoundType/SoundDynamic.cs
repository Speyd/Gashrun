using ProtoRender.Map;
using ProtoRender.Object;
using SFML.Audio;
using SFML.System;


namespace InteractionFramework.Audio.SoundType;
public class SoundDynamic : SoundEmitter
{
    public SoundDynamic(string soundBufferPath)
       : base(soundBufferPath)
    {}

    public override Sound Play(IMap? map, Vector3f positionSound = new Vector3f(), ListenerType? listenerType = null)
    {   
        var sound = new Sound(Sound);

        StandartPlay(this, sound, map, positionSound, listenerType);

        return sound;
    }

    public override Sound Play(IMap? map, IObject obj, ListenerType? listenerType = null)
    {
        var sound = new Sound(Sound);

        StandartPlay(this, sound, map, new Vector3f((float)obj.X.Axis, (float)obj.Y.Axis, (float)obj.Z.Axis), listenerType);

        _ = TrackObjectPosition(obj, sound);

        return sound;
    }
}
