using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using ScreenLib;
using ScreenLib.Output;
using UIFramework.Render;
using SFML.Graphics;
using ProtoRender.Object;


namespace UIFramework.Sprite;
public class UISprite: UIElement
{
    public override Vector2f PositionOnScreen
    {
        get => _positionOnScreen;
        set
        {
            _positionOnScreen = value;
            Sprite.Position = value;
        }
    }
    public Vector2f _scale = new Vector2f(1f, 1f);
    public Vector2f Scale 
    {
        get => _scale;
        set
        {
            _scale = new Vector2f(
                value.X / (Screen.BaseScreenWidth / PreviousScreenWidth),
                value.Y / (Screen.BaseScreenHeight / PreviousScreenHeight)
                );

            Sprite.Scale = _scale;
        }
    }

    public SFML.Graphics.Sprite Sprite { get; set; }

    public UISprite(UISprite uiSprite, IUnit? owner = null)
       : base(owner)
    {
        Sprite = new SFML.Graphics.Sprite(uiSprite.Sprite);
        _scale = uiSprite.Scale;

        FloatRect bounds = Sprite.GetLocalBounds();
        Sprite.Origin = new Vector2f(bounds.Width / 2f, bounds.Height / 2f);

        PositionOnScreen = uiSprite.PositionOnScreen;
        PreviousScreenHeight = uiSprite.PreviousScreenHeight;
        PreviousScreenWidth = uiSprite.PreviousScreenWidth;

        Drawables.Add(Sprite);
    }
    public UISprite(SFML.Graphics.Sprite sprite, IUnit? owner = null)   
        :base(owner)
    {
        Sprite = new SFML.Graphics.Sprite(sprite);
        Scale = sprite.Scale;

        FloatRect bounds = Sprite.GetLocalBounds();
        Sprite.Origin = new Vector2f(bounds.Width / 2f, bounds.Height / 2f);
        PositionOnScreen = sprite.Position;

        Drawables.Add(Sprite);
    }
    public UISprite(SFML.Graphics.Texture? texture, IUnit? owner = null)
        :base(owner)
    {
        if(texture is not null)
            Sprite = new SFML.Graphics.Sprite(texture);
        else
            Sprite = new SFML.Graphics.Sprite();
        Scale = new Vector2f(1f,1f);

        FloatRect bounds = Sprite.GetLocalBounds();
        Sprite.Origin = new Vector2f(bounds.Width / 2f, bounds.Height / 2f);
        PositionOnScreen = Sprite.Position;

        Drawables.Add(Sprite);
    }


    #region IUIElement
    public override void UpdateWidth()
    {
        float widthScale = Screen.ScreenWidth / PreviousScreenWidth;

        Sprite.Scale = new Vector2f(Sprite.Scale.X * widthScale, Sprite.Scale.Y);
        PositionOnScreen = new Vector2f(PositionOnScreen.X * widthScale, PositionOnScreen.Y);

        PreviousScreenWidth = Screen.ScreenWidth;
    }
    public override void UpdateHeight()
    {
        float heightScale = Screen.ScreenHeight / PreviousScreenHeight;

        Sprite.Scale = new Vector2f(Sprite.Scale.X, Sprite.Scale.Y * heightScale);
        PositionOnScreen = new Vector2f(PositionOnScreen.X, PositionOnScreen.Y * heightScale);

        PreviousScreenHeight = Screen.ScreenHeight;
    }
    public override void ToggleVisibilityObject()
    {
        if (IsHide && Drawables.Count > 0)
            Drawables.Clear();
        else
        {
            if (!Drawables.Contains(Sprite))
                Drawables.Add(Sprite);
        }
    }
    #endregion
}
