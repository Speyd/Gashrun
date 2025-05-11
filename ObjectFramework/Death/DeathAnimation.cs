using AnimationLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ObjectFramework.Death;
public class DeathAnimation
{
    public AnimationState Animation { get; set; }
    public bool IsExistsAfterDeath = false;

    public long LifetimeMilliseconds = 0;
    public Stopwatch Stopwatch = new();

    public DeathAnimation(AnimationState animation, bool isExistsAfterDeath, long lifetimeMilliseconds)
    {
        Animation = animation;
        IsExistsAfterDeath = isExistsAfterDeath;
        LifetimeMilliseconds = lifetimeMilliseconds;
    }
    public DeathAnimation(AnimationState animation)
    {
        Animation = animation;
    }
    public DeathAnimation()
    {
        Animation = new AnimationState();
    }

}
