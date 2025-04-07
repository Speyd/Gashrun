using ControlLib;
using EffectLib.VisualEffectLib.Effect;
using EntityLib;
using EntityLib.Player;
using HitBoxLib.Data.Observer;
using HitBoxLib.HitBoxSegment;
using HitBoxLib.Operations;
using HitBoxLib.PositionObject;
using HitBoxLib.Segment.SignsTypeSide;
using MapLib;
using MoveLib.Move;
using ObstacleLib;
using ObstacleLib.SpriteLib;
using ProtoRender.Object;
using ProtoRender.RenderAlgorithm;
using RayTracingLib;
using ScreenLib;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TextureLib;
using static UIFramework.Weapon.Patron.IBullet;

namespace UIFramework.Weapon.Patron;
public class StandartBullet : IBullet
{
    public BottomBinding? HitDrawbleObject { get; set; } = null;
    public BottomBinding? HitObject { get; set; } = null;
    public HitEffect? HitEffect { get; set; } = null;
    private Map Map { get; set; }

    public StandartBullet(Map map, BottomBinding? hitDrawbleObject, BottomBinding? hitObject, HitEffect? hitEffect)
    {
        HitDrawbleObject = hitDrawbleObject;
        HitEffect = hitEffect;
        HitObject = hitObject;

        Map = map;
    }

    public void Flight(Entity owner)
    {
        var result = Raycast.RaycastFun(Map, owner);


        if (result.Item1 is null)
            return;


        HitEffect?.Create(owner, result.Item2.X, result.Item2.Y, result.Item2.Z);
        if (result.Item1 is IDrawable drawable)
        {
            HitDrawbleObject?.Listen(drawable);
        }
        else
        {
            HitObject?.Listen();
        }
    }
}