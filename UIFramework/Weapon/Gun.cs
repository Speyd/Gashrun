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


namespace UIFramework.Weapon;
public class Gun
{
    public UIAnimation Animation { get; set; }
    public Magazine Magazine { get; set; }
    BottomBinding shoot;
    public Gun(UIAnimation animation, Magazine magazine, BottomBinding shoot, BottomBinding bottomBinding)
    {
        Animation = animation;

        bottomBinding.ExecutableFunction = Shoot;
        Animation.BottomBinding = bottomBinding;

        Magazine = magazine;
        this.shoot = shoot;
    }

    public void Shoot()
    {
        if (Animation.BottomBinding is null)
            return;

        Magazine.UseAmmo();
        if (Magazine.AmmoInGun == 0 && Magazine.CurrentAmmoInMagazine == 0 || Magazine.IsReload)
        {
            shoot.IsFreeze = true;
            Animation.IsAnimation = false;
        }
        else if (shoot.IsFreeze == true && (Magazine.AmmoInGun > 0 || Magazine.CurrentAmmoInMagazine > 0))
        {
            shoot.IsFreeze = false;
            Animation.IsAnimation = true;
        }

        shoot.Listen();
    }
}
