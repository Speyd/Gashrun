using ScreenLib;
using SFML.Audio;
using SFML.System;
using System.Numerics;
using ProtoRender.Map;
using ProtoRender.Object;


namespace InteractionFramework.Audio.SoundType;
public class SoundStatic : SoundEmitter
{
    public SoundStatic(string soundBufferPath)
        : base(soundBufferPath)
    {}

    public override Sound Play(IMap? map, Vector3f positionSound = new Vector3f(), ListenerType? listenerType = null)
    {
        StandartPlay(this, Sound, map, positionSound, listenerType);

        return Sound;
    }

    public override Sound Play(IMap? map, IObject obj, ListenerType? listenerType = null)
    {
        StandartPlay(this, Sound, map, new Vector3f((float)obj.X.Axis, (float)obj.Y.Axis, (float)obj.Z.Axis), listenerType);

        _ = TrackObjectPosition(obj, Sound);

        return Sound;
    }
}
