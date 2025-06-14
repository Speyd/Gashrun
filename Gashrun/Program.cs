using ScreenLib;
using ObstacleLib;
using ObstacleLib.TexturedWallLib;
using TextureLib.Loader;
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
using UIFramework.Sights.Crosses;
using UIFramework.Render;
using UIFramework.IndicatorsBar;
using UIFramework.IndicatorsBar.Content;
using HitBoxLib.PositionObject;
using DrawLib;
using UIFramework.Animation;
using ProtoRender.WindowInterface;
using UIFramework.Weapon;
using UIFramework.Weapon.Bullets;
using HitBoxLib.HitBoxSegment;
using HitBoxLib.Segment.SignsTypeSide;
using ObstacleLib.SpriteLib;
using ProtoRender.RenderAlgorithm;
using ObjectFramework;
using AnimationLib;
using EffectLib;
using RenderLib.HitBox;
using HitBoxLib.Operations;
using EffectLib.Effect;
using UIFramework.Weapon.BulletMagazine;
using InteractionFramework.Death;
using UIFramework.Text;
using BresenhamAlgorithm.Algorithm;
using InteractionFramework.VisualImpact;
using InteractionFramework.VisualImpact.Data;
using InteractionFramework.HitAction;
using InteractionFramework.HitAction.DrawableBatch;
using InteractionFramework.Trigger;
using UIFramework.Texture;
using UIFramework.Overlay;
using UIFramework.Text.Fading;
using UIFramework.Text.Fading.FadingEnums;
using InteractionFramework.Trigger.TriggerTouch;
using InteractionFramework.Trigger.Button;
using UIFramework.Text.AlignEnums;
using TextureLib.Textures.Pair;
using SFML.Audio;
using InteractionFramework;
using ProtoRender.Object;
using InteractionFramework.Audio.SoundType;
using InteractionFramework.Dialog;
using UIFramework.Dialog;
using UIFramework.Windows;
using UIFramework;
using UIFramework.Sprite;
using ControlLib.Buttons;



Screen.Initialize(1000, 600);
RayTracingLib.Raycast.CoordinatesMoving = 2;

string mainFillWall32 = Path.Combine("Resources", "Image", "Sprite", "Devil");
//MoveLib.Setting.MoveSpeed = 200f;
//RayTracingLib.Raycast.ScanRadius
DateTime from = DateTime.Now;
string mainBold = Path.Combine("Resources", "FontText", "ArialBold.ttf");
FPS fpsChecker = new FPS("FPS: ", 24, new Vector2f(10, 10), PathResolver.GetMainPath(mainBold), SFML.Graphics.Color.White);
MoveLib.Move.Collision.RadiusCheckTouch = 2;
string mainFillWall = Path.Combine("Resources", "Image", "WallTexture", "Wall1.png");
string mainFillDevil = Path.Combine("Resources", "Image", "Sprite", "Devil");
Console.WriteLine("Start");
var dddd = new SpriteObstacle(ImageLoader.Load(PathResolver.GetPath(Path.Combine("Resources", "ggg.gif"))));
Console.WriteLine("Endddddd");
var ffffg = new SpriteObstacle(ImageLoader.Load(PathResolver.GetPath(Path.Combine("Resources", "Image", "Sprite", "Flame"))));
Console.WriteLine("Endddddddd");
ffffg.Animation.Speed = 20;
ffffg.IsPassability = true;
ffffg.Scale = 64;
ffffg.Animation.IsAnimation = true;

dddd.Animation.IsAnimation = true;
dddd.Animation.Speed = 20;
dddd.IsPassability = true;
dddd.Scale = 23;
CircleShape pp = new CircleShape(20)
{
    FillColor = SFML.Graphics.Color.Red
};
var bath = new HitDrawableBatch(ImageLoader.Load(PathResolver.GetPath(Path.Combine("Resources", "Image", "BulletHole", "UniversalHole"))));
bath.Mode = HitDrawSelectMode.Random;


var ffff = new TexturedWall(PathResolver.GetMainPath(mainFillWall));
ffff.Z.Axis = 0;

Map map = new(ffff, 10, 10);
SpriteObstacle sprite1 = new SpriteObstacle(PathResolver.GetMainPath(mainFillWall32))
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
sprite1.HitBox.MainHitBox.RenderColor = SFML.Graphics.Color.Green;
sprite1.HitBox.MainHitBox.HeightRenderMode = HitBoxLib.Operations.RenderHeightMode.EdgeBased;
HitBox box = new HitBox();
box[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(20);
box[CoordinatePlane.X, SideSize.Larger]?.SetOffset(20);
box[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(20);
box[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(20);
box[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(0);
box[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(50);
sprite1.HitBox.SegmentedHitbox.Add(box.MainHitBox);
SpriteObstacle sprite2 = new SpriteObstacle(PathResolver.GetMainPath(mainFillWall32))
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
sprite2.HitBox.MainHitBox.RenderColor = SFML.Graphics.Color.Red;
sprite2.Animation.IsAnimation = true;
Unit unitBulletT = new Unit(map, sprite2, 1);
unitBulletT.HitBox.MainHitBox.HeightRenderMode = RenderHeightMode.EdgeBased;
unitBulletT.IsRenderable = false;
unitBulletT.MinDistanceFromWall = 0;
unitBulletT.MoveSpeed = 20;
unitBulletT.Animation.IsAnimation = true;
unitBulletT.Animation.Speed = 50;
string mainFillWall322 = Path.Combine("Resources", "Image", "Sprite", "GifSprite", "pokemon-8939_256.gif");
AnimationState deathAnim = new AnimationState(ImageLoader.Load(PathResolver.GetMainPath(mainFillWall322)) ?? new());
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
sprite1.IgnoreCollisonMainBox = false;
map.AddObstacle(5, 2,sprite1);
//map.AddObstacle(2, 3, unit);

MultiWall multiWall = new MultiWall();
multiWall.Z.Axis = 0;
multiWall.AddLevelWall(new TexturedWall(PathResolver.GetMainPath(mainFillWall)));
multiWall.AddLevelWall(new TexturedWall(PathResolver.GetMainPath(mainFillWall)));
multiWall.AddLevelWall(new TexturedWall(PathResolver.GetMainPath(mainFillWall)));
multiWall.Z.Axis = 0;
//multiWall.Z.Axis = 300;
Console.WriteLine($"ZZZZ: {multiWall.Z.Axis}");
Console.WriteLine($"ZZZZ: {multiWall.Walls[1].Z.Axis}");


map.AddObstacle(7, 7, multiWall);
var walll = new TexturedWall(PathResolver.GetMainPath(mainFillWall), PathResolver.GetMainPath(Path.Combine("Resources", "Image", "WallTexture", "Wall4.png")));
map.AddObstacle(7, 4, walll);

//map.AddObstacle(4, 4, new TexturedWall(ResourceManager.GetMainPath(mainFillWall)));

//Player player = new Player(500, 500)
//{
//    MaxRenderTile = 1200,

//};
//ResourceManager.GetMainPath(mainFillWall32), true
SpriteObstacle spriteObstacle = new SpriteObstacle(PathResolver.GetMainPath(mainFillWall32))
{
    Scale = 64,
    //SideBT = 30,
    //SideLR = 30,
    //Z = 0,
};
SpriteObstacle spriteObstacle1 = new SpriteObstacle(PathResolver.GetMainPath(mainFillWall322))
{
    Scale = 84,
    //SideBT = 30,
    //SideLR = 30,
    //Z = 0,
};


Unit player = new Unit(map, new SpriteObstacle(PathResolver.GetMainPath(mainFillWall32)), 100);
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
player.HitBox.MainHitBox.RenderColor = SFML.Graphics.Color.Red;
player.Hp.OnDepleted += Screen.Window.Close;
HitBox box2 = new HitBox();
box2.MainHitBox.RenderColor = SFML.Graphics.Color.Green;
box2[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(50);
box2[CoordinatePlane.X, SideSize.Larger]?.SetOffset(50);
box2[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(50);
box2[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(50);
box2[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(50);
box2[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(50);
player.HitBox.AddSegmentHitBox(new Box(box2.MainHitBox.Body, "Center"));

SoundDynamic soundHit = new SoundDynamic(PathResolver.GetPath(Path.Combine("Resources", "080884bullet-hit-39872.mp3")));
soundHit.Sound.MinDistance = 300f;
soundHit.Sound.Attenuation = 1f;
soundHit.Sound.RelativeToListener = false;

HitEffect hitEffectData = new HitEffect(new VisualImpactData(dddd, 1000), bath, soundHit);
HitEffect hitEffectData1 = new HitEffect(new VisualImpactData(ffffg, 1000), null, null);
HitDataCache.Load(PathResolver.GetMainPath(mainFillWall), hitEffectData);
HitDataCache.Load(PathResolver.GetMainPath(mainFillDevil), hitEffectData1);


RenderText textMa = new RenderText("", 30, new Vector2f(Screen.ScreenWidth, Screen.ScreenHeight), PathResolver.GetMainPath(mainBold), SFML.Graphics.Color.Yellow);
Unit player2 = new Unit(map, new SpriteObstacle(PathResolver.GetMainPath(mainFillWall32)), 100);
map.AddObstacle(6, 6, player2);
player2.Z.Axis = 0;
player2.Scale = 80;
//player2.Animation.IsAnimation = true;
player2.Animation.Speed = 15;
player2.MinDistanceFromWall = 0;
player2.HitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(100);
player2.HitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(100);
player2.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(100);
player2.HitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(100);
player2.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(100);
player2.HitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(100);
player2.HitBox.MainHitBox.RenderColor = SFML.Graphics.Color.Red;
player2.DialogSprite = new UISprite(player2.Animation.GetFrame(4)?.Texture);
player2.DialogSprite.Scale = new Vector2f(0.5f, 0.5f);
player2.DialogSprite.PositionOnScreen = new Vector2f(100, Screen.Setting.HalfHeight);
player2.DialogSprite.VerticalAlignment = VerticalAlign.Center;
player2.DialogSprite.HorizontalAlignment = HorizontalAlign.Center;
player2.DialogSprite.RenderOrder = RenderOrder.Dialog;
player2.DisplayName = new UIText(textMa);
player2.DisplayName.PositionOnScreen = new Vector2f(100, Screen.Setting.HalfHeight + player2.DialogSprite.Sprite.GetGlobalBounds().Height / 2);
player2.DisplayName.SetText("Monster");
player2.DisplayName.VerticalAlignment = VerticalAlign.Center;
player2.DisplayName.HorizontalAlignment = HorizontalAlign.Center;
player2.DisplayName.RenderOrder = RenderOrder.Dialog;

//player2.DialogSprite = new UISprite(player2.Animation.GetFrame(0)?.Texture);

HitBox box1 = new HitBox();
box1.MainHitBox.RenderColor = SFML.Graphics.Color.Green;
box1[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(25);
box1[CoordinatePlane.X, SideSize.Larger]?.SetOffset(25);
box1[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(25);
box1[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(25);
box1[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(50);
box1[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(50);
player2.HitBox.AddSegmentHitBox(box1.MainHitBox);
player2.IsPassability = true;
player2.IgnoreCollisonMainBox = true;
player2.IsPassability = false;
AnimationState animationState = new AnimationState(player2.Animation);
animationState.IsAnimation = true;
animationState.Speed = 10;
player2.DeathAnimation = new DeathEffect(animationState, DeathPhase.FrozenFinalFrame, 3000);

MiniMap miniMap = new MiniMap(5, PositionsMiniMap.UpperRightCorner, PathResolver.GetPath(@"Resources\Image\BorderMiniMap\Border.png"));

RenderAlgorithm bresenhamAlgorithm = new();
bresenhamAlgorithm.UseVerticalPerspective = false;
bresenhamAlgorithm.UseHeightPerspective = false;

ControlLib.Buttons.Button bottomW = new ControlLib.Buttons.Button(VirtualKey.W);
ControlLib.Buttons.Button bottomS = new ControlLib.Buttons.Button(VirtualKey.S);
ControlLib.Buttons.Button bottomA = new ControlLib.Buttons.Button(VirtualKey.A);
ControlLib.Buttons.Button bottomD = new ControlLib.Buttons.Button(VirtualKey.D);

ControlLib.Buttons.Button bottomUpArrow = new ControlLib.Buttons.Button(VirtualKey.UpArrow);
ControlLib.Buttons.Button bottomDownArrow = new ControlLib.Buttons.Button(VirtualKey.DownArrow);
ControlLib.Buttons.Button bottomLeftArrow = new ControlLib.Buttons.Button(VirtualKey.LeftArrow);
ControlLib.Buttons.Button bottomRightArrow = new ControlLib.Buttons.Button(VirtualKey.RightArrow);

ControlLib.Buttons.Button bottomN = new ControlLib.Buttons.Button(VirtualKey.N);
ControlLib.Buttons.Button bottomLeftMouse = new ControlLib.Buttons.Button(VirtualKey.LeftButton);
ControlLib.Buttons.Button bottomC = new ControlLib.Buttons.Button(VirtualKey.C);
ControlLib.Buttons.Button bottomCtrl = new ControlLib.Buttons.Button(VirtualKey.LeftControl);
List<ControlLib.Buttons.Button> controls = new List<ControlLib.Buttons.Button>() { bottomC, bottomCtrl };
List<ControlLib.Buttons.Button> controlsHide = new List<ControlLib.Buttons.Button>() { bottomN, bottomCtrl };

CrossSight crossSight = new CrossSight(4, SFML.Graphics.Color.Red)
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




ControlLib.Buttons.ButtonBinding keyBindingHideMap = new ControlLib.Buttons.ButtonBinding(controls, miniMap.Hide, 350);
ControlLib.Buttons.ButtonBinding keyBindingForward = new ControlLib.Buttons.ButtonBinding(bottomW, MovePositions.Move, new object[] { player, 1, 0 });
ControlLib.Buttons.ButtonBinding keyBindingBackward = new ControlLib.Buttons.ButtonBinding(bottomS, MovePositions.Move, new object[] { player, -1, 0 });
ControlLib.Buttons.ButtonBinding keyBindingLeft = new ControlLib.Buttons.ButtonBinding(bottomA, MovePositions.Move, new object[] {  player, 0, -1 });
ControlLib.Buttons.ButtonBinding keyBindingRight = new ControlLib.Buttons.ButtonBinding(bottomD, MovePositions.Move, new object[] { player, 0, 1 });

ControlLib.Buttons.ButtonBinding keyBindingForward1 = new ControlLib.Buttons.ButtonBinding(bottomUpArrow, MoveLib.Move.MovePositions.Move, new object[] { player2, 1, 0 });
ControlLib.Buttons.ButtonBinding keyBindingBackward1 = new ControlLib.Buttons.ButtonBinding(bottomDownArrow, MoveLib.Move.MovePositions.Move, new object[] { player2, -1, 0 });
ControlLib.Buttons.ButtonBinding keyBindingLeft1 = new ControlLib.Buttons.ButtonBinding(bottomLeftArrow, MoveLib.Move.MovePositions.Move, new object[] { player2, 0, -1 });
ControlLib.Buttons.ButtonBinding keyBindingRight1 = new ControlLib.Buttons.ButtonBinding(bottomRightArrow, MoveLib.Move.MovePositions.Move, new object[] { player2, 0, 1 });

ControlLib.Buttons.ButtonBinding keyBindingHideCross = new ControlLib.Buttons.ButtonBinding(controlsHide, crossSight.ToggleVisibility, 350);
player.Control.AddBottomBind(new ControlLib.Buttons.ButtonBinding(new ControlLib.Buttons.Button(VirtualKey.Q), Screen.Window.Close));
player.Control.AddBottomBind(new ControlLib.Buttons.ButtonBinding(new ControlLib.Buttons.Button(VirtualKey.None), MoveAngle.ResetAngle, new object[] { player }));
player2.Control.AddBottomBind(new ControlLib.Buttons.ButtonBinding(new ControlLib.Buttons.Button(VirtualKey.None), MoveAngle.ResetAngle, new object[] { player2 }));


player.Control.AddBottomBind(keyBindingForward);
player.Control.AddBottomBind(keyBindingBackward);
player.Control.AddBottomBind(keyBindingLeft);
player.Control.AddBottomBind(keyBindingRight);
player.Control.AddBottomBind(keyBindingHideMap);
player.Control.AddBottomBind(keyBindingHideCross);
player2.Control.AddBottomBind(keyBindingForward1);
player2.Control.AddBottomBind(keyBindingBackward1);
player2.Control.AddBottomBind(keyBindingLeft1);
player2.Control.AddBottomBind(keyBindingRight1);

//BottomBinding shoot = new ControlLib.BottomBinding(new List<Bottom>(){ bottomLeftMouse }, Drawing.DrawingPoint, 350, new object[] { map, player, 30, Color.Red });
//control.AddBottomBind(shoot);
//player.OnControlAction += control.MakePressed;


string mainTextureSky = Path.Combine("Resources", "Image", "PartsWorldTexture", "SeamlessSky.jpg");

Sky sky = new Sky(PathResolver.GetPath(mainTextureSky));
Floor floor = new Floor();
TexturedFloor texturedFloor = new TexturedFloor(PathResolver.GetPath(Path.Combine("Resources", "Image", "PartsWorldTexture", "Grass.jpg")), PathResolver.GetPath(Path.Combine("Resources", "Shader", "FloorSetting.glsl")));
RenderPartsWorld renderPartsWorld = new RenderPartsWorld(sky, floor);
//230 170
Texture tex = new Texture(PathResolver.GetPath(Path.Combine("Resources", "UI", "pistol.gif")));

void fffffff() { }
ControlLib.Buttons.ButtonBinding shoot = new ControlLib.Buttons.ButtonBinding(new ControlLib.Buttons.Button(VirtualKey.None), fffffff);

UIAnimation uIElement = new UIAnimation(player, shoot, PathResolver.GetPath(Path.Combine("Resources", "UI", "pistol.gif")))
{
    IsAnimation = true,
    Speed = 50,
    PercentShiftX = 6f,
    PercentShiftY = -38,
    ScaleY = 1.3f,
    ScaleX = 1.25f
}; UIAnimation uIElement1 = new UIAnimation(player2, shoot, PathResolver.GetPath(Path.Combine("Resources", "UI", "pistol.gif")))
{
    IsAnimation = true,
    Speed = 50,
    PercentShiftX = 6f,
    PercentShiftY = -38,
    ScaleY = 1.3f,
    ScaleX = 1.25f
};
ControlLib.Buttons.ButtonBinding shootGun = new ControlLib.Buttons.ButtonBinding(new ControlLib.Buttons.Button(VirtualKey.R), 350);
UIText textMa1 = new UIText(textMa, player);
textMa1.HorizontalAlignment = HorizontalAlign.Right;
textMa1.VerticalAlignment = VerticalAlign.Bottom;


UIText textMa2 = new UIText(textMa, player2);
FadingText fadingText = new FadingText(textMa, FadingType.Appears, FadingTextLife.OneShotFreeze, 2000, null);
fadingText.RenderText.Text.DisplayedString = "FFFFFFFFFF";
fadingText.PositionOnScreen = new Vector2f(Screen.GetPercentWidth(75), Screen.GetPercentHeight(75));

var fff = new SpriteObstacle(ImageLoader.Load(PathResolver.GetPath(Path.Combine("Resources", "Image", "Sprite", "Flame"))));

fff.Animation.IsAnimation = true;
fff.Animation.Speed = 20;
fff.IsPassability = true;
fff.Scale = 64;
ControlLib.Buttons.ButtonBinding draw = new ControlLib.Buttons.ButtonBinding(new List<ControlLib.Buttons.Button> { new ControlLib.Buttons.Button(VirtualKey.None) }, Drawing.DrawingObject, 0);

StandartBullet bull = new StandartBullet(50, null);
UnitBullet unitBullet = new UnitBullet(50, unitBulletT, null);
unitBullet.Unit.Scale = 20;
Magazine magazine = new Magazine(100, 12, bull, VirtualKey.R, textMa1);
Magazine magazine2 = new Magazine(100, 12, bull, VirtualKey.R, textMa2);


ControlLib.Buttons.ButtonBinding shoot3 = new ControlLib.Buttons.ButtonBinding(new List<ControlLib.Buttons.Button> { new ControlLib.Buttons.Button(VirtualKey.LeftButton) }, 150);
ControlLib.Buttons.ButtonBinding shoot4 = new ControlLib.Buttons.ButtonBinding(new List<ControlLib.Buttons.Button> { new ControlLib.Buttons.Button(VirtualKey.None) }, 2000);
Gun gun1 = new Gun(player2, uIElement1, magazine2, shoot4);
SoundDynamic SoundEmitter = new SoundDynamic(PathResolver.GetPath(Path.Combine("Resources", "mixkit-game-gun-shot-1662.mp3")));
SoundEmitter.Sound.MinDistance = 300f;
SoundEmitter.Sound.Attenuation = 1f;
SoundEmitter.SubscribeMonoListener(player);

Gun gun = new Gun(player, uIElement, magazine, shoot3);
gun.Sound = SoundEmitter;
List<ControlLib.Buttons.Button> controls3 = new List<ControlLib.Buttons.Button>() { new ControlLib.Buttons.Button(VirtualKey.M), new ControlLib.Buttons.Button(VirtualKey.LeftControl) };
ControlLib.Buttons.ButtonBinding bindGunHide = new ControlLib.Buttons.ButtonBinding(controls3, () => { gun.Animation.IsHide = !gun.Animation.IsHide; }, 350);
player.Control.AddBottomBind(bindGunHide);
//control.AddBottomBind(shoot);
player.Control.AddBottomBind(shoot3);
player.Control.AddBottomBind(shoot4);
player.Control.AddBottomBind(new ControlLib.Buttons.ButtonBinding(new ControlLib.Buttons.Button(VirtualKey.J), () => Camera.CurrentUnit = player2));
player.Control.AddBottomBind(new ControlLib.Buttons.ButtonBinding(new ControlLib.Buttons.Button(VirtualKey.K), () => Camera.CurrentUnit = player));


//AnimationContent a = new AnimationContent(ResourceManager.GetPath(Path.Combine("Resources", "UI", "small.gif")))
//{
//    IsAnimation = true,
//    SpeedAnimation = 8
//};
//new ColorContent(Color.Green)
FillBar bb = new FillBar(new ColorContent(SFML.Graphics.Color.Green), new ColorContent(SFML.Graphics.Color.Red), player.Hp, player)
{
    BorderThickness = 10,
    Width = 400,
    Height = 100,
    PositionOnScreen = new Vector2f(0, Screen.ScreenHeight),
    BorderFillColor = SFML.Graphics.Color.Black,
    
};

List<ControlLib.Buttons.Button> controls4 = new List<ControlLib.Buttons.Button>() { new ControlLib.Buttons.Button(VirtualKey.L), new ControlLib.Buttons.Button(VirtualKey.LeftControl) };
ControlLib.Buttons.ButtonBinding bindBurHide = new ControlLib.Buttons.ButtonBinding(controls4, bb.ToggleVisibility, 350);
Camera.CurrentUnit.Control.AddBottomBind(bindBurHide);
//FillBar Bar = new FillBar(bb, new AnimationContent(ResourceManager.GetPath(Path.Combine("Resources", "UI", "pistol.gif"))), new ColorContent(Color.Red));
//Sight sight = new RoundSight(Color.Blue, 4);


UIFramework.Windows.Button button = new UIFramework.Windows.Button(new Vector2f(300, 300), new Vector2f(150, 25), "Нажми на меня", PathResolver.GetPath(mainBold));
UIFramework.Windows.Button button1 = new UIFramework.Windows.Button(new Vector2f(400, 400), new Vector2f(150, 25), "Нажми на меня", PathResolver.GetPath(mainBold));

UIFramework.Windows.Button button2 = new UIFramework.Windows.Button(new Vector2f(200, 200), new Vector2f(150, 25), "Нажми на меня", PathResolver.GetPath(mainBold));
UIFramework.Windows.Button button3 = new UIFramework.Windows.Button(new Vector2f(400, 200), new Vector2f(150, 25), "Нажми на меня", PathResolver.GetPath(mainBold));

UIFramework.Windows.Button button4 = new UIFramework.Windows.Button(new Vector2f(200, 300), new Vector2f(150, 25), "Нажми на меня", PathResolver.GetPath(mainBold));
UIFramework.Windows.Button button5 = new UIFramework.Windows.Button(new Vector2f(300, 100), new Vector2f(150, 25), "Нажми на меня", PathResolver.GetPath(mainBold));
var size = new SFML.System.Vector2f(Screen.ScreenWidth, Screen.ScreenHeight);

RectangleShape ShapeBackround1 = new RectangleShape(size)
{
    Position = new SFML.System.Vector2f(Screen.ScreenWidth / 2, Screen.ScreenHeight / 2),
    Origin = new SFML.System.Vector2f(size.X / 2f, size.Y / 2f),
    FillColor = new SFML.Graphics.Color(0, 0, 0, 100),
};
DialogManager dialogManager = new DialogManager(player2, player, ShapeBackround1);



dialogManager.AddLevelDialog(0, new Dictionary<int, DialogLvl>()
{
    { 0, new DialogLvl(textMa,new Dictionary<int, UIFramework.Windows.Button>() { { 1, button }, { 2, button1 } }) }
});
dialogManager.AddLevelDialog(1, new Dictionary<int, DialogLvl>()
{
    { 1, new DialogLvl(textMa, new Dictionary<int, UIFramework.Windows.Button>() { { 1, button2 }, { 2, button3 } }) },
    { 2, new DialogLvl(textMa,new Dictionary<int, UIFramework.Windows.Button>() { { 1, button2 }, { 2, button3 } }) }
});
dialogManager.AddLevelDialog(2, new Dictionary<int, DialogLvl>()
{
    { 1, new DialogLvl(textMa, new Dictionary<int, UIFramework.Windows.Button>() { { 1, button4 }, { 2, button5 } }) },
});
VisualizerHitBox.VisualizerType = RenderLib.HitBox.VisualizerHitBoxType.VisualizeSelfRenderable;

var dd = new TriggerDistance(1,
    (owner) => { fadingText.Controller.FadingType = FadingType.Appears; fadingText.Controller.FadingTextLife = FadingTextLife.OneShotFreeze; fadingText.Owner = owner; },
    (owner) => { fadingText.SwapType(); fadingText.Controller.FadingTextLife = FadingTextLife.OneShotDispose; }, typeof(IDialogObject));
var bd = new TriggerButton(new ButtonBinding(new ControlLib.Buttons.Button(VirtualKey.E), () => { }), null, null);
TriggerAnd triggerAnd = new TriggerAnd(dd, bd);

triggerAnd.OnTriggered = (unit) =>
{
    var npc = dd.GetTarget();
    if (npc != null && !(typeof(IDialogObject).IsAssignableFrom(npc?.GetType())))
        map.DeleteObstacle(npc);
    else if (npc != null && (typeof(IDialogObject).IsAssignableFrom(npc?.GetType())))
        _ = dialogManager.StartAsyncDialog();
};
TriggerHandler.AddTriger(player, triggerAnd);
FadingSprite fadingSprite = new FadingSprite(ImageLoader.Load(PathResolver.GetPath(Path.Combine("Resources", "Image", "WallTexture", "Wall3.png"))).First().Texture, FadingType.Appears, FadingTextLife.OneShotFreeze, 2000, null);
fadingSprite.PositionOnScreen = new Vector2f(Screen.GetPercentWidth(50), Screen.GetPercentHeight(50));
fadingSprite.Sprite.Scale = new Vector2f(0.2f, 0.2f);
FadingSprite fadingSprite1 = new FadingSprite(ImageLoader.Load(PathResolver.GetPath(Path.Combine("Resources", "Image", "WallTexture", "Wall1.png"))).First().Texture, FadingType.Appears, FadingTextLife.OneShotFreeze, 2000, null);
fadingSprite1.PositionOnScreen = new Vector2f(Screen.GetPercentWidth(50), Screen.GetPercentHeight(50));
fadingSprite1.Sprite.Scale = new Vector2f(0.2f, 0.2f);
//TriggerHitBoxTouch triggerHitBoxTouch = new TriggerHitBoxTouch(player, player2,
//    (owner) => { fadingText.Text.DisplayedString = "WELCOM"; fadingText.Controller.FadingType = FadingType.Appears; fadingText.Restart(); fadingText.Controller.FadingTextLife = FadingTextLife.PingPongDispose; fadingText.Owner = owner; },
//    (owner) => { fadingText.Text.DisplayedString = "BYYEE"; fadingText.SwapType(); fadingText.Restart(); fadingText.Controller.FadingTextLife = FadingTextLife.OneShotDispose; fadingText.Owner = owner; });
TriggerCollision triggerHitBoxTouch1 = new TriggerCollision(player2, ObjectSide.Bottom, ObjectSide.Top,
    (owner) => { fadingSprite.Controller.FadingType = FadingType.Appears; fadingSprite.Restart(); fadingSprite.Controller.FadingTextLife = FadingTextLife.PingPongDispose; fadingSprite.Owner = owner; },
    (owner) => { fadingSprite1.Controller.FadingType = FadingType.Appears; fadingSprite1.Restart(); fadingSprite1.Controller.FadingTextLife = FadingTextLife.PingPongDispose; fadingSprite1.Owner = owner; });
//(owner) => { fadingSprite.Controller.FadingType = FadingType.Appears; fadingSprite.Restart(); fadingSprite.Controller.FadingTextLife = FadingTextLife.PingPongDispose; fadingSprite.Owner = owner; }
//triggerHitBoxTouch1.OnTriggered += (owner) =>
//{
//    TriggerManager.RemoveTriger(owner, triggerHitBoxTouch1);
//};
//TriggerManager.AddTriger(player, triggerAnd);
//TriggerManager.AddTriger(player, triggerAnd);

TriggerHandler.AddTriger(player, triggerHitBoxTouch1);






TriggerRay triggerRay = new TriggerRay(null, null);
//TriggerAnd triggerAnd = new TriggerAnd(dd, bd);
UIText textMa1aa = new UIText(textMa, null);
textMa1aa.PositionOnScreen = new Vector2f(0, 0);
TriggerAnd triggerAndAA = new TriggerAnd(triggerRay);

triggerAndAA.OnTriggered = (unit) =>
{
    var npc = triggerRay.GetTarget();
    if (npc is not null)
    {
        if (unit != textMa1aa.Owner)
            textMa1aa.Owner = unit;
        else
            textMa1aa.IsHide = false;

        textMa1aa.SetText(DebugOverlay.GetInfoOverlay(npc));
    }
};
triggerAndAA.OnUntriggered = (unit) =>
{
    textMa1aa.IsHide = true;
};
//TriggerHandler.AddTriger(player, triggerAndAA);



UIText textMa1aad1 = new UIText(textMa, null);
textMa1aad1.PositionOnScreen = new Vector2f(1000, 0);
textMa1aad1.HorizontalAlignment = HorizontalAlign.Right;

TriggerToggleButton trrr = new TriggerToggleButton(new ButtonBinding(new ControlLib.Buttons.Button(VirtualKey.F2), () => { }),
    (unit) => { if (unit != textMa1aad1.Owner) textMa1aad1.Owner = unit;else textMa1aad1.IsHide = false; textMa1aad1.SetText(DebugOverlay.GetInfoOverlay(unit)); },
    (unit) => { textMa1aad1.IsHide = true; }
    );
TriggerHandler.AddTriger(player, trrr);
//_ = TriggerHandler.CheckTriggersAsync();


SoundStatic soundMove = new SoundStatic(PathResolver.GetPath(Path.Combine("Resources", "move.wav")));
soundMove.Sound.MinDistance = 300f;
soundMove.Sound.Attenuation = 1f;
soundMove.Sound.RelativeToListener = true;
soundMove.Sound.Position = new Vector3f(0,0,0);


TriggerButton moveBB = new TriggerButton(new ButtonBinding(new ControlLib.Buttons.Button(VirtualKey.W), () => { }),
    (unit) => { if (soundMove.Sound.Status == SoundStatus.Stopped) soundMove.Sound.Play();  },
    (unit) => { soundMove.Sound.Stop(); }
    );
TriggerHandler.AddTriger(player, moveBB);
float NormalizeAngle(float angle)
{
    while (angle > Math.PI)
        angle -= 2 * (float)Math.PI;
    while (angle < -Math.PI)
        angle += 2 * (float)Math.PI;
    return angle;
}

VisualEffectHelper.VisualEffect = new Darkness(PathResolver.GetPath(Path.Combine("Resources", "Shader", "Effect", "DarknessEffect.glsl")));
VisualizerHitBox.VisualizerType = VisualizerHitBoxType.VisualizeSelfRenderable;

//Screen.ScreenHeight = 1000;
//Screen.ScreenWidth = 1500;
//player2.Control.FreezeControlsMouse = true;
try
{
    while (Screen.Window.IsOpen)
    {
        Camera.UpdateListener();

        Screen.Window.DispatchEvents();
        Screen.Window.Clear();
       // Screen.OutputPriority?.AddToPriority(ScreenLib.Output.OutputPriorityType.Interface, dialogManager.ShapeBackround);
        DrawingQueue.ExecuteAll();
        WriteQueue.ExecuteAll();

        //unitBullet.CHECK();
        bresenhamAlgorithm.CalculationAlgorithm(Camera.CurrentUnit);

       // //bresenhamAlgorithm1.CalculationAlgorithm(true);
       // //bresenhamAlgorithm2.CalculationAlgorithm(false);

        renderPartsWorld.Render(Camera.CurrentUnit);
        if (Camera.CurrentUnit?.Map is not null)
            miniMap.Render(Camera.CurrentUnit.Map, Camera.CurrentUnit);
        //// miniMap1.Render(map);

       // player2.Control.MakePressedParallel(Camera.CurrentUnit);
        Camera.CurrentUnit?.Control.MakePressedParallel(Camera.CurrentUnit);
        UIRender.DrawingByPriority();
        VisualizerHitBox.Render(Camera.CurrentUnit);
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