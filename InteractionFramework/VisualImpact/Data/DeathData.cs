using ObstacleLib.SpriteLib;
using ProtoRender.Object;
using System.Collections.Concurrent;
using InteractionFramework.Death;
using InteractionFramework.Audio.SoundType;
using ProtoRender.Map;
using TextureLib.Textures;
using AnimationLib.Core;
using AnimationLib.Core.Elements;
using AnimationLib.Enum;
using SFML.Graphics;


namespace InteractionFramework.VisualImpact.Data;
public class DeathData : IBeyoundData
{
    public SpriteObstacle Sprite;
    public DeathEffect DeathEffect;
    public SoundEmitter? SoundEmitter;

    public int Id {  get; set; } 
    public IUnit? Owner { get; set; }
    public IMap? Map { get; set; }

    public DeathData(SpriteObstacle sprite, DeathEffect deathEffect, SoundEmitter? soundEmitter = null)
    {
        Map = null;
        Sprite = sprite;
        DeathEffect = deathEffect;
        SoundEmitter = soundEmitter;

        Sprite.Animation = new Animator(DeathEffect.Animation);
    }
    public DeathData(DeathData deathData)
    {
        Map = deathData.Map;
        Sprite = deathData.Sprite;
        DeathEffect = deathData.DeathEffect;
        SoundEmitter = deathData.SoundEmitter;

        Sprite.Animation = new Animator(DeathEffect.Animation);
    }


    public void Render(IUnit observer)
    {
        Sprite?.Render(IBeyoundData.jammerResult, observer);
    }
    public void UpdateBeforeAdd(double x, double y, double z)
    {
        if (Map is null)
            return;

        SoundEmitter?.Play(Map, new SFML.System.Vector3f((float)x, (float)y, (float)z));
        DeathEffect.Stopwatch.Restart();
    }
    public void UpdateBeforeAdd()
    {
        UpdateBeforeAdd((float)Sprite.X.Axis, (float)Sprite.Y.Axis, (float)Sprite.Z.Axis);
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
                if (Sprite.Animation.CurrentFrame is null)
                    return;

                Sprite.Animation.CurrentFrame.PlayMode = PlayMode.Pause;                                                                       
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
        if (Sprite.Animation.CurrentFrame?.Index == Sprite.Animation.CurrentFrame?.CountElements - 1)
        {
            DeathEffect.LastFrame = true;
            ActionBeforeLastFrame();
        }

        if (DeathEffect.LastFrame)
            ActionAfterLastFrame();
    }

    public IBeyoundData GetCopy() => new DeathData(this);
}

