using ScreenLib.Output;
using ScreenLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using ProtoRender.Object;

namespace UIFramework
{
    public interface IUIElement
    {
        public static List<IUIElement> RendererUIElement { get; } = new List<IUIElement>();

        public Vector2f PositionOnScreen { get; set; }
        public List<Drawable> Drawables { get; init; }

        public static void Render()
        {
            foreach (var ui in RendererUIElement)
            {
                ui.UpdateInfo();
                foreach(var draw in ui.Drawables)
                    Screen.OutputPriority?.AddToPriority(OutputPriorityType.Interface, draw);
            }
        }

        public void UpdateInfo();
        public void UpdateScreenInfo();
        public void Hide();
    }
}
