using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimationLib;
using SFML.Graphics;

namespace UIFramework.IndicatorsBar.Content;
public class AnimationContent : Animation, IBarContent
{
    public AnimationContent(params string[] paths)
        : base(paths)
    { }

    public void UpdateContent(RectangleShape bar)
    {
        if (AnimationState.CurrentFrame is not null && AnimationState.IsAnimation == false)
            return;

        UpdateFrame();
        if(AnimationState.CurrentFrame?.Texture is null)
            return;

        UpdateTexture(bar, AnimationState.CurrentFrame.Texture);
    }
    public void UpdateTexture(RectangleShape bar, Texture texture)
    {
        bar.Texture = texture;

        var size = texture.Size;
        bar.TextureRect = new IntRect(0, 0, (int)size.X, (int)size.Y);
    }
}
