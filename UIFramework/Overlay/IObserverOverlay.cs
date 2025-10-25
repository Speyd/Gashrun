using ProtoRender.Object;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UIFramework.Overlay;
public static class IObserverOverlay
{
    public static string GetInfo(IObserver observer)
    {
        return
            $"Angle: {observer.Angle:0.00}\n" +
            $"Vertical Angle: {observer.VerticalAngle:0.00}\n" +
            $"Fov: {observer.Fov:0.00}\n" +
            $"Direction: {observer.LookDirection.X:0.00} | {observer.LookDirection.Y:0.00}";
    }
}
