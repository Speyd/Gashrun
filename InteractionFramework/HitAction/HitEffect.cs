using InteractionFramework.VisualImpact.Data;
using InteractionFramework.HitAction.DrawableBatch;


namespace InteractionFramework.HitAction;
public class HitEffect
{
    public VisualImpactData? VisualImpact { get; set; } = null;
    public HitDrawableBatch? DrawableBatch { get; set; } = new();


    public HitEffect(VisualImpactData? visualImpact, HitDrawableBatch? hitDrawableBatch)
    {
        VisualImpact = visualImpact;
        DrawableBatch = hitDrawableBatch;
    }
}
