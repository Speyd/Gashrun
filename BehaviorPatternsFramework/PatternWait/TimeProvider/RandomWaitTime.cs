using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework.PatternWait.TimeProvider;
public class RandomWaitTime : IWaitTimeProvider
{
    public int MinMs { get; }
    public int MaxMs { get; }

    private readonly Random _rng = new();

    public RandomWaitTime(int minMs, int maxMs)
    {
        MinMs = minMs;
        MaxMs = maxMs;
    }

    public int GetWaitTimeMs() => _rng.Next(MinMs, MaxMs);

    public IWaitTimeProvider GetDeepCopy() => new RandomWaitTime(MinMs, MaxMs);
}
