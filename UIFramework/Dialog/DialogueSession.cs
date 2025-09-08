using InteractionFramework.Dialog;
using ProtoRender.Object;
using ScreenLib;
using SFML.Graphics;
using UIFramework.Shape;

namespace UIFramework.Dialog;
public class DialogueSession
{
    public Dictionary<int, Dictionary<int, DialogueStep>> Levels { get; init; } = new();
    public int? SelectedAnswer { get; private set; } = null;

    public UIShape BackgroundShape { get; set; }
    public IDialogObject Speaker { get; set; }
    public IDialogObject Listener { get; set; }


    public DialogueSession(IDialogObject speaker, IDialogObject listener, RectangleShape shapeBackround)
    {
        Speaker = speaker;    
        Listener = listener;

        BackgroundShape = new(shapeBackround);
    }
    public DialogueSession(IDialogObject speaker, IDialogObject listener, UIShape backgroundShape)
    {
        Speaker = speaker;
        Listener = listener;

        BackgroundShape = new(backgroundShape);
    }

    public async Task RunAsync()
    {
        if (Listener is not IUnit listener)
            return;

        listener.Control.FreezeControlsKey = true;
        listener.Control.FreezeControlsMouse = true;
        Screen.Window.SetMouseCursorVisible(true);


        BackgroundShape.Owner = listener;
        int prevAns = 0;
        for (int i = 0; i < Levels.Count; i++)
        {
            if (!Levels.ContainsKey(i) || !Levels[i].ContainsKey(prevAns))
                break;

            prevAns = await Levels[i][prevAns].RunAsync(listener, Speaker);

            if (prevAns == -1)
                break;
        }
        SelectedAnswer = prevAns;
        BackgroundShape.Owner = null;

        Screen.Window.SetMouseCursorVisible(false);
        listener.Control.FreezeControlsKey = false;
        listener.Control.FreezeControlsMouse = false;
    }

    public void AddLevel(int level, Dictionary<int, DialogueStep> dialogLevel)
    {
        Levels[level] = dialogLevel;
    }
    public void AddLevelStep(int level, int lvl, DialogueStep dialogLevel)
    {
        Levels[level].Add(lvl, dialogLevel);
    }
}