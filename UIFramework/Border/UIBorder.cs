using ControlLib.Buttons;
using ProtoRender.Object;
using ScreenLib;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextureLib.Loader;
using TextureLib.Loader.ImageProcessing;
using UIFramework.Animation;
using static System.Formats.Asn1.AsnWriter;

namespace UIFramework.Border;

public class UIBorder : UIAnimation, IBorder
{
    public float ExtraScaleX { get; set; } = 1.7f;
    public float ExtraScaleY { get; set; } = 1.4f;


    public UIBorder(ImageLoadOptions? options = null, bool loadAsync = true, ButtonBinding? bottomBinding = null, params string[] paths)
        : base(options, loadAsync, bottomBinding, paths)
    {}

    public Vector2f GetScaledOrigin(Vector2u baseOrigin, Vector2f targetScaleOrigin)
    {
        float scaledBaseOriginX = (baseOrigin.X / ExtraScaleX * targetScaleOrigin.X);
        float scaledBaseOriginY = (baseOrigin.Y / ExtraScaleY * targetScaleOrigin.Y);

        return new Vector2f(baseOrigin.X / 2 + scaledBaseOriginX, baseOrigin.Y / 2 + scaledBaseOriginY);
    }
    public Vector2f GetOriginNormalized(Vector2f size, Vector2f origin)
    {
        return new Vector2f(
            size.X > 0 ? origin.X / size.X : 0,
            size.Y > 0 ? origin.Y / size.Y : 0
         );
    }
    public void Update(ITransformable2D transformable)
    {
        if (RenderSprite.Texture is null)
            return;
        RenderSprite.Texture.Smooth = false;

        Vector2f targetSize = new Vector2f(transformable.Size.X * transformable.Scale.X, transformable.Size.Y * transformable.Scale.Y);
        Vector2u textureSize = RenderSprite.Texture.Size;

        float scaleX = (targetSize.X + transformable.AddSize) / textureSize.X;
        float scaleY = (targetSize.Y + transformable.AddSize) / textureSize.Y;

        scaleX *= ExtraScaleX;
        scaleY *= ExtraScaleY;

        RenderSprite.Scale = new Vector2f(scaleX, scaleY);
        RenderSprite.Origin = GetScaledOrigin(textureSize, GetOriginNormalized(targetSize, new Vector2f(transformable.Origin.X * transformable.Scale.X, transformable.Origin.Y * transformable.Scale.Y)));

        RenderSprite.Position = transformable.PositionOnScreen + (targetSize / 2f);
        RenderSprite.Rotation = transformable.Rotation;
    }


    public void UpdateWidth()
    {
        float widthScale = Screen.ScreenWidth / PreviousScreenWidth;
        ExtraScaleX /= widthScale;
        PositionOnScreen = new Vector2f(PositionOnScreen.X * widthScale, PositionOnScreen.Y);

        PreviousScreenWidth = Screen.ScreenWidth;
    }
    public void UpdateHeight()
    {
        float heightScale = Screen.ScreenHeight / PreviousScreenHeight;
        ExtraScaleY /= heightScale;
        PositionOnScreen = new Vector2f(PositionOnScreen.X, PositionOnScreen.Y * heightScale);

        PreviousScreenHeight = Screen.ScreenHeight;
    }

}
