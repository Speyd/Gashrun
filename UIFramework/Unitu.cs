using InteractionFramework;
using MoveLib.Physics;
using MoveLib;
using ObstacleLib.SpriteLib;
using ProtoRender.Map;
using ScreenLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoRender.Object;

namespace UIFramework;

public class Unitu : Unit, IWorldAnchor
{
    public Unitu(SpriteObstacle obstacle, int maxHp)
      : base(obstacle, maxHp)
    {
      
    }
    public Unitu(IMap map, SpriteObstacle obstacle, int maxHp)
       : base(map, obstacle, maxHp)
    {
       
    }
    public Unitu(Unit unit, bool updateTexture = true)
       : base(unit)
    {
       
    }

}
