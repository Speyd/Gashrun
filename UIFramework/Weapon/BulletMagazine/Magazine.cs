using ControlLib;
using System.Diagnostics;
using UIFramework.Text;
using UIFramework.Weapon.Bullets;


namespace UIFramework.Weapon.BulletMagazine;
public class Magazine
{
    public ControlLib.ButtonBinding ReloadBind { get; set; }


    public MagazineState ClipBullet { get; set; }
    public MagazineState MagazineBullet { get; set; }



    public bool IsReload = false;
    public Stopwatch Stopwatch { get; init; } = new Stopwatch();
    private long _timeToReloadMls = 2000;
    public long TimeToReloadMls
    {
        get => _timeToReloadMls;
        set
        {
            _timeToReloadMls = value < 0 ? -value : value;
            ReloadBind.WaitingTimeMilliseconds = _timeToReloadMls;
        }
    }


    public UIText UIText { get; set; }

    public Magazine(int maxAmmoInMagazine, int maxTotalAmmo, IBullet bullet, UIText textMagazine, ControlLib.Control control)
    {

        ReloadBind = new(new Button(VirtualKey.R), Reload, _timeToReloadMls);
        control.AddBottomBind(ReloadBind);
        control.AddBottomBind(new ControlLib.ButtonBinding(new Button(VirtualKey.None), UpdateInfo));

        ClipBullet = new MagazineState(maxAmmoInMagazine, bullet);
        MagazineBullet = new MagazineState(maxTotalAmmo, bullet);

        UIText = new UIText(textMagazine);
    }
    public Magazine(int maxAmmoInMagazine, int maxTotalAmmo, UIText textMagazine, ControlLib.Control control)
    {

        ReloadBind = new(new Button(VirtualKey.R), Reload, _timeToReloadMls);
        control.AddBottomBind(ReloadBind);
        control.AddBottomBind(new ControlLib.ButtonBinding(new Button(VirtualKey.None), UpdateInfo));

        ClipBullet = new MagazineState(maxAmmoInMagazine);
        MagazineBullet = new MagazineState(maxTotalAmmo);

        UIText = new UIText(textMagazine);
    }

    private bool IsReloadMagazine()
    {
        if (!Stopwatch.IsRunning && MagazineBullet.Capacity != 0)
        {
            IsReload = true;
            Stopwatch.Start();

            return true;
        }
        else if (Stopwatch.IsRunning && Stopwatch.ElapsedMilliseconds >= TimeToReloadMls)
        {
            IsReload = false;
            Stopwatch.Stop();
            Stopwatch.Reset();

            return false;
        }
        return true;
    }
    public void Reload()
    {
        if (IsReloadMagazine())
            return;

        if (ClipBullet.Capacity == 0)
        {
            int ammoToReload = Math.Min(MagazineBullet.Capacity, ClipBullet.MaxCapacity);

            ClipBullet.AddBullet(MagazineBullet.GetBullet(ammoToReload));
        }
        else
        {
            int ammoNeeded = ClipBullet.MaxCapacity - ClipBullet.Capacity;
            int ammoToReload = Math.Min(MagazineBullet.Capacity, ammoNeeded);

            ClipBullet.AddBullet(MagazineBullet.GetBullet(ammoToReload));
        }
    }

    public async Task<bool> UseAmmoAsync(ProtoRender.Object.IUnit owner)
    {
        if (IsReload == true || (MagazineBullet.Capacity == 0 && ClipBullet.Capacity == 0))
            return false;

        if (ClipBullet.Capacity > 0)
        {
            var bullet = ClipBullet.GetBullet();
            if (bullet != null)
                await bullet.FlightAsync(owner);

        }
        if (ClipBullet.Capacity == 0)
            Reload();

        return !IsReload;
    }
    public bool UseAmmo(ProtoRender.Object.IUnit owner)
    {
        if(UIText.Owner != owner)
            UIText.Owner = owner;

        if (IsReload == true || (MagazineBullet.Capacity == 0 && ClipBullet.Capacity == 0))
            return false;

        if (ClipBullet.Capacity > 0)
        {
            ClipBullet.GetBullet()?.Flight(owner);
        }
        if (ClipBullet.Capacity == 0)
            Reload();

        return true;
    }

    public void UpdateInfo()
    {
        if (IsReload == true)
            Reload();

        UIText.SetText($"{MagazineBullet.Capacity} : {ClipBullet.Capacity}");
    }
}