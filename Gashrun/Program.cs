using ScreenLib;
using ObstacleLib;
using ObstacleLib.TexturedWallLib;
using TextureLib;
using ControlLib;
using FpsLib;
using SFML.Graphics;
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
using UIFramework.Weapon.Bullets;
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
using ObjectFramework.Death;
using UIFramework.Text;
using ProtoRender.Map;
using BresenhamAlgorithm.Algorithm;
using System.Collections.Immutable;
using ObjectFramework.VisualImpact;
using ObjectFramework.VisualImpact.Data;
using ObjectFramework.HitAction;
using ObjectFramework.HitAction.DrawableBatch;
using ObjectFramework.Trigger;
using UIFramework.Sprite;
using UIFramework.Texture;

Screen.Initialize(1000, 600);

string mainFillWall32 = Path.Combine("Resources", "Image", "Sprite", "Devil");
//MoveLib.Setting.MoveSpeed = 200f;
//RayTracingLib.Raycast.ScanRadius
DateTime from = DateTime.Now;
string mainBold = Path.Combine("Resources", "FontText", "ArialBold.ttf");
FPS fpsChecker = new FPS("FPS: ", 24, new Vector2f(10, 10), ResourceManager.GetMainPath(mainBold), Color.White);
MoveLib.Move.Collision.RadiusCheckTouch = 2;
string mainFillWall = Path.Combine("Resources", "Image", "WallTexture", "Wall1.png");
string mainFillDevil = Path.Combine("Resources", "Image", "Sprite", "Devil");

var dddd = new SpriteObstacle(ImageLoader.TexturesLoad(ResourceManager.GetPath(Path.Combine("Resources", "Image", "WallTexture", "nek.png"))));
var ffffg = new SpriteObstacle(ImageLoader.TexturesLoadFromFolder(ResourceManager.GetPath(Path.Combine("Resources", "Image", "Sprite", "Flame")), false));
ffffg.Animation.Speed = 20;
ffffg.IsPassability = true;
ffffg.Scale = 64;
ffffg.Animation.IsAnimation = true;

//dddd.Animation.IsAnimation = true;
dddd.Animation.Speed = 20;
dddd.IsPassability = true;
dddd.Scale = 64;
CircleShape pp = new CircleShape(20)
{
    FillColor = Color.Red
};
var bath = new HitDrawableBatch(new List<Drawable>() { pp, new Sprite(ImageLoader.TexturesLoad(ResourceManager.GetPath(Path.Combine("Resources", "Image", "WallTexture", "nek.png"))).First().Texture) });
bath.Mode = HitDrawSelectMode.Random;

HitEffect hitEffectData = new HitEffect(new VisualImpactData(dddd, 1000), bath);
HitEffect hitEffectData1 = new HitEffect(new VisualImpactData(ffffg, 1000), null);
HitDataCache.Load(ResourceManager.GetMainPath(mainFillWall), hitEffectData);
HitDataCache.Load(ResourceManager.GetMainPath(mainFillDevil), hitEffectData1);

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
sprite1.HitBox.SegmentedHitbox.Add(box.MainHitBox);
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
unitBulletT.Animation.IsAnimation = true;
unitBulletT.Animation.Speed = 50;
string mainFillWall322 = Path.Combine("Resources", "Image", "Sprite", "GifSprite", "pokemon-8939_256.gif");
Console.WriteLine(ImageLoader.TextureLoad(ResourceManager.GetMainPath(mainFillWall322))?.Count);
AnimationState deathAnim = new AnimationState(ImageLoader.TextureLoad(ResourceManager.GetMainPath(mainFillWall322)) ?? new());
deathAnim.Speed = 30000;
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

map.AddObstacle(5, 2,sprite1);
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
Camera.CurrentUnit = player;
player.Scale = 64;
map.AddObstacle(3, 3, player);

player.Z.Axis = 0;
player.HitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(20);
player.HitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(20);
player.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(20);
player.HitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(20);
player.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(100);
player.HitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(100);
player.HitBox.MainHitBox.RenderColor = Color.Red;
player.DeathAction += Screen.Window.Close;
HitBox box2 = new HitBox();
box2.MainHitBox.RenderColor = Color.Green;
box2[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(50);
box2[CoordinatePlane.X, SideSize.Larger]?.SetOffset(50);
box2[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(50);
box2[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(50);
box2[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(50);
box2[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(50);
player.HitBox.AddSegmentHitBox(box2.MainHitBox.Body, "Center");

Unit player2 = new Unit(map, spriteObstacle1, 100);
map.AddObstacle(6, 6, player2);
player2.Z.Axis = 0;
player2.Scale = 80;
player2.Animation.IsAnimation = true;
player2.Animation.Speed = 15;
player2.MinDistanceFromWall = 0;
player2.HitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(100);
player2.HitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(100);
player2.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(100);
player2.HitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(100);
player2.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(100);
player2.HitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(100);
player2.HitBox.MainHitBox.RenderColor = Color.Red;
HitBox box1 = new HitBox();
box1.MainHitBox.RenderColor = Color.Green;
box1[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(50);
box1[CoordinatePlane.X, SideSize.Larger]?.SetOffset(50);
box1[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(50);
box1[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(50);
box1[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(50);
box1[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(50);
player2.HitBox.AddSegmentHitBox(box1.MainHitBox);
player2.IsPassability = true;
AnimationState animationState = new AnimationState(player2.Animation);
animationState.IsAnimation = true;
animationState.Speed = 10;
player2.DeathAnimation = new DeathEffect(animationState, DeathPhase.FrozenFinalFrame, 3000);

MiniMap miniMap = new MiniMap(5, PositionsMiniMap.UpperRightCorner, ResourceManager.GetPath(@"Resources\Image\BorderMiniMap\Border.png"));

RenderAlgorithm bresenhamAlgorithm = new();
bresenhamAlgorithm.UseVerticalPerspective = false;
bresenhamAlgorithm.UseHeightPerspective = false;

Control control = new Control();

Bottom bottomW = new Bottom(VirtualKey.W);
Bottom bottomS = new Bottom(VirtualKey.S);
Bottom bottomA = new Bottom(VirtualKey.A);
Bottom bottomD = new Bottom(VirtualKey.D);

Bottom bottomUpArrow = new Bottom(VirtualKey.UpArrow);
Bottom bottomDownArrow = new Bottom(VirtualKey.DownArrow);
Bottom bottomLeftArrow = new Bottom(VirtualKey.LeftArrow);
Bottom bottomRightArrow = new Bottom(VirtualKey.RightArrow);

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
crossSight.Owner = player;
//RoundSight roundSight = new RoundSight(Color.Red, 3);

ControlLib.BottomBinding keyBindingHideMap = new ControlLib.BottomBinding(controls, miniMap.Hide, 350);
ControlLib.BottomBinding keyBindingForward = new ControlLib.BottomBinding(bottomW, MovePositions.Move, new object[] { map, player, 1, 0 });
ControlLib.BottomBinding keyBindingBackward = new ControlLib.BottomBinding(bottomS, MovePositions.Move, new object[] { map, player, -1, 0 });
ControlLib.BottomBinding keyBindingLeft = new ControlLib.BottomBinding(bottomA, MovePositions.Move, new object[] { map, player, 0, -1 });
ControlLib.BottomBinding keyBindingRight = new ControlLib.BottomBinding(bottomD, MovePositions.Move, new object[] { map, player, 0, 1 });

ControlLib.BottomBinding keyBindingForward1 = new ControlLib.BottomBinding(bottomUpArrow, MoveLib.Move.MovePositions.Move, new object[] { map, player2, 1, 0 });
ControlLib.BottomBinding keyBindingBackward1 = new ControlLib.BottomBinding(bottomDownArrow, MoveLib.Move.MovePositions.Move, new object[] { map, player2, -1, 0 });
ControlLib.BottomBinding keyBindingLeft1 = new ControlLib.BottomBinding(bottomLeftArrow, MoveLib.Move.MovePositions.Move, new object[] { map, player2, 0, -1 });
ControlLib.BottomBinding keyBindingRight1 = new ControlLib.BottomBinding(bottomRightArrow, MoveLib.Move.MovePositions.Move, new object[] { map, player2, 0, 1 });

ControlLib.BottomBinding keyBindingHideCross = new ControlLib.BottomBinding(controlsHide, crossSight.Hide, 350);
control.AddBottomBind(new ControlLib.BottomBinding(new Bottom(VirtualKey.Q), Screen.Window.Close));
control.AddBottomBind(new ControlLib.BottomBinding(new Bottom(VirtualKey.None), MoveAngle.ResetAngle, new object[] { player }));
control.AddBottomBind(new ControlLib.BottomBinding(new Bottom(VirtualKey.None), MoveAngle.ResetAngle, new object[] { player2 }));


control.AddBottomBind(keyBindingForward);
control.AddBottomBind(keyBindingBackward);
control.AddBottomBind(keyBindingLeft);
control.AddBottomBind(keyBindingRight);
control.AddBottomBind(keyBindingHideMap);
control.AddBottomBind(keyBindingHideCross);
control.AddBottomBind(keyBindingForward1);
control.AddBottomBind(keyBindingBackward1);
control.AddBottomBind(keyBindingLeft1);
control.AddBottomBind(keyBindingRight1);

//BottomBinding shoot = new ControlLib.BottomBinding(new List<Bottom>(){ bottomLeftMouse }, Drawing.DrawingPoint, 350, new object[] { map, player, 30, Color.Red });
//control.AddBottomBind(shoot);
//player.OnControlAction += control.MakePressed;


string mainTextureSky = Path.Combine("Resources", "Image", "PartsWorldTexture", "SeamlessSky.jpg");

Sky sky = new Sky(ResourceManager.GetPath(mainTextureSky));
Floor floor = new Floor();
//TexturedFloor texturedFloor = new TexturedFloor(ResourceManager.GetPath(Path.Combine("Resources", "Image", "PartsWorldTexture", "Grass.jpg")), ResourceManager.GetPath(Path.Combine("Resources", "Shader", "FloorSetting.glsl")));
RenderPartsWorld renderPartsWorld = new RenderPartsWorld(sky, floor);
//230 170
Texture tex = new Texture(ResourceManager.GetPath(Path.Combine("Resources", "UI", "pistol.gif")));

void fffffff() { }
ControlLib.BottomBinding shoot = new ControlLib.BottomBinding(new Bottom(VirtualKey.None), fffffff);

UIAnimation uIElement = new UIAnimation(player, shoot, ResourceManager.GetPath(Path.Combine("Resources", "UI", "pistol.gif")))
{
    IsAnimation = true,
    SpeedAnimation = 50,
    PercentShiftX = 6f,
    PercentShiftY = -38,
    ScaleY = 1.3f,
    ScaleX = 1.25f
}; UIAnimation uIElement1 = new UIAnimation(player2, shoot, ResourceManager.GetPath(Path.Combine("Resources", "UI", "pistol.gif")))
{
    IsAnimation = true,
    SpeedAnimation = 50,
    PercentShiftX = 6f,
    PercentShiftY = -38,
    ScaleY = 1.3f,
    ScaleX = 1.25f
};
ControlLib.BottomBinding shootGun = new ControlLib.BottomBinding(new Bottom(VirtualKey.R), 350);
RenderText textMa = new RenderText("", 24, new Vector2f(Screen.ScreenWidth, Screen.ScreenHeight), ResourceManager.GetMainPath(mainBold), Color.Yellow);
UIText textMa1 = new UIText(textMa, player);
UIText textMa2 = new UIText(textMa, player2);
FadingText fadingText = new FadingText(textMa, FadingType.Appears, FadingTextLife.OneShotFreeze, 2000, null);
fadingText.Text.DisplayedString = "FFFFFFFFFF";
fadingText.PositionOnScreen = new Vector2f(250, 250);

var fff = new SpriteObstacle(ImageLoader.TexturesLoadFromFolder(ResourceManager.GetPath(Path.Combine("Resources", "Image", "Sprite", "Flame")), false));

fff.Animation.IsAnimation = true;
fff.Animation.Speed = 20;
fff.IsPassability = true;
fff.Scale = 64;
ControlLib.BottomBinding draw = new ControlLib.BottomBinding(new List<Bottom> { new Bottom(VirtualKey.None) }, Drawing.DrawingObject, 0);

StandartBullet bull = new StandartBullet(50, null);
UnitBullet unitBullet = new UnitBullet(50, unitBulletT, null);
unitBullet.Unit.Scale = 20;
Magazine magazine = new Magazine(100, 12, unitBullet, textMa1, control);
Magazine magazine2 = new Magazine(100, 12, bull, textMa2, control);


ControlLib.BottomBinding shoot3 = new ControlLib.BottomBinding(new List<Bottom> { new Bottom(VirtualKey.LeftButton) }, 150);
ControlLib.BottomBinding shoot4 = new ControlLib.BottomBinding(new List<Bottom> { new Bottom(VirtualKey.None) }, 2000);
//Gun gun1 = new Gun(player2, uIElement1, magazine2, shoot4);

Gun gun = new Gun(player, uIElement, magazine, shoot3);
List<Bottom> controls3 = new List<Bottom>() { new Bottom(VirtualKey.M), new Bottom(VirtualKey.LeftControl) };
ControlLib.BottomBinding bindGunHide = new ControlLib.BottomBinding(controls3, gun.Animation.Hide, 350);
control.AddBottomBind(bindGunHide);
//control.AddBottomBind(shoot);
control.AddBottomBind(shoot3);
control.AddBottomBind(shoot4);
control.AddBottomBind(new ControlLib.BottomBinding(new Bottom(VirtualKey.J), () => Camera.CurrentUnit = player2));
control.AddBottomBind(new ControlLib.BottomBinding(new Bottom(VirtualKey.K), () => Camera.CurrentUnit = player));


//AnimationContent a = new AnimationContent(ResourceManager.GetPath(Path.Combine("Resources", "UI", "small.gif")))
//{
//    IsAnimation = true,
//    SpeedAnimation = 8
//};
//new ColorContent(Color.Green)
FillBar bb = new FillBar(new ColorContent(Color.Red), new ColorContent(Color.Red), 80, 20, player)
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




VisualizerHitBox.VisualizerType = RenderLib.HitBox.VisualizerHitBoxType.VisualizeSelfRenderable;

var dd = new TriggerDistance(1,
    (owner) => { fadingText.Controller.FadingType = FadingType.Appears; fadingText.Controller.FadingTextLife = FadingTextLife.OneShotFreeze; fadingText.Owner = owner; },
    (owner) => { fadingText.SwapType(); fadingText.Controller.FadingTextLife = FadingTextLife.OneShotDispose; });
var bd = new TriggerButton(player, new BottomBinding(new Bottom(VirtualKey.E), () => { }));
TriggerAnd triggerAnd = new TriggerAnd(dd, bd);
triggerAnd.OnTriggered = (unit) =>
{
    var npc = dd.GetTarget();
    if (npc != null)
        map.DeleteObstacle(npc);
};
FadingSprite fadingSprite = new FadingSprite(ImageLoader.TexturesLoad(ResourceManager.GetPath(Path.Combine("Resources", "Image", "WallTexture", "nek.png"))).First().Texture, FadingType.Appears, FadingTextLife.OneShotFreeze, 2000, null);
fadingSprite.PositionOnScreen = new Vector2f(Screen.GetPercentWidth(50), Screen.GetPercentHeight(50));
fadingSprite.Sprite.Scale = new Vector2f(0.5f, 0.5f);

//TriggerHitBoxTouch triggerHitBoxTouch = new TriggerHitBoxTouch(player, player2,
//    (owner) => { fadingText.Text.DisplayedString = "WELCOM"; fadingText.Controller.FadingType = FadingType.Appears; fadingText.Restart(); fadingText.Controller.FadingTextLife = FadingTextLife.PingPongDispose; fadingText.Owner = owner; },
//    (owner) => { fadingText.Text.DisplayedString = "BYYEE"; fadingText.SwapType(); fadingText.Restart(); fadingText.Controller.FadingTextLife = FadingTextLife.OneShotDispose; fadingText.Owner = owner; });
TriggerHitBoxTouch triggerHitBoxTouch1 = new TriggerHitBoxTouch(player, player2,
    (owner) => { fadingSprite.Controller.FadingType = FadingType.Appears; fadingSprite.Restart(); fadingSprite.Controller.FadingTextLife = FadingTextLife.PingPongDispose; fadingSprite.Owner = owner; },
    null);
//(owner) => { fadingSprite.Controller.FadingType = FadingType.Appears; fadingSprite.Restart(); fadingSprite.Controller.FadingTextLife = FadingTextLife.PingPongDispose; fadingSprite.Owner = owner; }
//triggerHitBoxTouch1.OnTriggered += (owner) =>
//{
//    TriggerManager.RemoveTriger(owner, triggerHitBoxTouch1);
//};
//TriggerManager.AddTriger(player, triggerAnd);
TriggerManager.AddTriger(player, triggerAnd);

TriggerManager.AddTriger(player, triggerHitBoxTouch1);
_ = TriggerManager.CheckTriggersAsync();
//Screen.ScreenHeight = 1000;
//Screen.ScreenWidth = 1500;
try
{
    while (Screen.Window.IsOpen)
    {
       

        Screen.Window.DispatchEvents();
        Screen.Window.Clear();
        DrawingQueue.ExecuteAll();
        //unitBullet.CHECK();
        //if (Camera.CurrentUnit?.Map is not null)
            bresenhamAlgorithm.CalculationAlgorithm(Camera.CurrentUnit.Map, Camera.CurrentUnit);

        //bresenhamAlgorithm1.CalculationAlgorithm(true);
        //bresenhamAlgorithm2.CalculationAlgorithm(false);

        renderPartsWorld.Render(Camera.CurrentUnit);
        if (Camera.CurrentUnit?.Map is not null)
            miniMap.Render(Camera.CurrentUnit.Map, Camera.CurrentUnit);
       // miniMap1.Render(map);

        control.MakePressedParallel(Camera.CurrentUnit);
        UIRender.DrawingByPriority();
        if (Camera.CurrentUnit.Map is not null)
            VisualizerHitBox.Render(Camera.CurrentUnit.Map, Camera.CurrentUnit);
        //VisualizerHitBox2.Render(map, player);
        BeyondRenderManager.Render(Camera.CurrentUnit);
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