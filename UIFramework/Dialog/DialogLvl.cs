using InteractionFramework.Dialog;
using NGenerics.Extensions;
using ProtoRender.Object;
using ProtoRender.WindowInterface;
using SFML.Audio;
using UIFramework.Text;
using UIFramework.Windows;


namespace UIFramework.Dialog;
public class DialogLvl
{
    public UIText QuestText { get; set; }
    public Dictionary<int, UIButton> AnswerText { get; set; } = new();

    public int AnswerNumber = -1;

    private TaskCompletionSource<int>? _answerSelected;
    public Action OnDialogueResponse { get; set; }
    public DialogLvl(RenderText render, Dictionary<int, UIButton> answerText, Action? onDialogueResponse = null)
    {
        OnDialogueResponse = onDialogueResponse ?? (() => { });

        QuestText = new UIText(render);
        QuestText.VerticalAlignment = Text.AlignEnums.VerticalAlign.Bottom;
        QuestText.HorizontalAlignment = Text.AlignEnums.HorizontalAlign.Center;

        QuestText.SetText("Что тЫ ТУТ ДЕАЛЕШЬ?!!?!??!");
        QuestText.PositionOnScreen = new SFML.System.Vector2f(500, ScreenLib.Screen.ScreenHeight);
        QuestText.RenderText.Text.FillColor = SFML.Graphics.Color.White;


        AnswerText = answerText;
        AnswerText.ForEach
            (
            ans =>
            ans.Value.TextButton.SetText($"{ans.Key}. {ans.Value.TextButton.RenderText.Text.DisplayedString}")
            );
    }

    private void SubscribeListener(IUnit listener, IDialogObject speaker)
    {
        if (speaker.DialogSprite is not null)
            speaker.DialogSprite.Owner = listener;
        if (speaker.DisplayName is not null)
            speaker.DisplayName.Owner = listener;
    }
    private void UnsubscribeListener(IUnit listener, IDialogObject speaker)
    {
        if (speaker.DialogSprite is not null)
            speaker.DialogSprite.Owner = null;
        if (speaker.DisplayName is not null)
            speaker.DisplayName.Owner = null;

        foreach (var btn in AnswerText.Values)
        {
            btn.Owner = null;
            QuestText.Owner = null;
        }
    }
    private async Task<int> InitializeAnswerButtons(IUnit listener, IDialogObject speaker)
    {
        _answerSelected = new TaskCompletionSource<int>();

        foreach (var item in AnswerText)
        {
            var button = item.Value;
            button.Owner = listener;
            QuestText.Owner = listener;

            int thisAnswer = item.Key;

            button.OnClick = null;
            button.OnClick = () =>
            {
                if (!_answerSelected.Task.IsCompleted)
                {
                    AnswerNumber = thisAnswer;
                    _answerSelected.TrySetResult(AnswerNumber);
                    OnDialogueResponse.Invoke();
                }
            };
        }

        return await _answerSelected.Task;
    }
    public async Task<int> StartAsync(IUnit listener, IDialogObject speaker)
    {
        SubscribeListener(listener, speaker);
        int answer = await InitializeAnswerButtons(listener, speaker);
        UnsubscribeListener(listener, speaker);

        return answer;
    }
}