using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework.PatternWait.TimeProvider;
public class FixedWaitTime : IWaitTimeProvider
{
    public int DurationMs { get; set; }
    public FixedWaitTime(int durationMs) => DurationMs = durationMs;

    public int GetWaitTimeMs() => DurationMs;
    public IWaitTimeProvider GetDeepCopy() => new FixedWaitTime(DurationMs);
}

