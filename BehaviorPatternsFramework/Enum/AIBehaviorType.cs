using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework.Enum;
public enum AIBehaviorType
{
    Movement,   // отвечает за ходьбу, прыжки, патрули
    Combat,     // отвечает за стрельбу, ближний бой
    Vision,     // отвечает за обзор, поиск врагов
    Emotion,    // отвечает за состояние (страх, агрессия, настороженность)
    Idle,       // базовое бездействие
}