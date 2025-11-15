using SFML.Graphics;
using TextureLib.Loader;
using TextureLib.DataCache;
using TextureLib.Textures;



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
    public HitDrawableBatch(ImageLoadOptions? options = null, params string[] paths)
    {
        if (options is not null && options.LoadAsync)
            _ = LoadAsync(ImageLoader.LoadAsync(options, paths));
        else
            LoadList(ImageLoader.Load(options, paths));
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
        {
            var cache = SpriteDataCache.Get(texture.PathTexture);
            if (cache is null)
                DrawList.Add(new Sprite(texture.Texture));
            else
            {
                foreach (var sprite in cache)
                    DrawList.Add(sprite);
            }
        }
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
