
using AnimationLib;

namespace UIFramework.Weapon.Enum;
public interface IGunAnimationPolicy
{
    Task PlayAnimationAsync(IAnimatable? animatable, string animationName);
    bool IsAnimationBlocking(IAnimatable? animatable, string animationName);
}
