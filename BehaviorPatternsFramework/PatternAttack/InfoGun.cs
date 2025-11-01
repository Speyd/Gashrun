using ControlLib.Buttons;

namespace BehaviorPatternsFramework.PatternAttack;
public class InfoGun
{
    public ButtonBinding AttackBind { get; set; }
    public Func<bool> IsReloading { get; set; }
    public Func<float> GetBulletSpeed { get; set; }
    public int SleepBulletHandler { get; set; }

    public InfoGun(ButtonBinding attackBind, Func<bool> isReloading, Func<float> getbulletSpeed, int sleepBulletHandler)
    {
        AttackBind = attackBind;
        IsReloading = isReloading;
        GetBulletSpeed = getbulletSpeed;
        SleepBulletHandler = sleepBulletHandler;
    }
}
