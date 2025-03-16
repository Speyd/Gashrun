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
        public Vector2f PositionOnScreen { get; set; }
        public List<Drawable> Drawables { get; init; }

        public void Render();
        public void UpdateInfo();
        public void UpdateScreenInfo();
        public void Hide();
    }
}
