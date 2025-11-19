using AnimationLib.Core;
using UIFramework.Weapon.Enum;
using AnimationLib;

namespace UIFramework.Weapon.AnimationPolicy;
public class NoWaitAnimationPolicy : IGunAnimationPolicy
{
    public bool IsAnimationBlocking(IAnimatable? animatable, string animationName)
    {
       return false;
    }

    public Task PlayAnimationAsync(IAnimatable? animatable, string animationName)
    {
        if(animatable is null)
            return Task.CompletedTask;


        var animation = animatable.Animation.GetAnimation(animationName);
        var animationEntry = animatable.Animation.GetAnimationEntry(animationName);

        if (animation is null || animationEntry is null)
            return Task.CompletedTask;


        if (!animationEntry.IsPlaying)
        {
            animation.IsFinishMode = false;
            animation.SetCurrentElement(0);
            animatable.Animation.Play(animationName);
        }
        else if (animationEntry.IsPlaying && animation.IsFinishMode)
        {
            animatable.Animation.Stop(animationName);
        }
        else if (animationEntry.IsPlaying)
        {
            animation.SetCurrentElement(0);
        }

        return Task.CompletedTask;
    }

}

