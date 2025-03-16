using ScreenLib.Output;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScreenLib;

namespace UIFramework.Render
{
    public static class UIRender
    {
        public static SortedDictionary<RenderOrder, List<IUIElement>> TreePriority { get; }
        static UIRender()
        {
            TreePriority = new();
        }

        public static void AddToPriority(RenderOrder objRender, IUIElement uiElement)
        {
            if (!TreePriority.TryGetValue(objRender, out List<IUIElement>? value))
            {
                value = new List<IUIElement>();
                TreePriority[objRender] = value;
            }

            value.Add(uiElement);
        }

        public static void DrawingByPriority()
        {
            foreach (KeyValuePair<RenderOrder, List<IUIElement>> item in TreePriority)
            {
                foreach (var value in item.Value)
                    value.Render();
            }
        }
    }
}
