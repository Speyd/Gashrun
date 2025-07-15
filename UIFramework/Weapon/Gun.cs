using UIFramework.Animation;
using UIFramework.Weapon.BulletMagazine;
using ControlLib.Buttons;
using InteractionFramework.Audio.SoundType;


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
    public SoundEmitter? Sound { get; set; } = null;

    public Gun(ProtoRender.Object.IUnit owner, UIAnimation animation, Magazine magazine, ButtonBinding bottomBinding)
    {
        Animation = animation;

        bottomBinding.ExecutableFunction = Shot;
        Animation.BottomBinding = bottomBinding;

        Magazine = magazine;
        Owner = owner;
    }
    public void Shot()
    {
        bool hasAmmo = Magazine.UseAmmo();
        if (hasAmmo && Sound is not null && Owner?.Map is not null)
        {
            Sound.Play(Owner.Map, new SFML.System.Vector3f((float)Owner.X.Axis, (float)Owner.Y.Axis, (float)Owner.Z.Axis));
        }
        Animation.IsAnimation = hasAmmo;
    }
}
