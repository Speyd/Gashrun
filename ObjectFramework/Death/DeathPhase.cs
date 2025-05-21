using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectFramework.Death;
public enum DeathPhase
{
    Animating,         // Проигрывается анимация смерти (все кадры)
    AfterAnimation,    // Объект остаётся после окончания анимации (как бы "мертв, но еще виден")
    FrozenFinalFrame,  // Зафиксирован на последнем кадре (не анимируется)
    Expired            // Удалён из мира, не должен быть отрисован
}
