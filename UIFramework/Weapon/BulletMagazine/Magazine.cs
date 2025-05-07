using ControlLib;
using ProtoRender.WindowInterface;
using ScreenLib.Output;
using ScreenLib;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIFramework.Render;
using System.Diagnostics;
using UIFramework.Weapon.Patron;

namespace UIFramework.Weapon.BulletMagazine;
public class Magazine : IUIElement
{
    public float PreviousScreenWidth { get; protected set; } = Screen.ScreenWidth;
    public float PreviousScreenHeight { get; protected set; } = Screen.ScreenHeight;
    private Vector2f _positionOnScreen;
    public Vector2f PositionOnScreen
    {
        get => _positionOnScreen;
        set
        {
            _positionOnScreen = AdjustTextSize(value);
            AmmoText.Text.Position = value;
        }
    }
    public List<Drawable> Drawables { get; init; } = new();


    private uint _originCharacterSize;
    private bool _isCharacterSizeSet = false;

    private RenderText _textMagazine;
    public RenderText AmmoText
    {
        get => _textMagazine;
        set
        {
            _textMagazine = value;

            if (_isCharacterSizeSet)
            {
                _positionOnScreen = new Vector2f(_positionOnScreen.X + _originCharacterSize,
                    _positionOnScreen.Y + _originCharacterSize);

                _isCharacterSizeSet = false;
            }
            _originCharacterSize = value.Text.CharacterSize;
        }
    }

    public ControlLib.BottomBinding ReloadBind { get; set; }


    public BulletStack BulletStack { get; set; }
    public BulletStack BulletStorage { get; set; }



    public bool IsReload = false;
    public Stopwatch Stopwatch { get; init; } = new Stopwatch();
    private float _timeToReloadMls = 2000;
    public float TimeToReloadMls
    {
        get => _timeToReloadMls;
        set
        {
            _timeToReloadMls = value < 0 ? -value : value;

        }
    }


    public Magazine(ControlLib.Control control, ControlLib.BottomBinding reloadBind, RenderText textMagazine, IBullet bullet, int maxAmmoInMagazine, int maxTotalAmmo)
    {

        ReloadBind = new ControlLib.BottomBinding(reloadBind.Bottoms, Reload, reloadBind.WaitingTimeMilliseconds, reloadBind.FixedParameters);
        control.AddBottomBind(ReloadBind);

        BulletStack = new BulletStack(maxAmmoInMagazine, bullet);
        BulletStorage = new BulletStack(maxTotalAmmo, bullet);

        AmmoText = textMagazine;
        PositionOnScreen = textMagazine.Text.Position;




        Screen.WidthChangesFun += UpdateScreenInfo;
        Screen.HeightChangesFun += UpdateScreenInfo;

        Drawables.Add(AmmoText.Text);
        UIRender.AddToPriority(RenderOrder.Indicators, this);
    }

    private bool IsReloadMagazine()
    {
        if (!Stopwatch.IsRunning && BulletStorage.Capacity != 0)
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

        if (BulletStack.Capacity == 0)
        {
            int ammoToReload = Math.Min(BulletStorage.Capacity, BulletStack.MaxCapacity);

            BulletStack.AddBullet(BulletStorage.GetBullet(ammoToReload));
        }
        else
        {
            int ammoNeeded = BulletStack.MaxCapacity - BulletStack.Capacity;
            int ammoToReload = Math.Min(BulletStorage.Capacity, ammoNeeded);

            BulletStack.AddBullet(BulletStorage.GetBullet(ammoToReload));
        }
    }
    public async Task<bool> UseAmmoAsync(ProtoRender.Object.IUnit owner)
    {
        if (IsReload == true || (BulletStorage.Capacity == 0 && BulletStack.Capacity == 0))
            return false;

        if (BulletStack.Capacity > 0)
        {
            var bullet = BulletStack.GetBullet();
            if (bullet != null)
                await bullet.FlightAsync(owner);

        }
        if (BulletStack.Capacity == 0)
            Reload();

        return true;
    }
    public bool UseAmmo(ProtoRender.Object.IUnit owner)
    {
        if (IsReload == true || (BulletStorage.Capacity == 0 && BulletStack.Capacity == 0))
            return false;

        if (BulletStack.Capacity > 0)
        {
            BulletStack.GetBullet()?.Flight(owner);
        }
        if (BulletStack.Capacity == 0)
            Reload();

        return true;
    }
    public void Render()
    {
        UpdateInfo();
        foreach (var draw in Drawables)
            Screen.OutputPriority?.AddToPriority(OutputPriorityType.Interface, draw);
    }
    public void UpdateInfo()
    {
        if (IsReload == true)
            Reload();

        string lastText = AmmoText.Text.DisplayedString;
        AmmoText.Text.DisplayedString = $"{BulletStorage.Capacity} : {BulletStack.Capacity}";
        AmmoText.Text.Origin = new Vector2f(AmmoText.Text.GetLocalBounds().Width, 0);

        AdjustTextPosition(lastText);
    }

    private Vector2f AdjustTextSize(Vector2f position)
    {
        uint previousCharacterSize = _isCharacterSizeSet ? AmmoText.Text.CharacterSize : 0;
        _isCharacterSizeSet = true;

        AmmoText.Text.CharacterSize = (uint)(_originCharacterSize / (Screen.ScreenRatio >= 1 ? Screen.ScreenRatio : 1 / Screen.ScreenRatio));

        float x = position.X + previousCharacterSize - AmmoText.Text.CharacterSize;
        float y = position.Y + previousCharacterSize - AmmoText.Text.CharacterSize;

        return new Vector2f(x, y);
    }
    private void AdjustTextPosition(string previousText)
    {
        float mult = AmmoText.Text.CharacterSize;
        float previousWidth = previousText.Length;
        float newWidth = AmmoText.Text.DisplayedString.Length;

        _positionOnScreen = new Vector2f(_positionOnScreen.X + previousWidth - newWidth, _positionOnScreen.Y);
        AmmoText.Text.Position = _positionOnScreen;
    }

    public void UpdateScreenInfo()
    {
        UpdateWidth();
        UpdateHeight();
    }
    public void UpdateWidth()
    {
        float widthScale = Screen.ScreenWidth / PreviousScreenWidth;
        PositionOnScreen = new Vector2f(PositionOnScreen.X * widthScale, PositionOnScreen.Y);

        PreviousScreenWidth = Screen.ScreenWidth;
    }
    public void UpdateHeight()
    {
        float heightScale = Screen.ScreenHeight / PreviousScreenHeight;
        PositionOnScreen = new Vector2f(PositionOnScreen.X, PositionOnScreen.Y * heightScale);

        PreviousScreenHeight = Screen.ScreenHeight;
    }
    public void Hide()
    {
        if (Drawables.Count > 0)
            Drawables.Clear();
        else
            Drawables.Add(AmmoText.Text);
    }

}

