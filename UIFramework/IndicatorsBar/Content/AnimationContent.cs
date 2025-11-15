using AnimationLib.Core.Elements;
using SFML.Graphics;
using TextureLib.Loader;

namespace UIFramework.IndicatorsBar.Content;
public class AnimationContent : Frame, IBarContent
{
    public AnimationContent(params string[] paths)
        : base(null, paths)
    { this.SpeedAnimation = 20; }
    public AnimationContent(ImageLoadOptions? options = null, params string[] paths)
        : base(options, paths)
    { this.SpeedAnimation = 20; }

    public AnimationContent(Frame state, ImageLoadOptions? options = null)
       : base(state, options)
    {
        this.SpeedAnimation = state.SpeedAnimation;
    }

    public void UpdateContent(RectangleShape bar)
    {
        if (CurrentElement is not null)
            return;

        Update();
        if (CurrentElement?.Texture is null)
            return;

        UpdateTexture(bar, CurrentElement.Texture);
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
