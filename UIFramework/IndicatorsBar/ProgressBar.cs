using InteractionFramework;
using ProtoRender.Object;
using SFML.Graphics;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UIFramework.IndicatorsBar.Content;

namespace UIFramework.IndicatorsBar;
public class ProgressBar : FillBar
{
    public static readonly Stat BaseStat = new Stat(100);
    public List<string> Milestones { get; init; } = new();

    public ProgressBar(RectangleShape border, IBarContent forwardFillContent, IBarContent backwardFillContent, IUnit? owner = null)
        : base(border, forwardFillContent, backwardFillContent, new Stat(BaseStat), owner)
    {
        Stat.SetValue(0);
    }
    public ProgressBar(IBarContent forwardFillContent, IBarContent backwardFillContent, IUnit? owner = null)
         : base(new RectangleShape(), forwardFillContent, backwardFillContent, new Stat(BaseStat), owner)
    {
        Stat.SetValue(0);
    }

    public void AddMilestone(
        int progress,
        [CallerMemberName] string caller = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0)
    {
        Milestones.Add($"Progress: {progress}% (called from {caller} in {file}:{line})");
        Stat.SetValue(Stat.Value + progress);
    }
}
