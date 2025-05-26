using ObstacleLib.SpriteLib;
using ProtoRender.Object;
using System.Collections.Concurrent;
using InteractionFramework.Death;
using InteractionFramework.Audio;


namespace InteractionFramework.VisualImpact.Data;
public class DeathData : IBeyoundData
{
    public SpriteObstacle Sprite;
    public DeathEffect DeathEffect;
    public SoundEmitter? SoundEmitter;

    public int Id {  get; set; } 
    public IUnit? Owner { get; set; }

    public DeathData(SpriteObstacle sprite, DeathEffect deathEffect, SoundEmitter? soundEmitter = null)
    {
        Sprite = sprite;
        DeathEffect = deathEffect;
        SoundEmitter = soundEmitter;

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
        SoundEmitter?.Play(new SFML.System.Vector3f((float)x, (float)y, (float)z));
        DeathEffect.Stopwatch.Restart();
    }
    public void UpdateBeforeAdd()
    {
        SoundEmitter?.Play(new SFML.System.Vector3f((float)Sprite.X.Axis, (float)Sprite.Y.Axis, (float)Sprite.Z.Axis));
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

