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

namespace UIFramework;
//public enum SurfaceCheckMode
//{
//    Ground, // искать поверхность под объектом
//    Ceiling // искать поверхность над объектом
//}
//public class JumpStrategy : IPhysicsUpdateStrategy
//{
//    public static double FindGroundHeight(SurfaceCheckMode mode, IObject subject, Box subjectBox, List<IObject> ignoreList)
//    {
//        return FindGroundHeight(mode, subject, subjectBox, subject.Z.Axis, ignoreList);
//    }

//    public static double FindGroundHeight(SurfaceCheckMode mode, IObject subject, Box subjectBox, double newZ, List<IObject> ignoreList)
//    {
//        var defaultValue = mode == SurfaceCheckMode.Ground ? double.MinValue : double.MaxValue;
//        if (subject.Map is null || subject is null)
//            return defaultValue;

//        ignoreList ??= new List<IObject>();


//        double highestSurface = defaultValue;
//        var (cellX, cellY) = Screen.Mapping(subject.X.Axis, subject.Y.Axis);

//        int minX = cellX - 200;
//        int maxX = cellX + 200;
//        int minY = cellY - 200;
//        int maxY = cellY + 200;

//        var (subMinX, subMaxX) = CollisionHelper.GetBounds(subjectBox, CoordinatePlane.X);
//        var (subMinY, subMaxY) = CollisionHelper.GetBounds(subjectBox, CoordinatePlane.Y);

//        var (subMinZ, subMaxZ) = CollisionHelper.GetBounds(subjectBox, CoordinatePlane.Z);
//        if (newZ != subject.Z.Axis)
//        {
//            subMinZ = newZ + (subjectBox[CoordinatePlane.Z, SideSize.Smaller]?.Offset ?? 0);
//            subMaxZ = newZ + (subjectBox[CoordinatePlane.Z, SideSize.Larger]?.Offset ?? 0);
//        }

//        for (int x = minX; x <= maxX; x += Screen.Setting.Tile)
//        {
//            for (int y = minY; y <= maxY; y += Screen.Setting.Tile)
//            {
//                if (subject.Map.Obstacles is null ||
//                    !subject.Map.Obstacles.TryGetValue((x, y), out var obstacles) ||
//                    obstacles is null)
//                {
//                    continue;
//                }

//                foreach (var obstacle in obstacles.Keys)
//                {
//                    if (obstacle is null || obstacle == subject || ignoreList.Contains(obstacle))
//                        continue;

//                    var targetBox = obstacle.HitBox.MainHitBox;
//                    if (targetBox is null)
//                        continue;

//                    var (tgtMinX, tgtMaxX) = CollisionHelper.GetBounds(targetBox, CoordinatePlane.X);
//                    var (tgtMinY, tgtMaxY) = CollisionHelper.GetBounds(targetBox, CoordinatePlane.Y);
//                    var (tgtMinZ, tgtMaxZ) = CollisionHelper.GetBounds(targetBox, CoordinatePlane.Z);

//                    bool intersectsXY =
//                        subMaxX >= tgtMinX &&
//                        subMinX <= tgtMaxX &&
//                        subMaxY >= tgtMinY &&
//                        subMinY <= tgtMaxY;

//                    if (!intersectsXY)
//                        continue;

//                    if (mode == SurfaceCheckMode.Ground &&
//                        subMinZ >= tgtMaxZ && tgtMaxZ > highestSurface)
//                    {
//                        highestSurface = tgtMaxZ;
//                    }

//                    else if (mode == SurfaceCheckMode.Ceiling &&
//                         subMaxZ >= tgtMinZ && subMaxZ <= tgtMaxZ && highestSurface > tgtMinZ)
//                    {

//                        highestSurface = tgtMinZ;
//                    }
//                }
//            }
//        }

//        return highestSurface;
//    }

//    public void Update(IObject obj)
//    {
//        if (obj is not IJumper jumper || !jumper.HasGravity)
//            return;

//        float deltaTime = (float)FPS.GetDeltaTime();

//        var ignoreList = obj is IMovable movable ? movable.IgnoreCollisionObjects.Keys.ToList() : new();
//        float surfaceZ = (float)FindGroundHeight(SurfaceCheckMode.Ground, obj, obj.HitBox.MainHitBox, ignoreList);

//        switch (jumper.GroundState)
//        {
//            case GroundState.Jumping:
//                HandleJumping(jumper, obj, deltaTime, ignoreList);
//                break;

//            case GroundState.Falling:
//                HandleFalling(jumper, obj, deltaTime, surfaceZ);
//                break;

//            default:
//                HandleGroundCheck(jumper, obj, surfaceZ);
//                break;
//        }
//    }

//    private void HandleJumping(IJumper jumper, IObject obj, double deltaTime, List<IObject> ignoreList)
//    {
//        jumper.JumpElapsed += (float)deltaTime;

//        float t = jumper.JumpElapsed / jumper.JumpDuration;
//        if (t > 1f) t = 1f;

//        var baseZ = obj.Z.Axis;
        
//        var g = FindGroundHeight(SurfaceCheckMode.Ceiling, obj, obj.HitBox.MainHitBox, jumper.InitialJumpHeight + jumper.JumpHeight * 4 * t * (1 - t), ignoreList);

//        if (g != double.MaxValue)
//        {        
//            jumper.GroundState = GroundState.Falling;
//            return;
//        }
//        else
//            obj.Z.Axis = jumper.InitialJumpHeight + jumper.JumpHeight * 4 * t * (1 - t);

//        if (t >= jumper.JumpApexTime)
//        {
//            jumper.GroundState = GroundState.Falling;
//        }
//    }
//    public static double FindCeilingHeight(IObject subject, Box subjectBox, double newZ, List<IObject> ignoreList)
//    {
//        if (subject == null || subject.Map == null)
//        {
//            return double.MaxValue;
//        }

//        if (ignoreList == null)
//        {
//            ignoreList = new List<IObject>();
//        }

//        double num = double.MinValue;
//        (int, int) tuple = Screen.Mapping(subject.X.Axis, subject.Y.Axis);
//        int item = tuple.Item1;
//        int item2 = tuple.Item2;
//        int num2 = item - 200;
//        int num3 = item + 200;
//        int num4 = item2 - 200;
//        int num5 = item2 + 200;
//        (double Min, double Max) bounds = CollisionHelper.GetBounds(subjectBox, CoordinatePlane.X);
//        double item3 = bounds.Min;
//        double item4 = bounds.Max;
//        (double Min, double Max) bounds2 = CollisionHelper.GetBounds(subjectBox, CoordinatePlane.Y);
//        double item5 = bounds2.Min;
//        double item6 = bounds2.Max;
//        double item7 = newZ + (subjectBox[CoordinatePlane.Z, SideSize.Larger]?.Offset ?? 0);// CollisionHelper.GetBounds(subjectBox, CoordinatePlane.Z).Max;// CollisionHelper.GetBounds(subjectBox, CoordinatePlane.Z).Max;

//        for (int i = num2; i <= num3; i += Screen.Setting.Tile)
//        {
//            for (int j = num4; j <= num5; j += Screen.Setting.Tile)
//            {
//                if (subject.Map.Obstacles == null || !subject.Map.Obstacles.TryGetValue((i, j), out ConcurrentDictionary<IObject, byte> value) || value == null)
//                {
//                    continue;
//                }

//                foreach (IObject key in value.Keys)
//                {
//                    if (key == null || key == subject || ignoreList.Contains(key))
//                    {
//                        continue;
//                    }

//                    Box mainHitBox = key.HitBox.MainHitBox;
//                    if (mainHitBox != null)
//                    {
//                        (double Min, double Max) bounds3 = CollisionHelper.GetBounds(mainHitBox, CoordinatePlane.X);
//                        double item8 = bounds3.Min;
//                        double item9 = bounds3.Max;
//                        (double Min, double Max) bounds4 = CollisionHelper.GetBounds(mainHitBox, CoordinatePlane.Y);
//                        double item10 = bounds4.Min;
//                        double item11 = bounds4.Max;
//                        double item12 = CollisionHelper.GetBounds(mainHitBox, CoordinatePlane.Z).Min;
//                        double item13 = CollisionHelper.GetBounds(mainHitBox, CoordinatePlane.Z).Max;

//                        if (item4 >= item8 && item3 <= item9 && item6 >= item10 && item5 <= item11 && item7 >= item12 && item7 <= item13 && num < item13)
//                        {

//                            num = item12;
//                            Console.WriteLine(num);
//                        }
//                    }
//                }
//            }
//        }
//        return (num == double.MinValue) ? double.MaxValue : num;
//    }
//    private void HandleFalling(IJumper jumper, IObject obj, double deltaTime, double surfaceZ)
//    {
//        if (surfaceZ > jumper.GroundLevel)
//            surfaceZ += GetBottomOffset(jumper, obj);
//        else
//            surfaceZ = jumper.GroundLevel;

//        jumper.CurrentJumpForce -= (float)(jumper.Gravity * deltaTime);
//        var newZ = obj.Z.Axis + jumper.CurrentJumpForce * deltaTime;

//        if (newZ <= surfaceZ)
//        {
//            newZ = surfaceZ;
//            jumper.GroundState = GroundState.OnGround;
//            jumper.CurrentJumpForce = 0;
//        }


//        obj.Z.Axis = newZ;
//    }

//    private void HandleGroundCheck(IJumper jumper, IObject obj, double surfaceZ)
//    {
//        if (surfaceZ > jumper.GroundLevel)
//            surfaceZ += GetBottomOffset(jumper, obj);
//        else
//            surfaceZ = jumper.GroundLevel;

//        if (obj.Z.Axis > surfaceZ)
//        {
//            jumper.GroundState = GroundState.Falling;
//            jumper.CurrentJumpForce = 0;
//        }
//        else
//        {
//            obj.Z.Axis = surfaceZ;
//            jumper.GroundState = GroundState.OnGround;
//            jumper.CurrentJumpForce = 0;
//        }
//    }

//    private double GetBottomOffset(IJumper jumper, IObject obj)
//    {
//        return (obj.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.OriginalOffset ?? 0) + jumper.GroundClearance;
//    }
//}

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
    public float JumpDuration { get; set; } = 0.2f;
    public float JumpHeight { get; set; } = 1500;
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
    public Unit(SpriteObstacle obstacle, int maxHp, bool createNewTexture = true)
       : base(obstacle, createNewTexture)
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
       : base(obstacle, createNewTexture)
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