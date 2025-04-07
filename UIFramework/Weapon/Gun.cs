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
using UIFramework.Weapon.Patron;
using EntityLib;


namespace UIFramework.Weapon;
public class Gun
{
    public Entity Owner { get; set; }
    public UIAnimation Animation { get; set; }
    public Magazine Magazine { get; set; }

    public Gun(Entity owner, UIAnimation animation, Magazine magazine, BottomBinding bottomBinding)
    {
        Owner = owner;
        Animation = animation;

        bottomBinding.ExecutableFunction = Shot;
        Animation.BottomBinding = bottomBinding;

        Magazine = magazine;
    }

    public void Shot()
    {
        bool hasAmmo = Magazine.UseAmmo(Owner);
        Animation.IsAnimation = hasAmmo;
    }
}
