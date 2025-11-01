using BehaviorPatternsFramework.Behavior;

namespace BehaviorPatternsFramework.PatternAttack.Strategy;
public interface IAimStrategy
{
    float GetAimAngle(AIContext context, InfoGun infoGun);
}
