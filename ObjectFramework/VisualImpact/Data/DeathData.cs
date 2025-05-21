using ObjectFramework.Death;
using ObstacleLib.SpriteLib;
using ProtoRender.Object;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectFramework.VisualImpact.Data;
public class DeathData : IBeyoundData
{
    public SpriteObstacle Sprite;
    public DeathEffect DeathEffect;

    public int Id {  get; set; } 
    public IUnit? Owner { get; set; }

    public DeathData(SpriteObstacle sprite, DeathEffect deathEffect)
    {
        Sprite = sprite;
        DeathEffect = deathEffect;

        Sprite.Animation = DeathEffect.Animation;
        Sprite.Animation.Index = 0;
    }
    public DeathData(DeathData deathData)
    {
        Sprite = deathData.Sprite;
        DeathEffect = deathData.DeathEffect;

        Sprite.Animation = DeathEffect.Animation;
        Sprite.Animation.Index = 0;
    }


    public void Render(IUnit observer)
    {
        Sprite?.Render(IBeyoundData.jammerResult, observer);
    }
    public void UpdateBeforeAdd(double x, double y, double z)
    {
        DeathEffect.Stopwatch.Restart();
    }
    public void UpdateBeforeAdd()
    {
        DeathEffect.Stopwatch.Restart();
    }

    private void ActionBeforeLastFrame()
    {
        switch (DeathEffect.DeathPhase)
        {
            case DeathPhase.Animating:
                break;
            case DeathPhase.AfterAnimation:
                break;
            case DeathPhase.FrozenFinalFrame:
                Sprite.Animation = new AnimationLib.AnimationState(Sprite.Animation.GetFrame(Sprite.Animation.AmountFrame - 1));
                Sprite.Animation.IsAnimation = false;                                                                                                          //            Sprite.Animation.IsAnimation = false;
                break;
        }
    }
    private void ActionAfterLastFrame()
    {
        switch (DeathEffect.DeathPhase)
        {
            case DeathPhase.Animating:
                BeyondRenderManager.Remove(Id);
                break;
            case DeathPhase.AfterAnimation:
                if (DeathEffect.Stopwatch.ElapsedMilliseconds >= DeathEffect.LifetimeMilliseconds)
                    BeyondRenderManager.Remove(Id);
                break;
            case DeathPhase.FrozenFinalFrame:
                if (DeathEffect.Stopwatch.ElapsedMilliseconds >= DeathEffect.LifetimeMilliseconds)
                    BeyondRenderManager.Remove(Id);
                break;

        }
    }
    public void UpdateCheckRemove(ConcurrentDictionary<int, IBeyoundData> toRemove)
    {
        if (Sprite.Animation.Index == Sprite.Animation.AmountFrame - 1)
        {
            DeathEffect.LastFrame = true;
            ActionBeforeLastFrame();
        }

        if (DeathEffect.LastFrame)
            ActionAfterLastFrame();
    }

    public IBeyoundData GetCopy() => new DeathData(this);
}

