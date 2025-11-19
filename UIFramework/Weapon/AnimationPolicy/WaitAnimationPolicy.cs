using AnimationLib.Core.Utils;
using AnimationLib.Core;
using UIFramework.Weapon.Enum;
using AnimationLib;

namespace UIFramework.Weapon.AnimationPolicy;
public class WaitAnimationPolicy : IGunAnimationPolicy
{
    public bool IsAnimationBlocking(IAnimatable? animatable, string animationName)
    {
        return animatable is not null && animatable.Animation.GetAnimationEntry(animationName)?.IsPlaying == true;
    }

    public async Task PlayAnimationAsync(IAnimatable? animatable, string animationName)
    {
        if (animatable is null)
            return;

        var animation = animatable.Animation.GetAnimation(animationName);
        var animationEntry = animatable.Animation.GetAnimationEntry(animationName);

        if (animation is null || animationEntry is null)
            return;

        animation.IsFinishMode = false;
        animatable.Animation.Play(animationName);

        while (!animation.IsFinishMode)
            await Task.Yield();

        animatable.Animation.Stop(animationName);
    }
}
