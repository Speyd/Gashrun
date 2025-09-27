
using ProtoRender.Map;
using System.Collections.Generic;

namespace UIFramework.Weapon.Bullets;
public static class BulletHandler
{
    private static readonly Dictionary<IMap, List<IBullet>> Bullets = new();
    private static readonly CancellationTokenSource _cts = new();
    private static readonly Thread _updateThread;

    static BulletHandler()
    {
        _updateThread = new Thread(UpdateLoop)
        {
            IsBackground = true,
            Name = "Bullet Update Thread"
        };
        _updateThread.Start();
    }

    public static void Add(IMap map, IBullet bullet)
    {
        lock (Bullets)
        {
            if (Bullets.TryGetValue(map, out var value))
                value.Add(bullet);
            else
                Bullets[map] = new List<IBullet> { bullet };
        }

    }

    public static void Stop()
    {
        _cts.Cancel();
        _updateThread.Join();
    }

    private static void UpdateLoop()
    {
        while (!_cts.Token.IsCancellationRequested)
        {
            Update();
            Thread.Sleep(16);
        }
    }

    private static void Update()
    {
        lock (Bullets)
        {
            foreach (var bulletsMap in Bullets)
            {
                if (bulletsMap.Key.ActiveAnchors.Count == 0)
                    return;

                var bulletList = bulletsMap.Value;
                for (int i = bulletList.Count - 1; i >= 0; i--)
                {
                    var bullet = bulletList[i];
                    if (!bullet.IsActive)
                    {
                        bulletList.RemoveAt(i);
                        continue;
                    }

                    bullet.Update();
                }
            }
        }
    }
}

