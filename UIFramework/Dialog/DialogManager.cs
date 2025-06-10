using InteractionFramework.Dialog;
using ScreenLib;
using SFML.Graphics;
using System.Drawing;
using UIFramework.Sprite;
using UIFramework.Windows;

namespace UIFramework.Dialog;
public class DialogManager
{
    public Dictionary<int, Dictionary<int, DialogLvl>> dialog { get; set; } = new();
    public IDialogObject Speaker { get; set; }

    public IDialogObject Listener { get; set; }

    public DialogManager(IDialogObject speaker, IDialogObject listener)
    {
        Speaker = speaker;    
        Listener = listener;
    }

    public async Task StartAsyncDialog()
    {
        Screen.Window.SetMouseCursorVisible(true);
        int prevAns = 0;

        for (int i = 0; i < dialog.Count; i++)
        {
            if (!dialog.ContainsKey(i) || !dialog[i].ContainsKey(prevAns))
                break;

            prevAns = await dialog[i][prevAns].StartAsync(Listener, Speaker);

            if (prevAns == -1)
                break;
        }

        Console.WriteLine($"Диалог завершён с ответом: {prevAns}");

        Screen.Window.SetMouseCursorVisible(false);
    }

    public void AddLevelDialog(int level, Dictionary<int, DialogLvl> dialogLevel)
    {
        dialog[level] = dialogLevel;
    }
}