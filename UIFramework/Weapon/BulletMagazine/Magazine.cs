using ControlLib;
using ControlLib.Buttons;
using ProtoRender.Object;
using System.Diagnostics;
using UIFramework.Text;
using UIFramework.Weapon.Bullets;


namespace UIFramework.Weapon.BulletMagazine;
public class Magazine
{
    public ButtonBinding ReloadBind { get; set; }
    public ButtonBinding UpdateInfoBinding { get; init; }


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
    private IUnit? _owner = null;
    public IUnit? Owner
    {
        get => _owner;
        set
        {
            UIText.Owner = value;

            if (_owner is not null)
            {
                _owner?.Control.DeleteReferenceBottomBind(ReloadBind);
                _owner?.Control.DeleteReferenceBottomBind(UpdateInfoBinding);
            }
            if (_owner != value)
            {
                _owner = value;

                _owner?.Control.AddBottomBind(ReloadBind);
                _owner?.Control.AddBottomBind(UpdateInfoBinding);

            }
        }
    }

    public Magazine(int maxAmmoInMagazine, int maxTotalAmmo, IBullet bullet, VirtualKey reloadKey, UIText textMagazine)
    {

        ReloadBind = new(new Button(reloadKey), Reload, _timeToReloadMls);
        UpdateInfoBinding = new ButtonBinding(new Button(VirtualKey.None), UpdateInfo);

        ClipBullet = new MagazineState(maxAmmoInMagazine, bullet);
        MagazineBullet = new MagazineState(maxTotalAmmo, bullet);

        UIText = new UIText(textMagazine);
    }
    public Magazine(int maxAmmoInMagazine, int maxTotalAmmo, VirtualKey reloadKey, UIText textMagazine)
    {
        ReloadBind = new(new Button(reloadKey), Reload, _timeToReloadMls);
        UpdateInfoBinding = new ButtonBinding(new Button(VirtualKey.None), UpdateInfo);

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
    public bool UseAmmo()
    {
        if (IsReload == true || (MagazineBullet.Capacity == 0 && ClipBullet.Capacity == 0))
            return false;

        if (ClipBullet.Capacity > 0)
        {
            ClipBullet.GetBullet()?.Flight(Owner);
           
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