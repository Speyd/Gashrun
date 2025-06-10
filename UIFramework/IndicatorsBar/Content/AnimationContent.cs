using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using AnimationLib;
using SFML.Graphics;
using UIFramework.Animation;

namespace UIFramework.IndicatorsBar.Content;
public class AnimationContent : AnimationState, IBarContent
{
    public AnimationContent(params string[] paths)
        : base(paths)
    { }

    public void UpdateContent(RectangleShape bar)
    {
        if (CurrentFrame is not null && IsAnimation == false)
            return;

        AnimationManager.DefiningDesiredSprite(this, 0);
        if (CurrentFrame?.Texture is null)
            return;

        UpdateTexture(bar, CurrentFrame.Texture);
    }
    public void UpdateTexture(RectangleShape bar, SFML.Graphics.Texture? texture)
    {
        if (texture is null)
            return;

        bar.Texture = texture;

        var size = texture.Size;
        bar.TextureRect = new IntRect(0, 0, (int)size.X, (int)size.Y);
    }
}
