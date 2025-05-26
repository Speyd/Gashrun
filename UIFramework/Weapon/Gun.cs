using UIFramework.Animation;
using UIFramework.Weapon.BulletMagazine;
using InteractionFramework.Audio;
using SFML.Audio;
using ObjectFramework;
using SFML.System;


namespace UIFramework.Weapon;
public class Gun
{
    public ProtoRender.Object.IUnit Owner { get; set; }
    public UIAnimation Animation { get; set; }
    public Magazine Magazine { get; set; }
    public SoundEmitter? Sound { get; set; } = null;

    public Gun(ProtoRender.Object.IUnit owner, UIAnimation animation, Magazine magazine, ControlLib.ButtonBinding bottomBinding)
    {
        Owner = owner;
        Animation = animation;

        bottomBinding.ExecutableFunction = ShotAsync;
        Animation.BottomBinding = bottomBinding;

        Magazine = magazine;
        Magazine.UIText.Owner = owner;
    }

    public async Task ShotAsync()
    {
        bool hasAmmo = await Magazine.UseAmmoAsync(Owner);

        if (hasAmmo && Sound is not null)
        {
            Sound.Play(new SFML.System.Vector3f((float)Owner.X.Axis, (float)Owner.Y.Axis, (float)Owner.Z.Axis));
        }
        Animation.IsAnimation = hasAmmo;
    }
    public void Shot()
    {
        bool hasAmmo = Magazine.UseAmmo(Owner);
        if (hasAmmo && Sound is not null)
        {
            Sound.Play(new SFML.System.Vector3f((float)Owner.X.Axis, (float)Owner.Y.Axis, (float)Owner.Z.Axis));
        }
        Animation.IsAnimation = hasAmmo;
    }
}
