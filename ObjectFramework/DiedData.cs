using ObstacleLib.SpriteLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectFramework;
public class DiedData
{
    public SpriteObstacle Sprite;
    public DeathAnimation Animation;
    public bool LastFrame;

    public DiedData(SpriteObstacle sprite, DeathAnimation animation, bool lastFrame)
    {
        Sprite = sprite;
        Animation = animation;
        LastFrame = lastFrame;
    }
}

