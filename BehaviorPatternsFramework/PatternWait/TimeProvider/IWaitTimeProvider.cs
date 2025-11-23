using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework.PatternWait.TimeProvider;
public interface IWaitTimeProvider
{
    int GetWaitTimeMs();
    IWaitTimeProvider GetDeepCopy();
}
