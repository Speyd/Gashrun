using InteractionFramework.Dialog;
using NGenerics.Extensions;
using ProtoRender.Object;
using ProtoRender.WindowInterface;
using SFML.Audio;
using UIFramework.Shape;
using UIFramework.Text;
using UIFramework.Windows.Button;

namespace UIFramework.Dialog;
public class DialogueStep
{
    public UIText QuestText { get; set; }
    public UIShape? BackgroundQuestText { get; set; } = null;
    public Dictionary<int, UIButton> AnswerButtons { get; set; } = new();


    public int SelectedAnswer = -1;

    private TaskCompletionSource<int>? _answerSelectedTcs;
    public Action OnResponse { get; set; }


    public DialogueStep(RenderText render, Dictionary<int, UIButton> answerText, Action? onDialogueResponse = null)
    {
        OnResponse = onDialogueResponse ?? (() => { });

        QuestText = new UIText(render);

        AnswerButtons = answerText;
        AnswerButtons.ForEach
            (
            ans =>
            ans.Value.Text.SetText($"{ans.Key}. {ans.Value.Text.RenderText.Text.DisplayedString}")
            );
    }
    public DialogueStep(UIText text, Dictionary<int, UIButton> answerText, Action? onDialogueResponse = null)
    {
        OnResponse = onDialogueResponse ?? (() => { });

        QuestText = new UIText(text);

        AnswerButtons = answerText;
        AnswerButtons.ForEach
            (
            ans =>
            ans.Value.Text.SetText($"{ans.Key}. {ans.Value.Text.RenderText.Text.DisplayedString}")
            );
    }

    private void AttachToListener(IUnit listener, IDialogObject speaker)
    {
        if (speaker.DialogSprite is not null)
            speaker.DialogSprite.Owner = listener;
        if (speaker.DisplayName is not null)
            speaker.DisplayName.Owner = listener;
    }
    private void DetachFromListener(IUnit listener, IDialogObject speaker)
    {
        if (speaker.DialogSprite is not null)
            speaker.DialogSprite.Owner = null;
        if (speaker.DisplayName is not null)
            speaker.DisplayName.Owner = null;

        foreach (var btn in AnswerButtons.Values)
        {
            btn.Owner = null;
        }

        QuestText.Owner = null;
        if (BackgroundQuestText is not null)
            BackgroundQuestText.Owner = null;
    }
    private async Task<int> InitializeAnswerButtons(IUnit listener, IDialogObject speaker)
    {
        _answerSelectedTcs = new TaskCompletionSource<int>();

        foreach (var item in AnswerButtons)
        {
            var button = item.Value;
            button.Owner = listener;
            QuestText.Owner = listener;
            if(BackgroundQuestText is not null)
                BackgroundQuestText.Owner = listener;

            int thisAnswer = item.Key;

            button.OnClick = null;
            button.OnClick = () =>
            {
                if (!_answerSelectedTcs.Task.IsCompleted)
                {
                    SelectedAnswer = thisAnswer;
                    _answerSelectedTcs.TrySetResult(SelectedAnswer);
                    OnResponse.Invoke();
                }
            };
        }

        return await _answerSelectedTcs.Task;
    }
    public async Task<int> RunAsync(IUnit listener, IDialogObject speaker)
    {
        AttachToListener(listener, speaker);
        int answer = await InitializeAnswerButtons(listener, speaker);
        DetachFromListener(listener, speaker);

        return answer;
    }
}