using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimationLib;
using SFML.System;
using TextureLib;
using ObstacleLib.SpriteLib;
using ObstacleLib.SpriteLib.Add;
using ControlLib;
using ScreenLib;
using UIFramework.Animation;
using ProtoRender.WindowInterface;
using DrawLib;
using UIFramework.Weapon.BulletMagazine;

namespace UIFramework.Weapon;
public class Gun
{
    public ProtoRender.Object.IUnit Owner { get; set; }
    public UIAnimation Animation { get; set; }
    public Magazine Magazine { get; set; }

    public Gun(ProtoRender.Object.IUnit owner, UIAnimation animation, Magazine magazine, ControlLib.BottomBinding bottomBinding)
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
        Animation.IsAnimation = hasAmmo;
    }
    public void Shot()
    {
        bool hasAmmo = Magazine.UseAmmo(Owner);
        Animation.IsAnimation = hasAmmo;
    }
}
