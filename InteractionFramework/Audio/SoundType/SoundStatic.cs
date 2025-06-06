using ScreenLib;
using SFML.Audio;
using SFML.System;
using System.Numerics;
using ProtoRender.Map;


namespace InteractionFramework.Audio.SoundType;
public class SoundStatic : SoundEmitter
{
    public SoundStatic(string soundBufferPath)
        : base(soundBufferPath)
    {

    }

    public override void Play(IMap map, Vector3f positionSound = new Vector3f())
    {
        StandartPlay(this, Sound, map, positionSound);
    }
}
