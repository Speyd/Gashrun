using ObstacleLib.SpriteLib;
using ScreenLib;
using SFML.System;
using ProtoRender.Object;
using ProtoRender.Map;
using HitBoxLib.Data.Observer;
using System.Collections.Concurrent;
using InteractionFramework.Death;
using InteractionFramework.VisualImpact;
using InteractionFramework.VisualImpact.Data;


namespace ObjectFramework;
public class Unit : SpriteObstacle, IUnit, IDamageable
{
    #region IMoveble
    public float MoveSpeed { get; set; } = 200f;
    public double MoveSpeedAngel { get; set; } = 1;
    public double TempVerticalAngle { get; set; } = 0.0;
    public double TempAngle { get; set; } = 0.0;
    public float MinDistanceFromWall { get; set; } = 100;
    public float MouseSensitivity { get; set; } = 0.001f;
    public bool IsMouseCaptured { get; set; } = true;

    public double MinVerticalAngle { get; set; } = -Math.PI / 2;
    public double MaxVerticalAngle { get; set; } = Math.PI / 2;
    public ConcurrentDictionary<IObject, byte> IgnoreCollisionObjects { get; set; } = new();
    #endregion

    #region IUnit
    //-----------------------Angle-----------------------
    /// <summary>X - Cos(Angle); Y - Sin(Angle)</summary>
    public Vector2f Direction { get; protected set; }
    /// <summary>X - -Sin(Angle); Y - Cos(Angle)</summary>
    public Vector2f Plane { get; protected set; }

    private double _angle;
    /// <summary>Angle Entity(Horizontal axis)</summary>
    public double Angle
    {
        get => _angle;
        set
        {
            if (_angle != value)
            {
                _angle = value;

                float cos = (float)Math.Cos(_angle);
                float sin = (float)Math.Sin(_angle);
                Direction = new Vector2f(cos, sin);
                Plane = new Vector2f(-sin, cos);
            }
        }
    }
    /// <summary>Vertical Angle Entity(Vertical axis)</summary>
    public double VerticalAngle { get; set; }
    public Vector2f OriginPosition
    {
        get
        {
            return new Vector2f((float)X.Axis, (float)Y.Axis);
        }
    }
    private double _fov;
    public double Fov
    {
        get => _fov;
        set
        {
            _fov = value;
            HalfFov = value / 2;
        }
    }
    /// <summary>Half Fov Entity</summary>
    public double HalfFov { get; private set; }


    //---------------------Ray Setting----------------------
    /// <summary>Maximum rendering distance(depends on Screen.Setting.Tile)</summary>
    public int MaxRenderTile { get; set; }

    //---------------------Render Setting----------------------
    /// <summary>This is the angle between adjacent rays in the rendering system</summary>
    public double DeltaAngle { get; private set; }
    /// <summary>Projected height(depends on the distance between the Entity and the object)</summary>
    public double ProjCoeff { get; private set; }
    #endregion

    #region IDamageable
    private void SetHp(float maxHp)
    {
        if (maxHp < _hp)
            Hp = maxHp;
    }
    private float _maxHp = 0;
    public float MaxHp
    {
        get => _maxHp;
        private set
        {
            _maxHp = value > 0 ? value : _maxHp;
            SetHp(_maxHp);
        }
    }

    private void CheckHp()
    {
        if (_hp <= 0)
            DeathAction?.Invoke();
    }
    private float _hp = 0;
    public float Hp
    {
        get => _hp;
        set
        {
            _hp = value;
            CheckHp();
        }
    }

    private void ValidateDamage(float damage)
    {
        Hp -= damage;
    }
    public Action<float>? DamageAction { get; set; }

    public Action? DeathAction { get; set; }
    public DeathEffect? DeathAnimation { get; set; } = null;
    private void ClearingDataAfteDeath()
    {
        Map?.DeleteObstacle(this);
        SpritesToRender.Remove(this);

        Map = null;

        if (DeathAnimation is not null)
            BeyondRenderManager.Create(this, new DeathData(this, DeathAnimation));
    }
    #endregion

    public Unit(IMap map, SpriteObstacle obstacle, int maxHp)
        : base(obstacle)
    {
        Map = map;
        MaxHp = maxHp;
        Hp = maxHp;

        DeathAction += ClearingDataAfteDeath;
        DamageAction += ValidateDamage;

        Fov = Math.PI / 3;
        HalfFov = (float)Fov / 2;

        Angle = 0;
        Angle -= 0.000001;
        VerticalAngle -= 0.000001;
        MaxRenderTile = 1200;
        ObserverSettingChangesFun();
        Screen.WidthChangesFun += ObserverSettingChangesFun;
    }
    public Unit(SpriteObstacle obstacle, int maxHp)
       : base(obstacle)
    {
        MaxHp = maxHp;
        Hp = maxHp;

        DeathAction += ClearingDataAfteDeath;
        DamageAction += ValidateDamage;

        Fov = Math.PI / 3;
        HalfFov = (float)Fov / 2;

        Angle = 0;
        Angle -= 0.000001;
        VerticalAngle -= 0.000001;
        MaxRenderTile = 1200;
        ObserverSettingChangesFun();
        Screen.WidthChangesFun += ObserverSettingChangesFun;
    }
    public Unit(Unit unit, bool updateTexture = true)
       : base(unit, updateTexture)
    {
        Map = unit.Map;
        MaxHp = unit.MaxHp;
        Hp = unit.MaxHp;

        Fov = unit.Fov;
        HalfFov = unit.HalfFov;

        Angle = unit.Angle;
        TempAngle = unit.TempAngle;

        VerticalAngle = unit.VerticalAngle;
        TempVerticalAngle = unit.TempVerticalAngle;
        MaxVerticalAngle = unit.MaxVerticalAngle;
        MinVerticalAngle = unit.MinVerticalAngle;

        MaxRenderTile = unit.MaxRenderTile;
        MoveSpeed = unit.MoveSpeed;
        MouseSensitivity = unit.MouseSensitivity;
        MoveSpeedAngel = unit.MoveSpeedAngel;
        MinDistanceFromWall = unit.MinDistanceFromWall;

        DeathAction += ClearingDataAfteDeath;
        DamageAction += ValidateDamage;
        ObserverSettingChangesFun();
        Screen.WidthChangesFun += ObserverSettingChangesFun;
    }


    #region IUnit
    public override double GetDisplayAngle(double spriteAngle)
    {
        double angle = this.Angle - spriteAngle;

        if (angle > Math.PI)
            angle -= 2 * Math.PI;
        else if (angle < -Math.PI)
            angle += 2 * Math.PI;

        return angle;
    }

    public void ObserverSettingChangesFun()
    {
        float dist = Screen.Setting.AmountRays / (2 * (float)Math.Tan(HalfFov));
        ProjCoeff = dist * Screen.Setting.Tile;

        DeltaAngle = (float)Fov / Screen.Setting.AmountRays;
    }
    public ObserverInfo GetObserverInfo()
    {
        ObserverInfo observerInfo = new ObserverInfo();
        observerInfo.fov = Fov;
        observerInfo.verticalAngle = VerticalAngle;
        observerInfo.angle = Angle;
        observerInfo.deltaAngle = DeltaAngle;
        observerInfo.position = new Vector3f((float)X.Axis, (float)Y.Axis, (float)Z.Axis);

        return observerInfo;
    }
    public new IUnit GetCopy()
    {
        return new Unit(this);
    }
    #endregion
}
