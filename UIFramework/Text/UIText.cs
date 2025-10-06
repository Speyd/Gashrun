using ProtoRender.Object;
using ProtoRender.WindowInterface;
using ScreenLib;
using SFML.Graphics;
using SFML.System;
using UIFramework.Render;
using UIFramework.Text.AlignEnums;
using static System.Net.Mime.MediaTypeNames;


namespace UIFramework.Text;
public class UIText : UIElement
{  
    public override Vector2f PositionOnScreen
    {
        get => _positionOnScreen;
        set
        {
            _positionOnScreen = value;

            AdjustTextSize();
            RenderText.Text.Position = value;

            HorizontalAlignment = HorizontalAlignment;
            VerticalAlignment = VerticalAlignment;
        }
    }

    public override HorizontalAlign HorizontalAlignment
    {
        get => _horizontalAlignment;
        set
        {
            _horizontalAlignment = value;
            RenderText.Text.Origin = new Vector2f(GetHorizontalBounds(RenderText.Text.GetLocalBounds()), RenderText.Text.Origin.Y);
        }
    }
    public override VerticalAlign VerticalAlignment
    {
        get => _verticalAlignment;
        set
        {
            _verticalAlignment = value;
            RenderText.Text.Origin = new Vector2f(RenderText.Text.Origin.X, GetVerticalBounds(RenderText.Text.GetLocalBounds()));
        }
    }

    public uint OriginCharacterSize{ get; protected set; }
    private string _originalText = "";
    public RenderText RenderText { get; set; }


    private TextResizeMode _textResizeMode = TextResizeMode.Fixed;
    public TextResizeMode TextResizeMode
    {
        get => _textResizeMode;
        set
        {
            if (_textResizeMode == value)
                return;

            _textResizeMode = value;

            if (_textResizeMode == TextResizeMode.Fixed)
            {
                RenderText.Text.DisplayedString = _originalText;
                RenderText.Text.CharacterSize = OriginCharacterSize;
            }
            else if (_textResizeMode == TextResizeMode.AutoFit)
            {
                AdjustTextSize();
            }
        }
    }

    private FloatRect _textBounds = new();
    public FloatRect TextBounds 
    {
        get => _textBounds;
        set
        {
            _textBounds = new FloatRect(
                value.Left * Screen.MultWidth,
                value.Top * Screen.MultHeight,
                value.Width * Screen.MultWidth,
                value.Height * Screen.MultHeight
            );


            ApplyTextSettings(_originalText);
        }
    }


    public UIText(string text, uint size, Vector2f position, string pathToFont, SFML.Graphics.Color color, IUnit? owner = null)
        :base(owner)
    {
        _originalText = text;
        RenderText = new RenderText(text, size, position, pathToFont, color);
        OriginCharacterSize = RenderText.Text.CharacterSize;
        PositionOnScreen = position;

        Drawables.Add(RenderText.Text);
    }
    public UIText(RenderText render, IUnit? owner = null)
        : base(owner)
    {
        RenderText = new RenderText(render);
        _originalText = render.Text.DisplayedString;
        OriginCharacterSize = RenderText.Text.CharacterSize;
        PositionOnScreen = render.Text.Position;

        Drawables.Add(RenderText.Text);
    }
    public UIText(UIText uIText, IUnit? owner = null)
        :base(owner ?? uIText.Owner)
    {
        RenderText = new RenderText(uIText.RenderText);
        _originalText = uIText.RenderText.Text.DisplayedString;
        OriginCharacterSize = uIText.OriginCharacterSize;
        _positionOnScreen = uIText._positionOnScreen;

        VerticalAlignment = uIText.VerticalAlignment;
        HorizontalAlignment = uIText.HorizontalAlignment;

        RenderOrder = uIText.RenderOrder;
        _isHide = uIText.IsHide;

        Drawables.Add(RenderText.Text);
    }



    public void AdjustTextSize()
    {
        if (TextBounds.Width <= 0 || TextBounds.Height <= 0)
            return;

        uint size = OriginCharacterSize;
        string sourceText = _originalText;

      
        do
        {
            RenderText.Text.CharacterSize = size;
            string wrappedText = WrapTextToWidth(sourceText, size, TextBounds.Width);
            RenderText.Text.DisplayedString = wrappedText;

            var bounds = RenderText.Text.GetLocalBounds();

            if (bounds.Height <= TextBounds.Height)
                break;

            size--;
        }
        while (size > 1);

        var finalBounds = RenderText.Text.GetLocalBounds();
        RenderText.Text.Origin = new Vector2f(GetHorizontalBounds(finalBounds), GetVerticalBounds(finalBounds));
    }
    private string WrapTextToWidth(string text, uint charSize, float maxWidth)
    {
        string[] words = text.Split(' ');
        string result = "";
        string currentLine = "";

        var tempText = new SFML.Graphics.Text("", RenderText.Text.Font)
        {
            CharacterSize = charSize
        };
        foreach (var word in words)
        {
           
            string testLine = (currentLine.Length == 0) ? word : currentLine + " " + word;
            tempText.DisplayedString = testLine;

            var bounds = tempText.GetLocalBounds();

            if (bounds.Width > maxWidth)
            {
                result += (currentLine + "\n");
                currentLine = word;
            }
            else
            {
                currentLine = testLine;
            }
        }

        result += currentLine;
        return result;
    }

    internal virtual void SetTextAsync(string text)
    {
        RenderText.Text.DisplayedString = text;
        UpdateDisplayedText(text);

        var bounds = RenderText.Text.GetLocalBounds();
        RenderText.Text.Origin = new Vector2f(GetHorizontalBounds(bounds), GetVerticalBounds(bounds));
        RenderText.Text.Position = _positionOnScreen;
    }
    public virtual void SetText(string text)
    {
        if (_originalText == text)
            return;

        WriteQueue.EnqueueDraw(this, text);
    }

    private void ApplyTextSettings(string text)
    {
        switch (TextResizeMode)
        {
            case TextResizeMode.AutoFit:
                AdjustTextSize();
                break;

            case TextResizeMode.Fixed:
                RenderText.Text.DisplayedString = text;
                RenderText.Text.CharacterSize = OriginCharacterSize;
                break;
        }
    }
    private void UpdateDisplayedText(string text)
    {
        _originalText = text;
        ApplyTextSettings(text);
    }


    #region IUIElement
    public override void ToggleVisibilityObject()
    {
        if (IsHide && Drawables.Count > 0)
            Drawables.Clear();
        else if(!Drawables.Contains(RenderText.Text))
            Drawables.Add(RenderText.Text);
    }
    #endregion
}
