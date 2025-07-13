using ProtoRender.Object;
using ProtoRender.WindowInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIFramework.Render;
using UIFramework.Text.Fading.FadingEnums;

namespace UIFramework.Text.Fading;
public class FadingController
{
    public static readonly byte MaxAlphaValue = 255;

    public long FadingTimeMilliseconds { get; set; }

    public readonly Stopwatch Stopwatch = new();
    private bool StartStopwatch = false;

    private FadingType _fadingType;
    public FadingType FadingType
    {
        get => _fadingType;
        set
        {
            if (_fadingType != value)
            {
                InvertProgress();
                _fadingType = value;
            }
            usedFadingType = value;

        }
    }
    private FadingType usedFadingType;

    private FadingTextLife _fadingTextLife;
    public FadingTextLife FadingTextLife
    {
        get => _fadingTextLife;
        set
        {
            if (_fadingTextLife == value)
                return;

            _fadingTextLife = value;
            if (!StartStopwatch)
            {
                Stopwatch.Reset();
                StartStopwatch = true;
                manualElapsed = 0;
            }
        }
    }

    private long manualElapsed = 0;

    public Action<float>? OnAlphaChanged;
    public Action? OnDispose;

    public FadingController(FadingType fasingType, FadingTextLife fadingTextLife, long fadingTimeMilliseconds)
    {
        _fadingType = fasingType;
        usedFadingType = fasingType;
        FadingTextLife = fadingTextLife;
        FadingTimeMilliseconds = fadingTimeMilliseconds;
    }

    private float GetCurrentProgress()
    {
        long elapsed = Stopwatch.IsRunning ? Stopwatch.ElapsedMilliseconds + manualElapsed : manualElapsed;
        float progress = (float)elapsed / FadingTimeMilliseconds;
        return Math.Clamp(progress, 0f, 1f);
    }
    private void InvertProgress()
    {
        float currentProgress = GetCurrentProgress();
        manualElapsed = (long)(FadingTimeMilliseconds * (1f - currentProgress));
        Stopwatch.Restart();
    }
    public void Update()
    {
        if (!Stopwatch.IsRunning && StartStopwatch)
        {
            Stopwatch.Restart();
        }
       

        long elapsedTime = manualElapsed + (Stopwatch.IsRunning ? Stopwatch.ElapsedMilliseconds : 0);
        if (elapsedTime >= FadingTimeMilliseconds)
        {
            ResetTime();
        }

        float progress = (float)elapsedTime / FadingTimeMilliseconds;

        float alpha = usedFadingType == FadingType.Appears ? progress : 1f - progress;
        OnAlphaChanged?.Invoke(alpha);
    }

    private void ResetTime()
    {
        switch (FadingTextLife)
        {
            case FadingTextLife.Loop:
                manualElapsed = 0;
                Stopwatch.Restart();
                break;

            case FadingTextLife.PingPong:
                manualElapsed = 0;
                Stopwatch.Restart();
                SwapUsedType();
                break;

            case FadingTextLife.PingPongDispose:
                if (usedFadingType == FadingType)
                {
                    manualElapsed = 0;
                    Stopwatch.Restart();
                    SwapUsedType();
                    break;
                }
                manualElapsed = FadingTimeMilliseconds;
                Stopwatch.Stop();
                StartStopwatch = false;
                OnDispose?.Invoke();
                break;

            case FadingTextLife.OneShotFreeze:
                manualElapsed = FadingTimeMilliseconds;
                Stopwatch.Stop();
                StartStopwatch = false;
                break;

            case FadingTextLife.OneShotDispose:
                manualElapsed = FadingTimeMilliseconds;
                Stopwatch.Stop();
                StartStopwatch = false;
                OnDispose?.Invoke();
                break;
        }
    }

    public void SwapType()
    {
        FadingType = FadingType == FadingType.Appears ? FadingType.Disappears : FadingType.Appears;
    }

    private void SwapUsedType()
    {
        usedFadingType = usedFadingType == FadingType.Appears ? FadingType.Disappears : FadingType.Appears;
    }

    public void Restart()
    {
        Stopwatch.Reset();
        manualElapsed = 0;
        StartStopwatch = true;

        if (FadingType == FadingType.Appears)
            OnAlphaChanged?.Invoke(0f);
        else
            OnAlphaChanged?.Invoke(1f);
    }
}
