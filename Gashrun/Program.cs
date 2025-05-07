using ScreenLib;
using MapLib;
using ObstacleLib;
using ObstacleLib.TexturedWallLib;
using TextureLib;
using RenderLib;
using EntityLib;
using EntityLib.Player;
using ControlLib;
using FpsLib;
using SFML.Graphics;
using static EntityLib.Player.Player;
using SFML.System;
using PartsWorldLib;
using PartsWorldLib.Up;
using PartsWorldLib.Down;
using MiniMapLib;
using MiniMapLib.ObjectInMap.Positions;
using MoveLib.Angle;
using MoveLib.Move;
using ProtoRender.Object;
using UIFramework.Sights;
using UIFramework.Sights.Crosses;
using UIFramework.Render;
using UIFramework.IndicatorsBar;
using UIFramework.IndicatorsBar.Content;
using HitBoxLib.PositionObject;
using RayTracingLib.Detection;
using RayTracingLib;
using DrawLib;
using UIFramework.Animation;
using ProtoRender.WindowInterface;
using UIFramework.Weapon;
using UIFramework.Weapon.Patron;
using HitBoxLib.HitBoxSegment;
using HitBoxLib.Segment.SignsTypeSide;
using ObstacleLib.SpriteLib;
using HitBoxLib.Data.Observer;
using ProtoRender.RenderAlgorithm;
using System;
using HitBoxLib.Data.HitBoxObject;
using System.Runtime.Intrinsics.X86;
using ObjectFramework;
using AnimationLib;
using EffectLib;
using ProtoRender.RenderInterface;
using RenderLib.HitBox;
using DataPipes;
using HitBoxLib.Operations;
using EffectLib.Effect;
using System.Numerics;
using UIFramework.Weapon.Bullet;
using UIFramework.Weapon.BulletMagazine;
using DataPipes.Pool;
using NGenerics.Extensions;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.InteropServices;
using ObstacleLib.SpriteLib.Add;
using System.Linq;
using System.Reflection.Metadata;
using System.Diagnostics;

Screen.Initialize(1000, 600);

string mainFillWall32 = Path.Combine("Resources", "Image", "Sprite", "Devil");
//MoveLib.Setting.MoveSpeed = 200f;
//RayTracingLib.Raycast.ScanRadius
DateTime from = DateTime.Now;
string mainBold = Path.Combine("Resources", "FontText", "ArialBold.ttf");
FPS fpsChecker = new FPS("FPS: ", 24, new Vector2f(10, 10), ResourceManager.GetMainPath(mainBold), Color.White);
MoveLib.Move.Collision.RadiusCheckTouch = 2;
string mainFillWall = Path.Combine("Resources", "Image", "WallTexture", "Wall1.png");
var ffff = new TexturedWall(ResourceManager.GetMainPath(mainFillWall));
ffff.Z.Axis = 0;

Map map = new(ffff, 10, 10);
SpriteObstacle sprite1 = new SpriteObstacle(ResourceManager.GetMainPath(mainFillWall32), true)
{
    Scale = 64,
    //SideBT = 30,
    //SideLR = 30,
    //Z = 0,
    ShiftCubedX = 50,
    ShiftCubedY = 50,
};
Console.WriteLine($"obstacleDown: {map.Obstacles[(0, 0)].First().HitBox.MainHitBox[CoordinatePlane.Z, SideSize.Smaller].Side}");
Console.WriteLine($"obstacleUptyUp: {map.Obstacles[(0, 0)].First().HitBox.MainHitBox[CoordinatePlane.Z, SideSize.Larger].Side}");
//Console.WriteLine(map.Obstacles[(0, 0)].First().HitBox.MainHitBox[CoordinatePlane.Z, SideSize.Larger].Side);
sprite1.Z.Axis = 0;
sprite1.HitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(0);
sprite1.HitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(Screen.Setting.Tile);
sprite1.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(0);
sprite1.HitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(Screen.Setting.Tile);
sprite1.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(50);
sprite1.HitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(50);
sprite1.HitBox.MainHitBox.RenderColor = Color.Green;
sprite1.HitBox.MainHitBox.HeightRenderMode = HitBoxLib.Operations.RenderHeightMode.EdgeBased;
HitBox box = new HitBox();
box[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(20);
box[CoordinatePlane.X, SideSize.Larger]?.SetOffset(20);
box[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(20);
box[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(20);
box[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(0);
box[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(50);
sprite1.HitBox.AddSegmentHitBox(box.MainHitBox.Body, "Center");
SpriteObstacle sprite2 = new SpriteObstacle(ResourceManager.GetMainPath(mainFillWall32), true)
{
    Scale = 64,
    //SideBT = 30,
    //SideLR = 30,
    //Z = 0,
    ShiftCubedX = 50,
    ShiftCubedY = 50,
};
sprite2.HitBox.MainHitBox.HeightRenderMode = HitBoxLib.Operations.RenderHeightMode.CenterBased;
sprite2.Z.Axis = 0;
sprite2.HitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(30);
sprite2.HitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(30);
sprite2.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(30);
sprite2.HitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(30);
sprite2.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(100);
sprite2.HitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(100);
sprite2.HitBox.MainHitBox.RenderColor = Color.Red;
sprite2.Animation.IsAnimation = true;
Unit unitBulletT = new Unit(map, sprite2, 1);
unitBulletT.HitBox.MainHitBox.HeightRenderMode = RenderHeightMode.EdgeBased;
unitBulletT.IsRenderable = false;
unitBulletT.MinDistanceFromWall = 0;
unitBulletT.MoveSpeed = 20;

string mainFillWall322 = Path.Combine("Resources", "Image", "Sprite", "GifSprite", "pokemon-8939_256.gif");
Console.WriteLine(ImageLoader.TextureLoad(ResourceManager.GetMainPath(mainFillWall322))?.Count);
AnimationState deathAnim = new AnimationState(ImageLoader.TextureLoad(ResourceManager.GetMainPath(mainFillWall322)) ?? new());
deathAnim.Speed = 300;
deathAnim.IsAnimation = true;


//Unit unit = new Unit(map, new Observer(), sprite2, 100);
//unit.Z.Axis = 0;
//unit.HitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(30);
//unit.HitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(30);
//unit.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(30);
//unit.HitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(30);
//unit.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(100);
//unit.HitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(100);
//unit.Animation.IsAnimation = false;
//unit.DeathAnimation = new DeathAnimation(deathAnim, true, new TimeSpan(10));

//map.AddObstacle(2, 2,sprite1);
//map.AddObstacle(2, 3, unit);

MultiWall multiWall = new MultiWall();
multiWall.Z.Axis = 0;
multiWall.AddLevelWall(new TexturedWall(ResourceManager.GetMainPath(mainFillWall)));
multiWall.AddLevelWall(new TexturedWall(ResourceManager.GetMainPath(mainFillWall)));
multiWall.AddLevelWall(new TexturedWall(ResourceManager.GetMainPath(mainFillWall)));
multiWall.Z.Axis = 0;
//multiWall.Z.Axis = 300;
Console.WriteLine($"ZZZZ: {multiWall.Z.Axis}");
Console.WriteLine($"ZZZZ: {multiWall.Walls[1].Z.Axis}");


map.AddObstacle(7, 7, multiWall);
map.AddObstacle(7, 4, (new TexturedWall(ResourceManager.GetMainPath(mainFillWall))));

//map.AddObstacle(4, 4, new TexturedWall(ResourceManager.GetMainPath(mainFillWall)));

//Player player = new Player(500, 500)
//{
//    MaxRenderTile = 1200,

//};
//ResourceManager.GetMainPath(mainFillWall32), true
SpriteObstacle spriteObstacle = new SpriteObstacle(ResourceManager.GetMainPath(mainFillWall32), true)
{
    Scale = 64,
    //SideBT = 30,
    //SideLR = 30,
    //Z = 0,
};
SpriteObstacle spriteObstacle1 = new SpriteObstacle(ResourceManager.GetMainPath(mainFillWall322), false)
{
    Scale = 84,
    //SideBT = 30,
    //SideLR = 30,
    //Z = 0,
};


Unit player = new Unit(map, new SpriteObstacle(ResourceManager.GetMainPath(mainFillWall32), true), 100);
player.Scale = 64;
map.AddObstacle(3, 3, player);

player.Z.Axis = 600;
player.HitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(50);
player.HitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(50);
player.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(50);
player.HitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(50);
player.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(50);
player.HitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(50);
player.HitBox.MainHitBox.RenderColor = Color.Red;
Unit player2 = new Unit(map, spriteObstacle1, 100);
map.AddObstacle(6, 6, player2);
player2.Z.Axis = -300;
player2.Scale = 80;


player2.HitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(100);
player2.HitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(100);
player2.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(100);
player2.HitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(100);
player2.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(100);
player2.HitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(100);
player2.HitBox.MainHitBox.RenderColor = Color.Red;


MiniMap miniMap = new MiniMap(player, 5, PositionsMiniMap.UpperRightCorner, ResourceManager.GetPath(@"Resources\Image\BorderMiniMap\Border.png"));
MiniMap miniMap1 = new MiniMap(player2, 5, PositionsMiniMap.UpperRightCorner, ResourceManager.GetPath(@"Resources\Image\BorderMiniMap\Border.png"));
//BresenhamAlgorithm.Algorithm.RenderAlgorithm bresenhamAlgorithm = new(map, player2);
BresenhamAlgorithm.Algorithm.RenderAlgorithm bresenhamAlgorithm  = new (player);
bresenhamAlgorithm.UseVerticalPerspective = false;
bresenhamAlgorithm.UseHeightPerspective = false;

BresenhamAlgorithm.Algorithm.RenderAlgorithm bresenhamAlgorithm1  = new (player);
BresenhamAlgorithm.Algorithm.RenderAlgorithm bresenhamAlgorithm2  = new (player2);

//RenderAlgorithm2 bresenhamAlgorithm3 = new RenderAlgorithm2(map, player2);

ControlLib.Control control = new ControlLib.Control(player);

Bottom bottomW = new Bottom(VirtualKey.W);
Bottom bottomS = new Bottom(VirtualKey.S);
Bottom bottomA = new Bottom(VirtualKey.A);
Bottom bottomD = new Bottom(VirtualKey.D);
Bottom bottomN = new Bottom(VirtualKey.N);
Bottom bottomLeftMouse = new Bottom(VirtualKey.LeftButton);
Bottom bottomC = new Bottom(VirtualKey.C);
Bottom bottomCtrl = new Bottom(VirtualKey.LeftControl);
List<Bottom> controls = new List<Bottom>() { bottomC, bottomCtrl };
List<Bottom> controlsHide = new List<Bottom>() { bottomN, bottomCtrl };

CrossSight crossSight = new CrossSight(4, Color.Red)
{
    WidthCross = 15,
    HeightCross = 5,
    IndentFromCenter = 15,
    RotationObjectType = RotationType.AroundItsAxis,
    GeneralDegreeObject = 45,
    StartDegree = 45,
    GeneralDegreePosition = 90,
    InvertCrossParts = true,
};
//RoundSight roundSight = new RoundSight(Color.Red, 3);
ControlLib.BottomBinding keyBindingHideMap = new ControlLib.BottomBinding(controls, miniMap.Hide, 350);
ControlLib.BottomBinding keyBindingForward = new ControlLib.BottomBinding(bottomW,MoveLib.Move.MovePositions.Move, new object[] { map, player, 1, 0 });
ControlLib.BottomBinding keyBindingBackward = new ControlLib.BottomBinding(bottomS, MoveLib.Move.MovePositions.Move, new object[] { map, player, -1, 0 });
ControlLib.BottomBinding keyBindingLeft = new ControlLib.BottomBinding(bottomA, MoveLib.Move.MovePositions.Move, new object[] { map, player, 0, -1 });
ControlLib.BottomBinding keyBindingRight = new ControlLib.BottomBinding(bottomD, MoveLib.Move.MovePositions.Move, new object[] { map, player, 0, 1 });
ControlLib.BottomBinding keyBindingHideCross = new ControlLib.BottomBinding(controlsHide, crossSight.Hide, 350);
control.AddBottomBind(new ControlLib.BottomBinding(new Bottom(VirtualKey.Q), Screen.Window.Close));
control.AddBottomBind(new ControlLib.BottomBinding(new Bottom(VirtualKey.None), MoveAngle.ResetAngle, new object[] { player }));
void See(IUnit unit)
{
    unit.Angle -= 0.01;
    if (unit.Angle > Math.PI)
    {
        unit.Angle -= Math.PI * 2.0;
    }

    if (unit.Angle < -Math.PI)
    {
        unit.Angle += Math.PI * 2.0;
    }
}
control.AddBottomBind(new ControlLib.BottomBinding(new Bottom(VirtualKey.LeftArrow), See, new object[] { player2 }));

control.AddBottomBind(keyBindingForward);
control.AddBottomBind(keyBindingBackward);
control.AddBottomBind(keyBindingLeft);
control.AddBottomBind(keyBindingRight);
control.AddBottomBind(keyBindingHideMap);
control.AddBottomBind(keyBindingHideCross);


//BottomBinding shoot = new ControlLib.BottomBinding(new List<Bottom>(){ bottomLeftMouse }, Drawing.DrawingPoint, 350, new object[] { map, player, 30, Color.Red });
//control.AddBottomBind(shoot);
//player.OnControlAction += control.MakePressed;


string mainTextureSky = Path.Combine("Resources", "Image", "PartsWorldTexture", "SeamlessSky.jpg");

Sky sky = new Sky(ResourceManager.GetPath(mainTextureSky));
Floor floor = new Floor();
RenderPartsWorld renderPartsWorld = new RenderPartsWorld(sky, floor);
//230 170
Texture tex = new Texture(ResourceManager.GetPath(Path.Combine("Resources", "UI", "pistol.gif")));

ControlLib.BottomBinding shoot = new ControlLib.BottomBinding(new List<Bottom> { new Bottom(VirtualKey.None) }, Drawing.DrawingPoint, 0, new object[] { map, player, 30, Color.Red });

UIAnimation uIElement = new UIAnimation(shoot, ResourceManager.GetPath(Path.Combine("Resources", "UI", "pistol.gif")))
{
    IsAnimation = true,
    SpeedAnimation = 5,
    PercentShiftX = 6f,
    PercentShiftY = -38,
    ScaleY = 1.3f,
    ScaleX = 1.25f
};
ControlLib.BottomBinding shootGun = new ControlLib.BottomBinding(new Bottom(VirtualKey.R), 350);
RenderText textMa = new RenderText("", 24, new Vector2f(Screen.ScreenWidth, Screen.ScreenHeight), ResourceManager.GetMainPath(mainBold), Color.Yellow);

var fff = new SpriteObstacle(ImageLoader.TexturesLoadFromFolder(@"D:\C++ проекты\Gashrun\Resources\Image\Sprite\Flame", false));
fff.Animation.IsAnimation = true;
fff.Animation.Speed = 20;
fff.IsPassability = true;
fff.Scale = 64;
HitEffect hitEffect = new HitEffect(fff, new TimeSpan(5000000));
ControlLib.BottomBinding draw = new ControlLib.BottomBinding(new List<Bottom> { new Bottom(VirtualKey.None) }, Drawing.DrawingObjectPointAsync, 0, new object[] { 30, Color.Red });

StandartBullet bull = new StandartBullet(map, draw, null, hitEffect);
UnitBullet unitBullet = new UnitBullet(unitBulletT, draw, null, hitEffect);
unitBullet.Unit.Scale = 20;
Magazine magazine = new Magazine(control, shootGun, textMa, unitBullet, 100, 12);

ControlLib.BottomBinding shoot3 = new ControlLib.BottomBinding(new List<Bottom> { new Bottom(VirtualKey.LeftButton) }, 150);

Gun gun = new Gun(player, uIElement, magazine, shoot3);
List<Bottom> controls3 = new List<Bottom>() { new Bottom(VirtualKey.M), new Bottom(VirtualKey.LeftControl) };
ControlLib.BottomBinding bindGunHide = new ControlLib.BottomBinding(controls3, gun.Animation.Hide, 350);
control.AddBottomBind(bindGunHide);
//control.AddBottomBind(shoot);
control.AddBottomBind(shoot3);


//AnimationContent a = new AnimationContent(ResourceManager.GetPath(Path.Combine("Resources", "UI", "small.gif")))
//{
//    IsAnimation = true,
//    SpeedAnimation = 8
//};
//new ColorContent(Color.Green)
FillBar bb = new FillBar(new ColorContent(Color.Green), new ColorContent(Color.Red), 80, 20)
{
    BorderThickness = 10,
    Width = 400,
    Height = 100,
    PositionOnScreen = new Vector2f(0, Screen.ScreenHeight),
    BorderFillColor = Color.Black,
    
};
List<Bottom> controls4 = new List<Bottom>() { new Bottom(VirtualKey.L), new Bottom(VirtualKey.LeftControl) };
ControlLib.BottomBinding bindBurHide = new ControlLib.BottomBinding(controls4, bb.Hide, 350);
control.AddBottomBind(bindBurHide);
//FillBar Bar = new FillBar(bb, new AnimationContent(ResourceManager.GetPath(Path.Combine("Resources", "UI", "pistol.gif"))), new ColorContent(Color.Red));
//Sight sight = new RoundSight(Color.Blue, 4);
//Screen.ScreenHeight = 1000;
//Screen.ScreenWidth = 1500;


VisualizerHitBox.VisualizerType = RenderLib.HitBox.VisualizerHitBoxType.VisualizeSelfRenderable;

try
{
    while (Screen.Window.IsOpen)
    {
        Screen.Window.DispatchEvents();
        Screen.Window.Clear();
        DrawingQueue.ExecuteAll();
        //unitBullet.CHECK();
        bresenhamAlgorithm.CalculationAlgorithm(map);

        //bresenhamAlgorithm1.CalculationAlgorithm(true);
        //bresenhamAlgorithm2.CalculationAlgorithm(false);

        renderPartsWorld.Render(player);
        miniMap.Render(map);
       // miniMap1.Render(map);

        control.MakePressed();
        UIRender.DrawingByPriority();
       VisualizerHitBox.Render(map, player);
        //VisualizerHitBox2.Render(map, player);
        HitEffect.Render();
       // DeathManager.Render(player);
        fpsChecker.Track();

        //mapMini.Render(map);

        //partsWorld.Render(player);

        //CircleShape point = new CircleShape(3)
        //{
        //    FillColor = Color.Red,
        //    Position = new Vector2f(Screen.Setting.HalfWidth, Screen.Setting.HalfHeight)
        //};

        //Screen.Window.Draw(point);

        Screen.OutputPriority?.DrawingByPriority();
        Screen.Window.Display();
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    Console.WriteLine(e.StackTrace);
}
//public static class VisualizerHitBox
//{
//    /// <summary> The type of hitboxes that will be rendered </summary>
//    public static VisualizerHitBoxType VisualizerType { get; set; } = VisualizerHitBoxType.None;
//    /// <summary> Limits rendering to a maximum distance </summary>
//    public static bool IsDistanceLimited { get; set; } = true;
//    private const float indexStepDepth = 0.0000001f;

//    public static void Render(Map map, ProtoRender.Object.IUnit unit)
//    {
//        float index = indexStepDepth;
//        float step = indexStepDepth;

//        ObserverInfo observerInfo = unit.GetObserverInfo();
//        var obstacleValues = map.Obstacles.Values
//          .Select(list =>
//          {
//              lock (list) // Копия каждого списка под локом
//              {
//                  return System.Linq.Enumerable.ToList(list); ;
//              }
//          }).ToList();

//        Parallel.ForEach(obstacleValues, Screen.Setting.ParallelOptions, obstList =>
//        {
//            if (obstList is null)
//                return;

//            Parallel.ForEach(obstList, Screen.Setting.ParallelOptions, obstacle =>
//            {
//                if (obstacle is null)
//                    return;

//                var hitboxObjectInfo = obstacle.GetRenderHitBoxInfo();

//                if (IsDistanceLimited && MathUtils.CalculateDistance(hitboxObjectInfo.position, observerInfo.position) > unit.MaxRenderTile)
//                    return;

//                foreach (var box in HitBoxLib.Operations.Render.BuildHitBoxMesh(hitboxObjectInfo, observerInfo))
//                {
//                    switch (VisualizerType)
//                    {
//                        case VisualizerHitBoxType.VisualizeRayRenderable when obstacle is IRayRenderable:
//                        case VisualizerHitBoxType.VisualizeSelfRenderable when obstacle is ISelfRenderable:
//                        case VisualizerHitBoxType.VisualizeAll:
//                            ZBuffer.AddToZBuffer(box, index);
//                            break;
//                    }
//                    index += step;
//                }
//            });
//        });

//    }
//}

//public static class Render12
//{
//    //
//    // Сводка:
//    //     Amount peaks in hitbox
//    public const int maxCountPeaks = 8;

//    private const float baseScreenHeightForHitBox = 600f;

//    private const float baseScreenWidthForHitBox = 1000f;

//    //
//    // Сводка:
//    //     Hitbox Edges Array
//    public static int[,] Edges { get; }

//    //
//    // Сводка:
//    //     Multiplier to normalize hitbox height on different screens
//    public static float MultHeight { get; private set; }

//    private static void SetNewMult()
//    {
//        MultHeight = (float)Screen.ScreenHeight / 600f;
//        MultHeight /= (float)Screen.ScreenWidth / 1000f;
//        MultHeight = (float)Screen.ScreenHeight / (600f * MultHeight);
//    }

//    static Render12()
//    {
//        Edges = new int[12, 2]
//        {
//            { 0, 1 },
//            { 1, 3 },
//            { 3, 2 },
//            { 2, 0 },
//            { 4, 5 },
//            { 5, 7 },
//            { 7, 6 },
//            { 6, 4 },
//            { 0, 4 },
//            { 1, 5 },
//            { 2, 6 },
//            { 3, 7 }
//        };
//        SetNewMult();
//        Screen.HeightChangesFun = (Action)Delegate.Combine(Screen.HeightChangesFun, new Action(SetNewMult));
//        Screen.WidthChangesFun = (Action)Delegate.Combine(Screen.WidthChangesFun, new Action(SetNewMult));
//    }

//    private static List<Vector3f> GetCoordinatesParallelepiped(RenderInfo objectHitBox, Box currentBox, ObserverInfo observer, ref Vector2f center)
//    {
//        float num = (float)(currentBox[CoordinatePlane.X, SideSize.Smaller]?.Side ?? 0.0);
//        float num2 = (float)(currentBox[CoordinatePlane.X, SideSize.Larger]?.Side ?? 0.0);
//        float num3 = (float)(currentBox[CoordinatePlane.Y, SideSize.Smaller]?.Side ?? 0.0);
//        float num4 = (float)(currentBox[CoordinatePlane.Y, SideSize.Larger]?.Side ?? 0.0);
//        float num5 = (float)(currentBox[CoordinatePlane.Z, SideSize.Smaller]?.Side ?? 0.0);
//        float num6 = (float)(currentBox[CoordinatePlane.Z, SideSize.Larger]?.Side ?? 0.0);
//        float num7 = ((objectHitBox.position.X == observer.position.X && objectHitBox.position.Y == observer.position.Y && (num5 + num6) / 2f == observer.position.Z) ? MultHeight : 1f);
//        num5 = num5 * MultHeight - observer.position.Z * MultHeight * num7;
//        num6 = num6 * MultHeight - observer.position.Z * MultHeight * num7;
//        center.X = (num2 + num) / 2f;
//        center.Y = (num4 + num3) / 2f;
//        return new List<Vector3f>
//        {
//            new Vector3f(num3, num6, num),
//            new Vector3f(num4, num6, num),
//            new Vector3f(num3, num5, num),
//            new Vector3f(num4, num5, num),
//            new Vector3f(num3, num6, num2),
//            new Vector3f(num4, num6, num2),
//            new Vector3f(num3, num5, num2),
//            new Vector3f(num4, num5, num2)
//        };
//    }

//    private static Vector2f GetPositionForAngle(RenderInfo objectHitBox, Box currentBox, Vector3f vertex, Vector2f center)
//    {
//        return currentBox.HeightRenderMode switch
//        {
//            RenderHeightMode.EdgeBased => new Vector2f(vertex.Z, vertex.X),
//            RenderHeightMode.CenterBased => new Vector2f(center.X, center.Y),
//            _ => default(Vector2f),
//        };
//    }

//    private static List<Vector2f> GetHitboxCoordinatesOnScreen(ref int countNonRender, RenderInfo objectHitBox, Box currentBox, ObserverInfo observer)
//    {
//        Vector2f center = default(Vector2f);
//        List<Vector3f> coordinatesParallelepiped = GetCoordinatesParallelepiped(objectHitBox, currentBox, observer, ref center);
//        List<Vector2f> list = new List<Vector2f>();
//        foreach (Vector3f item in coordinatesParallelepiped)
//        {
//            Vector2f positionForAngle = GetPositionForAngle(objectHitBox, currentBox, item, center);
//            float x = MathUtils.CalculateDistance(positionForAngle, observer.position);
//            float num = MathF.Max(x, 0.1f);
//            double targetAngle = MathUtils.CalculateAngleToTarget(new Vector2f(item.Z, item.X), observer.position);
//            double num2 = MathUtils.NormalizeAngleDifference(observer.angle, targetAngle);
//            float num3 = objectHitBox.worldToScreenX(num2, observer.deltaAngle);
//            float num4 = objectHitBox.worldToScreenY(item.Y, num, observer.verticalAngle, num2);
//            if (Math.Abs(num2) > observer.fov || (num3 < 0f && num3 > (float)Screen.ScreenWidth) || (num4 < 0f && num4 > (float)Screen.ScreenHeight))
//            {
//                countNonRender++;
//            }

//            list.Add(new Vector2f(num3, num4));
//        }

//        return list;
//    }

//    private static VertexArray VertexToArray(RenderInfo objectHitBox, Box currentBox, List<Vector2f> vertices, int countNonRender)
//    {
//        Vertex[] array = new Vertex[2];
//        VertexArray vertexArray = new VertexArray(PrimitiveType.Lines);
//        if (countNonRender == 8)
//        {
//            return vertexArray;
//        }

//        for (int i = 0; i < Edges.GetLength(0); i++)
//        {
//            int index = Edges[i, 0];
//            int index2 = Edges[i, 1];
//            array[0] = new Vertex(vertices[index], currentBox.RenderColor);
//            array[1] = new Vertex(vertices[index2], currentBox.RenderColor);
//            vertexArray.Append(array[0]);
//            vertexArray.Append(array[1]);
//        }

//        return vertexArray;
//    }

//    private static VertexArray BuildBoxMesh(RenderInfo objectHitBox, Box currentBox, ObserverInfo observer)
//    {
//        int countNonRender = 0;
//        List<Vector2f> hitboxCoordinatesOnScreen = GetHitboxCoordinatesOnScreen(ref countNonRender, objectHitBox, currentBox, observer);
//        return VertexToArray(objectHitBox, currentBox, hitboxCoordinatesOnScreen, countNonRender);
//    }

//    //
//    // Сводка:
//    //     Building the edges of a hitbox
//    //
//    // Параметры:
//    //   objectHitBox:
//    //     Info about render hitBox
//    //
//    //   observer:
//    //     Info about observer
//    public static List<VertexArray> BuildHitBoxMesh(RenderInfo objectHitBox, ObserverInfo observer)
//    {
//        List<VertexArray> vertexArrays = new List<VertexArray>();
//        HitBox hitBox = objectHitBox.hitBox;
//        vertexArrays.Add(BuildBoxMesh(objectHitBox, hitBox.MainHitBox, observer));
//        hitBox.SegmentedHitbox.ForEach(delegate (Box b)
//        {
//            vertexArrays.Add(BuildBoxMesh(objectHitBox, b, observer));
//        });
//        return vertexArrays;
//    }
//}
//public static class VisualizerHitBox
//{
//    private const float indexStepDepth = 1E-07f;

//    //
//    // Сводка:
//    //     The type of hitboxes that will be rendered
//    public static VisualizerHitBoxType VisualizerType { get; set; } = VisualizerHitBoxType.None;


//    //
//    // Сводка:
//    //     Limits rendering to a maximum distance
//    public static bool IsDistanceLimited { get; set; } = true;


//    public static void Render(Map map, IUnit unit)
//    {
//        IUnit unit2 = unit;
//        float index = 1E-07f;
//        float step = 1E-07f;
//        ObserverInfo observerInfo = unit2.GetObserverInfo();
//        Parallel.ForEach<KeyValuePair<(int, int), List<IObject>>>(map.Obstacles, Screen.Setting.ParallelOptions, delegate (KeyValuePair<(int, int), List<IObject>> obstList)
//        {
//            Parallel.ForEach(obstList.Value, Screen.Setting.ParallelOptions, delegate (IObject obstacle)
//            {
//                RenderInfo renderHitBoxInfo = obstacle.GetRenderHitBoxInfo();
//                if (IsDistanceLimited && MathUtils.CalculateDistance(renderHitBoxInfo.position, observerInfo.position) > (float)unit2.MaxRenderTile)
//                {
//                    return;
//                }

//                foreach (VertexArray item in Render12.BuildHitBoxMesh(renderHitBoxInfo, observerInfo))
//                {
//                    switch (VisualizerType)
//                    {
//                        case VisualizerHitBoxType.VisualizeRayRenderable:
//                            if (obstacle is IRayRenderable)
//                            {
//                                ZBuffer.AddToZBuffer(item, index);
//                            }

//                            break;
//                        case VisualizerHitBoxType.VisualizeSelfRenderable:
//                            if (obstacle is ISelfRenderable)
//                            {
//                                ZBuffer.AddToZBuffer(item, index);
//                            }

//                            break;
//                        case VisualizerHitBoxType.VisualizeAll:
//                            ZBuffer.AddToZBuffer(item, index);
//                            break;
//                    }

//                    index += step;
//                }
//            });
//        });
//    }
//}
//public class TexturedWall : Obstacle, IWall, IDrawable
//{
//    //----------------------Textures--------------------------
//    public MultiTexturedObject MultiTextured { get; init; }
//    public TexturedPair? CurrentRenderTexture { get; set; } = null;

//    //----------------------Setting---------------------
//    /// <summary>Current wall level in world wall level</summary>
//    public int LvlWall { get; private set; } = IWall.minLvlWall;



//    #region Constructor
//    private static SFML.Graphics.Color GetDefaultWallColor() => SFML.Graphics.Color.Red;
//    private string GetFirstTextureOrThrow()
//    {
//        return MultiTextured.UniqueTexture.GetFirstValue()?.Base.PathTexture
//               ?? throw new Exception("Error loading Texture (TexturedWall)");
//    }
//    private void InitializeWall()
//    {
//        UpdateBaseHeightHitBox();
//        Z.Axis = (LvlWall - 1) * Screen.Setting.HalfTile;
//    }

//    public TexturedWall(params string[] texturePaths)
//       : base(0, 0, GetDefaultWallColor(), false)
//    {
//        if (texturePaths.Length == 0)
//            throw new ArgumentException("At least one texture path must be provided.");

//        MultiTextured = new MultiTexturedObject(texturePaths);
//        TextureInMiniMap = new TextureObstacle(GetFirstTextureOrThrow());

//        InitializeWall();
//    }
//    public TexturedWall(bool isPassability = false, params string[] texturePaths)
//       : base(0, 0, GetDefaultWallColor(), isPassability)
//    {
//        if (texturePaths.Length == 0)
//            throw new ArgumentException("At least one texture path must be provided.");

//        MultiTextured = new MultiTexturedObject(texturePaths);
//        TextureInMiniMap = new TextureObstacle(GetFirstTextureOrThrow());

//        InitializeWall();
//    }
//    public TexturedWall(List<(ObjectSide, string)> textures, bool isPassability = false)
//        : base(0, 0, GetDefaultWallColor(), isPassability)
//    {
//        if (textures == null || textures.Count == 0)
//            throw new ArgumentException("Texture list cannot be empty.");

//        MultiTextured = new MultiTexturedObject(textures);
//        TextureInMiniMap = new TextureObstacle(GetFirstTextureOrThrow());

//        InitializeWall();
//    }
//    public TexturedWall(List<(ObjectSide, TextureObstacle)> textures, bool isPassability = false)
//        : base(0, 0, GetDefaultWallColor(), isPassability)
//    {
//        if (textures == null || textures.Count == 0)
//            throw new ArgumentException("Texture list cannot be empty.");

//        MultiTextured = new MultiTexturedObject(textures);
//        TextureInMiniMap = new TextureObstacle(GetFirstTextureOrThrow());

//        InitializeWall();
//    }

//    public TexturedWall(TexturedWall texturedWall)
//        : base(0, 0, SFML.Graphics.Color.Black, false)
//    {
//        HitBox = new HitBox(texturedWall.HitBox);

//        X = new Coordinate(texturedWall.X, HitBox);
//        Y = new Coordinate(texturedWall.Y, HitBox);
//        Z = new Coordinate(texturedWall.Z, HitBox);

//        ColorInMap = texturedWall.ColorInMap;
//        TextureInMiniMap = texturedWall.TextureInMiniMap is not null ? new TextureObstacle(texturedWall.TextureInMiniMap) : null;

//        IsPassability = texturedWall.IsPassability;
//        IsSingleAddable = texturedWall.IsSingleAddable;

//        SizeScale = texturedWall.SizeScale;
//        PositionScale = texturedWall.PositionScale;

//        MultiTextured = new MultiTexturedObject(texturedWall.MultiTextured);
//        CurrentRenderTexture = null;

//        LvlWall = texturedWall.LvlWall;
//    }
//    #endregion

//    #region MapAdder_Implementation
//    private void UpdateBaseHeightHitBox()
//    {
//        HitBox.MainHitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(Screen.Setting.HalfVerticalTile);
//        HitBox.MainHitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(Screen.Setting.HalfVerticalTile);
//    }
//    public override void HandleObjectAddition(double x, double y, bool resetHitBoxSide = true)
//    {
//        if (resetHitBoxSide)
//        {
//            HitBox.MainHitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(0);
//            HitBox.MainHitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(Screen.Setting.Tile);
//            HitBox.MainHitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(0);
//            HitBox.MainHitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(Screen.Setting.Tile);
//        }

//        X.Axis = x;
//        Y.Axis = y;
//    }
//    #endregion

//    #region IMiniMapRenderable_Implementation
//    public override void FillingColorShape(RectangleShape rectangleShape, float OutlineThickness = 1)
//    {
//        rectangleShape.OutlineThickness = OutlineThickness;
//        rectangleShape.FillColor = ColorInMap;
//    }
//    public override void FillingTextureShape(RectangleShape rectangleShape)
//    {
//        if (TextureInMiniMap is not null)
//            rectangleShape.Texture = TextureInMiniMap.Texture;
//        else
//            rectangleShape.FillColor = ColorInMap;
//    }
//    public override Vector2f ConversionToMapCoordinates(float mapTile)
//    {
//        float x = (float)X.Axis / Screen.Setting.Tile * mapTile;
//        float y = (float)Y.Axis / Screen.Setting.Tile * mapTile;

//        return new Vector2f(x, y);
//    }
//    #endregion

//    #region IRenderable_Implementation
//    public override CoordinateOnScreen GetPositionOnScreen(Result result, IUnit unit)
//    {
//        float positionX = WorldToScreenX(result.Ray);

//        int lvlWall = RenderOperation.NormalizeLvlWall(this);

//        float differentHeight = (float)((Z.Axis * HitBoxLib.Operations.Render.MultHeight - unit.Z.Axis) / (result.Depth / Screen.Setting.Tile));
//        float positionY = (float)(WorldToScreenY(unit.VerticalAngle) - result.ProjHeight / 2) - differentHeight;

//        float screenCenterY = (float)WorldToScreenY(unit.VerticalAngle);

//        // Положение "середины" объекта
//       //float positionX = (float)result.Offset; // X-координата на экране (зависит от Offset)

//        // Скорректировать на смещение камеры по Z (высота наблюдателя)
//        float verticalShift = (float)((Z.Axis * HitBoxLib.Operations.Render.MultHeight - unit.Z.Axis * HitBoxLib.Operations.Render.MultHeight) / (result.Depth / Screen.Setting.Tile));

//        // Верхняя и нижняя позиции
//        float top = screenCenterY - (float)(result.ProjHeight / 2) - verticalShift;
//        float bottom = screenCenterY + (float)(result.ProjHeight / 2) - verticalShift;
//        CoordinateOnScreen coo = new CoordinateOnScreen();
//        coo.X = positionX;
//        coo.Up = top;
//        coo.Bottom = bottom;

//        return coo;
//    }

//    public void ProcessForRendering(List<InfoObject> infoObject, double coordinate, double depth, double maxDepth)
//    {
//        if (depth < maxDepth)
//            infoObject.Add(new InfoObject(depth, coordinate, this));
//    }

//    #endregion

//    #region IWall_Implementation
//    public bool IsOffScreen(Result result, Vector2f position, int heightTexture, Vector2f scale)
//    {

//        //if (result.PositionPreviousObject is not null && position.Up < result.PositionPreviousObject.Value.Up)
//        //{
//        //    return true;
//        //}

//        //if (position.Up + scale.Y * heightTexture < 0)
//        //    return true;
//        //if (position.Up > Screen.ScreenHeight)
//        //    return true;

//        return false;
//    }
//    public bool IsOffScreenn(IUnit unit, Result result, CoordinateOnScreen position, int heightTexture, Vector2f scale)
//    {
//        //if (result.PositionPreviousObject is not null && position.Up < result.PositionPreviousObject.Value.Up)
//        //{
//        //    return true;
//        //}
//        if (result.PositionPreviousObject is not null && position.Up > result.PositionPreviousObject.Value.Up && position.Bottom < result.PositionPreviousObject.Value.Bottom)
//        {
//           // Console.WriteLine($"LVL: {LvlWall}");
//            return true;
//        }

//        if (position.Up + scale.Y * heightTexture < 0)
//            return true;
//        if (position.Up > Screen.ScreenHeight)
//            return true;

//        return false;
//    }
//    public void SetLevelWall(double currentZ, double baseOffset, int lvl)
//    {
//        LvlWall = lvl;
//        HitBox.MainHitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(baseOffset);
//        HitBox.MainHitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(baseOffset);

//        Z.Axis = currentZ + (lvl - 1) * baseOffset;
//    }
//    #endregion

//    #region IDrawable_Implementation
//    public float CalculateTextureX(Vector2f UV, ObjectSide side)
//    {
//        CurrentRenderTexture = MultiTextured[side];
//        if (CurrentRenderTexture is null)
//            throw new Exception("CurrentRenderTexture is null (GetTextureCoordinate)");

//        float textureX = UV.X > UV.Y ? UV.X : UV.Y;
//        textureX *= CurrentRenderTexture.Base.Width / Screen.Setting.Scale;

//        return textureX - (float)Math.Pow(CurrentRenderTexture.Base.Height / TextureObstacle.BaseHeight, 4.5f);
//    }
//    public float BringingToStandard(float heightObj)
//    {
//        if (CurrentRenderTexture is null)
//            throw new Exception("CurrentRenderTexture is null(BringingToStandard)");

//        heightObj *= TextureObstacle.DifferenceHeight(CurrentRenderTexture.Base.Height);
//        return heightObj;
//    }
//    public float GetAveragedMult(float baseMult)
//    {
//        if (CurrentRenderTexture is null)
//            throw new Exception("CurrentRenderTexture is null(GetAveragedMult)");


//        float newMult = baseMult * Screen.ScreenRatio;
//        newMult *= (float)TextureObstacle.BaseHeight / CurrentRenderTexture.Base.Height;

//        return newMult;
//    }
//    public float CalculateTextureY(IUnit unit, float ProjHeight, float mult, float addCoordinates)
//    {
//        if (CurrentRenderTexture is null)
//            throw new Exception("CurrentRenderTexture is null(GetAveragedMult)");

//        float textureY = ProjHeight * (float)unit.VerticalAngle * mult;

//        float heightDifference = (float)(Z.Axis - unit.Z.Axis);
//        float textureMult = Screen.Setting.VerticalTile / (CurrentRenderTexture?.Base.Height ?? 1);
//        return CurrentRenderTexture.Base.Height / 2 + textureY - addCoordinates + (heightDifference / textureMult);
//    }
//    public float CalculateTextureY(float verticalAngle, float ProjHeight, float mult, float addCoordinates)
//    {
//        if (CurrentRenderTexture is null)
//            throw new Exception("CurrentRenderTexture is null(GetAveragedMult)");

//        float textureY = ProjHeight * verticalAngle * mult;
//        return CurrentRenderTexture.Base.Height / 2 + textureY - addCoordinates;
//    }
//    public bool IsInsideTexture(float textureX, float textureY)
//    {
//        if (CurrentRenderTexture is null)
//            return true;

//        var baseTexure = CurrentRenderTexture.Base;
//        if (textureX < 0 || textureX > baseTexure.Width)
//            return true;
//        else if (textureY < 0 || textureY > baseTexure.Height)
//            return true;

//        return false;
//    }

//    public void DrawObject(Drawable drawObject)
//    {
//        if (CurrentRenderTexture is null)
//            return;

//        CurrentRenderTexture.Mod.Draw(drawObject);
//        CurrentRenderTexture.Mod.Display();
//    }
//    public void DrawObjectAsync(Drawable drawObject)
//    {
//        ProtoRender.RenderAlgorithm.DrawingQueue.EnqueueDraw((DrawObject, drawObject));
//    }
//    #endregion

//    public override IObject GetCopy()
//    {
//        return new TexturedWall(this);
//    }

//    public override void Render(Result result, IUnit unit)
//    {
//        RenderInternal(result, unit, RenderOperation.SelectCurrentRenderTexture(this, result, unit));
//    }
//    public void RenderMultiWall(Result result, IUnit unit, ObjectSide objectSide)
//    {
//        RenderInternal(result, unit, MultiTextured[objectSide]);
//    }


//    private void RenderInternal(Result result, IUnit unit, TexturedPair? currentRenderTexture)
//    {
//        if (currentRenderTexture is null)
//            return;

//        IntRect textureRect = TextureObstacle.SetIntegerRectangle((int)result.Offset, Screen.Setting.Tile, currentRenderTexture.Base);
//        CoordinateOnScreen position = GetPositionOnScreen(result, unit);
//        Vector2f scale = RenderOperation.CalculationTextureScale(result, currentRenderTexture);

//        if (IsOffScreenn(unit, result, position, textureRect.Height, scale))
//        {

//            return;
//        }

//        VertexArray vertexArray = new VertexArray(PrimitiveType.Quads, 4);

//        SFML.Graphics.Color blackoutColor = VisualEffectHelper.VisualEffect.TransformationColor(result.Depth);

//        Vector2f topLeftTexCoords = new Vector2f(textureRect.Left, textureRect.Top);
//        Vector2f topRightTexCoords = new Vector2f(textureRect.Left + textureRect.Width, textureRect.Top);
//        Vector2f bottomRightTexCoords = new Vector2f(textureRect.Left + textureRect.Width, textureRect.Top + textureRect.Height);
//        Vector2f bottomLeftTexCoords = new Vector2f(textureRect.Left, textureRect.Top + textureRect.Height);

//        Vector2f topLeftPosition = new Vector2f(position.X, position.Up);
//        Vector2f topRightPosition = new Vector2f(position.X + scale.X * textureRect.Width, position.Up);
//        Vector2f bottomRightPosition = new Vector2f(position.X + scale.X * textureRect.Width, position.Up + scale.Y * textureRect.Height);
//        Vector2f bottomLeftPosition = new Vector2f(position.X, position.Up + scale.Y * textureRect.Height);

//        vertexArray[0] = new Vertex(topLeftPosition, blackoutColor, topLeftTexCoords);
//        vertexArray[1] = new Vertex(topRightPosition, blackoutColor, topRightTexCoords);
//        vertexArray[2] = new Vertex(bottomRightPosition, blackoutColor, bottomRightTexCoords);
//        vertexArray[3] = new Vertex(bottomLeftPosition, blackoutColor, bottomLeftTexCoords);

//        RenderStates renderStates = new RenderStates(currentRenderTexture.Mod.Texture);

//        result.Depth += (LvlWall + 1) * 0.01;
//        ZBuffer.AddToZBuffer(vertexArray, result.Depth, renderStates);
//    }
//}
//public class MultiWall : Obstacle, IDrawable, IRayRenderable
//{
//    //-------------------------Wall-------------------------
//    public List<TexturedWall> Walls { get; private set; } = new();
//    /// <summary>Current wall levels being processed</summary>
//    private int CurrentLevelWall { get; set; } = 0;

//    private double _baseOffset = Screen.Setting.HalfVerticalTile;
//    public double BaseOffset
//    {
//        get => _baseOffset;
//        set
//        {
//            _baseOffset = value;
//            ResetZ();
//        }
//    }


//    #region Constructor
//    public MultiWall() : base(0, 0, Color.Red, false)
//    {
//        Z.MoveAxis += ResetZ;
//    }
//    public MultiWall(List<TexturedWall> walls)
//        : base(0, 0, Color.Red, false)
//    {
//        AddLevelWall(walls);
//    }
//    public MultiWall(MultiWall multiWall)
//       : base(0, 0, SFML.Graphics.Color.Black, false)
//    {
//        HitBox = new HitBox(multiWall.HitBox);

//        X = new Coordinate(multiWall.X, HitBox);
//        Y = new Coordinate(multiWall.Y, HitBox);
//        Z = new Coordinate(multiWall.Z, HitBox);

//        ColorInMap = multiWall.ColorInMap;
//        TextureInMiniMap = multiWall.TextureInMiniMap is not null ? new TextureObstacle(multiWall.TextureInMiniMap) : null;

//        IsPassability = multiWall.IsPassability;
//        IsSingleAddable = multiWall.IsSingleAddable;

//        SizeScale = multiWall.SizeScale;
//        PositionScale = multiWall.PositionScale;

//        foreach (var wall in multiWall.Walls)
//            Walls.Add(new TexturedWall(wall));

//        CurrentLevelWall = multiWall.CurrentLevelWall;
//    }
//    #endregion

//    #region IDrawable_Implementation
//    public float CalculateTextureX(Vector2f UV, ObjectSide side)
//    {
//        if (Walls.Count == 0)
//            return 0;


//        for (int i = 0; i < Walls.Count; i++)
//        {
//            if (i < Walls.Count - 1)
//                Walls[i].CalculateTextureX(UV, side);
//            else
//                return Walls[i].CalculateTextureX(UV, side);
//        }

//        return 0;
//    }
//    public float CalculateTextureY(IUnit unit, float ProjHeight, float mult, float addCoordinates)
//    {
//        for (int i = 0; i < Walls.Count; i++)
//        {
//            if (Walls[i].CurrentRenderTexture is null)
//                continue;

//            float normalizedCoordinate = Walls[i].CalculateTextureY(unit, ProjHeight / (float)(i + 1), mult * (float)(i + 1), addCoordinates);

//            if (normalizedCoordinate > 0 && normalizedCoordinate < Walls[i].CurrentRenderTexture?.Base.Height)
//            {
//                CurrentLevelWall = i;
//                return normalizedCoordinate;
//            }
//        }

//        return 0;
//    }
//    public float BringingToStandard(float heightObj)
//    {
//        if (Walls.Count == 0) return 0;

//        return Walls.First().BringingToStandard(heightObj);
//    }
//    public float GetAveragedMult(float baseMult)
//    {
//        if (Walls.Count == 0) return 0;

//        return Walls.First().GetAveragedMult(baseMult);
//    }
//    public bool IsInsideTexture(float textureX, float textureY)
//    {
//        var wall = Walls[CurrentLevelWall].CurrentRenderTexture;
//        if (wall is null)
//            return true;

//        var baseTexure = wall.Base;
//        if (textureX < 0 || textureX > baseTexure.Width)
//            return true;
//        else if (textureY < 0 || textureY > baseTexure.Height)
//            return true;

//        return false;
//    }

//    public void DrawObject(Drawable drawObject)
//    {
//        if (CurrentLevelWall < 0 || CurrentLevelWall >= Walls.Count)
//            return;
//        else if (Walls[CurrentLevelWall].CurrentRenderTexture is null)
//            return;

//        Walls[CurrentLevelWall].CurrentRenderTexture?.Mod.Draw(drawObject);
//        Walls[CurrentLevelWall].CurrentRenderTexture?.Mod.Display();
//    }
//    public void DrawObjectAsync(Drawable drawObject)
//    {
//        ProtoRender.RenderAlgorithm.DrawingQueue.EnqueueDraw((DrawObject, drawObject));
//    }
//    #endregion

//    #region MapAdder_Implementation
//    private void UpdateHeightHitBox(double newZ)
//    {
//        HitBox.MainHitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(newZ);
//        HitBox.MainHitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(newZ);
//    }
//    private void UpdateHeight()
//    {
//        UpdateHeightHitBox(Walls.Count * BaseOffset);

//        Z.Axis = (Walls.Count - 1) * BaseOffset;
//    }
//    private void HandleObjectAdditionWalls(double x, double y, bool resetHitBoxSide)
//    {
//        foreach (var wall in Walls)
//            wall.HandleObjectAddition(x, y, resetHitBoxSide);
//    }
//    public override void HandleObjectAddition(double x, double y, bool resetHitBoxSide = true)
//    {
//        if (resetHitBoxSide)
//        {
//            HitBox.MainHitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(0);
//            HitBox.MainHitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(Screen.Setting.Tile);
//            HitBox.MainHitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(0);
//            HitBox.MainHitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(Screen.Setting.Tile);
//        }
//        HandleObjectAdditionWalls(x, y, resetHitBoxSide);


//        X.Axis = x;
//        Y.Axis = y;
//    }
//    #endregion

//    #region IMiniMapRenderable_Implementation
//    public override void FillingColorShape(RectangleShape rectangleShape, float OutlineThickness = 1)
//    {
//        rectangleShape.OutlineThickness = OutlineThickness;
//        rectangleShape.FillColor = ColorInMap;
//    }
//    public override void FillingTextureShape(RectangleShape rectangleShape)
//    {
//        if (Walls.Count > 0)
//        {
//            foreach (var wall in Walls)
//            {
//                TextureObstacle? textureInMap = wall.MultiTextured.UniqueTexture.GetFirstValue()?.Base;

//                if (textureInMap is not null)
//                {
//                    rectangleShape.Texture = textureInMap.Texture;
//                    return;
//                }
//            }
//        }

//        rectangleShape.FillColor = ColorInMap;
//    }
//    public override Vector2f ConversionToMapCoordinates(float mapTile)
//    {
//        float x = (float)X.Axis / Screen.Setting.Tile * mapTile;
//        float y = (float)Y.Axis / Screen.Setting.Tile * mapTile;

//        return new Vector2f(x, y);
//    }
//    #endregion

//    #region IRenderable_Implementation
//    public void ProcessForRendering(List<InfoObject> infoObject, double coordinate, double depth, double maxDepth)
//    {
//        if (depth < maxDepth)
//            infoObject.Add(new InfoObject(depth, coordinate, this));
//    }
//    public override CoordinateOnScreen GetPositionOnScreen(Result result, IUnit unit)
//    {
//        if (Walls.Count == 0)
//            return new CoordinateOnScreen();

//        TexturedWall? lastWall = Walls.LastOrDefault();
//        return lastWall is not null ? lastWall.GetPositionOnScreen(result, unit) : new CoordinateOnScreen();
//    }
//    #endregion

//    public override IObject GetCopy()
//    {
//        return new MultiWall(this);
//    }
//    public override void Render(Result result, IUnit unit)
//    {
//        if (Walls.Count <= 0)
//            return;

//        ObjectSide objectSide = RenderOperation.SelectCurrentObjectSide(Walls.First(), result, unit);

//        foreach (var wall in Walls)
//            wall.RenderMultiWall(result, unit, objectSide);
//    }


//    private void ResetZ()
//    {
//        for (int i = 0; i < Walls.Count; i++)
//        {
//            Walls[i].SetLevelWall(Z.Axis - ((Walls.Count - (i + 1)) * BaseOffset), BaseOffset, i + 1);
//        }
//    }
//    public void AddLevelWall(TexturedWall wall)
//    {
//        wall.HandleObjectAddition(X.Axis, Y.Axis);
//        Walls.Add(wall);

//        UpdateHeight();
//    }
//    public void AddLevelWall(List<TexturedWall> walls)
//    {
//        if (walls.Count == 0)
//            return;

//        foreach (var wall in walls)
//        {
//            wall.HandleObjectAddition(X.Axis, Y.Axis);
//            Walls.Add(wall);
//        }
//        UpdateHeight();
//    }
//    public void DeleteWall(int lvl)
//    {
//        if (Walls.Count == 0 || lvl < 1 || lvl > Walls.Count)
//            return;

//        lvl--;
//        Walls.RemoveAt(lvl);
//        UpdateHeight();
//    }
//}

//class RenderOperation1
//{
//    public static Vector2f GetPositionOnScreen(SpriteObstacle1 sprite, IUnit unit, float height)
//    {
//        float x = sprite.WorldToScreenX(sprite.AngleToObserver, unit.DeltaAngle);
//        float num = (float)(unit.Z.Axis * HitBoxLib.Operations.Render.MultHeight  / (sprite.Distance / (double)Screen.Setting.Tile));
//        float y = sprite.WorldToScreenY(unit.VerticalAngle) - (float)(sprite.Z.Axis * HitBoxLib.Operations.Render.MultHeight / (sprite.Distance / (double)Screen.Setting.Tile)) + num;
//        return new Vector2f(x, y);
//    }

//    public static void DrawSprite(SpriteObstacle1 sprite, IUnit unit, float height)
//    {
//        if (sprite.Animation.CurrentFrame != null && sprite.Animation.CurrentFrame.Texture != null)
//        {
//            uint width = sprite.Animation.CurrentFrame.Width;
//            uint height2 = sprite.Animation.CurrentFrame.Height;
//            sprite.RenderSprite = new Sprite(sprite.Animation.CurrentFrame.Texture);
//            sprite.RenderSprite.Color = VisualEffectHelper.VisualEffect.TransformationColor(sprite.Distance);
//            sprite.RenderSprite.Origin = new Vector2f(width / 2, height2 / 2);
//            sprite.RenderSprite.Position = GetPositionOnScreen(sprite, unit, height);
//            sprite.RenderSprite.Scale = new Vector2f(height / (float)width, height / (float)height2);
//            ZBuffer.AddToZBuffer(sprite.RenderSprite, sprite.Distance);
//        }
//    }
//}
//class Adder1
//{
//    public static void AddTexture(SpriteObstacle1 sprite, TextureObstacle texture)
//    {
//        sprite.Animation.AddFrame(texture);
//        if (sprite.TextureInMiniMap == null && sprite.Animation.AmountFrame > 0)
//        {
//            sprite.TextureInMiniMap = sprite.Animation.GetFrame(0);
//        }
//    }

//    public static void AddTextures(SpriteObstacle1 sprite, List<TextureObstacle> textures)
//    {
//        foreach (TextureObstacle texture in textures)
//        {
//            AddTexture(sprite, texture);
//        }
//    }

//    public static void AddTextures(SpriteObstacle1 sprite, List<string> paths)
//    {
//        foreach (string path in paths)
//        {
//            AddTexture(sprite, path);
//        }
//    }

//    public static void AddTexture(SpriteObstacle1 sprite, string path)
//    {
//        sprite.Animation.AddFrame(ImageLoader.LoadFrame(path));
//    }

//    public static void AddTextureFromFolder(SpriteObstacle1 sprite, string path, bool isDirectory, bool folderAccounting)
//    {
//        if (isDirectory)
//        {
//            sprite.Animation.AddFrames(ImageLoader.TexturesLoadFromFolder(path, folderAccounting));
//        }
//        else
//        {
//            AddTexture(sprite, path);
//        }
//    }
//}
//public class SpriteObstacle1 : Obstacle, ISelfRenderable, IRenderable
//{
//    private float scale = 1f;

//    public static List<SpriteObstacle1> SpritesToRender { get; private set; } = new();


//    private bool IsAdded { get; set; } = false;


//    public AnimationState Animation { get; init; } = new AnimationState();


//    public Sprite RenderSprite { get; set; } = new Sprite();


//    //
//    // Сводка:
//    //     Texture scale
//    public float Scale
//    {
//        get
//        {
//            return scale * Screen.ScreenRatio;
//        }
//        set
//        {
//            scale = ((value == 0f) ? 1f : value);
//        }
//    }

//    //
//    // Сводка:
//    //     Size scale
//    public override float SizeScale { get; set; } = 2f;


//    //
//    // Сводка:
//    //     Position scale in MiniMap
//    public override float PositionScale { get; set; } = 4f;


//    //
//    // Сводка:
//    //     Angle relative to this object and the observer
//    public double AngleToObserver { get; set; }

//    public double Distance { get; set; }

//    public SpriteObstacle1(List<TextureObstacle> textures, bool isPassability = false)
//        : base(0.0, 0.0, Color.White, isPassability)
//    {
//        Adder1.AddTextures(this, textures);
//    }

//    public SpriteObstacle1(TextureObstacle texture, bool isPassability = false)
//        : base(0.0, 0.0, Color.White, isPassability)
//    {
//        Adder1.AddTexture(this, texture);
//    }

//    public SpriteObstacle1(string path, bool isDirectory, bool folderAccounting = false, bool isPassability = false)
//        : base(0.0, 0.0, Color.White, isPassability)
//    {
//        Adder1.AddTextureFromFolder(this, path, isDirectory, folderAccounting);
//    }

//    public SpriteObstacle1(List<string> paths, bool isPassability = false)
//        : base(0.0, 0.0, Color.White, isPassability)
//    {
//        Adder1.AddTextures(this, paths);
//    }

//    //
//    // Сводка:
//    //     Constructor class AnimationState
//    //
//    // Параметры:
//    //   spriteObstacle:
//    //     Object of SpriteObstacle
//    //
//    //   updateAnimation:
//    //     true - frames and state are duplicated, false - frames are duplicated but state
//    //     is not copied
//    public SpriteObstacle1(SpriteObstacle1 spriteObstacle, bool updateAnimation = true)
//        : base(0.0, 0.0, Color.Black, isPassability: false)
//    {
//        HitBox = new HitBox(spriteObstacle.HitBox);
//        X = new Coordinate(spriteObstacle.X, HitBox);
//        Y = new Coordinate(spriteObstacle.Y, HitBox);
//        Z = new Coordinate(spriteObstacle.Z, HitBox);
//        ColorInMap = spriteObstacle.ColorInMap;
//        TextureInMiniMap = ((spriteObstacle.TextureInMiniMap != null) ? new TextureObstacle(spriteObstacle.TextureInMiniMap) : null);
//        IsPassability = spriteObstacle.IsPassability;
//        IsSingleAddable = spriteObstacle.IsSingleAddable;
//        SizeScale = spriteObstacle.SizeScale;
//        PositionScale = spriteObstacle.PositionScale;
//        Scale = spriteObstacle.Scale;
//        AngleToObserver = spriteObstacle.AngleToObserver;
//        Distance = spriteObstacle.Distance;
//        if (updateAnimation)
//        {
//            Animation = new AnimationState(spriteObstacle.Animation);
//        }
//        else
//        {
//            Animation = new AnimationState(spriteObstacle.Animation.GetFrames());
//        }

//        IsAdded = spriteObstacle.IsAdded;
//    }

//    public override void HandleObjectAddition(double x, double y, bool resetHitBoxSide = false)
//    {
//        X.Axis = x;
//        Y.Axis = y;
//        base.ShiftCubedX = base.ShiftCubedX;
//        base.ShiftCubedY = base.ShiftCubedY;
//    }

//    public override void FillingColorShape(RectangleShape rectangleShape, float OutlineThickness = 1f)
//    {
//        rectangleShape.OutlineThickness = OutlineThickness;
//        rectangleShape.FillColor = ColorInMap;
//    }

//    public override void FillingTextureShape(RectangleShape rectangleShape)
//    {
//        if (TextureInMiniMap != null)
//        {
//            rectangleShape.Texture = TextureInMiniMap.Texture;
//        }
//        else if (TextureInMiniMap == null && Animation.AmountFrame > 0)
//        {
//            TextureInMiniMap = Animation.GetFrame(0);
//            rectangleShape.Texture = TextureInMiniMap?.Texture;
//        }
//        else
//        {
//            rectangleShape.FillColor = ColorInMap;
//        }
//    }

//    public override Vector2f ConversionToMapCoordinates(float mapTile)
//    {
//        float x = (float)X.Axis / (float)Screen.Setting.Tile * mapTile;
//        float y = (float)Y.Axis / (float)Screen.Setting.Tile * mapTile;
//        return new Vector2f(x, y);
//    }

//    public override CoordinateOnScreen GetPositionOnScreen(Result result, IUnit unit)
//    {
//        Distance = result.Depth;
//        float height = (float)((double)Screen.ScreenHeight / Distance * (double)Scale);
//        var coo = RenderOperation1.GetPositionOnScreen(this, unit, height);
//        CoordinateOnScreen coordinate = new CoordinateOnScreen();
//        coordinate.X = coo.X;
//        coordinate.Up = coo.Y;
//        coordinate.Bottom = coo.Y;

//        return coordinate;
//    }

//    public void ProcessForRendering(ConcurrentDictionary<Type, bool> uniqueSelfDrawableTypes, ref bool hasNewTypes)
//    {
//        Type type = GetType();
//        if (uniqueSelfDrawableTypes.TryAdd(type, value: true))
//        {
//            hasNewTypes = true;
//        }

//        AddObstacleToRenderList();
//    }

//    public void AddObstacleToRenderList()
//    {
//        if (!IsAdded && !SpritesToRender.Contains(this))
//        {
//            SpritesToRender.Add(this);
//            //SpritesToRender = (List<SpriteObstacle1>)SpritesToRender.OrderByDescending(sprite => sprite.Distance);
//            IsAdded = true;
//        }
//    }

//    public static void RenderSelfDrawableList(Result result, IUnit unit)
//    {
//        if (SpritesToRender.Count == 0)
//        {
//            return;
//        }

//        foreach (SpriteObstacle1 item in SpritesToRender)
//        {
//            if (item != unit)
//            {
//                item.Render(result, unit);
//            }
//        }
//    }

//    public override IObject GetCopy()
//    {
//        return new SpriteObstacle1(this);
//    }
//    public bool isss;
//    public override void Render(Result result, IUnit unit)
//    {
//        Vector2f target = new Vector2f((float)X.Axis, (float)Y.Axis);
//        double num = MathUtils.CalculateAngleToTarget(target, unit.OriginPosition);
//        Distance = MathUtils.CalculateDistance(target, unit.OriginPosition);
//        if (!(Distance > (double)unit.MaxRenderTile))
//        {
//            AngleToObserver = MathUtils.NormalizeAngleDifference(unit.Angle, num);
//            if (Math.Abs(AngleToObserver) <= unit.Fov)
//            {
//                AnimationManager.DefiningDesiredSprite(Animation, num);
//                Distance *= Math.Cos(AngleToObserver);
//                Distance = Math.Max(Distance, 0.1);
//                float height = (float)((double)Screen.ScreenHeight / Distance * (double)Scale);

//                RenderOperation1.DrawSprite(this, unit, height);
//            }
//        }
//    }
//}
//public class Unit1 : SpriteObstacle1, IUnit
//{
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

//    public DeathAnimation DeathAnimation { get; set; }
//    public Action? DeathAction { get; set; }
//    public Action<float>? DamageAction { get; set; }

//    private void SetHp(float maxHp)
//    {
//        if (maxHp < _hp)
//            Hp = maxHp;
//    }
//    private float _maxHp = 0;
//    public float MaxHp
//    {
//        get => _maxHp;
//        private set
//        {
//            _maxHp = value > 0 ? value : _maxHp;
//            SetHp(_maxHp);
//        }
//    }

//    private void CheckHp()
//    {
//        if (_hp <= 0)
//            DeathAction?.Invoke();
//    }
//    private float _hp = 0;
//    public float Hp
//    {
//        get => _hp;
//        set
//        {
//            _hp = value;
//            CheckHp();
//        }
//    }


//    public MapLib.Map Map { get; set; }

//    public Unit1(MapLib.Map map, SpriteObstacle1 obstacle, int maxHp)
//        : base(obstacle)
//    {
//        Map = map;
//        MaxHp = maxHp;
//        Hp = maxHp;

//        DeathAction += AddToDeathManager;
//        DamageAction += ValidateDamage;
//        // OnPositionChanged += map.UpdateCoordinatesObstacle;

//        Fov = Math.PI / 3;
//        HalfFov = (float)Fov / 2;
//        // Z.Axis = 50;


//        Angle = 0;
//        Angle -= 0.000001;
//        VerticalAngle -= 0.000001;
//        MaxRenderTile = 1200;
//        ObserverSettingChangesFun();
//        Screen.WidthChangesFun += ObserverSettingChangesFun;
//    }
//    public Unit1(Unit1 unit)
//       : base(unit)
//    {

//        Map = unit.Map;
//        MaxHp = unit.MaxHp;
//        Hp = unit.MaxHp;
//        //OnPositionChanged += unit.Map.UpdateCoordinatesObstacle;

//        DeathAction += AddToDeathManager;
//        DamageAction += ValidateDamage;


//        Fov = unit.Fov;
//        HalfFov = unit.HalfFov;
//        // Z.Axis = 50;

//        Angle = unit.Angle;
//        VerticalAngle = unit.VerticalAngle;
//        MaxRenderTile = unit.MaxRenderTile;
//        ObserverSettingChangesFun();
//        Screen.WidthChangesFun += ObserverSettingChangesFun;
//    }

//    private void AddToDeathManager()
//    {
//        SpriteObstacle1.SpritesToRender.Remove(this);
//        Map.DeleteObstacleAsync(this);

//        Animation.SetFrames(DeathAnimation.Animation.GetFrames());
//        Animation.Index = 0;
//        Animation.IsAnimation = DeathAnimation.Animation.IsAnimation;
//        Animation.Speed = DeathAnimation.Animation.Speed;

//       // DeathManager.AddDiedObject(new DiedData(this, DeathAnimation, false));
//    }
//    private void ValidateDamage(float damage)
//    {
//        Hp -= damage;
//    }



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

//    public IUnit GetCopy()
//    {
//        return new Unit1(this);
//    }
//}

//class RenderOperation
//{
//    internal static Vector2f CalculationTextureScale(Result result, TexturedPair CurrentRenderTexture)
//    {
//        float x = (float)CurrentRenderTexture.Base.Scale / (float)CurrentRenderTexture.Base.Width;
//        float y = (float)result.ProjHeight / (float)CurrentRenderTexture.Base.Height;
//        return new Vector2f(x, y);
//    }

//    internal static int NormalizeLvlWall(TexturedWall Wall)
//    {
//        return Math.Abs(Wall.LvlWall - 1 + Wall.LvlWall);
//    }

//    internal static TexturedPair? SelectCurrentRenderTexture(TexturedWall Wall, Result result, IUnit unit)
//    {
//        ObjectSide objectSide = RayDetectionX.DetermineObjectSides(Wall, unit, result);
//        return (objectSide == ObjectSide.Error) ? null : Wall.MultiTextured.UniqueTexture.GetValue(objectSide);
//    }

//    internal static ObjectSide SelectCurrentObjectSide(TexturedWall Wall, Result result, IUnit unit)
//    {
//        return RayDetectionX.DetermineObjectSides(Wall, unit, result);
//    }
//}
//public class MultiWall2 : Obstacle, IDrawable, IRayRenderable, IRenderable
//{
//    private double _baseOffset = Screen.Setting.HalfVerticalTile;

//    public List<TexturedWall> Walls { get; private set; } = new List<TexturedWall>();


//    //
//    // Сводка:
//    //     Current wall levels being processed
//    private int CurrentLevelWall { get; set; } = 0;


//    public double BaseOffset
//    {
//        get
//        {
//            return _baseOffset;
//        }
//        set
//        {
//            _baseOffset = value;
//            ResetZ();
//        }
//    }

//    public MultiWall2()
//        : base(0.0, 0.0, Color.Red, isPassability: false)
//    {
//        Coordinate z = Z;
//        z.MoveAxis = (Action)Delegate.Combine(z.MoveAxis, new Action(ResetZ));
//    }

//    public MultiWall2(List<TexturedWall> walls)
//        : base(0.0, 0.0, Color.Red, isPassability: false)
//    {
//        AddLevelWall(walls);
//    }

//    public MultiWall2(MultiWall2 multiWall)
//        : base(0.0, 0.0, Color.Black, isPassability: false)
//    {
//        HitBox = new HitBox(multiWall.HitBox);
//        X = new Coordinate(multiWall.X, HitBox);
//        Y = new Coordinate(multiWall.Y, HitBox);
//        Z = new Coordinate(multiWall.Z, HitBox);
//        ColorInMap = multiWall.ColorInMap;
//        TextureInMiniMap = ((multiWall.TextureInMiniMap != null) ? new TextureObstacle(multiWall.TextureInMiniMap) : null);
//        IsPassability = multiWall.IsPassability;
//        IsSingleAddable = multiWall.IsSingleAddable;
//        SizeScale = multiWall.SizeScale;
//        PositionScale = multiWall.PositionScale;
//        foreach (TexturedWall wall in multiWall.Walls)
//        {
//            Walls.Add(new TexturedWall(wall));
//        }

//        CurrentLevelWall = multiWall.CurrentLevelWall;
//    }

//    public float CalculateTextureX(Vector2f UV, ObjectSide side)
//    {
//        if (Walls.Count == 0)
//        {
//            return 0f;
//        }

//        for (int i = 0; i < Walls.Count; i++)
//        {
//            if (i < Walls.Count - 1)
//            {
//                Walls[i].CalculateTextureX(UV, side);
//                continue;
//            }

//            return Walls[i].CalculateTextureX(UV, side);
//        }

//        return 0f;
//    }

//    public float CalculateTextureY(IUnit unit, float ProjHeight, float mult, float addCoordinates)
//    {
//        for (int i = 0; i < Walls.Count; i++)
//        {
//            if (Walls[i].CurrentRenderTexture != null)
//            {
//                float num = Walls[i].CalculateTextureY(unit, ProjHeight / (float)(i + 1), mult * (float)(i + 1), addCoordinates);
//                float num2 = Screen.Setting.VerticalTile / (float)(Walls[i].CurrentRenderTexture?.Base.Height ?? 1);
//               // num -= (float)(Walls[i].Z.Axis / (double)num2);
//                //if (i != 0 && Walls[i].CurrentRenderTexture != null)
//               // {
//                    //num = num;// + num;
//               // }

//                if (num > 0f && num < (float?)Walls[i].CurrentRenderTexture?.Base.Height)
//                {
//                    CurrentLevelWall = i;
//                    return num;
//                }
//            }
//        }

//        return 0f;
//    }

//    public float BringingToStandard(float heightObj)
//    {
//        if (Walls.Count == 0)
//        {
//            return 0f;
//        }

//        return Walls.First().BringingToStandard(heightObj);
//    }

//    public float GetAveragedMult(float baseMult)
//    {
//        if (Walls.Count == 0)
//        {
//            return 0f;
//        }

//        return Walls.First().GetAveragedMult(baseMult);
//    }

//    public bool IsInsideTexture(float textureX, float textureY)
//    {
//        TexturedPair currentRenderTexture = Walls[CurrentLevelWall].CurrentRenderTexture;
//        if (currentRenderTexture == null)
//        {
//            return true;
//        }

//        TextureObstacle @base = currentRenderTexture.Base;
//        if (textureX < 0f || textureX > (float)@base.Width)
//        {
//            return true;
//        }

//        if (textureY < 0f || textureY > (float)@base.Height)
//        {
//            return true;
//        }

//        return false;
//    }

//    public void DrawObject(Drawable drawObject)
//    {
//        if (CurrentLevelWall >= 0 && CurrentLevelWall < Walls.Count && Walls[CurrentLevelWall].CurrentRenderTexture != null)
//        {
//            Walls[CurrentLevelWall].CurrentRenderTexture?.Mod.Draw(drawObject);
//            Walls[CurrentLevelWall].CurrentRenderTexture?.Mod.Display();
//        }
//    }

//    public void DrawObjectAsync(Drawable drawObject)
//    {
//        DrawingQueue.EnqueueDraw((DrawObject, drawObject));
//    }

//    private void UpdateHeightHitBox(double newZ)
//    {
//        HitBox.MainHitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(newZ);
//        HitBox.MainHitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(newZ);
//    }

//    private void UpdateHeight()
//    {
//        UpdateHeightHitBox((double)Walls.Count * BaseOffset);
//        Z.Axis = (double)(Walls.Count - 1) * BaseOffset;
//    }

//    private void HandleObjectAdditionWalls(double x, double y, bool resetHitBoxSide)
//    {
//        foreach (TexturedWall wall in Walls)
//        {
//            wall.HandleObjectAddition(x, y, resetHitBoxSide);
//        }
//    }

//    public override void HandleObjectAddition(double x, double y, bool resetHitBoxSide = true)
//    {
//        if (resetHitBoxSide)
//        {
//            HitBox.MainHitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(0.0);
//            HitBox.MainHitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(Screen.Setting.Tile);
//            HitBox.MainHitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(0.0);
//            HitBox.MainHitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(Screen.Setting.Tile);
//        }

//        HandleObjectAdditionWalls(x, y, resetHitBoxSide);
//        X.Axis = x;
//        Y.Axis = y;
//    }

//    public override void FillingColorShape(RectangleShape rectangleShape, float OutlineThickness = 1f)
//    {
//        rectangleShape.OutlineThickness = OutlineThickness;
//        rectangleShape.FillColor = ColorInMap;
//    }

//    public override void FillingTextureShape(RectangleShape rectangleShape)
//    {
//        if (Walls.Count > 0)
//        {
//            foreach (TexturedWall wall in Walls)
//            {
//                TextureObstacle textureObstacle = wall.MultiTextured.UniqueTexture.GetFirstValue()?.Base;
//                if (textureObstacle != null)
//                {
//                    rectangleShape.Texture = textureObstacle.Texture;
//                    return;
//                }
//            }
//        }

//        rectangleShape.FillColor = ColorInMap;
//    }

//    public override Vector2f ConversionToMapCoordinates(float mapTile)
//    {
//        float x = (float)X.Axis / (float)Screen.Setting.Tile * mapTile;
//        float y = (float)Y.Axis / (float)Screen.Setting.Tile * mapTile;
//        return new Vector2f(x, y);
//    }

//    public void ProcessForRendering(List<InfoObject> infoObject, double coordinate, double depth, double maxDepth)
//    {
//        if (depth < maxDepth)
//        {
//            infoObject.Add(new InfoObject(depth, coordinate, this));
//        }
//    }

//    public override CoordinateOnScreen GetPositionOnScreen(Result result, IUnit unit)
//    {
//        if (Walls.Count == 0)
//        {
//            return default(CoordinateOnScreen);
//        }

//        return Walls.LastOrDefault()?.GetPositionOnScreen(result, unit) ?? default(CoordinateOnScreen);
//    }

//    public override IObject GetCopy()
//    {
//        return new MultiWall2(this);
//    }

//    public override void Render(Result result, IUnit unit)
//    {
//        if (Walls.Count <= 0)
//        {
//            return;
//        }

//        ObjectSide objectSide = RenderOperation.SelectCurrentObjectSide(Walls.First(), result, unit);
//        foreach (TexturedWall wall in Walls)
//        {
//            wall.RenderMultiWall(result, unit, objectSide);
//        }
//    }

//    private void ResetZ()
//    {
//        for (int i = 0; i < Walls.Count; i++)
//        {
//            Walls[i].SetLevelWall(Z.Axis - (double)(Walls.Count - (i + 1)) * BaseOffset, BaseOffset, i + 1);
//        }
//    }

//    public void AddLevelWall(TexturedWall wall)
//    {
//        wall.HandleObjectAddition(X.Axis, Y.Axis, resetHitBoxSide: true);
//        Walls.Add(wall);
//        UpdateHeight();
//    }

//    public void AddLevelWall(List<TexturedWall> walls)
//    {
//        if (walls.Count == 0)
//        {
//            return;
//        }

//        foreach (TexturedWall wall in walls)
//        {
//            wall.HandleObjectAddition(X.Axis, Y.Axis, resetHitBoxSide: true);
//            Walls.Add(wall);
//        }

//        UpdateHeight();
//    }

//    public void DeleteWall(int lvl)
//    {
//        if (Walls.Count != 0 && lvl >= 1 && lvl <= Walls.Count)
//        {
//            lvl--;
//            Walls.RemoveAt(lvl);
//            UpdateHeight();
//        }
//    }
//}

//public class RenderAlgorithm2(Map map, ProtoRender.Object.IUnit unit)
//{
//    //------------------------------Pool-------------------------------
//    static ObjectPool<Result> resultobjectPool = new ObjectPool<Result>();
//    static ListPool<InfoObject> infoObjectPool = new ListPool<InfoObject>();


//    //------------------------------Setting Render-------------------------------
//    /// <summary> List of unique types of objects that render themselves (not with rays) </summary>
//    private ConcurrentDictionary<Type, bool> UniqueSelfDrawableTypes { get; init; } = new();
//    /// <summary> List with rendering methods of objects that render themselves (not rays) </summary>
//    private Dictionary<Type, Action<Result, IUnit>> CachedDelegates { get; init; } = new();
//    /// <summary> Check if a new object type has been added that renders itself </summary>
//    private bool HasNewTypes = false;


//    /// <summary> Writes a new type of object that renders itself </summary>
//    public void PrepareRenderObjects()
//    {
//        foreach (var type in UniqueSelfDrawableTypes.Keys)
//        {
//            if (!CachedDelegates.TryGetValue(type, out var del))
//            {
//                var method = type.GetMethod(ISelfRenderable.NameRenderFun,
//                                            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

//                if (method != null)
//                {
//                    del = (Action<Result, IUnit>)Delegate.CreateDelegate(typeof(Action<Result, IUnit>), method);
//                    CachedDelegates[type] = del;
//                }
//            }
//            del?.Invoke(new Result(), unit);
//        }
//    }

//    /// <summary> Checks the type of objects in a map cell </summary>
//    private bool ProcessingHeightObstacle(List<InfoObject> infoObject,
//        double x, double y,
//        int mappedX, int mappedY,
//        double depth_h, double depth_v,
//        double auxiliary, bool isVertical)
//    {
//        bool isAdded = false;

//        if (!map.Obstacles.TryGetValue((mappedX, mappedY), out var obstacles))
//            return false;

//        double coordinate = 0;
//        double depth = 0;
//        if (isVertical)
//        {
//            coordinate = y;
//            depth = depth_v;
//        }
//        else
//        {
//            coordinate = x;
//            depth = depth_h;
//        }


//        foreach (var obstacle in obstacles)
//        {
//            switch (obstacle)
//            {
//                case ISelfRenderable self:
//                    self.ProcessForRendering(UniqueSelfDrawableTypes, ref HasNewTypes);
//                    break;

//                case IRayRenderable ray:
//                    ray.ProcessForRendering(infoObject, coordinate, depth, unit.MaxRenderTile); isAdded = true;
//                    break;

//                default:
//                    throw new Exception($"Invalid object for rendering(CheckAndAddObstacle)\nType: {obstacle.GetType()}");
//            }
//        }

//        return isAdded;
//    }

//    /// <summary> Filters horizontal and vertical rays by height and range </summary>
//    /// /// <returns> Returns a list of InfoObjects to render</returns>
//    private static List<InfoObject> FilterVisibleObstacles(IUnit unit, List<InfoObject> info, bool rayPassability)
//    {
//        int count = info.Count;

//        if (count <= 1) return info;
//        if (!rayPassability && count == 2)
//        {
//            var first = info[0];
//            var second = info[1];

//            return new List<InfoObject> { first.depth < second.depth ? first : second };
//        }


//        var span = CollectionsMarshal.AsSpan(info);
//        span.Sort((a, b) => a.depth.CompareTo(b.depth));

//        var filtered = new List<InfoObject>(count);
//        InfoObject? current = null;
//        bool t = false;
//        foreach (var item in span)
//        {
//            //if (item.Object?.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.Side < -1000)
//            //{
//            //   // Console.WriteLine($"item: {item.Object?.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.Side} | cur: {current?.Object?.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.Side}");
//            //   // Console.WriteLine($"dep item: {item.depth} | dep cur: {current?.depth}");
//            //}
//            if (t  && (current == null || (item.depth > current.depth)))// && item.Object?.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.Side < current.Object?.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.Side))
//            {

//                filtered.Add(item);
//                current = item;
//            }
//            if (!t && (current == null || (item.depth > current.depth && item.Object?.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.Side < current.Object?.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.Side || item.Object?.HitBox[CoordinatePlane.Z, SideSize.Larger]?.Side > current.Object?.HitBox[CoordinatePlane.Z, SideSize.Larger]?.Side)))
//            {

//                filtered.Add(item);
//                current = item;
//            }
//            //if (current == null || (item.depth > current.depth && item.Object?.Z.Axis > current.Object?.Z.Axis))
//            //{
//            //    filtered.Add(item);
//            //    current = item;
//            //}
//        }

//        return filtered;
//    }

//    /// <summary> Determines in which axis the ray should move </summary>
//    private static void CheckVericals(ref double coordinate, ref double auxiliaryA, double mapCoordinate, double ratio)
//    {

//        if (ratio >= 0)
//        {
//            coordinate = mapCoordinate + Screen.Setting.Tile;
//            auxiliaryA = 1;
//        }
//        else
//        {
//            coordinate = mapCoordinate;
//            auxiliaryA = -1;
//        }
//    }


//    /// <summary> Render ready sorted objects (which are rendered using rays) </summary>
//    private void RenderRayObstacles(int ray, bool rayPassability, double carAngleRay, List<InfoObject> InfoObject, Result ParallelResult)
//    {
//        var visibleObstacles = FilterVisibleObstacles(unit, InfoObject, rayPassability);
//        int sizeVisibleObst = visibleObstacles.Count;
//        for (int obst = 0; obst < sizeVisibleObst; obst++)
//        {
//            if (obst > 0 && sizeVisibleObst > 1)
//            {
//                ParallelResult.PositionPreviousObject = visibleObstacles[obst - 1].Object?.GetPositionOnScreen(ParallelResult, unit);
//            }

//            ParallelResult.CalculationSettingRender(unit, ray, visibleObstacles[obst].depth, visibleObstacles[obst].coordinate, carAngleRay);
//            visibleObstacles[obst].Object?.Render(ParallelResult, unit);
//        }
//    }

//    /// <summary> Bresenham's algorithm renders objects </summary>
//    public void CalculationAlgorithm(bool rayPassability = true)
//    {
//        double carAngle = unit.Angle - unit.HalfFov;

//        Vector2i coordinates = Screen.MappingVector(unit.X.Axis, unit.Y.Axis);

//        double unitX = unit.X.Axis;
//        double unitY = unit.Y.Axis;
//        double unitDeltaAngle = unit.DeltaAngle;
//        double unitMaxRenderTile = unit.MaxRenderTile;
//        int tile = Screen.Setting.Tile;

//        Parallel.For(0, Screen.Setting.AmountRays, Screen.Setting.ParallelOptions, ray =>
//        {
//            var ParallelResult = resultobjectPool.Get();
//            var ParallelInfoObj = infoObjectPool.Get();

//            double hx = 0, x = 0, auxiliaryX = 0, depth_h = 0;
//            double vy = 0, y = 0, auxiliaryY = 0, depth_v = 0;

//            double carAngleRay = carAngle + ray * unitDeltaAngle;
//            double sinA = Math.Sin(carAngleRay);
//            double cosA = Math.Cos(carAngleRay);
//            ParallelResult.SinCarAngle = sinA;
//            ParallelResult.CosCarAngle = cosA;

//            CheckVericals(ref x, ref auxiliaryX, coordinates.X, cosA);
//            for (int j = 0; j < unitMaxRenderTile; j += tile)
//            {
//                depth_v = (x - unitX) / cosA;
//                vy = unitY + depth_v * sinA;

//                (int, int) mappX = Screen.Mapping(x + auxiliaryX, vy);
//                if (map.CheckTrueCoordinates(mappX))
//                {
//                    if (ProcessingHeightObstacle(ParallelInfoObj, x, vy, mappX.Item1, mappX.Item2, depth_h, depth_v, auxiliaryX, true) && !rayPassability)
//                        break;
//                }
//                else
//                    break;

//                x += auxiliaryX * tile;
//            };

//            CheckVericals(ref y, ref auxiliaryY, coordinates.Y, sinA);
//            for (int j = 0; j < unitMaxRenderTile; j += tile)
//            {
//                depth_h = (y - unitY) / sinA;
//                hx = unitX + depth_h * cosA;

//                (int, int) mappY = Screen.Mapping(hx, y + auxiliaryY);
//                if (map.CheckTrueCoordinates(Screen.Mapping(hx, y + auxiliaryY)))
//                {
//                    if (ProcessingHeightObstacle(ParallelInfoObj, hx, y, mappY.Item1, mappY.Item2, depth_h, depth_v, auxiliaryY, false) && !rayPassability)
//                        break;
//                }
//                else
//                    break;

//                y += auxiliaryY * tile;
//            };


//            RenderRayObstacles(ray, rayPassability, carAngleRay, ParallelInfoObj, ParallelResult);

//            resultobjectPool.Return(ParallelResult);
//            infoObjectPool.Return(ParallelInfoObj);
//        });


//        if (HasNewTypes)
//        {
//            PrepareRenderObjects();
//            HasNewTypes = false;
//        }
//        CachedDelegates.ForEach(cd => cd.Value(new Result(), unit));
//        ZBuffer.Render();
//    }
//}

//public class RenderAlgorithm3(Map map, ProtoRender.Object.IUnit unit)
//{
//    //------------------------------Pool-------------------------------
//    static ObjectPool<Result> resultobjectPool = new ObjectPool<Result>();
//    static ListPool<InfoObject> infoObjectPool = new ListPool<InfoObject>();


//    //------------------------------Setting Render-------------------------------
//    /// <summary> List of unique types of objects that render themselves (not with rays) </summary>
//    private ConcurrentDictionary<Type, bool> UniqueSelfDrawableTypes { get; init; } = new();
//    /// <summary> List with rendering methods of objects that render themselves (not rays) </summary>
//    private Dictionary<Type, Action<Result, IUnit>> CachedDelegates { get; init; } = new();
//    /// <summary> Check if a new object type has been added that renders itself </summary>
//    private bool HasNewTypes = false;


//    /// <summary> Writes a new type of object that renders itself </summary>
//    public void PrepareRenderObjects()
//    {
//        foreach (var type in UniqueSelfDrawableTypes.Keys)
//        {
//            if (!CachedDelegates.TryGetValue(type, out var del))
//            {

//                var method = type.GetMethod(ISelfRenderable.NameRenderFun,
//                                            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

//                if (method != null)
//                {
//                    del = (Action<Result, IUnit>)Delegate.CreateDelegate(typeof(Action<Result, IUnit>), method);
//                    CachedDelegates[type] = del;
//                }
//            }
//            del?.Invoke(new Result(), unit);
//        }
//    }

//    /// <summary> Checks the type of objects in a map cell </summary>
//    private bool ProcessingHeightObstacle(List<InfoObject> infoObject,
//        double x, double y,
//        int mappedX, int mappedY,
//        double depth_h, double depth_v,
//        double auxiliary, bool isVertical)
//    {
//        bool isAdded = false;

//        if (!map.Obstacles.TryGetValue((mappedX, mappedY), out var obstacles))
//            return false;

//        double coordinate = 0;
//        double depth = 0;
//        if (isVertical)
//        {
//            coordinate = y;
//            depth = depth_v;
//        }
//        else
//        {
//            coordinate = x;
//            depth = depth_h;
//        }


//        foreach (var obstacle in obstacles)
//        {
//            switch (obstacle)
//            {
//                case ISelfRenderable self:
//                    self.ProcessForRendering(UniqueSelfDrawableTypes, ref HasNewTypes);
//                    break;

//                case IRayRenderable ray:
//                    ray.ProcessForRendering(infoObject, coordinate, depth, unit.MaxRenderTile); isAdded = true;
//                    break;

//                default:
//                    throw new Exception($"Invalid object for rendering(CheckAndAddObstacle)\nType: {obstacle.GetType()}");
//            }
//        }

//        return isAdded;
//    }

//    /// <summary> Filters horizontal and vertical rays by height and range </summary>
//    /// /// <returns> Returns a list of InfoObjects to render</returns>
//    private static List<InfoObject> FilterVisibleObstacles(List<InfoObject> info, bool rayPassability)
//    {
//        int count = info.Count;

//        if (count <= 1) return info;
//        if (!rayPassability && count == 2)
//        {
//            var first = info[0];
//            var second = info[1];

//            return new List<InfoObject> { first.depth < second.depth ? first : second };
//        }


//        var span = CollectionsMarshal.AsSpan(info);
//        span.Sort((a, b) => a.depth.CompareTo(b.depth));

//        var filtered = new List<InfoObject>(count);
//        InfoObject? current = null;
//        foreach (var item in span)
//        {
//            if (current == null || (item.depth > current.depth && item.Object?.Z.Axis > current.Object?.Z.Axis))
//            {
//                filtered.Add(item);
//                current = item;
//            }
//        }

//        return filtered;
//    }

//    /// <summary> Determines in which axis the ray should move </summary>
//    private static void CheckVericals(ref double coordinate, ref double auxiliaryA, double mapCoordinate, double ratio)
//    {

//        if (ratio >= 0)
//        {
//            coordinate = mapCoordinate + Screen.Setting.Tile;
//            auxiliaryA = 1;
//        }
//        else
//        {
//            coordinate = mapCoordinate;
//            auxiliaryA = -1;
//        }
//    }


//    /// <summary> Render ready sorted objects (which are rendered using rays) </summary>
//    private void RenderRayObstacles(int ray, bool rayPassability, double carAngleRay, List<InfoObject> InfoObject, Result ParallelResult)
//    {
//        var visibleObstacles = FilterVisibleObstacles(InfoObject, rayPassability);
//        int sizeVisibleObst = visibleObstacles.Count;
//        for (int obst = 0; obst < sizeVisibleObst; obst++)
//        {
//            if (obst > 0 && sizeVisibleObst > 1)
//                ParallelResult.PositionPreviousObject = visibleObstacles[obst - 1].Object?.GetPositionOnScreen(ParallelResult, unit);

//            ParallelResult.CalculationSettingRender(unit, ray, visibleObstacles[obst].depth, visibleObstacles[obst].coordinate, carAngleRay);
//            visibleObstacles[obst].Object?.Render(ParallelResult, unit);
//        }
//    }

//    /// <summary> Bresenham's algorithm renders objects </summary>
//    public void CalculationAlgorithm(bool i, bool rayPassability = true)
//    {
//        double carAngle = unit.Angle - unit.HalfFov;

//        Vector2i coordinates = Screen.MappingVector(unit.X.Axis, unit.Y.Axis);

//        double unitX = unit.X.Axis;
//        double unitY = unit.Y.Axis;
//        double unitDeltaAngle = unit.DeltaAngle;
//        double unitMaxRenderTile = unit.MaxRenderTile;
//        int tile = Screen.Setting.Tile;

//        Parallel.For(0, Screen.Setting.AmountRays, Screen.Setting.ParallelOptions, ray =>
//        {
//            if ((!i && ray < Screen.Setting.HalfWidth) || (i && ray > Screen.Setting.HalfWidth))
//                return;

//            var ParallelResult = resultobjectPool.Get();
//            var ParallelInfoObj = infoObjectPool.Get();

//            double hx = 0, x = 0, auxiliaryX = 0, depth_h = 0;
//            double vy = 0, y = 0, auxiliaryY = 0, depth_v = 0;

//            double carAngleRay = carAngle + ray * unitDeltaAngle;
//            double sinA = Math.Sin(carAngleRay);
//            double cosA = Math.Cos(carAngleRay);
//            ParallelResult.SinCarAngle = sinA;
//            ParallelResult.CosCarAngle = cosA;

//            CheckVericals(ref x, ref auxiliaryX, coordinates.X, cosA);
//            for (int j = 0; j < unitMaxRenderTile; j += tile)
//            {
//                depth_v = (x - unitX) / cosA;
//                vy = unitY + depth_v * sinA;

//                (int, int) mappX = Screen.Mapping(x + auxiliaryX, vy);
//                if (map.CheckTrueCoordinates(mappX))
//                {
//                    if (ProcessingHeightObstacle(ParallelInfoObj, x, vy, mappX.Item1, mappX.Item2, depth_h, depth_v, auxiliaryX, true) && !rayPassability)
//                        break;
//                }
//                else
//                    break;

//                x += auxiliaryX * tile;
//            };

//            CheckVericals(ref y, ref auxiliaryY, coordinates.Y, sinA);
//            for (int j = 0; j < unitMaxRenderTile; j += tile)
//            {
//                depth_h = (y - unitY) / sinA;
//                hx = unitX + depth_h * cosA;

//                (int, int) mappY = Screen.Mapping(hx, y + auxiliaryY);
//                if (map.CheckTrueCoordinates(Screen.Mapping(hx, y + auxiliaryY)))
//                {
//                    if (ProcessingHeightObstacle(ParallelInfoObj, hx, y, mappY.Item1, mappY.Item2, depth_h, depth_v, auxiliaryY, false) && !rayPassability)
//                        break;
//                }
//                else
//                    break;

//                y += auxiliaryY * tile;
//            };


//            RenderRayObstacles(ray, rayPassability, carAngleRay, ParallelInfoObj, ParallelResult);

//            resultobjectPool.Return(ParallelResult);
//            infoObjectPool.Return(ParallelInfoObj);
//        });


//        if (HasNewTypes)
//        {
//            PrepareRenderObjects();
//            HasNewTypes = false;
//        }
//        CachedDelegates.ForEach(cd => cd.Value(new Result(), unit));
//        ZBuffer.Render();
//    }
//}
//public abstract class Obstacle : IObject, IRenderable, ProtoRender.Map.IMiniMapRenderable, HitBoxLib.Data.HitBoxObject.IHitBoxProcessor, ProtoRender.Map.IMapAdder
//{
//    private double shiftCubedX = 0.0;

//    private double shiftCubedY = 0.0;

//    public Action<IObject, double, double>? OnPositionChanged { get; set; }

//    public virtual Coordinate X { get; init; }

//    public virtual Coordinate Y { get; init; }

//    public virtual Coordinate Z { get; init; }

//    public virtual HitBox HitBox { get; init; } = new HitBox();


//    //
//    // Сводка:
//    //     Offset of an object along the current map cell(On the X axis)
//    public double ShiftCubedX
//    {
//        get
//        {
//            return shiftCubedX;
//        }
//        set
//        {
//            shiftCubedX = ((value < 0.0) ? 1.0 : ((value > 99.0) ? 99.0 : value));
//            X.Axis = (double)Screen.Mapping(X.Axis, Screen.Setting.Tile) + shiftCubedX;
//        }
//    }

//    //
//    // Сводка:
//    //     Offset of an object along the current map cell(On the Y axis)
//    public double ShiftCubedY
//    {
//        get
//        {
//            return shiftCubedY;
//        }
//        set
//        {
//            shiftCubedY = ((value < 0.0) ? 1.0 : ((value > 99.0) ? 99.0 : value));
//            Y.Axis = (double)Screen.Mapping(Y.Axis, Screen.Setting.Tile) + shiftCubedY;
//        }
//    }

//    public virtual SFML.Graphics.Color ColorInMap { get; set; }

//    public virtual TextureObstacle? TextureInMiniMap { get; set; }

//    public virtual float SizeScale { get; set; } = 1f;


//    public virtual float PositionScale { get; set; } = 1f;


//    //
//    // Сводка:
//    //     The passability of an object through the current object
//    public virtual bool IsPassability { get; set; }

//    //
//    // Сводка:
//    //     Possibility to add an object to the same cell where the current object is located
//    public virtual bool IsSingleAddable { get; set; } = false;


//    public void SetShifts(double shifts)
//    {
//        ShiftCubedX = shifts;
//        ShiftCubedY = shifts;
//    }

//    public void SetShifts(double shiftsX, double shiftsY)
//    {
//        ShiftCubedX = shiftsX;
//        ShiftCubedY = shiftsY;
//    }

//    public Obstacle(double x, double y, SFML.Graphics.Color colorInMap, bool isPassability)
//    {
//        ColorInMap = colorInMap;
//        IsPassability = isPassability;
//        X = new Coordinate(CoordinatePlane.X, HitBox);
//        Y = new Coordinate(CoordinatePlane.Y, HitBox);
//        Z = new Coordinate(CoordinatePlane.Z, HitBox);
//    }

//    public abstract void Render(Result result, IUnit unit);

//    public abstract IObject GetCopy();

//    public abstract void HandleObjectAddition(double x, double y, bool resetHitBoxSide);

//    public virtual float WorldToScreenY(double angleVertical, float addVariable = 0f)
//    {
//        if (angleVertical <= 0.0)
//        {
//            return (float)((double)Screen.Setting.HalfHeight - (double)Screen.Setting.HalfHeight * angleVertical - (double)addVariable);
//        }

//        angleVertical += 1.0;
//        return (float)((double)Screen.Setting.HalfHeight / angleVertical - (double)addVariable);
//    }

//    public virtual float WorldToScreenX(double Angle, double DeltaAngle)
//    {
//        int num = (int)(Angle / DeltaAngle);
//        int num2 = Screen.Setting.CenterRay + num;
//        return num2;
//    }

//    public virtual float WorldToScreenX(int ray)
//    {
//        return (float)ray * (float)Screen.Setting.Scale;
//    }

//    public abstract CoordinateOnScreen GetPositionOnScreen(Result result, IUnit unit);

//    public abstract void FillingColorShape(RectangleShape rectangleShape, float OutlineThickness = 1f);

//    public abstract void FillingTextureShape(RectangleShape rectangleShape);

//    public abstract Vector2f ConversionToMapCoordinates(float mapTile);

//    public virtual float CoordinatesOffsetMap(float baseOffset)
//    {
//        return baseOffset / PositionScale;
//    }

//    public virtual float SizeOffsetMap(float baseOffset)
//    {
//        return baseOffset / SizeScale;
//    }

//    public virtual float WorldToScreenSideY(double side, double distance, double verticalAngle, double angleObject)
//    {
//        distance /= (double)Screen.Setting.Tile;
//        distance *= Math.Cos(angleObject);
//        distance = Math.Max(distance, 0.40000000596046448);
//        return WorldToScreenY(verticalAngle) - (float)(side / distance);
//    }

//    public virtual HitBoxLib.Data.HitBoxObject.RenderInfo GetRenderHitBoxInfo()
//    {
//        HitBoxLib.Data.HitBoxObject.RenderInfo result = default(HitBoxLib.Data.HitBoxObject.RenderInfo);
//        result.position = new Vector3f((float)X.Axis, (float)Y.Axis, (float)Z.Axis);
//        result.hitBox = HitBox;
//        result.worldToScreenY = WorldToScreenSideY;
//        result.worldToScreenX = WorldToScreenX;
//        return result;
//    }
//}

//public static class Adder
//{
//    public static void AddTexture(SpriteObstacle sprite, TextureObstacle texture)
//    {
//        sprite.Animation.AddFrame(texture);
//        if (sprite.TextureInMiniMap == null && sprite.Animation.AmountFrame > 0)
//        {
//            sprite.TextureInMiniMap = sprite.Animation.GetFrame(0);
//        }
//    }

//    public static void AddTextures(SpriteObstacle sprite, List<TextureObstacle> textures)
//    {
//        foreach (TextureObstacle texture in textures)
//        {
//            AddTexture(sprite, texture);
//        }
//    }

//    public static void AddTextures(SpriteObstacle sprite, List<string> paths)
//    {
//        foreach (string path in paths)
//        {
//            AddTexture(sprite, path);
//        }
//    }

//    public static void AddTexture(SpriteObstacle sprite, string path)
//    {
//        sprite.Animation.AddFrame(ImageLoader.LoadFrame(path));
//    }

//    public static void AddTextureFromFolder(SpriteObstacle sprite, string path, bool isDirectory, bool folderAccounting)
//    {
//        if (isDirectory)
//        {
//            sprite.Animation.AddFrames(ImageLoader.TexturesLoadFromFolder(path, folderAccounting));
//        }
//        else
//        {
//            AddTexture(sprite, path);
//        }
//    }
//}

//public class SpriteObstacle : Obstacle, ISelfRenderable
//{

//    //-------------------List Sprites Render------------------
//    public static List<SpriteObstacle> SpritesToRender { get; private set; } = new List<SpriteObstacle>();
//    private bool IsAdded { get; set; } = false;


//    //---------------------------Textures------------------------------
//    public AnimationState Animation { get; init; } = new();
//    public SFML.Graphics.Sprite RenderSprite { get; set; } = new SFML.Graphics.Sprite();


//    //--------------------------Setting-----------------------------

//    private float scale = 1;
//    /// <summary>Texture scale</summary>
//    public float Scale
//    {
//        get => scale * Screen.ScreenRatio;
//        set => scale = value == 0 ? 1 : value;
//    }
//    /// <summary>Size scale</summary>
//    public override float SizeScale { get; set; } = 2;
//    /// <summary>Position scale in MiniMap</summary>
//    public override float PositionScale { get; set; } = 4;
//    //---------------------Render Parameters----------------------
//    /// <summary>Angle relative to this object and the observer</summary>
//    public double AngleToObserver { get; set; }
//    public double Distance { get; set; }


//    #region Constructor
//    public SpriteObstacle(List<TextureObstacle> textures, bool isPassability = false)
//        : base(0, 0, SFML.Graphics.Color.White, isPassability)
//    {
//        Adder.AddTextures(this, textures);
//    }
//    public SpriteObstacle(TextureObstacle texture, bool isPassability = false)
//       : base(0, 0, SFML.Graphics.Color.White, isPassability)
//    {
//        Adder.AddTexture(this, texture);
//    }
//    public SpriteObstacle(string path, bool isDirectory, bool folderAccounting = false, bool isPassability = false)
//       : base(0, 0, SFML.Graphics.Color.White, isPassability)
//    {
//        Adder.AddTextureFromFolder(this, path, isDirectory, folderAccounting);
//    }
//    public SpriteObstacle(List<string> paths, bool isPassability = false)
//       : base(0, 0, SFML.Graphics.Color.White, isPassability)
//    {
//        Adder.AddTextures(this, paths);
//    }
//    /// <summary>Constructor class AnimationState</summary>
//    /// <param name="spriteObstacle">Object of SpriteObstacle</param>
//    /// <param name="updateAnimation">true - frames and state are duplicated, false - frames are duplicated but state is not copied</param>
//    public SpriteObstacle(SpriteObstacle spriteObstacle, bool updateAnimation = true)
//    : base(0, 0, SFML.Graphics.Color.Black, false)
//    {
//        HitBox = new HitBox(spriteObstacle.HitBox);

//        X = new Coordinate(spriteObstacle.X, HitBox);
//        Y = new Coordinate(spriteObstacle.Y, HitBox);
//        Z = new Coordinate(spriteObstacle.Z, HitBox);

//        ColorInMap = spriteObstacle.ColorInMap;
//        TextureInMiniMap = spriteObstacle.TextureInMiniMap is not null ? new TextureObstacle(spriteObstacle.TextureInMiniMap) : null;

//        IsPassability = spriteObstacle.IsPassability;
//        IsSingleAddable = spriteObstacle.IsSingleAddable;

//        SizeScale = spriteObstacle.SizeScale;
//        PositionScale = spriteObstacle.PositionScale;

//        Scale = spriteObstacle.Scale;
//        AngleToObserver = spriteObstacle.AngleToObserver;
//        Distance = spriteObstacle.Distance;

//        if (updateAnimation == true)
//            Animation = new AnimationState(spriteObstacle.Animation);
//        else
//            Animation = new AnimationState(spriteObstacle.Animation.GetFrames());

//        IsAdded = spriteObstacle.IsAdded;
//    }
//    #endregion

//    #region MapAdder_Implementation
//    public override void HandleObjectAddition(double x, double y, bool resetHitBoxSide = false)
//    {
//        X.Axis = x;
//        Y.Axis = y;

//        ShiftCubedX = ShiftCubedX;
//        ShiftCubedY = ShiftCubedY;
//    }

//    #endregion

//    #region IMiniMapRenderable_Implementation
//    public override void FillingColorShape(RectangleShape rectangleShape, float OutlineThickness = 1)
//    {
//        rectangleShape.OutlineThickness = OutlineThickness;
//        rectangleShape.FillColor = ColorInMap;
//    }
//    public override void FillingTextureShape(RectangleShape rectangleShape)
//    {
//        if (TextureInMiniMap is not null)
//            rectangleShape.Texture = TextureInMiniMap.Texture;
//        else if (TextureInMiniMap is null && Animation.AmountFrame > 0)
//        {
//            TextureInMiniMap = Animation.GetFrame(0);
//            rectangleShape.Texture = TextureInMiniMap?.Texture;
//        }
//        else
//            rectangleShape.FillColor = ColorInMap;
//    }
//    public override Vector2f ConversionToMapCoordinates(float mapTile)
//    {
//        float x = (float)X.Axis / Screen.Setting.Tile * mapTile;
//        float y = (float)Y.Axis / Screen.Setting.Tile * mapTile;

//        return new Vector2f(x, y);
//    }
//    #endregion

//    #region IRenderable_Implementation
//    public override CoordinateOnScreen GetPositionOnScreen(Result result, IUnit unit)
//    {
//        Distance = result.Depth;

//        float height = (float)(Screen.ScreenHeight / Distance * Scale);

//        var coo = RenderOperation2.GetPositionOnScreen(this, unit, height);
//        CoordinateOnScreen coordinate = new CoordinateOnScreen();
//        coordinate.X = coo.X;
//        coordinate.Up = coo.Y;
//        coordinate.Bottom = coo.Y;

//        return coordinate;
//    }
//    #endregion

//    #region ISelfDrawable_Implementation
//    public void ProcessForRendering(ConcurrentDictionary<Type, bool> uniqueSelfDrawableTypes, ref bool hasNewTypes)
//    {
//        var type = this.GetType();
//        if (uniqueSelfDrawableTypes.TryAdd(type, true))
//            hasNewTypes = true;

//        this.AddObstacleToRenderList();
//    }
//    public void AddObstacleToRenderList()
//    {
//        if (IsAdded == true)
//            return;
//        else if (SpritesToRender.Contains(this))
//            return;

//        SpritesToRender.Add(this);
//        IsAdded = true;
//    }
//    public static void RenderSelfDrawableList(Result result, IUnit unit)
//    {
//        if (SpritesToRender.Count == 0)
//            return;

//        foreach (var sprite in SpritesToRender)
//        {
//            if (sprite == unit)
//                continue;

//            sprite.Render(result, unit);
//        }
//    }
//    #endregion

//    public override IObject GetCopy()
//    {
//        return new SpriteObstacle(this);
//    }
//    public override void Render(Result result, IUnit unit)
//    {
//        Vector2f pos = new Vector2f((float)X.Axis, (float)Y.Axis);
//        double spriteAngle = MathUtils.CalculateAngleToTarget(pos, unit.OriginPosition);
//        Distance = MathUtils.CalculateDistance(pos, unit.OriginPosition);

//        if (Distance > unit.MaxRenderTile)
//            return;

//        AngleToObserver = MathUtils.NormalizeAngleDifference(unit.Angle, spriteAngle);

//        if (Math.Abs(AngleToObserver) <= unit.Fov)
//        {
//            AnimationManager.DefiningDesiredSprite(Animation, spriteAngle);

//            Distance *= Math.Cos(AngleToObserver);
//            Distance = Math.Max(Distance, 0.1);
//            float height = (float)(Screen.ScreenHeight / Distance * Scale);

//            RenderOperation2.DrawSprite(this, unit, height);
//        }

//    }
//}
//class RenderOperation2
//{
//    public static Vector2f GetPositionOnScreen(SpriteObstacle sprite, IUnit unit, float height)
//    {
//        float x = sprite.WorldToScreenX(sprite.AngleToObserver, unit.DeltaAngle);
//        float num = (float)(unit.Z.Axis / (sprite.Distance / (double)Screen.Setting.Tile));
//        float y = sprite.WorldToScreenY(unit.VerticalAngle) - (float)(sprite.Z.Axis / (sprite.Distance / (double)Screen.Setting.Tile)) + num;
//        return new Vector2f(x, y);
//    }

//    public static void DrawSprite(SpriteObstacle sprite, IUnit unit, float height)
//    {
//        if (sprite.Animation.CurrentFrame != null && sprite.Animation.CurrentFrame.Texture != null)
//        {
//            uint width = sprite.Animation.CurrentFrame.Width;
//            uint height2 = sprite.Animation.CurrentFrame.Height;
//            sprite.RenderSprite = new Sprite(sprite.Animation.CurrentFrame.Texture);
//            sprite.RenderSprite.Color = VisualEffectHelper.VisualEffect.TransformationColor(sprite.Distance);
//            sprite.RenderSprite.Origin = new Vector2f(width / 2, height2 / 2);
//            sprite.RenderSprite.Position = GetPositionOnScreen(sprite, unit, height);
//            sprite.RenderSprite.Scale = new Vector2f(height / (float)width, height / (float)height2);
//            ZBuffer.AddToZBuffer(sprite.RenderSprite, sprite.Distance);
//        }
//    }
//}