using InteractionFramework.VisualImpact.Data;
using InteractionFramework.HitAction.DrawableBatch;
using SFML.Audio;
using SFML.System;
using TextureLib.Loader;
using InteractionFramework.Audio;


namespace InteractionFramework.HitAction;
public class HitEffect
{
    public VisualImpactData? VisualImpact { get; set; } = null;
    public HitDrawableBatch? DrawableBatch { get; set; } = null;
    public SoundEmitter? SoundHit { get; set; } = null;

    public HitEffect(VisualImpactData? visualImpact, HitDrawableBatch? hitDrawableBatch, SoundEmitter? soundHit)
    {
        VisualImpact = visualImpact;
        DrawableBatch = hitDrawableBatch;
        SoundHit = soundHit;  
    }
}
