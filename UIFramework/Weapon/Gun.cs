using UIFramework.Animation;
using UIFramework.Weapon.BulletMagazine;
using ControlLib.Buttons;
using InteractionFramework.Audio.SoundType;
using AnimationLib;
using ObjectFramework;
using ProtoRender.Object;
using AnimationLib.Core;
using UIFramework.Weapon.Enum;
using UIFramework.Weapon.AnimationPolicy;


namespace UIFramework.Weapon;
public class Gun
{
    private ProtoRender.Object.IUnit? _owner = null;
    public ProtoRender.Object.IUnit? Owner 
    {
        get => _owner;
        set
        {
            _owner = value;
            animatable = value as IAnimatable;
            Magazine.Owner = value;
            UIAnimation.Owner = value;
            UIAnimation.BottomBinding = UIAnimation.BottomBinding;
        }
    }
    private IAnimatable? animatable = null;

    public IGunAnimationPolicy AnimationPolicy { get; set; } = new NoWaitAnimationPolicy();
    public UIAnimation UIAnimation { get; set; }
    public string SpriteAnimationName { get; set; } = string.Empty;


    public Magazine Magazine { get; set; }
    public SoundEmitter? SoundShot { get; set; } = null;
    public ButtonBinding? ShootBinding 
    {
        get => UIAnimation.BottomBinding;
        set => UIAnimation.BottomBinding = value;
    }



    public Gun(UIAnimation animation, Magazine magazine, ButtonBinding bottomBinding, ProtoRender.Object.IUnit? owner = null)
    {
        UIAnimation = animation;

        bottomBinding.ExecutableFunction = ShotAsync;
        UIAnimation.BottomBinding = bottomBinding;

        Magazine = magazine;
        if(owner is not null) 
            Owner = owner;
    }
    public Gun(Gun gun, ButtonBinding? bottomBinding = null, ProtoRender.Object.IUnit? owner = null)
        :this(
             new (gun.UIAnimation),
             new(gun.Magazine),
             bottomBinding ?? new(gun.UIAnimation.BottomBinding?.Buttons, null, gun.UIAnimation.BottomBinding?.WaitingTimeMilliseconds ?? 0),
             owner ?? gun.Owner)
    {
    }

   
    public async Task ShotAsync()
    {
        if (!CanShoot())
            return;

        await AnimationPolicy.PlayAnimationAsync(animatable, SpriteAnimationName);

        if (Camera.CurrentUnit != Owner && Magazine.IsReload == true)
            Magazine.Reload();

        bool hasAmmo = Magazine.UseAmmo();
        if (hasAmmo && SoundShot is not null && Owner?.Map is not null)
        {
            SoundShot.Play(Owner.Map, new SFML.System.Vector3f((float)Owner.X.Axis, (float)Owner.Y.Axis, (float)Owner.Z.Axis));
        }

        UIAnimation.IsFinishMode = !hasAmmo;
    }

    private bool CanShoot()
    {
        return Owner is not null
            && ShootBinding?.IsWaiting != false
            && !AnimationPolicy.IsAnimationBlocking(animatable, SpriteAnimationName);
    }
}
