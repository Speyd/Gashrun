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

Screen.Initialize(1000, 600);
string mainFillWall32 = Path.Combine("Resources", "Image", "Sprite", "Devil");
MoveLib.Setting.MoveSpeed = 200f;
DateTime from = DateTime.Now;
string mainBold = Path.Combine("Resources", "FontText", "ArialBold.ttf");
FPS fpsChecker = new FPS("FPS: ", 24, new Vector2f(10, 10), ResourceManager.GetMainPath(mainBold), Color.White);
MoveLib.Move.Collision.RadiusCheckTouch = 2;
string mainFillWall = Path.Combine("Resources", "Image", "WallTexture", "Wall1.png");
Map map = new(new TexturedWall(ResourceManager.GetMainPath(mainFillWall)), 10, 10);
SpriteObstacle sprite1 = new SpriteObstacle(ResourceManager.GetMainPath(mainFillWall32), true)
{
    Scale = 64,
    //SideBT = 30,
    //SideLR = 30,
    //Z = 0,
    ShiftCubedX = 50,
    ShiftCubedY = 50,
};
sprite1.Z.Axis = 60;
sprite1.HitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(80);
sprite1.HitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(80);
sprite1.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(80);
sprite1.HitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(80);
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

map.AddObstacle(2, 2,sprite1);
map.AddObstacle(2, 3, sprite2);

//MultiWall multiWall = new MultiWall();
//multiWall.AddLevelWall(new TexturedWall(ResourceManager.GetMainPath(mainFillWall)));
//multiWall.AddLevelWall(new TexturedWall(ResourceManager.GetMainPath(mainFillWall)));
//map.AddObstacle(7, 7, multiWall);
map.AddObstacle(4, 4, new TexturedWall(ResourceManager.GetMainPath(mainFillWall)));

Player player = new Player(500, 500)
{
    MaxRenderTile = 1200,
};

MiniMap miniMap = new MiniMap(player, 5, PositionsMiniMap.UpperRightCorner, ResourceManager.GetPath(@"Resources\Image\BorderMiniMap\Border.png"));
BresenhamAlgorithm.Algorithm.BresenhamAlgorithm bresenhamAlgorithm = new(map, player);
Control control = new Control();
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
    GeneralDegreeObject = 0,
    StartDegree = 0,
    GeneralDegreePosition = 90,
    InvertCrossParts = true,
};
ControlLib.BottomBinding keyBindingHideMap = new ControlLib.BottomBinding(controls, miniMap.Hide, 350);
ControlLib.BottomBinding keyBindingForward = new ControlLib.BottomBinding(bottomW, MovePositions.Move, new object[] { map, player, 1, 0 });
ControlLib.BottomBinding keyBindingBackward = new ControlLib.BottomBinding(bottomS, MovePositions.Move, new object[] { map, player, -1, 0 });
ControlLib.BottomBinding keyBindingLeft = new ControlLib.BottomBinding(bottomA, MovePositions.Move, new object[] { map, player, 0, -1 });
ControlLib.BottomBinding keyBindingRight = new ControlLib.BottomBinding(bottomD, MovePositions.Move, new object[] { map, player, 0, 1 });
ControlLib.BottomBinding keyBindingHideCross = new ControlLib.BottomBinding(controlsHide, crossSight.Hide, 350);
control.AddBottomBind(new BottomBinding(new Bottom(VirtualKey.Q), Screen.Window.Close));
control.AddBottomBind(new ControlLib.BottomBinding(new Bottom(VirtualKey.None), MoveAngle.ResetAngle, new object[] { player }));
control.AddBottomBind(keyBindingForward);
control.AddBottomBind(keyBindingBackward);
control.AddBottomBind(keyBindingLeft);
control.AddBottomBind(keyBindingRight);
control.AddBottomBind(keyBindingHideMap);
control.AddBottomBind(keyBindingHideCross);


//BottomBinding shoot = new ControlLib.BottomBinding(new List<Bottom>(){ bottomLeftMouse }, Drawing.DrawingPoint, 350, new object[] { map, player, 30, Color.Red });
//control.AddBottomBind(shoot);
player.OnControlAction += control.MakePressed;


string mainTextureSky = Path.Combine("Resources", "Image", "PartsWorldTexture", "SeamlessSky.jpg");

Sky sky = new Sky(ResourceManager.GetPath(mainTextureSky));
Floor floor = new Floor();
RenderPartsWorld renderPartsWorld = new RenderPartsWorld(sky, floor);
//230 170
Texture tex = new Texture(ResourceManager.GetPath(Path.Combine("Resources", "UI", "pistol.gif")));

BottomBinding shoot = new ControlLib.BottomBinding(new List<Bottom> { new Bottom(VirtualKey.None) }, Drawing.DrawingPoint, 0, new object[] { map, player, 30, Color.Red });

UIAnimation uIElement = new UIAnimation(shoot, ResourceManager.GetPath(Path.Combine("Resources", "UI", "pistol.gif")))
{
    IsAnimation = true,
    SpeedAnimation = 5,
    PercentShiftX = 6f,
    PercentShiftY = -38,
    ScaleY = 1.3f,
    ScaleX = 1.25f
};
BottomBinding shootGun = new ControlLib.BottomBinding(new Bottom(VirtualKey.R), 350);
RenderText textMa = new RenderText("", 24, new Vector2f(Screen.ScreenWidth, Screen.ScreenHeight), ResourceManager.GetMainPath(mainBold), Color.Yellow);

var fff = new SpriteObstacle(ImageLoader.TexturesLoadFromFolder(@"D:\C++ проекты\Gashrun\Resources\Image\Sprite\Flame", false));
fff.Animation.IsAnimation = true;
fff.Animation.Speed = 20;
fff.IsPassability = true;
fff.Scale = 64;
HitEffect hitEffect = new HitEffect(fff, new TimeSpan(5000000));
BottomBinding draw = new ControlLib.BottomBinding(new List<Bottom> { new Bottom(VirtualKey.None) }, Drawing.DrawingObjectPoint, 0, new object[] { player, 30, Color.Red });

StandartBullet bull = new StandartBullet(map, draw, null, hitEffect);
Magazine magazine = new Magazine(control, shootGun, textMa, bull, 100, 12);

BottomBinding shoot3 = new ControlLib.BottomBinding(new List<Bottom> { new Bottom(VirtualKey.LeftButton) }, 150);
Gun gun = new Gun(player, uIElement, magazine, shoot3);
List<Bottom> controls3 = new List<Bottom>() { new Bottom(VirtualKey.M), new Bottom(VirtualKey.LeftControl) };
BottomBinding bindGunHide = new BottomBinding(controls3, gun.Animation.Hide, 350);
control.AddBottomBind(bindGunHide);
//control.AddBottomBind(shoot);
control.AddBottomBind(shoot3);


AnimationContent a = new AnimationContent(ResourceManager.GetPath(Path.Combine("Resources", "UI", "small.gif")))
{
    IsAnimation = true,
    SpeedAnimation = 8
};
//new ColorContent(Color.Green)
FillBar bb = new FillBar(a, new ColorContent(Color.Red), 80, 20)
{
    BorderThickness = 10,
    Width = 400,
    Height = 100,
    PositionOnScreen = new Vector2f(0, Screen.ScreenHeight),
    BorderFillColor = Color.Black,
    
};
List<Bottom> controls4 = new List<Bottom>() { new Bottom(VirtualKey.L), new Bottom(VirtualKey.LeftControl) };
BottomBinding bindBurHide = new BottomBinding(controls4, bb.Hide, 350);
control.AddBottomBind(bindBurHide);
//FillBar Bar = new FillBar(bb, new AnimationContent(ResourceManager.GetPath(Path.Combine("Resources", "UI", "pistol.gif"))), new ColorContent(Color.Red));
//Sight sight = new RoundSight(Color.Blue, 4);
//Screen.ScreenHeight = 1000;
//Screen.ScreenWidth = 1500;
RenderLib.HitBox.VisualizerHitBox.VisualizerType = RenderLib.HitBox.VisualizerHitBoxType.VisualizeSelfRenderable;

try
{
    while (Screen.Window.IsOpen)
    {

        Screen.Window.DispatchEvents();
        Screen.Window.Clear();

        bresenhamAlgorithm.CalculationAlgorithm();
        renderPartsWorld.Render(player);
        miniMap.Render(map);

        player.MakePressed();
        UIRender.DrawingByPriority();
        RenderLib.HitBox.VisualizerHitBox.Render(map, player);
        HitEffect.Render();
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
}