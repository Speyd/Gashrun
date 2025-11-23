using ControlLib.Buttons;

namespace BehaviorPatternsFramework.PatternAttack;
public class InfoGun
{
    public ButtonBinding AttackBind { get; set; }
    public Func<bool> IsReloading { get; set; }
    public Func<float> GetHorizontalBulletSpeed { get; set; } = () => 0;
    public Func<float> GetVerticalBulletSpeed { get; set; } = () => 0;

    public int SleepBulletHandler { get; set; } = 1;

    public InfoGun(ButtonBinding attackBind, Func<bool> isReloading, int sleepBulletHandler)
    {
        AttackBind = attackBind;
        IsReloading = isReloading;
        SleepBulletHandler = sleepBulletHandler;
    }
}
