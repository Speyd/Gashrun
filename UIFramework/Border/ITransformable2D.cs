using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIFramework.Border;
public interface ITransformable2D
{
    Vector2f PositionOnScreen { get; }
    Vector2f Origin { get; }
    Vector2f Size { get; }
    Vector2f Scale { get; }
    float AddSize { get; }
    float Rotation { get; }
}
