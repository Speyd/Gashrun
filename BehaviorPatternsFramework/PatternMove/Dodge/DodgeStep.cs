using ControlLib.Buttons;
using ProtoRender.Object;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework.PatternMove.Dodge;
public struct DodgeStep
{
    public DodgeDirection Direction { get; }
    public Vector2f VectorDirection { get; set; } = new();
    public int DurationMs { get; }

    public DodgeStep(DodgeDirection direction, int durationMs)
    {
        Direction = direction;
        DurationMs = durationMs;

        SetVectorDirection();
    }

    private void SetVectorDirection()
    {
        switch (Direction)
        {
            case DodgeDirection.Left:
                VectorDirection = new Vector2f(0, -1);
                break;
            case DodgeDirection.Right:
                VectorDirection = new Vector2f(0, 1);
                break;

            default:
                VectorDirection = new Vector2f(0, 0);
                break;
        }
    }
}
