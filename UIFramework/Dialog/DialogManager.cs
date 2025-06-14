using InteractionFramework.Dialog;
using ProtoRender.Object;
using ScreenLib;
using SFML.Graphics;
using System.Drawing;
using UIFramework.Sprite;
using UIFramework.Windows;

namespace UIFramework.Dialog;
public class DialogManager
{
    public Dictionary<int, Dictionary<int, DialogLvl>> dialog { get; set; } = new();
    public int? ListenerAnswer { get; private set; } = null;
    public UIShape ShapeBackround { get; set; }
    public IDialogObject Speaker { get; set; }

    public IDialogObject Listener { get; set; }

    public DialogManager(IDialogObject speaker, IDialogObject listener, RectangleShape shapeBackround)
    {
        Speaker = speaker;    
        Listener = listener;

        ShapeBackround = new(shapeBackround);
        ShapeBackround.HorizontalAlignment = Text.AlignEnums.HorizontalAlign.Center;
        ShapeBackround.VerticalAlignment = Text.AlignEnums.VerticalAlign.Center;
        ShapeBackround.RenderOrder = UIFramework.Render.RenderOrder.Dialog;
    }

    public async Task StartAsyncDialog()
    {
        if (Listener is not IUnit listener)
            return;

        listener.Control.FreezeControlsKey = true;
        listener.Control.FreezeControlsMouse = true;
        Screen.Window.SetMouseCursorVisible(true);


        ShapeBackround.Owner = listener;
        int prevAns = 0;
        for (int i = 0; i < dialog.Count; i++)
        {
            if (!dialog.ContainsKey(i) || !dialog[i].ContainsKey(prevAns))
                break;

            prevAns = await dialog[i][prevAns].StartAsync(listener, Speaker);

            if (prevAns == -1)
                break;
        }
        ListenerAnswer = prevAns;
        ShapeBackround.Owner = null;

        Screen.Window.SetMouseCursorVisible(false);
        listener.Control.FreezeControlsKey = false;
        listener.Control.FreezeControlsMouse = false;
    }

    public void AddLevelDialog(int level, Dictionary<int, DialogLvl> dialogLevel)
    {
        dialog[level] = dialogLevel;
    }
}