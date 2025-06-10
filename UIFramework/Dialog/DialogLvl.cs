
using InteractionFramework;
using InteractionFramework.Dialog;
using ProtoRender.Object;
using ProtoRender.WindowInterface;
using SFML.Graphics;
using System.Drawing;
using UIFramework.Render;
using UIFramework.Sprite;
using UIFramework.Text;
using UIFramework.Windows;

namespace UIFramework.Dialog;
public class DialogLvl
{
    public UIText QuestText { get; set; }
    public Dictionary<int, Button> AnswerText { get; set; } = new();
    public UIShape ShapeBackround { get; set; }

    public int AnswerNumber = -1;

    private TaskCompletionSource<int>? _answerSelected;

    public DialogLvl(RenderText render, Dictionary<int, Button> answerText, RectangleShape shapeBackround)
    {
        ShapeBackround = new(shapeBackround);
        ShapeBackround.HorizontalAlignment = Text.AlignEnums.HorizontalAlign.Center;
        ShapeBackround.VerticalAlignment = Text.AlignEnums.VerticalAlign.Center;
        ShapeBackround.RenderOrder = RenderOrder.Dialog;

        QuestText = new UIText(render);
        QuestText.VerticalAlignment = Text.AlignEnums.VerticalAlign.Bottom;
        QuestText.HorizontalAlignment = Text.AlignEnums.HorizontalAlign.Center;

        QuestText.SetText("Что тЫ ТУТ ДЕАЛЕШЬ?!!?!??!");
        QuestText.PositionOnScreen = new SFML.System.Vector2f(500, ScreenLib.Screen.ScreenHeight);
        QuestText.RenderText.Text.FillColor = SFML.Graphics.Color.White;


        AnswerText = answerText;
    }

    public async Task<int> StartAsync(IDialogObject listener, IDialogObject speaker)
    {
        if (listener is not IUnit unit)
            return -1;
        _answerSelected = new TaskCompletionSource<int>();

        ShapeBackround.Owner = unit;
        if (speaker.DialogSprite is not null)
            speaker.DialogSprite.Owner = unit;
        speaker.DisplayName.Owner = unit;

        foreach (var item in AnswerText)
        {
            var button = item.Value;
            button.Owner = unit;
            QuestText.Owner = unit;

            int thisAnswer = item.Key;

            button.OnClick = null;
            button.OnClick = () =>
            {
                if (!_answerSelected.Task.IsCompleted)
                {
                    AnswerNumber = thisAnswer;
                    _answerSelected.TrySetResult(AnswerNumber);
                }
            };
        }

        int answer = await _answerSelected.Task;

        ShapeBackround.Owner = null;
        speaker.DisplayName.Owner = null;
        if (speaker.DialogSprite is not null)
            speaker.DialogSprite.Owner = null;

        foreach (var btn in AnswerText.Values)
        {
            btn.Owner = null;
            QuestText.Owner = null;
        }

        return answer;
    }
}