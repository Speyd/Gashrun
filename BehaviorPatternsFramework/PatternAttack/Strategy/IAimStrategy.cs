using BehaviorPatternsFramework.Behavior;

namespace BehaviorPatternsFramework.PatternAttack.Strategy;
public interface IAimStrategy
{
    float GetAimHorizontalAngle(AIContext context, InfoGun infoGun);
    float GetAimVerticalAngle(AIContext context, InfoGun infoGun);
}
