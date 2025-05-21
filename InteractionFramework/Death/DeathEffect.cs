using AnimationLib;
using System.Diagnostics;


namespace InteractionFramework.Death;
public class DeathEffect
{
    public AnimationState Animation { get; set; }

    public long LifetimeMilliseconds = 0;
    public Stopwatch Stopwatch = new();
    public DeathPhase DeathPhase { get; set; } = DeathPhase.Animating;
    public bool LastFrame { get; internal set; } = false;

    public DeathEffect(AnimationState animation, DeathPhase deathPhas, long lifetimeMilliseconds)
    {
        Animation = animation;
        DeathPhase = deathPhas;
        LifetimeMilliseconds = lifetimeMilliseconds;
    }
    public DeathEffect(AnimationState animation)
    {
        Animation = animation;
    }
    public DeathEffect()
    {
        Animation = new AnimationState();
    }

}
