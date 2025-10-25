using UIFramework.Animation;
using UIFramework.Weapon.BulletMagazine;
using ControlLib.Buttons;
using InteractionFramework.Audio.SoundType;
using AnimationLib;
using ObjectFramework;


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
            Animation.Owner = value;
            Animation.BottomBinding = Animation.BottomBinding;
        }
    }

    public UIAnimation Animation { get; set; }
    public Magazine Magazine { get; set; }
    public SoundEmitter? SoundShot { get; set; } = null;

    public Gun(UIAnimation animation, Magazine magazine, ButtonBinding bottomBinding, ProtoRender.Object.IUnit? owner = null)
    {
        Animation = animation;

        bottomBinding.ExecutableFunction = Shot;
        Animation.BottomBinding = bottomBinding;

        Magazine = magazine;
        if(owner is not null) 
            Owner = owner;
    }
    public Gun(Gun gun, ButtonBinding? bottomBinding = null, ProtoRender.Object.IUnit? owner = null)
        :this(
             new (gun.Animation),
             new(gun.Magazine),
             bottomBinding ?? new(gun.Animation.BottomBinding?.Buttons, null, gun.Animation.BottomBinding?.WaitingTimeMilliseconds ?? 0),
             owner ?? gun.Owner)
    {
    }

    public void Shot()
    {

        if(Camera.CurrentUnit != Owner && Magazine.IsReload == true)
            Magazine.Reload();

        bool hasAmmo = Magazine.UseAmmo();

        if (hasAmmo && SoundShot is not null && Owner?.Map is not null)
        {
            SoundShot.Play(Owner.Map, new SFML.System.Vector3f((float)Owner.X.Axis, (float)Owner.Y.Axis, (float)Owner.Z.Axis));
        }
        Animation.AnimationMode = hasAmmo? AnimationMode.Animated : AnimationMode.Static;
    }
}
