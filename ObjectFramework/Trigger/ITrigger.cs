using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoRender;
using ProtoRender.Object;

namespace ObjectFramework.Trigger;
public interface ITrigger
{
    bool isTriggered {  get; }
    public Action<IUnit>? OnTriggered { get; set; }
    public Action<IUnit>? OnUntriggered { get; set; }
    void CheckTrigger(IUnit unit);
}
