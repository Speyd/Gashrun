using ProtoRender.Object;
using ProtoRender.WindowInterface;
using ScreenLib;
using ScreenLib.Output;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIFramework.Render;
using UIFramework.Weapon.BulletMagazine;


namespace UIFramework.Text;
public class UIText : RenderText, IUIElement
{
    public float PreviousScreenWidth { get; protected set; } = Screen.ScreenWidth;
    public float PreviousScreenHeight { get; protected set; } = Screen.ScreenHeight;
    private Vector2f _positionOnScreen;
    public Vector2f PositionOnScreen
    {
        get => _positionOnScreen;
        set
        {
            _positionOnScreen = AdjustTextSize(value);
            Text.Position = value;
        }
    }
    public List<Drawable> Drawables { get; init; } = new();
    private RenderOrder _renderOrder = RenderOrder.Indicators;
    public RenderOrder RenderOrder
    {
        get => _renderOrder;
        set
        {
            IUIElement.SetRenderOrder(Owner, _renderOrder, value, this);
            _renderOrder = value;
        }
    }


    private IUnit? _owner = null;
    public IUnit? Owner
    {
        get => _owner;
        set
        {
            IUIElement.SetOwner(_owner, value, this);
            _owner = value;
        }
    }
    public uint OriginCharacterSize{ get; protected set; }


    public UIText(string text, uint size, Vector2f position, string pathToFont, SFML.Graphics.Color color, IUnit? owner = null)
        :base(text, size, position, pathToFont, color)
    {
        Owner = owner;

        OriginCharacterSize = Text.CharacterSize;
        PositionOnScreen = position;

        Screen.WidthChangesFun += UpdateScreenInfo;
        Screen.HeightChangesFun += UpdateScreenInfo;

        Drawables.Add(Text);
    }
    public UIText(RenderText render, IUnit? owner = null)
        : base(render)
    {
        Owner = owner;

        OriginCharacterSize = Text.CharacterSize;
        PositionOnScreen = render.Text.Position;

        Screen.WidthChangesFun += UpdateScreenInfo;
        Screen.HeightChangesFun += UpdateScreenInfo;

        Drawables.Add(Text);
    }



    private Vector2f AdjustTextSize(Vector2f position)
    {
        uint previousCharacterSize = OriginCharacterSize == Text.CharacterSize? 0: Text.CharacterSize;

        Text.CharacterSize = (uint)(OriginCharacterSize / (Screen.ScreenRatio >= 1 ? Screen.ScreenRatio : 1 / Screen.ScreenRatio));

        float x = position.X + previousCharacterSize - Text.CharacterSize;
        float y = position.Y + previousCharacterSize - Text.CharacterSize;

        return new Vector2f(x, y);
    }
    private void AdjustTextPosition(string previousText)
    {
        float mult = Text.CharacterSize;
        float previousWidth = previousText.Length;
        float newWidth = Text.DisplayedString.Length;

        _positionOnScreen = new Vector2f(_positionOnScreen.X + previousWidth - newWidth, _positionOnScreen.Y);
        Text.Position = _positionOnScreen;
    }
    public void SetText(string text)
    {
        if (Text is null || Font is null || Text.CPointer == IntPtr.Zero)
            return;

        string lastText = Text.DisplayedString;
        Text.DisplayedString = text;
        if(Text.DisplayedString != String.Empty)
            Text.Origin = new Vector2f(Text.GetLocalBounds().Width, 0);

        AdjustTextPosition(lastText);
    }


    public void UpdateWidth()
    {
        if (Screen.ScreenWidth == PreviousScreenWidth)
            return;

        float widthScale = Screen.ScreenWidth / PreviousScreenWidth;
        PositionOnScreen = new Vector2f(PositionOnScreen.X * widthScale, PositionOnScreen.Y);

        PreviousScreenWidth = Screen.ScreenWidth;
    }
    public void UpdateHeight()
    {
        if (Screen.ScreenHeight == PreviousScreenHeight)
            return;

        float heightScale = Screen.ScreenHeight / PreviousScreenHeight;
        PositionOnScreen = new Vector2f(PositionOnScreen.X, PositionOnScreen.Y * heightScale);

        PreviousScreenHeight = Screen.ScreenHeight;
    }


    #region IUIElement
    public void Render()
    {
        UpdateInfo();
        foreach (var draw in Drawables)
            Screen.OutputPriority?.AddToPriority(IUIElement.OutputPriorityType, draw);
    }
    public virtual void UpdateInfo()
    {

    }
    public void UpdateScreenInfo()
    {
        UpdateWidth();
        UpdateHeight();
    }
    public void Hide()
    {
        if (Drawables.Count > 0)
            Drawables.Clear();
        else
            Drawables.Add(Text);
    }
    #endregion
}
