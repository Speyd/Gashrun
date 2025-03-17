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
using UIFramework.Sights;


namespace UIFramework;
public class Gun : AnimationObject
{
    Sight? Sights { get; set; }
    public Gun(BottomBinding bottomBinding, Vector2f position, params string[] paths)
        : base(bottomBinding)
    {
        this.PositionOnScreen = position;

        IsAnimation = true;
        this.AnimationState.AddFrames(ImageLoader.TexturesLoad(paths));
        this.AnimationState = new AnimationState(paths);
    }
    public Gun(BottomBinding bottomBinding, params string[] paths)
        : this(bottomBinding, new Vector2f(), paths)
    {
        SetPositionCenter();
    }
    public Gun(BottomBinding bottomBinding, Vector2f position, string path)
        :base(bottomBinding)
    {
        this.PositionOnScreen = position;

        var frames = TextureLib.ImageLoader.TextureLoad(path);
        if(frames is not null) 
            this.AnimationState.AddFrames(frames);
    }
    public Gun(BottomBinding bottomBinding, string path)
        : this(bottomBinding, new Vector2f(), path)
    {
        SetPositionCenter();
        Console.WriteLine(AnimationState.AmountFrame);
    }


    public override void UpdateInfo()
    {
        if (AnimationState.Index == AnimationState.AmountFrame - 1)
        {
            UpdateFrame();
            IsAnimatingOnPress = false;
        }
        if (AnimationState.IsAnimation && BottomBinding.IsPress == true || IsAnimatingOnPress == true)
        {
            IsAnimatingOnPress = true;
            UpdateFrame();
        }
        else if (AnimationState.IsAnimation && BottomBinding.IsPress == false)
        {
            AnimationState.IsAnimation = false;
            UpdateFrame();
            AnimationState.IsAnimation = true;
        }


        if (AnimationState.CurrentFrame is not null)
            RenderSprite.Texture = AnimationState.CurrentFrame.Texture;
    }
}
