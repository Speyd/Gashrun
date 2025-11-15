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
using InteractionFramework.Dialog;
using UIFramework.Dialog;
using UIFramework.Sprite;
using UIFramework.Text;
using ObjectFramework;
using InteractionFramework;
using FpsLib;
using HitBoxLib.PositionObject;
using MoveLib.Move;
using HitBoxLib.HitBoxSegment;
using HitBoxLib.Segment.SignsTypeSide;
using MoveLib.Physics;
using ProtoRender.Physics;
using MoveLib;
using RayTracingLib;
using InteractionFramework.Trigger.TriggerTouch;
using System.Numerics;
using UIFramework.Weapon.Bullets.Variants;
using TextureLib.Textures.Pair;
using AnimationLib;
using DataPipes;
using EffectLib.EffectCore;
using ObstacleLib;
using ProtoRender.RenderAlgorithm;
using ProtoRender.RenderInterface;
using SFML.Graphics;
using TextureLib.Loader.ImageProcessing;
using TextureLib.Loader;
using TextureLib.Textures;
using BehaviorPatternsFramework.Behavior;
using System;
using AnimationLib.Core;

namespace UIFramework;
public class Unit : SpriteObstacle, IUnit, IDamageable, IDialogObject, IJumper, IKnockbackable
{
    public Vector2f Vector2 { get; set; } = new Vector2f();
    public ControlState ControlState { get; set; } = ControlState.Normal;
    public GroundState GroundState { get; set; } = GroundState.OnGround;

    public AIController behavioral { get; set; }
    public float KnockbackPower { get; set; } = 10000;
    public float KnockbackAngle { get; set; }
    public Vector2f Velocity { get; set; }
    public bool HasGravity { get; set; } = true;
    public float KnockbackVelocityEpsilon { get; set; } = 1;
    public float GroundLevel { get; set; } = 0;
    public float JumpElapsed { get; set; } = 0;
    public float JumpDuration { get; set; } = 0.2f;
    public float JumpHeight { get; set; } = 200;
    public float CurrentJumpForce { get; set; } = 1000;
    public float InitialJumpHeight { get; set; } = 0;
    public float Gravity { get; } = 100000;
    public float Friction { get; } = 0.6f;

    /// <summary>
    /// Specifies the minimum distance (in units) above the surface
    /// at which the object is positioned after landing or when standing on the ground.
    /// Default is 1.
    /// </summary>
    public float GroundClearance { get; set; } = 1;

    /// <summary>
    /// The relative time (from 0 to 1) at which the jump reaches its peak height.
    /// By default, 0.5 means the apex occurs at the midpoint of the jump duration.
    /// Adjust this value to make the apex earlier or later.
    /// </summary>
    public float JumpApexTime { get; set; } = 0.5f;




    #region IMoveble
    public float MoveSpeed { get; set; } = 200f;
    public double MoveSpeedAngel { get; set; } = 1;
    public double TempVerticalAngle { get; set; } = 0.0;
    public double TempAngle { get; set; } = 0.0;
    public float MinDistanceFromWall { get; set; } = 0;
    public float MouseSensitivity { get; set; } = 0.001f;
    public bool IsMouseCaptured { get; set; } = true;
    public Vector2f MoveDirection { get; set; } = new Vector2f();

    public double MinVerticalAngle { get; set; } = -Math.PI / 2;
    public double MaxVerticalAngle { get; set; } = Math.PI / 2;
    public ConcurrentDictionary<IObject, byte> IgnoreCollisionObjects { get; set; } = new();
    #endregion

    #region IUnit
    //-----------------------Angle-----------------------
    /// <summary>X - Cos(Angle); Y - Sin(Angle)</summary>
    public Vector2f LookDirection { get; protected set; }
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
                LookDirection = new Vector2f(cos, sin);
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
    public Stat Hp { get; set; }
    public DeathEffect? DeathAnimation { get; set; } = null;
    private void ClearingDataAfteDeath()
    {

        RemoveObstacle(this);
        Map = null;
        IsAdded = false;

        if (DeathAnimation is not null)
         BeyondRenderManager.Create(this, new DeathData(this, DeathAnimation));
    }
    #endregion

    #region IDialogObject
    public UISprite? DialogSprite { get; set; } = null;
    public UIText? DisplayName { get; set; } = null;
    #endregion

    #region IControlHandler
    public ControlLib.Control Control { get; init; } = new ControlLib.Control();
    #endregion

    public Unit(Animator animator, int maxHp, ImageLoadOptions? option = null)
       : base(animator, option)
    {
        Hp = new Stat(maxHp);

        Hp.OnDepleted += ClearingDataAfteDeath;

        Fov = Math.PI / 3;
        HalfFov = (float)Fov / 2;

        Angle = 0;
        Angle -= 0.000001;
        VerticalAngle -= 0.000001;
        MaxRenderTile = 1200;
        ObserverSettingChangesFun();
        Screen.WidthChangesFun += ObserverSettingChangesFun;
        physicsStrategies = new List<IPhysicsUpdateStrategy>
        {
            new JumpStrategy(),
            new KnockbackStrategy(),
        };
        PhysicsHandler.Register(this);
    }
    public Unit(SpriteObstacle obstacle, int maxHp, ImageLoadOptions? option = null)
       : base(obstacle, option)
    {
        Hp = new Stat(maxHp);

        Hp.OnDepleted += ClearingDataAfteDeath;

        Fov = Math.PI / 3;
        HalfFov = (float)Fov / 2;

        Angle = 0;
        Angle -= 0.000001;
        VerticalAngle -= 0.000001;
        MaxRenderTile = 1200;
        ObserverSettingChangesFun();
        Screen.WidthChangesFun += ObserverSettingChangesFun;
        physicsStrategies = new List<IPhysicsUpdateStrategy>
        {
            new JumpStrategy(),
            new KnockbackStrategy(),
        };
        PhysicsHandler.Register(this);
    }
    public Unit(IMap map, SpriteObstacle obstacle, int maxHp, ImageLoadOptions? option = null)
       : base(obstacle, option)
    {
        Map = map;
        Animation = obstacle.Animation;
        Hp = new Stat(maxHp);

        Hp.OnDepleted += ClearingDataAfteDeath;

        Fov = Math.PI / 3;
        HalfFov = (float)Fov / 2;

        Angle = 0;
        Angle -= 0.000001;
        VerticalAngle -= 0.000001;
        MaxRenderTile = 1200;
        ObserverSettingChangesFun();
        Screen.WidthChangesFun += ObserverSettingChangesFun;
        physicsStrategies = new List<IPhysicsUpdateStrategy>
        {
            new JumpStrategy(),
            new KnockbackStrategy(),
        };
        PhysicsHandler.Register(this);
    }
    public Unit(Unit unit, ImageLoadOptions? option = null)
       : base(unit, option)
    {
        physicsStrategies = new List<IPhysicsUpdateStrategy>
        {
            new JumpStrategy(),
            new KnockbackStrategy(),
        };
        PhysicsHandler.Register(this);
        Map = unit.Map;
        Hp = unit.Hp;

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

        Hp.OnDepleted += ClearingDataAfteDeath;
        ObserverSettingChangesFun();
        Screen.WidthChangesFun += ObserverSettingChangesFun;
    }


    private List<IPhysicsUpdateStrategy> physicsStrategies;
    public void Jump()
    {
        if (GroundState is not GroundState.OnGround) return;

        GroundState = GroundState.Jumping;
        JumpElapsed = 0f;
        InitialJumpHeight = (float)Z.Axis;
    }
    public void Knockback()
    {
        Vector2f direction = new Vector2f(MathF.Cos(KnockbackAngle), MathF.Sin(KnockbackAngle));

        Velocity = direction * KnockbackPower;
        ControlState = ControlState.Knockback;
    }

    public void UpdatePhysics()
    {
        foreach (var strategy in physicsStrategies)
        {
            strategy.Update(this);
        }
    }

    #region IUnit
    public override double GetDisplayAngle(double spriteAngle)
    {
        double angle = spriteAngle - this.Angle;

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