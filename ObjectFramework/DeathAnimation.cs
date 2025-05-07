using AnimationLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ObjectFramework;
public class DeathAnimation
{
    public AnimationState Animation {  get; set; }
    public bool IsExistsAfterDeath = false;
    public TimeSpan Lifetime = new TimeSpan(0);
    public DateTime CreationTime;

    public DeathAnimation(AnimationState animation, bool isExistsAfterDeath, TimeSpan lifetime)
    {
        Animation = animation;
        IsExistsAfterDeath = isExistsAfterDeath;
        Lifetime = lifetime;
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
