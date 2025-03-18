using AnimationLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextureLib;

namespace UIFramework;
public class Animation
{
    protected AnimationState AnimationState { get; init; }

    public bool _isAnimation = true;
    public bool IsAnimation
    {
        get => _isAnimation;
        set
        {
            _isAnimation = value;
            AnimationState.IsAnimation = value;
        }
    }
    private int _speedAnimation = 100;
    public int SpeedAnimation
    {
        get => _speedAnimation;
        set
        {
            _speedAnimation = value;
            AnimationState.Speed = value;
        }
    }

    public Animation(params string[] paths)
    {
        AnimationState = new AnimationState();
        var frames = TextureLib.ImageLoader.TexturesLoad(paths);
        if (frames is not null)
            this.AnimationState.AddFrames(frames);
    }

    public virtual void UpdateFrame(float angle = 0)
    {
        AnimationManager.DefiningDesiredSprite(AnimationState, angle);
    }
    public virtual void AddFrame(params string[] paths)
    {
        AnimationState.AddFrames(ImageLoader.TexturesLoad(paths));
    }
    public virtual void RemoveFrame(int index)
    {
        if(index < 0 || index >= AnimationState.AmountFrame) 
            return;

        var frame = AnimationState.GetFrame(index);
        if(frame is not null)
            AnimationState.RemoveFrame(frame);
    }
}
