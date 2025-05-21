using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UIFramework.Text;
public enum FadingTextLife
{
    Loop,            // Постоянный цикл: исчезает -> появляется -> исчезает -> ...
    PingPong,        // Двусторонний цикл: появляется -> исчезает -> появляется -> ...
    PingPongDispose,
    OneShotFreeze,    // Один раз до конца, затем остановка (freeze на финальном значении)
    OneShotDispose
}
