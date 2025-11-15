using UIFramework.Animation;
using UIFramework.Weapon.BulletMagazine;
using ControlLib.Buttons;
using InteractionFramework.Audio.SoundType;
using AnimationLib;
using ObjectFramework;
using ProtoRender.Object;
using AnimationLib.Core;


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
            Magazine.Owner = value;
            UIAnimation.Owner = value;
            UIAnimation.BottomBinding = UIAnimation.BottomBinding;
        }
    }

    public UIAnimation UIAnimation { get; set; }
    public string? SpriteAnimationName { get; set; }


    public Magazine Magazine { get; set; }
    public SoundEmitter? SoundShot { get; set; } = null;
    public ButtonBinding? ShootBinding 
    {
        get => UIAnimation.BottomBinding;
        set => UIAnimation.BottomBinding = value;
    }

    public Predicate<IUnit> Predicate { get; set; }
    public bool IsRunning { get; private set; } = false;

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


    public async Task WaitAnimation(Animator? animator)
    {
        if (Owner is null || animator is null || SpriteAnimationName is null)
            return;

        var animation = animator.GetAnimation(SpriteAnimationName);
        if(animation is null)
            return;

        animation.IsFinishMode = false;
        animator.Play(SpriteAnimationName);
        while (!animation.IsFinishMode)
        {
            await Task.Yield();
        }

        animator.Stop(SpriteAnimationName);
    }


    public async Task ShotAsync()
    {
        if (Owner is null || IsRunning || ShootBinding?.IsWaiting == false)
            return;

        IsRunning = true;
        if (TryGetAnimator(out var animator))
            await WaitAnimation(animator);

        if (Camera.CurrentUnit != Owner && Magazine.IsReload == true)
            Magazine.Reload();

        bool hasAmmo = Magazine.UseAmmo();

        if (hasAmmo && SoundShot is not null && Owner?.Map is not null)
        {
            SoundShot.Play(Owner.Map, new SFML.System.Vector3f((float)Owner.X.Axis, (float)Owner.Y.Axis, (float)Owner.Z.Axis));
        }

        UIAnimation.IsFinishMode = !hasAmmo;
        IsRunning = false;
    }

    public bool TryGetAnimator(out Animator? animator)
    {
        if (Owner is IAnimatable anim)
        {
            animator = anim.Animation;
            return animator != null;
        }

        animator = null;
        return false;
    }


}
