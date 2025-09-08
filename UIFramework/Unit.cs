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

namespace UIFramework;


public class Unit : SpriteObstacle, IUnit, IDamageable, IDialogObject, IJumper, IKnockbackable
{
    public Vector2f Vector2 { get; set; } = new Vector2f();
    public ControlState ControlState { get; set; } = ControlState.Normal;
    public GroundState GroundState { get; set; } = GroundState.OnGround;


    public float KnockbackPower { get; set; } = 10000;
    public float KnockbackAngle { get; set; }
    public Vector2f Velocity { get; set; }
    public bool HasGravity { get; set; } = true;
    public float KnockbackVelocityEpsilon { get; set; } = 1;
    public float GroundLevel { get; set; } = 0;
    public float JumpElapsed { get; set; } = 0;
    public float JumpDuration { get; set; } = 0.1f;
    public float JumpHeight { get; set; } = 500;
    public float CurrentJumpForce { get; set; } = 1000;
    public float InitialJumpHeight { get; set; } = 0;
    public float Gravity { get; } = 500000;
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
    public Stat Hp { get; set; }
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

    #region IDialogObject
    public UISprite? DialogSprite { get; set; } = null;
    public UIText? DisplayName { get; set; } = null;
    #endregion

    #region IControlHandler
    public ControlLib.Control Control { get; init; } = new ControlLib.Control();
    #endregion
    public Unit(SpriteObstacle obstacle, int maxHp, bool createNewTexture = true)
       : base(obstacle, true)
    {
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
    public Unit(IMap map, SpriteObstacle obstacle, int maxHp, bool createNewTexture = true)
       : base(obstacle, false)
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
    public Unit(Unit unit, bool updateTexture = true)
       : base(unit, false)
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
//using ObstacleLib.SpriteLib;
//using ScreenLib;
//using SFML.System;
//using ProtoRender.Object;
//using ProtoRender.Map;
//using HitBoxLib.Data.Observer;
//using System.Collections.Concurrent;
//using InteractionFramework.Death;
//using InteractionFramework.VisualImpact;
//using InteractionFramework.VisualImpact.Data;
//using InteractionFramework.Dialog;
//using UIFramework.Dialog;
//using UIFramework.Sprite;
//using UIFramework.Text;
//using ObjectFramework;
//using InteractionFramework;
//using FpsLib;
//using HitBoxLib.PositionObject;
//using MoveLib.Move;
//using HitBoxLib.HitBoxSegment;
//using HitBoxLib.Segment.SignsTypeSide;
//using MoveLib.Physics;
//using ProtoRender.Physics;
//using MoveLib;
//using RayTracingLib;
//using InteractionFramework.Trigger.TriggerTouch;
//using System.Numerics;

//namespace UIFramework;
//public class KnockbackStrategy : IPhysicsUpdateStrategy
//{

//    public void Update(IObject obj)
//    {
//        if (obj is not IKnockbackable knockbackable)
//            return;
//        else if (knockbackable.ControlState is not ControlState.Knockback)
//            return;

//        float deltaTime = (float)FPS.GetDeltaTime();

//        float deltaX = knockbackable.Velocity.X * deltaTime;
//        float deltaY = knockbackable.Velocity.Y * deltaTime;

//        List<IObject> ignoreList = obj is IMovable movable ? movable.IgnoreCollisionObjects.Keys.ToList() : new List<IObject>();
//        var collided = Collision.GetCollision(obj, deltaX, deltaY, ignoreList);


//        if (collided.CollisionObject is not null && collided.CollisionCoordinate.HasValue)
//        {
//            Collision.ResolvePositionCollision(obj, collided.CollisionObject, collided.CollisionCoordinate.Value, deltaX, deltaY);

//            knockbackable.Velocity = new Vector2f(0, 0);
//            knockbackable.ControlState = ControlState.Normal;
//        }
//        else
//        {
//            obj.X.Axis += deltaX;
//            obj.Y.Axis += deltaY;

//            knockbackable.Velocity *= knockbackable.Friction;

//            float velocityLength = MathF.Sqrt(knockbackable.Velocity.X * knockbackable.Velocity.X + knockbackable.Velocity.Y * knockbackable.Velocity.Y);
//            if (velocityLength < 0.1f)
//            {
//                knockbackable.Velocity = new Vector2f(0, 0);
//                knockbackable.ControlState = ControlState.Normal;
//            }
//        }
//    }
//}
//public class Unit : SpriteObstacle, IUnit, IDamageable, IDialogObject, IJumper, IKnockbackable
//{
//    public Vector2f Vector2 { get; set; } = new Vector2f();
//    public ControlState ControlState { get; set; } = ControlState.Normal;
//    public GroundState GroundState { get; set; } = GroundState.OnGround;


//    public float KnockbackPower { get; set; } = 10000;
//    public float KnockbackAngle { get; set; }
//    public Vector2f Velocity { get; set; }
//    public float GroundLevel { get; set; } = 0;
//    public float JumpElapsed { get; set; } = 0;
//    public float JumpDuration { get; set; } = 0.1f;
//    public float JumpHeight { get; set; } = 500;
//    public float CurrentJumpForce { get; set; } = 1000;
//    public float InitialJumpHeight { get; set; } = 0;
//    public float Gravity { get; } = 500000;
//    public float Friction { get; } = 0.6f;

//    /// <summary>
//    /// Specifies the minimum distance (in units) above the surface
//    /// at which the object is positioned after landing or when standing on the ground.
//    /// Default is 1.
//    /// </summary>
//    public float GroundClearance { get; set; } = 1;

//    /// <summary>
//    /// The relative time (from 0 to 1) at which the jump reaches its peak height.
//    /// By default, 0.5 means the apex occurs at the midpoint of the jump duration.
//    /// Adjust this value to make the apex earlier or later.
//    /// </summary>
//    public float JumpApexTime { get; set; } = 0.5f;




//    #region IMoveble
//    public float MoveSpeed { get; set; } = 200f;
//    public double MoveSpeedAngel { get; set; } = 1;
//    public double TempVerticalAngle { get; set; } = 0.0;
//    public double TempAngle { get; set; } = 0.0;
//    public float MinDistanceFromWall { get; set; } = 100;
//    public float MouseSensitivity { get; set; } = 0.001f;
//    public bool IsMouseCaptured { get; set; } = true;

//    public double MinVerticalAngle { get; set; } = -Math.PI / 2;
//    public double MaxVerticalAngle { get; set; } = Math.PI / 2;
//    public ConcurrentDictionary<IObject, byte> IgnoreCollisionObjects { get; set; } = new();
//    #endregion

//    #region IUnit
//    //-----------------------Angle-----------------------
//    /// <summary>X - Cos(Angle); Y - Sin(Angle)</summary>
//    public Vector2f Direction { get; protected set; }
//    /// <summary>X - -Sin(Angle); Y - Cos(Angle)</summary>
//    public Vector2f Plane { get; protected set; }

//    private double _angle;
//    /// <summary>Angle Entity(Horizontal axis)</summary>
//    public double Angle
//    {
//        get => _angle;
//        set
//        {
//            if (_angle != value)
//            {
//                _angle = value;

//                float cos = (float)Math.Cos(_angle);
//                float sin = (float)Math.Sin(_angle);
//                Direction = new Vector2f(cos, sin);
//                Plane = new Vector2f(-sin, cos);
//            }
//        }
//    }
//    /// <summary>Vertical Angle Entity(Vertical axis)</summary>
//    public double VerticalAngle { get; set; }
//    public Vector2f OriginPosition
//    {
//        get
//        {
//            return new Vector2f((float)X.Axis, (float)Y.Axis);
//        }
//    }
//    private double _fov;
//    public double Fov
//    {
//        get => _fov;
//        set
//        {
//            _fov = value;
//            HalfFov = value / 2;
//        }
//    }
//    /// <summary>Half Fov Entity</summary>
//    public double HalfFov { get; private set; }


//    //---------------------Ray Setting----------------------
//    /// <summary>Maximum rendering distance(depends on Screen.Setting.Tile)</summary>
//    public int MaxRenderTile { get; set; }

//    //---------------------Render Setting----------------------
//    /// <summary>This is the angle between adjacent rays in the rendering system</summary>
//    public double DeltaAngle { get; private set; }
//    /// <summary>Projected height(depends on the distance between the Entity and the object)</summary>
//    public double ProjCoeff { get; private set; }
//    #endregion

//    #region IDamageable
//    public Stat Hp { get; set; }
//    public DeathEffect? DeathAnimation { get; set; } = null;
//    private void ClearingDataAfteDeath()
//    {
//        Map?.DeleteObstacle(this);
//        SpritesToRender.Remove(this);

//        Map = null;

//        if (DeathAnimation is not null)
//            BeyondRenderManager.Create(this, new DeathData(this, DeathAnimation));
//    }
//    #endregion

//    #region IDialogObject
//    public UISprite? DialogSprite { get; set; } = null;
//    public UIText? DisplayName { get; set; } = null;
//    #endregion

//    #region IControlHandler
//    public ControlLib.Control Control { get; init; } = new ControlLib.Control();
//    public float KnockbackVelocityEpsilon { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//    #endregion

//    //public Unit(SpriteObstacle obstacle, int maxHp)
//    //    : base(obstacle)
//    //{
//    //    Map = map;
//    //    Hp = new Stat(maxHp);

//    //    Hp.OnDepleted += ClearingDataAfteDeath;

//    //    Fov = Math.PI / 3;
//    //    HalfFov = (float)Fov / 2;

//    //    Angle = 0;
//    //    Angle -= 0.000001;
//    //    VerticalAngle -= 0.000001;
//    //    MaxRenderTile = 1200;
//    //    ObserverSettingChangesFun();
//    //    Screen.WidthChangesFun += ObserverSettingChangesFun;
//    //}
//    public Unit(SpriteObstacle obstacle, int maxHp, bool createNewTexture = true)
//       : base(obstacle, true)
//    {
//        Animation = obstacle.Animation;
//        Hp = new Stat(maxHp);

//        Hp.OnDepleted += ClearingDataAfteDeath;

//        Fov = Math.PI / 3;
//        HalfFov = (float)Fov / 2;

//        Angle = 0;
//        Angle -= 0.000001;
//        VerticalAngle -= 0.000001;
//        MaxRenderTile = 1200;
//        ObserverSettingChangesFun();
//        Screen.WidthChangesFun += ObserverSettingChangesFun;
//        physicsStrategies = new List<IPhysicsUpdateStrategy>
//        {
//            new JumpStrategy(),
//            new KnockbackStrategy(),
//        };
//        PhysicsHandler.Register(this);
//    }
//    public Unit(IMap map, SpriteObstacle obstacle, int maxHp, bool createNewTexture = true)
//       : base(obstacle, false)
//    {
//        Map = map;
//        Animation = obstacle.Animation;
//        Hp = new Stat(maxHp);

//        Hp.OnDepleted += ClearingDataAfteDeath;

//        Fov = Math.PI / 3;
//        HalfFov = (float)Fov / 2;

//        Angle = 0;
//        Angle -= 0.000001;
//        VerticalAngle -= 0.000001;
//        MaxRenderTile = 1200;
//        ObserverSettingChangesFun();
//        Screen.WidthChangesFun += ObserverSettingChangesFun;
//        physicsStrategies = new List<IPhysicsUpdateStrategy>
//        {
//            new JumpStrategy(),
//            new KnockbackStrategy(),
//        };
//        PhysicsHandler.Register(this);
//    }
//    public Unit(Unit unit, bool updateTexture = true)
//       : base(unit, false)
//    {
//        physicsStrategies = new List<IPhysicsUpdateStrategy>
//        {
//            new JumpStrategy(),
//            new KnockbackStrategy(),
//        };
//        PhysicsHandler.Register(this);
//        Map = unit.Map;
//        Hp = unit.Hp;

//        Fov = unit.Fov;
//        HalfFov = unit.HalfFov;

//        Angle = unit.Angle;
//        TempAngle = unit.TempAngle;

//        VerticalAngle = unit.VerticalAngle;
//        TempVerticalAngle = unit.TempVerticalAngle;
//        MaxVerticalAngle = unit.MaxVerticalAngle;
//        MinVerticalAngle = unit.MinVerticalAngle;

//        MaxRenderTile = unit.MaxRenderTile;
//        MoveSpeed = unit.MoveSpeed;
//        MouseSensitivity = unit.MouseSensitivity;
//        MoveSpeedAngel = unit.MoveSpeedAngel;
//        MinDistanceFromWall = unit.MinDistanceFromWall;

//        Hp.OnDepleted += ClearingDataAfteDeath;
//        ObserverSettingChangesFun();
//        Screen.WidthChangesFun += ObserverSettingChangesFun;
//    }


//    private List<IPhysicsUpdateStrategy> physicsStrategies;
//    public void Jump()
//    {
//        if (GroundState is not GroundState.OnGround) return;

//        GroundState = GroundState.Jumping;
//        JumpElapsed = 0f;
//        InitialJumpHeight = (float)Z.Axis;
//    }
//    public void Knockback()
//    {
//        float knockbackAngle = KnockbackAngle;
//        Vector2f direction = new Vector2f(MathF.Cos(knockbackAngle), MathF.Sin(knockbackAngle));

//        Velocity = direction * KnockbackPower;
//        ControlState = ControlState.Knockback;
//    }

//    public void UpdatePhysics()
//    {
//        foreach (var strategy in physicsStrategies)
//        {
//            strategy.Update(this);
//        }
//    }

//    #region IUnit
//    public void ObserverSettingChangesFun()
//    {
//        float dist = Screen.Setting.AmountRays / (2 * (float)Math.Tan(HalfFov));
//        ProjCoeff = dist * Screen.Setting.Tile;

//        DeltaAngle = (float)Fov / Screen.Setting.AmountRays;
//    }
//    public ObserverInfo GetObserverInfo()
//    {
//        ObserverInfo observerInfo = new ObserverInfo();
//        observerInfo.fov = Fov;
//        observerInfo.verticalAngle = VerticalAngle;
//        observerInfo.angle = Angle;
//        observerInfo.deltaAngle = DeltaAngle;
//        observerInfo.position = new Vector3f((float)X.Axis, (float)Y.Axis, (float)Z.Axis);

//        return observerInfo;
//    }
//    public new IUnit GetCopy()
//    {
//        return new Unit(this);
//    }
//    #endregion
//}
//public static class Collision
//{
//    private static int _radiusCheckTouch = Screen.Setting.Tile;

//    /// <summary>
//    /// Gets or sets the tile-based radius used to check nearby obstacles for collision.
//    /// </summary>
//    public static int RadiusCheckTouch
//    {
//        get => _radiusCheckTouch;
//        set => _radiusCheckTouch = value * Screen.Setting.Tile;
//    }

//    private static int _stepCheckTouch = Screen.Setting.Tile;
//    public static int StepCheckTouch
//    {
//        get => _stepCheckTouch;

//        set => _stepCheckTouch = value;
//    }


//    private static int _iterations = 20;
//    public static int Iterations
//    {
//        get => _iterations;
//        set => _iterations = value;
//    }


//    #region Detection 
//    /// <summary>
//    /// Checks if there is any collision around the unit at the specified next position.
//    /// Iterates over nearby obstacles within a radius and returns true if a collision is detected.
//    /// </summary>
//    /// <param name="subject">The IObject to check collisions for.</param>
//    /// <param name="nextX">The candidate next X position of the unit.</param>
//    /// <param name="nextY">The candidate next Y position of the unit.</param>
//    /// <param name="ignoreList">List of objects to ignore in collision checks.</param>
//    /// <param name="ignorePassability">If true, ignores obstacle passability during collision checks.</param>
//    /// <returns>True if a collision is detected; otherwise false.</returns>
//    public static bool IsCollisionNear(IObject subject, double nextX, double nextY, List<IObject> ignoreList, bool ignorePassability = false)
//    {
//        if (subject.Map is null)
//            return false;

//        var (cellX, cellY) = Screen.Mapping(subject.X.Axis, subject.Y.Axis);

//        int minX = cellX - RadiusCheckTouch;
//        int maxX = cellX + RadiusCheckTouch;
//        int minY = cellY - RadiusCheckTouch;
//        int maxY = cellY + RadiusCheckTouch;

//        for (int x = minX; x <= maxX; x += Screen.Setting.Tile)
//        {
//            for (int y = minY; y <= maxY; y += Screen.Setting.Tile)
//            {
//                if (!subject.Map.Obstacles.TryGetValue((x, y), out var value))
//                    continue;

//                foreach (var obstacle in value.Keys)
//                {
//                    if (obstacle == subject || ignoreList.Contains(obstacle))
//                        continue;

//                    if (!obstacle.IgnoreCollisonMainBox)
//                    {
//                        if (CollisionHelper.IsCollidingAtPosition(subject, obstacle, obstacle.HitBox.MainHitBox, nextX, nextY, ignorePassability))
//                            return true;
//                    }
//                    foreach (var box in obstacle.HitBox.SegmentedHitbox)
//                    {
//                        if (CollisionHelper.IsCollidingAtPosition(subject, obstacle, box, nextX, nextY, ignorePassability))
//                            return true;
//                    }
//                }
//            }
//        }

//        return false;
//    }

//    /// <summary>
//    /// Finds the first obstacle the unit will collide with at the specified next position, and returns the collision point.
//    /// </summary>
//    /// <param name="subject">The unit to check collisions for.</param>
//    /// <param name="nextX">The candidate next X position of the unit.</param>
//    /// <param name="nextY">The candidate next Y position of the unit.</param>
//    /// <param name="ignoreList">List of units to ignore in collision checks.</param>
//    /// <param name="ignorePassability">If true, ignores obstacle passability during collision checks.</param>
//    /// <returns>A tuple containing the colliding object and the collision point, or (null, null) if no collision.</returns>
//    public static CollisionResult GetCollisionNear(IObject subject, double nextX, double nextY, List<IObject> ignoreList, bool ignorePassability = false)
//    {
//        if (subject.Map is null)
//            return default;

//        var (cellX, cellY) = Screen.Mapping(subject.X.Axis, subject.Y.Axis);

//        int minX = cellX - RadiusCheckTouch;
//        int maxX = cellX + RadiusCheckTouch;
//        int minY = cellY - RadiusCheckTouch;
//        int maxY = cellY + RadiusCheckTouch;

//        for (int x = minX; x <= maxX; x += Screen.Setting.Tile)
//        {
//            for (int y = minY; y <= maxY; y += Screen.Setting.Tile)
//            {
//                if (!subject.Map.Obstacles.TryGetValue((x, y), out var value))
//                    continue;

//                foreach (var obstacle in value.Keys)
//                {
//                    if (obstacle == subject || ignoreList.Contains(obstacle))
//                        continue;

//                    if (!obstacle.IgnoreCollisonMainBox)
//                    {
//                        var point = CollisionHelper.GetCollisionPointAt(subject, obstacle, obstacle.HitBox.MainHitBox, nextX, nextY, ignorePassability);
//                        if (point != null)
//                            return new(obstacle, point);
//                    }
//                    foreach (var box in obstacle.HitBox.SegmentedHitbox)
//                    {
//                        var pointSegment = CollisionHelper.GetCollisionPointAt(subject, obstacle, box, nextX, nextY, ignorePassability);
//                        if (pointSegment != null)
//                            return new(obstacle, pointSegment);
//                    }
//                }
//            }
//        }

//        return default;
//    }

//    /// <summary>
//    /// Finds the highest surface below the given object within a search radius.
//    /// Returns the height (Z) of the surface that supports the object.
//    /// </summary>
//    public static double FindGroundHeight(IObject subject, Box subjectBox)
//    {
//        if (subject.Map == null)
//            return 0;

//        double highestSurface = double.MinValue;
//        var (cellX, cellY) = Screen.Mapping(subject.X.Axis, subject.Y.Axis);

//        int minX = cellX - RadiusCheckTouch;
//        int maxX = cellX + RadiusCheckTouch;
//        int minY = cellY - RadiusCheckTouch;
//        int maxY = cellY + RadiusCheckTouch;

//        var (subMinX, subMaxX) = CollisionHelper.GetBounds(subjectBox, CoordinatePlane.X);
//        var (subMinY, subMaxY) = CollisionHelper.GetBounds(subjectBox, CoordinatePlane.Y);
//        var (subMinZ, _) = CollisionHelper.GetBounds(subjectBox, CoordinatePlane.Z);

//        for (int x = minX; x <= maxX; x += Screen.Setting.Tile)
//        {
//            for (int y = minY; y <= maxY; y += Screen.Setting.Tile)
//            {
//                if (!subject.Map.Obstacles.TryGetValue((x, y), out var obstacles))
//                    continue;

//                foreach (var obstacle in obstacles.Keys)
//                {
//                    if (obstacle == subject)
//                        continue;

//                    var targetBox = obstacle.HitBox.MainHitBox;

//                    var (tgtMinX, tgtMaxX) = CollisionHelper.GetBounds(targetBox, CoordinatePlane.X);
//                    var (tgtMinY, tgtMaxY) = CollisionHelper.GetBounds(targetBox, CoordinatePlane.Y);
//                    var (_, tgtMaxZ) = CollisionHelper.GetBounds(targetBox, CoordinatePlane.Z);

//                    bool intersectsXY =
//                        subMaxX >= tgtMinX &&
//                        subMinX <= tgtMaxX &&
//                        subMaxY >= tgtMinY &&
//                        subMinY <= tgtMaxY;

//                    if (!intersectsXY)
//                        continue;

//                    if (subMinZ >= tgtMaxZ && tgtMaxZ > highestSurface)
//                    {
//                        highestSurface = tgtMaxZ;
//                    }
//                }
//            }
//        }

//        return highestSurface == double.MinValue ? 0 : highestSurface;
//    }



//    private static Vector3f FindSafePosition(
//    IObject subject,
//    double safeX, double safeY,
//    double collX, double collY,
//    List<IObject> ignoreList,
//    bool ignorePassability)
//    {
//        double startX = safeX;
//        double startY = safeY;

//        // конечная точка, где обнаружена коллизия
//        double endX = collX;
//        double endY = collY;

//        // выставляем левую и правую границу поиска
//        double leftX = startX;
//        double leftY = startY;
//        double rightX = endX;
//        double rightY = endY;

//        float Z = (float)subject.Z.Axis;

//        for (int i = 0; i < Iterations; i++)
//        {
//            double midX = (leftX + rightX) / 2.0;
//            double midY = (leftY + rightY) / 2.0;

//            var collisionResult = GetCollisionNear(subject, midX, midY, ignoreList, ignorePassability);
//            Z = collisionResult.CollisionCoordinate.HasValue
//                ? collisionResult.CollisionCoordinate.Value.Z
//                : Z;

//            if (collisionResult.CollisionObject is not null &&
//                collisionResult.CollisionCoordinate is not null)
//            {
//                // если коллизия есть — ищем левее
//                rightX = midX;
//                rightY = midY;
//            }
//            else
//            {
//                // если коллизии нет — запоминаем как безопасную и идем правее
//                leftX = midX;
//                leftY = midY;
//            }
//        }

//        return new Vector3f((float)leftX, (float)leftY, Z);
//    }

//    public static CollisionResult TraceCollision(
//    IObject subject,
//    double nextX,
//    double nextY,
//    List<IObject> ignoreList,
//    bool ignorePassability = false)
//    {
//        if (subject.Map is null)
//            return default;

//        double startX = subject.X.Axis;
//        double startY = subject.Y.Axis;

//        double dx = nextX - startX;
//        double dy = nextY - startY;
//        double distance = Math.Sqrt(dx * dx + dy * dy);

//        if (distance == 0)
//            return default;

//        double dirX = dx / distance;
//        double dirY = dy / distance;

//        double lastSafeX = startX;
//        double lastSafeY = startY;

//        for (double travelled = 0; travelled <= distance; travelled += StepCheckTouch)
//        {
//            double checkX = startX + dirX * travelled;
//            double checkY = startY + dirY * travelled;

//            var collisionResult = GetCollisionNear(subject, checkX, checkY, ignoreList, ignorePassability);

//            if (collisionResult.CollisionObject is not null &&
//                collisionResult.CollisionCoordinate is not null)
//            {
//                var safePoint = FindSafePosition(subject, lastSafeX, lastSafeY, checkX, checkY, ignoreList, ignorePassability);
//                return new(collisionResult.CollisionObject, safePoint);
//            }

//            lastSafeX = checkX;
//            lastSafeY = checkY;
//        }

//        return default;
//    }
//    #endregion

//    #region Handle
//    public static void ResolvePositionCollision(
//    IObject subject,
//    IObject target,
//    Vector3f safePoint,
//    double deltaX,
//    double deltaY)
//    {
//        float minDistFromWall = subject is IMovable movable ? movable.MinDistanceFromWall / 2 : 0;

//        double targetX = subject.X.Axis + deltaX + (minDistFromWall * Math.Sign(deltaX));
//        double targetY = subject.Y.Axis + deltaY + (minDistFromWall * Math.Sign(deltaY));


//        double originalX = subject.X.Axis;
//        double originalY = subject.Y.Axis;
//        var closest = new Vector2f(
//            Math.Clamp((float)target.X.Axis,
//                (float)(subject.HitBox[CoordinatePlane.X, SideSize.Smaller]?.Side ?? 0f),
//                (float)(subject.HitBox[CoordinatePlane.X, SideSize.Larger]?.Side ?? 0f)),
//            Math.Clamp((float)target.Y.Axis,
//                (float)(subject.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.Side ?? 0f),
//                (float)(subject.HitBox[CoordinatePlane.Y, SideSize.Larger]?.Side ?? 0f))
//        );


//        subject.X.Axis += deltaX;
//        if (IsCollisionNear(subject, subject.X.Axis, subject.Y.Axis, new List<IObject>()))
//        {
//            subject.X.Axis = originalX;
//        }
//        else if ((deltaX > 0 && subject.X.Axis < safePoint.X - closest.X) ||
//                  (deltaX < 0 && subject.X.Axis > safePoint.X + closest.X))
//        {
//            subject.X.Axis = originalX;
//        }

//        subject.Y.Axis += deltaY;
//        if (IsCollisionNear(subject, subject.X.Axis, subject.Y.Axis, new List<IObject>()))
//        {
//            subject.Y.Axis = originalY;
//        }
//        else if ((deltaY > 0 && subject.Y.Axis < safePoint.Y - closest.Y) ||
//                 (deltaY < 0 && subject.Y.Axis > safePoint.Y + closest.Y))
//        {
//            subject.Y.Axis = originalY;
//        }
//    }

//    private static bool HandleCollision(
//    IObject subject,
//    CollisionResult collisionResult,
//    double deltaX,
//    double deltaY)
//    {
//        var obj = collisionResult.CollisionObject;
//        var coo = collisionResult.CollisionCoordinate;

//        if (obj is null)
//        {
//            subject.X.Axis += deltaX;
//            subject.Y.Axis += deltaY;
//            return false;
//        }
//        else
//        {
//            ResolvePositionCollision(subject, obj, coo!.Value, deltaX, deltaY);
//            return true;
//        }
//    }
//    #endregion

//    #region Collision
//    /// <summary>
//    /// Attempts to move the unit by the specified deltas and checks if a collision occurs with obstacles.
//    /// </summary>
//    /// <param name="subject">The IObject to move.</param>
//    /// <param name="nextX">Delta movement in the X direction.</param>
//    /// <param name="nextY">Delta movement in the Y direction.</param>
//    /// <param name="ignoreList">List of objects to ignore during collision detection.</param>
//    /// <param name="ignorePassability">If true, ignores obstacle passability.</param>
//    /// <returns>True if a collision occurred; otherwise, false.</returns>
//    public static bool TryMoveWithCollision(IObject subject, double deltaX, double deltaY, List<IObject> ignoreList, bool ignorePassability = false)
//    {
//        if (deltaX == 0 && deltaY == 0)
//            return false;

//        float minDistFromWall = subject is IMovable movable ? movable.MinDistanceFromWall / 2 : 0;

//        double targetX = subject.X.Axis + deltaX + (minDistFromWall * Math.Sign(deltaX));
//        double targetY = subject.Y.Axis + deltaY + (minDistFromWall * Math.Sign(deltaY));

//        var collisionResult = TraceCollision(subject, targetX, targetY, ignoreList, ignorePassability);

//        return HandleCollision(subject, collisionResult, deltaX, deltaY);
//    }

//    /// <summary>
//    /// Attempts to move the unit by the specified deltas and returns the first obstacle and collision point encountered.
//    /// </summary>
//    /// <param name="subject">The IObject to move.</param>
//    /// <param name="nextX">Delta movement in the X direction.</param>
//    /// <param name="nextY">Delta movement in the Y direction.</param>
//    /// <param name="ignoreList">List of units to ignore during collision detection.</param>
//    /// <param name="ignorePassability">If true, ignores obstacle passability.</param>
//    /// <returns>
//    /// Tuple containing the colliding object and collision point if collision occurs; otherwise, (null, null).
//    /// </returns>
//    public static CollisionResult TryMoveAndGetCollision(IObject subject, double deltaX, double deltaY, List<IObject> ignoreList, bool ignorePassability = false)
//    {
//        if (deltaX == 0 && deltaY == 0)
//            return default;

//        float minDistFromWall = subject is IMovable movable ? movable.MinDistanceFromWall / 2 : 0;

//        double targetX = subject.X.Axis + deltaX + (minDistFromWall * Math.Sign(deltaX));
//        double targetY = subject.Y.Axis + deltaY + (minDistFromWall * Math.Sign(deltaY));

//        var collisionResult = TraceCollision(subject, targetX, targetY, ignoreList, ignorePassability);

//        HandleCollision(subject, collisionResult, deltaX, deltaY);
//        return collisionResult;
//    }

//    /// <summary>
//    /// Attempts to move the unit by the specified deltas and checks if a collision occurs with obstacles.
//    /// </summary>
//    /// <param name="subject">The IObject to move.</param>
//    /// <param name="nextX">Delta movement in the X direction.</param>
//    /// <param name="nextY">Delta movement in the Y direction.</param>
//    /// <param name="ignoreList">List of objects to ignore during collision detection.</param>
//    /// <param name="ignorePassability">If true, ignores obstacle passability.</param>
//    /// <returns>True if a collision occurred; otherwise, false.</returns>
//    public static bool WillCollide(IObject subject, double deltaX, double deltaY, List<IObject> ignoreList, bool ignorePassability = false)
//    {
//        if (deltaX == 0 && deltaY == 0)
//            return false;

//        float minDistFromWall = subject is IMovable movable ? movable.MinDistanceFromWall / 2 : 0;

//        double targetX = subject.X.Axis + deltaX + (minDistFromWall * Math.Sign(deltaX));
//        double targetY = subject.Y.Axis + deltaY + (minDistFromWall * Math.Sign(deltaY));

//        var collisionResult = TraceCollision(subject, targetX, targetY, ignoreList, ignorePassability);

//        return !(collisionResult.CollisionObject is null);
//    }

//    /// <summary>
//    /// Attempts to move the unit by the specified deltas and returns the first obstacle and collision point encountered.
//    /// </summary>
//    /// <param name="subject">The IObject to move.</param>
//    /// <param name="nextX">Delta movement in the X direction.</param>
//    /// <param name="nextY">Delta movement in the Y direction.</param>
//    /// <param name="ignoreList">List of units to ignore during collision detection.</param>
//    /// <param name="ignorePassability">If true, ignores obstacle passability.</param>
//    /// <returns>
//    /// Tuple containing the colliding object and collision point if collision occurs; otherwise, (null, null).
//    /// </returns>
//    public static CollisionResult GetCollision(IObject subject, double deltaX, double deltaY, List<IObject> ignoreList, bool ignorePassability = false)
//    {
//        if (deltaX == 0 && deltaY == 0)
//            return default;

//        float minDistFromWall = subject is IMovable movable ? movable.MinDistanceFromWall / 2 : 0;

//        double targetX = subject.X.Axis + deltaX + (minDistFromWall * Math.Sign(deltaX));
//        double targetY = subject.Y.Axis + deltaY + (minDistFromWall * Math.Sign(deltaY));

//        var collisionResult = TraceCollision(subject, targetX, targetY, ignoreList, ignorePassability);
//        return collisionResult;
//    }
//    #endregion
//}
//public record struct CollisionResult(IObject? CollisionObject, Vector3f? CollisionCoordinate);
