using ObstacleLib.SpriteLib;
using ProtoRender.Object;
using ProtoRender.RenderAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ObjectFramework.Death;
public static class DeathManager
{
    static List<DeathData> DiedSprite = new();
    static List<DeathData> AfterDiedSprite = new();

    static readonly Result jammerResult = new Result();

    public static void Render(IUnit unit)
    {
        var diedSnapshot = DiedSprite;
        Parallel.ForEach(diedSnapshot, diedObj =>
        {
            UpdateDied(diedObj);
            diedObj.Sprite.Render(jammerResult, unit);
        });

        var afterDiedSnapshot = AfterDiedSprite;
        Parallel.ForEach(afterDiedSnapshot, diedObj =>
        {
            UpdateAfterDied(diedObj);
            diedObj.Sprite.Render(jammerResult, unit);
        });
    }
    private static void UpdateDied(DeathData diedObj)
    {
        if (diedObj.Sprite.Animation.Index == diedObj.Sprite.Animation.AmountFrame - 1)
            diedObj.LastFrame = true;

        if (diedObj.Sprite.Animation.Index == 0 && diedObj.LastFrame)
        {
            DiedSprite.Remove(diedObj);
            if (diedObj.Animation.IsExistsAfterDeath)
                AfterDiedSprite.Add(diedObj);
        }
    }
    private static void UpdateAfterDied(DeathData diedObj)
    {
        if (diedObj.Animation.Stopwatch.ElapsedMilliseconds >= diedObj.Animation.LifetimeMilliseconds)
            AfterDiedSprite.Remove(diedObj);
    }
    public static void AddDiedObject(DeathData diedData)
    {
        diedData.Animation.Stopwatch.Restart();
        DiedSprite.Add(diedData);
    }
}
