using ObjectFramework.Death;
using ProtoRender.Object;
using ProtoRender.WindowInterface;
using ScreenLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIFramework.Render;


namespace UIFramework.Text;
//public class FadingText : UIText
//{
//    private static readonly byte MaxAlphaValue = 255;

//    private long _fadingTimeMilliseconds;
//    public long FadingTimeMilliseconds
//    {
//        get => _fadingTimeMilliseconds;
//        set
//        {
//            _fadingTimeMilliseconds = value;
//            multTimeToByte = MaxAlphaValue / (float)value;
//        }
//    }
//    private float multTimeToByte = 0;

//    private readonly Stopwatch Stopwatch = new();
//    private bool StartStopwatch = true;

//    private FadingType _fadingType;
//    public FadingType FadingType
//    {
//        get => _fadingType;
//        set
//        {
//            if (_fadingType != value)
//            {
//                InvertProgress();
//                _fadingType = value;
//                usedFadingType = value;
//            }
//        }
//    }
//    private FadingType usedFadingType;

//    private FadingTextLife _fadingTextLife;
//    public FadingTextLife FadingTextLife
//    {
//        get => _fadingTextLife;
//        set
//        {
//            _fadingTextLife = value;
//            if (!StartStopwatch)
//            {
//                Stopwatch.Reset();
//                StartStopwatch = true;
//                manualElapsed = 0;
//            }
//        }
//    }

//    private long manualElapsed = 0;

//    public FadingText(RenderText text, FadingType fasingType, FadingTextLife fadingTextLife, long fadingTimeMilliseconds, IUnit? owner)
//        : base(text, owner)
//    {
//        FadingType = fasingType;
//        usedFadingType = fasingType;
//        FadingTextLife = fadingTextLife;
//        FadingTimeMilliseconds = fadingTimeMilliseconds;
//        manualElapsed = 0;
//    }

//    private void SetAlpha(float normalizedAlpha)
//    {
//        byte alpha = (byte)Math.Clamp(normalizedAlpha * MaxAlphaValue, 0, MaxAlphaValue);
//        var fill = Text.FillColor;
//        Text.FillColor = new SFML.Graphics.Color(fill.R, fill.G, fill.B, alpha);
//    }

//    private float GetCurrentProgress()
//    {
//        long elapsed = Stopwatch.IsRunning ? Stopwatch.ElapsedMilliseconds + manualElapsed : manualElapsed;
//        float progress = (float)elapsed / FadingTimeMilliseconds;
//        return Math.Clamp(progress, 0f, 1f);
//    }

//    private void InvertProgress()
//    {
//        float currentProgress = GetCurrentProgress();
//        manualElapsed = (long)(FadingTimeMilliseconds * (1f - currentProgress));
//        Stopwatch.Restart();
//    }

//    public override void UpdateInfo()
//    {
//        if (!Stopwatch.IsRunning && StartStopwatch)
//            Stopwatch.Restart();

//        long elapsedTime = manualElapsed + (Stopwatch.IsRunning ? Stopwatch.ElapsedMilliseconds : 0);

//        if (elapsedTime >= FadingTimeMilliseconds)
//        {
//            ResetTime();
//        }

//        float progress = (float)elapsedTime / FadingTimeMilliseconds;

//        switch (usedFadingType)
//        {
//            case FadingType.Appears:
//                SetAlpha(progress);
//                break;
//            case FadingType.Disappears:
//                SetAlpha(1f - progress);
//                break;
//        }
//    }

//    private void ResetTime()
//    {
//        switch (FadingTextLife)
//        {
//            case FadingTextLife.Loop:
//                manualElapsed = 0;
//                Stopwatch.Restart();
//                break;

//            case FadingTextLife.PingPong:
//                manualElapsed = 0;
//                Stopwatch.Restart();
//                SwapUsedType();
//                break;

//            case FadingTextLife.PingPongDispose:
//                if(usedFadingType == FadingType)
//                {
//                    manualElapsed = 0;
//                    Stopwatch.Restart();
//                    SwapUsedType();
//                    break;
//                }
//                manualElapsed = FadingTimeMilliseconds;
//                Stopwatch.Stop();
//                StartStopwatch = false;
//                UIRender.RemoveFromPriority(Owner, RenderOrder, this);
//                break;

//            case FadingTextLife.OneShotFreeze:
//                manualElapsed = FadingTimeMilliseconds;
//                Stopwatch.Stop();
//                StartStopwatch = false;
//                break;

//            case FadingTextLife.OneShotDispose:
//                manualElapsed = FadingTimeMilliseconds;
//                Stopwatch.Stop();
//                StartStopwatch = false;
//                UIRender.RemoveFromPriority(Owner, RenderOrder, this);
//                break;
//        }
//    }

//    public void SwapType()
//    {
//        FadingType = FadingType == FadingType.Appears ? FadingType.Disappears : FadingType.Appears;
//        // Прогресс автоматически инвертируется в сеттере FadingType
//    }

//    private void SwapUsedType()
//    {
//        usedFadingType = usedFadingType == FadingType.Appears ? FadingType.Disappears : FadingType.Appears;
//    }

//    public void Restart()
//    {
//        Stopwatch.Restart();
//        manualElapsed = 0;
//        StartStopwatch = true;

//        if (FadingType == FadingType.Appears)
//            SetAlpha(0f);
//        else
//            SetAlpha(1f);
//    }

//}
public class FadingText : UIText
{
    public FadingController Controller;
    public FadingText(RenderText text, FadingType fasingType, FadingTextLife fadingTextLife, long fadingTimeMilliseconds, IUnit? owner)
        : base(text, owner)
    {
        Controller = new FadingController(fasingType, fadingTextLife, fadingTimeMilliseconds);
        Controller.OnAlphaChanged = SetAlpha;
        Controller.OnDispose = () => UIRender.RemoveFromPriority(Owner, RenderOrder, this);

        Controller.Restart();
    }
    public FadingText(RenderText text, FadingController controller, IUnit? owner)
       : base(text, owner)
    {
        Controller = controller;
        Controller.OnAlphaChanged = SetAlpha;
        Controller.OnDispose = () => UIRender.RemoveFromPriority(Owner, RenderOrder, this);

        Controller.Restart();
    }
    private void SetAlpha(float normalizedAlpha)
    {
        byte alpha = (byte)Math.Clamp(normalizedAlpha * 255f, 0, 255);
        var fill = Text.FillColor;
        Text.FillColor = new SFML.Graphics.Color(fill.R, fill.G, fill.B, alpha);
    }

    public override void UpdateInfo()
    {
        Controller.Update();
    }

    public void SwapType() => Controller.SwapType();
    public void Restart() => Controller.Restart();

}
