using HitBoxLib.Segment.SignsTypeSide;
using InteractionFramework.Audio.SoundSide;
using System.Numerics;


namespace InteractionFramework.Audio;
public static class AudioMath
{
    public static SoundHorizontalSide GetRelativeSoundHorizontalSide(Vector3 listenerPos, Vector3 sourcePos, Vector3? up = null)
    {
        Vector3 vector = up ?? Vector3.UnitY;

        Vector3 toSource = Vector3.Normalize(sourcePos - listenerPos);
        float side = Vector3.Dot(toSource, vector);

        if (side > 0.01f) return SoundHorizontalSide.Right;
        if (side < -0.01f) return SoundHorizontalSide.Left;
        return SoundHorizontalSide.Center;
    }
    public static SoundVerticalSide GetRelativeSoundVerticalSide(Vector3 listenerPos, Vector3 sourcePos)
    {
        float heightDiff = sourcePos.Z - listenerPos.Z;

        if (heightDiff < 0)
            return SoundVerticalSide.Above;

        if (heightDiff > 0)
            return SoundVerticalSide.Below;

        return SoundVerticalSide.Level;
    }
}
