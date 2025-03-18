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


namespace UIFramework;
public interface IUIElement
{
    float PreviousScreenWidth { get; }
    float PreviousScreenHeight { get; }

    Vector2f PositionOnScreen { get; set; }
    List<Drawable> Drawables { get; init; }

    void Render();
    void UpdateInfo();
    void UpdateScreenInfo();
    void Hide();
}
