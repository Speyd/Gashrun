using ProtoRender.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UIFramework.Overlay;
public static class IObjectOverlay
{
    public static string GetInfo(IObject iObj)
    {
        return $"X: {iObj.X.Axis:0.00}\n" +
               $"Y: {iObj.Y.Axis:0.00}\n" +
               $"Z: {iObj.Z.Axis:0.00}\n" +
               $"Is Passability: {iObj.IsPassability}";

    }
}
