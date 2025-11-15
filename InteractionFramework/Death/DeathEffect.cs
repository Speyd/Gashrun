using AnimationLib.Core.Elements;
using System.Diagnostics;


namespace InteractionFramework.Death;
public class DeathEffect
{
    public Frame Animation { get; set; }

    public long LifetimeMilliseconds = 0;
    public Stopwatch Stopwatch = new();
    public DeathPhase DeathPhase { get; set; } = DeathPhase.Animating;
    public bool LastFrame { get; set; } = false;

    public DeathEffect(Frame animation, DeathPhase deathPhas, long lifetimeMilliseconds)
    {
        Animation = animation;
        DeathPhase = deathPhas;
        LifetimeMilliseconds = lifetimeMilliseconds;
    }
    public DeathEffect(Frame animation)
    {
        Animation = animation;
    }
    public DeathEffect()
    {
        Animation = new Frame();
    }

}
