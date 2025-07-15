using NGenerics.Extensions;
using ObstacleLib.SpriteLib.Add;
using ObstacleLib.TexturedWallLib;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextureLib.Loader;
using TextureLib.DataCache;
using TextureLib.Textures;
using System.Xml.Linq;
using TextureLib.Loader.ImageProcessing;


namespace InteractionFramework.HitAction.DrawableBatch;
public class HitDrawableBatch
{
    public List<Drawable> DrawList { get; set; } = new();
    public HitDrawSelectMode Mode { get; set; } = HitDrawSelectMode.First;
    public bool IsLoaded { get; private set; } = false;

    public HitDrawableBatch(Drawable? drawObject = null)
    {
        if (drawObject is not null)
            DrawList.Add(drawObject);

        IsLoaded = true;
    }
    public HitDrawableBatch(ImageLoadOptions? options = null, bool loadAsync = true, params string[] paths)
    {
        options = options ?? new ImageLoadOptions();

        if (loadAsync)
            _ = LoadAsync(ImageLoader.LoadAsync(options, true, paths));
        else
            LoadList(ImageLoader.Load(options, true, paths));
    }
    public HitDrawableBatch(List<Drawable> drawList)
    {
        DrawList.AddRange(drawList);
    }
    public HitDrawableBatch(List<TextureWrapper> drawList)
    {
        LoadList(drawList);
    }

    private async Task LoadAsync(Task<List<TextureWrapper>> texturesTask)
    {
        var textures = texturesTask.Result;
        LoadList(textures);
    }
    private void LoadList(List<TextureWrapper> drawList)
    {
        foreach (var texture in drawList)
            DrawList.Add(SpriteDataCache.Get(texture.PathTexture)?.First() ?? new Sprite(texture.Texture));
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
