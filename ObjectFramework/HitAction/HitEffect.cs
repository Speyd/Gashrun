using ObjectFramework.HitAction.DrawableBatch;
using ObjectFramework.VisualImpact.Data;
using ObstacleLib.SpriteLib;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectFramework.HitAction;
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
