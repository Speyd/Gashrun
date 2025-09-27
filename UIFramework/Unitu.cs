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
    public Unitu(SpriteObstacle obstacle, int maxHp, bool createNewTexture = true)
      : base(obstacle, maxHp, createNewTexture)
    {
      
    }
    public Unitu(IMap map, SpriteObstacle obstacle, int maxHp, bool createNewTexture = true)
       : base(map, obstacle, maxHp, createNewTexture)
    {
       
    }
    public Unitu(Unit unit, bool updateTexture = true)
       : base(unit, updateTexture)
    {
       
    }

}
