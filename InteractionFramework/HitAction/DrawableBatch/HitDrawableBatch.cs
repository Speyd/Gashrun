using NGenerics.Extensions;
using ObstacleLib.SpriteLib.Add;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InteractionFramework.HitAction.DrawableBatch;
public class HitDrawableBatch
{
    public List<Drawable> DrawList { get; set; } = new();
    public HitDrawSelectMode Mode { get; set; } = HitDrawSelectMode.First;

    public HitDrawableBatch(Drawable? drawObject = null)
    {
        if (drawObject is not null)
            DrawList.Add(drawObject);
    }
    public HitDrawableBatch(List<Drawable> drawList)
    {
        DrawList.AddRange(drawList);
    }

    public Drawable? Get(int index = 0)
    {
        if (DrawList.Count == 0)
            return null;

        return Mode switch
        {
            HitDrawSelectMode.First => DrawList[0],
            HitDrawSelectMode.Last => DrawList[^1],
            HitDrawSelectMode.Random => DrawList[Random.Shared.Next(0, DrawList.Count)],
            HitDrawSelectMode.ByIndex => (index >= 0 && index < DrawList.Count) ? DrawList[index] : null,
            _ => null
        };
    }
}
