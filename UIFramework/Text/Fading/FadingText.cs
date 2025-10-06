using ProtoRender.Object;
using ProtoRender.WindowInterface;
using UIFramework.Render;
using UIFramework.Text.Fading.FadingEnums;


namespace UIFramework.Text.Fading;
public class FadingText : UIText
{
    public FadingController Controller;
    public FadingText(UIText text, FadingType fadingType, FadingTextLife fadingTextLife, long fadingTimeMilliseconds, IUnit? owner = null)
        : base(text, owner)
    {
        InitController(new FadingController(fadingType, fadingTextLife, fadingTimeMilliseconds));
    }

    public FadingText(RenderText text, FadingType fadingType, FadingTextLife fadingTextLife, long fadingTimeMilliseconds, IUnit? owner = null)
        : base(text, owner)
    {
        InitController(new FadingController(fadingType, fadingTextLife, fadingTimeMilliseconds));
    }

    public FadingText(RenderText text, FadingController controller, IUnit? owner = null)
        : base(text, owner)
    {
        InitController(controller);
    }

    public FadingText(FadingText fadingText, IUnit? owner = null)
        : base(fadingText, owner)
    {
        InitController(new FadingController(fadingText.Controller.FadingType,
                                            fadingText.Controller.FadingTextLife,
                                            fadingText.Controller.FadingTimeMilliseconds));
    }

    private void InitController(FadingController controller)
    {
        Controller = controller;
        Controller.OnAlphaChanged = SetAlpha;
        Controller.OnDispose = () => UIRender.RemoveFromPriority(Owner, RenderOrder, this);
        Controller.Restart();
    }

    private void SetAlpha(float normalizedAlpha)
    {
        byte alpha = (byte)Math.Clamp(normalizedAlpha * 255f, 0, 255);
        var fill = RenderText.Text.FillColor;
        RenderText.Text.FillColor = new SFML.Graphics.Color(fill.R, fill.G, fill.B, alpha);
    }

    public override void UpdateInfo()
    {
        Controller.Update();
    }

    public void SwapType() => Controller.SwapType();
    public void Restart() => Controller.Restart();
}