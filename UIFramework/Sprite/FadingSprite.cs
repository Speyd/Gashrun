using ProtoRender.Object;
using ProtoRender.WindowInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIFramework.Sprite;
using UIFramework.Render;
using SFML.Graphics;
using UIFramework.Text.Fading;
using UIFramework.Text.Fading.FadingEnums;
using TextureLib.Textures;


namespace UIFramework.Texture;
public class FadingSprite : UISprite
{
    public FadingController Controller;
    public FadingSprite(SFML.Graphics.Sprite sprite, FadingType fasingType, FadingTextLife fadingTextLife, long fadingTimeMilliseconds, IUnit? owner)
        : base(sprite, owner)
    {
        Controller = new FadingController(fasingType, fadingTextLife, fadingTimeMilliseconds);
        Controller.OnAlphaChanged = SetAlpha;
        Controller.OnDispose = () => UIRender.RemoveFromPriority(Owner, RenderOrder, this);

        Controller.Restart();
    }
    public FadingSprite(SFML.Graphics.Sprite sprite, FadingController controller, IUnit? owner)
       : base(sprite, owner)
    {
        Controller = controller;
        Controller.OnAlphaChanged = SetAlpha;
        Controller.OnDispose = () => UIRender.RemoveFromPriority(Owner, RenderOrder, this);

        Controller.Restart();
    }
    public FadingSprite(TextureWrapper texture, FadingType fasingType, FadingTextLife fadingTextLife, long fadingTimeMilliseconds, IUnit? owner)
        : base(texture, owner)
    {
        Controller = new FadingController(fasingType, fadingTextLife, fadingTimeMilliseconds);
        Controller.OnAlphaChanged = SetAlpha;
        Controller.OnDispose = () => UIRender.RemoveFromPriority(Owner, RenderOrder, this);

        Controller.Restart();
    }
    public FadingSprite(TextureWrapper texture, FadingController controller, IUnit? owner)
       : base(texture, owner)
    {
        Controller = controller;
        Controller.OnAlphaChanged = SetAlpha;
        Controller.OnDispose = () => UIRender.RemoveFromPriority(Owner, RenderOrder, this);

        Controller.Restart();
    }

    private void SetAlpha(float normalizedAlpha)
    {
        byte alpha = (byte)Math.Clamp(normalizedAlpha * 255f, 0, 255);
        var fill = Sprite.Color;
        Sprite.Color = new SFML.Graphics.Color(fill.R, fill.G, fill.B, alpha);
    }

    public override void UpdateInfo()
    {
        Controller.Update();
    }

    public void SwapType() => Controller.SwapType();
    public void Restart() => Controller.Restart();

}
