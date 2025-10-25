using ProtoRender.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIFramework.Weapon.Bullets;


namespace UIFramework.Weapon.BulletMagazine;
public class MagazineState
{
    private int _maxCapacity = 0;
    public int MaxCapacity 
    { 
        get => _maxCapacity;
        set
        {
            _maxCapacity = value;
            SetCapacity(_maxCapacity);
        }
    }

    public int Capacity {  get; private set; }

    public List<IBullet> Bullets { get; private set; } = new List<IBullet>();

    public MagazineState(int maxCapacity)
    {
        MaxCapacity = maxCapacity;
    }
    public MagazineState(int maxCapacity, IBullet bullet)
    {
        MaxCapacity = maxCapacity;
        AddBullet(bullet, maxCapacity);
    }
    public MagazineState(int maxCapacity, List<IBullet> bullets)
    {
        MaxCapacity = maxCapacity;
        AddBullet(bullets);
    }

    private void SetCapacity(int maxCapacity)
    {
        List<IBullet> newBullets = new();

        for (int i = 0; i < maxCapacity && i < Bullets.Count; i++)
            newBullets.Add(Bullets[i]);

        Bullets = newBullets;
    }

    public IBullet? GetBullet()
    {
        if (Bullets.Count == 0)
            return null;

        var tempBullet = Bullets[^1];
        Bullets.RemoveAt(Bullets.Count - 1);
        Capacity--;

        return tempBullet;
    }
    public List<IBullet> GetBullet(int bulletCount)
    {
        int actualCount = Math.Min(bulletCount, Bullets.Count);
        int startIndex = Bullets.Count - actualCount;

        List<IBullet> removed = Bullets.GetRange(startIndex, actualCount);
        Bullets.RemoveRange(startIndex, actualCount);
        Capacity -= actualCount;

        return removed;
    }

    public void AddBullet(IBullet bullet, int count = 1)
    {
        for (int i = 0; i < count && Bullets.Count < MaxCapacity; i++)
        {
            Bullets.Add(bullet.GetCopy());
            Capacity++;
        }
    }
    public void AddBullet(List<IBullet> bullets)
    {
        int count = bullets.Count;

        for (int i = 0; i < count && Bullets.Count < MaxCapacity; i++)
        {
            Bullets.Add(bullets[i].GetCopy());
            Capacity++;
        }
    }
}
