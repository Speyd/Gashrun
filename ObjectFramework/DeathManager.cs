using ObstacleLib.SpriteLib;
using ProtoRender.Object;
using ProtoRender.RenderAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ObjectFramework;
public static class DeathManager
{
    static List<DiedData> DiedSprite = new();
    static List<DiedData> AfterDiedSprite = new();

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
    private static void UpdateDied(DiedData diedObj)
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
    private static void UpdateAfterDied(DiedData diedObj)
    {
        if (DateTime.UtcNow - diedObj.Animation.CreationTime >= diedObj.Animation.Lifetime)
           AfterDiedSprite.Remove(diedObj);
    }
    public static void AddDiedObject(DiedData diedData)
    {
        diedData.Animation.CreationTime = DateTime.UtcNow;
        DiedSprite.Add(diedData);
    }
}
