
namespace UIFramework.Weapon.Bullets;
public static class BulletHandler
{
    private static readonly List<IBullet> Bullets = new();
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

    public static void Add(IBullet bullet)
    {
        lock (Bullets) Bullets.Add(bullet);
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
            for (int i = Bullets.Count - 1; i >= 0; i--)
            {
                var bullet = Bullets[i];
                if (!bullet.IsActive)
                {
                    Bullets.RemoveAt(i);
                    continue;
                }

                bullet.Update();
            }
        }
    }
}

