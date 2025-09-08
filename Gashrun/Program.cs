using Gashrun;
using ObjectFramework;
using ScreenLib;
using TextureLib.Loader;
using UIFramework;
using FpsLib;
using EffectLib.Effect;
using ObstacleLib.TexturedWallLib;
using SFML.System;
using UIFramework.Text;
using AnimationLib;
using UIFramework.IndicatorsBar.Content;
using UIFramework.IndicatorsBar;
using PartsWorldLib.Down;
using PartsWorldLib.Up;
using ControlLib.Buttons;
using TextureLib.Loader.ImageProcessing;
using TextureLib.Loader.LoaderMode;
using UIFramework.Animation;
using UIFramework.Render;
using UIFramework.Text.AlignEnums;
using UIFramework.Weapon.BulletMagazine;
using UIFramework.Weapon.Bullets.Variants;
using UIFramework.Weapon;
using InteractionFramework.Audio.SoundType;
using InteractionFramework.HitAction.DrawableBatch;
using InteractionFramework.HitAction;
using InteractionFramework.VisualImpact.Data;
using HitBoxLib.PositionObject;
using HitBoxLib.Segment.SignsTypeSide;
using UIFramework.Sprite;
using UIFramework.Windows.Button;
using SFML.Graphics;
using UIFramework.Dialog;
using InteractionFramework.Trigger.Button;
using UIFramework.Text.Fading;
using UIFramework.Text.Fading.FadingEnums;
using InteractionFramework.Trigger.TriggerTouch;
using InteractionFramework.Trigger;
using UIFramework.Shape;
using ProtoRender.Object;
using InteractionFramework.Trigger.Touch;
using InteractionFramework.VisualImpact;
using TextureLib.Textures.Pair;
using UIFramework.Texture;
using RenderLib.Algorithm;
using MiniMapLib;
using DataPipes.DTO.Converters;
using MiniMapLib.DTO;

Screen.Initialize(1000, 600);
#region Static Properties
FPS.BufferSize = 50;

RayTracingLib.Raycast.CoordinatesMoving = 2;
  
RenderAlgorithm.UseHeightPerspective = false;
RenderAlgorithm.UseVerticalPerspective = false;

MoveLib.Move.Collision.RadiusCheckTouch = 3;
#endregion

#region Effect

//EffectManager.CurrentEffect = new CustomEffect(DefaultPresets.BlinEffect);
//EffectManager.CurrentEffect.EffectEnd = 15;


CustomEffect effectBorderWall = new CustomEffect();
effectBorderWall.EffectColor = new SFML.Graphics.Glsl.Vec4(0, 0, 0, 0);
effectBorderWall.EffectEnd = 4;
#endregion

#region Font
string fontArialBold = PathResolver.GetMainPath(Path.Combine("Resources", "FontText", "ArialBold.ttf"));
#endregion

#region Shader
string pathCeilingShader = PathResolver.GetMainPath(Path.Combine("Resources", "Shader", "CeilingSettingWithoutBOM.glsl"));
#endregion

#region Base UIText
UIText ArialBold_Cyan_20 = new UIText("", 20, new Vector2f(0, 0), fontArialBold, SFML.Graphics.Color.Cyan);
#endregion

#region Paths Texture
string RedBarrierPng = PathResolver.GetMainPath(Path.Combine("Resources", "Image", "WallTexture", "Barrier.png"));
string BrickWindowPng = PathResolver.GetMainPath(Path.Combine("Resources", "Image", "WallTexture", "BrickWindow.png"));
string BrickWallPng = PathResolver.GetMainPath(Path.Combine("Resources", "Image", "WallTexture", "BrickWall.png"));
string BrickDoorPng = PathResolver.GetMainPath(Path.Combine("Resources", "Image", "WallTexture", "BrickDoor.png"));
string WoodCeiling = PathResolver.GetMainPath(Path.Combine("Resources", "Image", "PartsWorldTexture", "textu26.png"));

#endregion



#region Create Main Map
#region Map
TexturedWall boundaryWall = new TexturedWall(null, false, RedBarrierPng);
boundaryWall.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(100000);
boundaryWall.HitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(100000);
boundaryWall.Effect = effectBorderWall;

var map = new Map(boundaryWall, 100, 100);
#endregion

#region Create Structure

#region Mistic House

#region Create Wall House
TexturedWall MainMapMisticBrickWall = new TexturedWall(null, false, BrickWallPng);
map.AddObstacle(8, 1, MainMapMisticBrickWall.GetDeepCopy());
map.AddObstacle(8, 3, MainMapMisticBrickWall.GetDeepCopy());
map.AddObstacle(8, 5, MainMapMisticBrickWall.GetDeepCopy());
map.AddObstacle(8, 6, MainMapMisticBrickWall.GetDeepCopy());
map.AddObstacle(9, 6, MainMapMisticBrickWall.GetDeepCopy());
map.AddObstacle(13, 6, MainMapMisticBrickWall.GetDeepCopy());
map.AddObstacle(14, 6, MainMapMisticBrickWall.GetDeepCopy());
map.AddObstacle(14, 6, MainMapMisticBrickWall.GetDeepCopy());
map.AddObstacle(14, 5, MainMapMisticBrickWall.GetDeepCopy());
map.AddObstacle(14, 3, MainMapMisticBrickWall.GetDeepCopy());
map.AddObstacle(14, 1, MainMapMisticBrickWall.GetDeepCopy());

TexturedWall MainMapMisticBrickWindow = new TexturedWall(null, false, BrickWindowPng);
map.AddObstacle(8, 2, MainMapMisticBrickWindow.GetDeepCopy());
map.AddObstacle(8, 4, MainMapMisticBrickWindow.GetDeepCopy());
map.AddObstacle(10, 6, MainMapMisticBrickWindow.GetDeepCopy());
map.AddObstacle(12, 6, MainMapMisticBrickWindow.GetDeepCopy());
map.AddObstacle(14, 4, MainMapMisticBrickWindow.GetDeepCopy());
map.AddObstacle(14, 2, MainMapMisticBrickWindow.GetDeepCopy());

TexturedWall MainMapMisticBrickDoor = new TexturedWall(null, false, BrickDoorPng);
map.AddObstacle(11, 6, MainMapMisticBrickDoor);
#endregion
#endregion

#endregion

#endregion

#region Create Unit

#region Main Unit
Unit unit = new Unit(new ObstacleLib.SpriteLib.SpriteObstacle(PathResolver.GetMainPath(Path.Combine("Resources", "Image", "WallTexture", "Wall1.png"))), 100);
Camera.CurrentUnit = unit;
unit.HitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(50);
unit.HitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(50);
unit.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(50);
unit.HitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(50);
unit.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(100);
unit.HitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(100);

unit.ShiftCubedX = 50;
unit.ShiftCubedY = 50;

map?.AddObstacle(5, 5, unit);
#endregion

#endregion


#region Create Mistic House Map

Map? houseMap = null;
Vector2i positionMisticHouseBrickDoor = new Vector2i(0, 3);

Action CreateHouseMap = () =>
{
    houseMap = new Map(8, 7);

    TexturedWall MisticHouseBrickDoor = new TexturedWall(null, false, BrickDoorPng);
    houseMap.AddObstacle(positionMisticHouseBrickDoor.X, positionMisticHouseBrickDoor.Y, MisticHouseBrickDoor);

    TexturedWall MisticHouseBrickWall = new TexturedWall(null, false, BrickWallPng);
    houseMap.AddObstacle(0, 0, new TexturedWall(MisticHouseBrickWall, true));
    houseMap.AddObstacle(0, 1, new TexturedWall(MisticHouseBrickWall, true));
    houseMap.AddObstacle(0, 6, new TexturedWall(MisticHouseBrickWall, true));
    houseMap.AddObstacle(0, 5, new TexturedWall(MisticHouseBrickWall, true));
    houseMap.AddObstacle(1, 0, new TexturedWall(MisticHouseBrickWall, true));
    houseMap.AddObstacle(1, 6, new TexturedWall(MisticHouseBrickWall, true));
    houseMap.AddObstacle(3, 0, new TexturedWall(MisticHouseBrickWall, true));
    houseMap.AddObstacle(3, 6, new TexturedWall(MisticHouseBrickWall, true));
    houseMap.AddObstacle(4, 0, new TexturedWall(MisticHouseBrickWall, true));
    houseMap.AddObstacle(4, 6, new TexturedWall(MisticHouseBrickWall, true));
    houseMap.AddObstacle(6, 0, new TexturedWall(MisticHouseBrickWall, true));
    houseMap.AddObstacle(6, 6, new TexturedWall(MisticHouseBrickWall, true));
    houseMap.AddObstacle(7, 0, new TexturedWall(MisticHouseBrickWall, true));
    houseMap.AddObstacle(7, 6, new TexturedWall(MisticHouseBrickWall, true));
    houseMap.AddObstacle(7, 5, new TexturedWall(MisticHouseBrickWall, true));
    houseMap.AddObstacle(7, 1, new TexturedWall(MisticHouseBrickWall, true));
    houseMap.AddObstacle(7, 3, new TexturedWall(MisticHouseBrickWall, true));

    TexturedWall MisticHouseBrickWindow = new TexturedWall(null, false, BrickWindowPng);
    houseMap.AddObstacle(0, 4, new TexturedWall(MisticHouseBrickWindow, true));
    houseMap.AddObstacle(0, 2, new TexturedWall(MisticHouseBrickWindow, true));
    houseMap.AddObstacle(2, 0, new TexturedWall(MisticHouseBrickWindow, true));
    houseMap.AddObstacle(2, 6, new TexturedWall(MisticHouseBrickWindow, true));
    houseMap.AddObstacle(5, 0, new TexturedWall(MisticHouseBrickWindow, true));
    houseMap.AddObstacle(5, 6, new TexturedWall(MisticHouseBrickWindow, true));
    houseMap.AddObstacle(7, 4, new TexturedWall(MisticHouseBrickWindow, true));
    houseMap.AddObstacle(7, 2, new TexturedWall(MisticHouseBrickWindow, true));
};
#endregion


#region Create Triggers

#region Create Trigger Mistic House Door
FadingText fadingTextOpenDoor = new FadingText(ArialBold_Cyan_20, FadingType.Appears, FadingTextLife.OneShotFreeze, 2000, null);
fadingTextOpenDoor.RenderText.Text.DisplayedString = "Press E to open";
fadingTextOpenDoor.PositionOnScreen = new Vector2f(Screen.GetPercentWidth(65), Screen.GetPercentHeight(65));

var buttonOpenEntranceDoorTrigger = new TriggerButton(new ButtonBinding(new ControlLib.Buttons.Button(VirtualKey.E), () => { }, 1000), null, null);
var distanceOpenEntranceDoorDistancTrigger = new TriggerDistance((target) => target is not null && target == MainMapMisticBrickDoor,
    (owner) => { fadingTextOpenDoor.Controller.FadingType = FadingType.Appears; fadingTextOpenDoor.Controller.FadingTextLife = FadingTextLife.OneShotFreeze; fadingTextOpenDoor.Owner = owner; },
    (owner) => { fadingTextOpenDoor.SwapType(); fadingTextOpenDoor.Controller.FadingTextLife = FadingTextLife.OneShotDispose; });

var buttonOpenExitDoorTrigger = new TriggerButton(new ButtonBinding(new ControlLib.Buttons.Button(VirtualKey.E), () => { }, 1000), null, null);
var distanceOpenExitDoorTrigger = new TriggerDistance
    (
    (target) =>
    {
        if (houseMap != null && houseMap.Obstacles.TryGetValue(
            (positionMisticHouseBrickDoor.X * Screen.Setting.Tile,
            positionMisticHouseBrickDoor.Y * Screen.Setting.Tile),
         out var value))
        {
            return target is not null && target == value.First().Key;
        }

        return false;
    },
    (owner) => { fadingTextOpenDoor.Controller.FadingType = FadingType.Appears; fadingTextOpenDoor.Controller.FadingTextLife = FadingTextLife.OneShotFreeze; fadingTextOpenDoor.Owner = owner; },
    (owner) => { fadingTextOpenDoor.SwapType(); fadingTextOpenDoor.Controller.FadingTextLife = FadingTextLife.OneShotDispose; }
);


UIShape blackSreenOpenDoor = new UIShape(new RectangleShape(new Vector2f(Screen.ScreenWidth, Screen.ScreenHeight)));
blackSreenOpenDoor.RectangleShape.FillColor = Color.Black;
blackSreenOpenDoor.RenderOrder = RenderOrder.SystemNotification;

Vector2i safePositionOpenEntranceDoor = new Vector2i(MainMapMisticBrickDoor.CellX / Screen.Setting.Tile, MainMapMisticBrickDoor.CellY / Screen.Setting.Tile + 1);
Vector2i safePositionOpenExitDoor = new Vector2i(positionMisticHouseBrickDoor.X + 1, positionMisticHouseBrickDoor.Y);

TexturedCeiling? woodCeilingMisticHouse = null;
Action CreateWoodCeilingMisticHouse = () =>
{
    woodCeilingMisticHouse =  new TexturedCeiling(WoodCeiling, pathCeilingShader);
};

TriggerAnd openEntranceDoorTrigger = new TriggerAnd(buttonOpenEntranceDoorTrigger, distanceOpenEntranceDoorDistancTrigger);
openEntranceDoorTrigger.OnTriggered = (unit) =>
{
    var npc = distanceOpenEntranceDoorDistancTrigger.GetTarget();
    if (npc is null)
        return;

    if (blackSreenOpenDoor.Owner != unit)
        blackSreenOpenDoor.Owner = unit;
    blackSreenOpenDoor.IsHide = false;

    Camera.CurrentUnit.Map = null;
    if (houseMap is null)
        CreateHouseMap.Invoke();
    if (woodCeilingMisticHouse is null)
        CreateWoodCeilingMisticHouse.Invoke();

    houseMap?.AddObstacle(safePositionOpenExitDoor.X, safePositionOpenExitDoor.Y, Camera.CurrentUnit);

    GameManager.PartsWorld.UpPart = woodCeilingMisticHouse;
    blackSreenOpenDoor.IsHide = true;
};
TriggerHandler.AddTriger(unit, openEntranceDoorTrigger);

TriggerAnd openExitDoorTrigger = new TriggerAnd(buttonOpenExitDoorTrigger, distanceOpenExitDoorTrigger);
openExitDoorTrigger.OnTriggered = (unit) =>
{

    var npc = distanceOpenExitDoorTrigger.GetTarget();
    if (npc is null)
        return;

    if (blackSreenOpenDoor.Owner != unit)
        blackSreenOpenDoor.Owner = unit;
    blackSreenOpenDoor.IsHide = false;

    Camera.CurrentUnit.Map = null;
    map.AddObstacle(safePositionOpenEntranceDoor.X, safePositionOpenEntranceDoor.Y, Camera.CurrentUnit);


    blackSreenOpenDoor.IsHide = true;
};
TriggerHandler.AddTriger(unit, openExitDoorTrigger);

#endregion

#endregion





Unit devil = new Unit(new ObstacleLib.SpriteLib.SpriteObstacle(PathResolver.GetMainPath(Path.Combine("Resources", "Image", "Sprite", "Devil"))), 100);
devil.Scale = 100;
devil.HitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(150);
devil.HitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(150);
devil.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(150);
devil.HitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(150);
devil.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(800);
devil.HitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(400);
map?.AddObstacle(54, 54, devil);
devil.DialogSprite = new UISprite(PathResolver.GetMainPath(Path.Combine("Resources", "Image", "Sprite", "Devil", "3.png")));
devil.DialogSprite.Scale = new Vector2f(0.5f, 0.5f);
devil.DialogSprite.PositionOnScreen = new Vector2f(Screen.GetPercentWidth(10), Screen.GetPercentHeight(40));
devil.DialogSprite.VerticalAlignment = VerticalAlign.Center;
devil.DialogSprite.HorizontalAlignment = HorizontalAlign.Center;
devil.DialogSprite.RenderOrder = RenderOrder.DialogItem;
devil.DisplayName = new UIText("", 20, new Vector2f(0, 0), fontArialBold, SFML.Graphics.Color.Cyan);
devil.DisplayName.PositionOnScreen = new Vector2f(devil.DialogSprite.PositionOnScreen.X, devil.DialogSprite.PositionOnScreen.Y + devil.DialogSprite.Sprite.GetGlobalBounds().Height / 1.5f);
devil.DisplayName.SetText("Monster");
devil.DisplayName.VerticalAlignment = VerticalAlign.Center;
devil.DisplayName.HorizontalAlignment = HorizontalAlign.Center;
devil.DisplayName.RenderOrder = RenderOrder.DialogItem;
//devil.IsPassability = true;

#region HitData
#region Brick Wall
ObstacleLib.SpriteLib.SpriteObstacle spriteHitBrickWall = new(new AnimationState(new ImageLoadOptions() { FrameLoadMode = FrameLoadMode.FullFrame }, false, PathResolver.GetPath(Path.Combine("Resources", "effectHitToBrick.gif"))));
spriteHitBrickWall.Animation.IsAnimation = true;
spriteHitBrickWall.Animation.Speed = 30;
spriteHitBrickWall.IsPassability = true;
spriteHitBrickWall.Scale = 45;
VisualImpactData visualHitBrickWall = new VisualImpactData(spriteHitBrickWall, 500, false);

HitDrawableBatch drawHitBrickWall = new HitDrawableBatch(null, true, PathResolver.GetPath(Path.Combine("Resources", "Image", "BulletHole", "UniversalHole")));


SoundEmitter soundHitBrickWall = new SoundDynamic(PathResolver.GetPath(Path.Combine("Resources", "soundHitToBrick.wav")));
soundHitBrickWall.Sound.MinDistance = 300f;
soundHitBrickWall.Sound.Attenuation = 1f;

HitEffect hitBrickWall = new HitEffect(visualHitBrickWall, drawHitBrickWall, soundHitBrickWall);
HitEffect hitBrickWall1 = new HitEffect(null, drawHitBrickWall, null);

HitDataCache.Load(PathResolver.GetMainPath(Path.Combine("Resources", "Image", "WallTexture", "Wall1.png")), hitBrickWall);
#endregion
#region Border Wall
ObstacleLib.SpriteLib.SpriteObstacle spriteHitBorderWall = new(new AnimationState(new ImageLoadOptions() { FrameLoadMode = FrameLoadMode.FullFrame }, false, PathResolver.GetPath(Path.Combine("Resources", "effectHitToBorder.gif"))));
spriteHitBorderWall.Animation.IsAnimation = true;
spriteHitBorderWall.Animation.Speed = 20;
spriteHitBorderWall.IsPassability = true;
spriteHitBorderWall.Scale = 30;
ObstacleLib.SpriteLib.SpriteObstacle spriteHitBorderWall1 = new(new AnimationState(new ImageLoadOptions() { FrameLoadMode = FrameLoadMode.Accumulate }, false, PathResolver.GetPath(Path.Combine("Resources", "effectHitToBrick.gif"))));
spriteHitBorderWall1.Animation.IsAnimation = true;
spriteHitBorderWall1.Animation.Speed = 20;
spriteHitBorderWall1.IsPassability = true;
spriteHitBorderWall1.Scale = 30;
VisualImpactData visualHitBorderWall = new VisualImpactData(spriteHitBorderWall, 500, false);
VisualImpactData visualHitBorderWall1 = new VisualImpactData(spriteHitBorderWall1, 500, false);


SoundEmitter soundHitBorderWall = new SoundDynamic(PathResolver.GetPath(Path.Combine("Resources", "soundHitToBorder.wav")));
soundHitBorderWall.Sound.MinDistance = 300f;
soundHitBorderWall.Sound.Attenuation = 1f;
SoundEmitter soundHitBorderWall1 = new SoundDynamic(PathResolver.GetPath(Path.Combine("Resources", "soundHitToBrick.wav")));
soundHitBorderWall1.Sound.MinDistance = 300f;
soundHitBorderWall1.Sound.Attenuation = 1f;

HitEffect hitBorderWall = new HitEffect(visualHitBorderWall, null, soundHitBorderWall);
HitDataCache.Load(RedBarrierPng, hitBorderWall);


HitEffect hitBorderWall122 = new HitEffect(visualHitBorderWall1, null, soundHitBorderWall1);
HitDataCache.Load(BrickWindowPng, hitBorderWall, new EffectArea(new Vector2f(300, 500), new Vector2f(700, 1200)));
HitDataCache.Append(BrickWindowPng, hitBorderWall122);
#endregion

#endregion

#region UI
UIText FpsText = new UIText("", 20, new Vector2f(0, 0), fontArialBold, SFML.Graphics.Color.Cyan, Camera.CurrentUnit);
FpsText.IsHide = true;

AnimationState hpBarAnimation = new AnimationState(null, true, PathResolver.GetPath(Path.Combine("Resources", "UI", "small.gif")))
{
    IsAnimation = true,
    Speed = 30
};

FillBar bar = new FillBar(new AnimationContent(hpBarAnimation), new ColorContent(SFML.Graphics.Color.Red), unit.Hp, unit)
{
    BorderThickness = 10,
    Width = 400,
    Height = 100,
    PositionOnScreen = new Vector2f(0, Screen.ScreenHeight),
    BorderFillColor = SFML.Graphics.Color.Black,
};
#endregion

#region Part World
GameManager.PartsWorld.UpPart = new Sky(PathResolver.GetMainPath(Path.Combine("Resources", "Image", "PartsWorldTexture", "nightSky.jpg")));
var floor = new TexturedFloor(PathResolver.GetMainPath(Path.Combine("Resources", "Image", "PartsWorldTexture", "Grass1.png")),
                                            PathResolver.GetMainPath(Path.Combine("Resources", "Shader", "FloorSettingWithoutBOM.glsl")));
floor.DownAngleLogScale = 2.7f;
floor.Scale = 0.4f;
floor.TextureScrollingSpeed = 0.4f;
GameManager.PartsWorld.DownPart = floor;
#endregion

#region Gun
#region Pistol
ImageLoadOptions pistolImageLoadOptions = new ImageLoadOptions() { FrameLoadMode = FrameLoadMode.FullFrame };
UIAnimation pistolAnimation = new UIAnimation(pistolImageLoadOptions, true, PathResolver.GetMainPath(Path.Combine("Resources", "UI", "pistol.gif")))
{
    IsAnimation = true,
    Speed = 35,
    PercentShiftX = 6f,
    PercentShiftY = -38,
    ScaleY = 1.3f,
    ScaleX = 1.25f
};
pistolAnimation.RenderOrder = RenderOrder.Hands;

UIText pistolBulletText = new UIText("", 20, new Vector2f(0, 0), fontArialBold, SFML.Graphics.Color.Cyan, Camera.CurrentUnit);
pistolBulletText.PositionOnScreen = new Vector2f(Screen.GetPercentWidth(95), Screen.GetPercentHeight(98));
pistolBulletText.VerticalAlignment = VerticalAlign.Center;
pistolBulletText.HorizontalAlignment = HorizontalAlign.Center;
pistolBulletText.RenderOrder = RenderOrder.Hands;

//StandartBullet pistolBullet = new StandartBullet(20, null);
var b = new ObstacleLib.SpriteLib.SpriteObstacle(PathResolver.GetMainPath(Path.Combine("Resources", "Image", "Sprite", "Devil")));
var u = new Unit(b, 0, false);
u.Animation.IsAnimation = true;
u.Animation.Speed = 30;
u.Scale = 15;

UnitBullet pistolBullet = new UnitBullet(20, 15, u, null);
Magazine pistolMagazine = new Magazine(20, 140, pistolBullet, VirtualKey.R, pistolBulletText);

ControlLib.Buttons.ButtonBinding shootPistol = new ControlLib.Buttons.ButtonBinding(new ControlLib.Buttons.Button(VirtualKey.LeftButton), null, 300);
Gun pistol = new Gun(Camera.CurrentUnit, pistolAnimation, pistolMagazine, shootPistol);
#endregion
#endregion

#region Control
ControlLib.Buttons.Button W = new ControlLib.Buttons.Button(VirtualKey.W);
ControlLib.Buttons.Button S = new ControlLib.Buttons.Button(VirtualKey.S);
ControlLib.Buttons.Button A = new ControlLib.Buttons.Button(VirtualKey.A);
ControlLib.Buttons.Button D = new ControlLib.Buttons.Button(VirtualKey.D);
ControlLib.Buttons.Button Q = new ControlLib.Buttons.Button(VirtualKey.Q);
ControlLib.Buttons.Button F6 = new ControlLib.Buttons.Button(VirtualKey.F6);
ControlLib.Buttons.Button Space = new ControlLib.Buttons.Button(VirtualKey.Space);
ControlLib.Buttons.Button H = new ControlLib.Buttons.Button(VirtualKey.H);
ControlLib.Buttons.Button L = new ControlLib.Buttons.Button(VirtualKey.L);

ControlLib.Buttons.Button LeftArrow = new ControlLib.Buttons.Button(VirtualKey.LeftArrow);


ControlLib.Buttons.ButtonBinding forward = new ControlLib.Buttons.ButtonBinding(W, MoveLib.Move.MovePositions.Move, 0, new object[] { Camera.CurrentUnit, 1, 0 });
ControlLib.Buttons.ButtonBinding backward = new ControlLib.Buttons.ButtonBinding(S, MoveLib.Move.MovePositions.Move, 0, new object[] { Camera.CurrentUnit, -1, 0 });
ControlLib.Buttons.ButtonBinding left = new ControlLib.Buttons.ButtonBinding(A, MoveLib.Move.MovePositions.Move, 0, new object[] { Camera.CurrentUnit, 0, -1 });
ControlLib.Buttons.ButtonBinding right = new ControlLib.Buttons.ButtonBinding(D, MoveLib.Move.MovePositions.Move, 0, new object[] { Camera.CurrentUnit, 0, 1 });


//void Discard(Unit unit, float discardAngle)
//{
//    unit.JumpWithDiscard(jumpHeight: 1500, discardForce: 4000, unit.GetOppositeAngle(discardAngle));
//}
ControlLib.Buttons.ButtonBinding jumb = new ButtonBinding(Space, devil.Jump);
ControlLib.Buttons.ButtonBinding knock = new ButtonBinding(L, unit.Knockback, 300);

Camera.CurrentUnit?.Control.AddBottomBind(knock);

Camera.CurrentUnit?.Control.AddBottomBind(new ButtonBinding(new Button(VirtualKey.None), () => { FpsText.SetText($"Fps: {FPS.TextFPS}"); }));
Camera.CurrentUnit?.Control.AddBottomBind(new ButtonBinding(F6, () => { FpsText.IsHide = !FpsText.IsHide; }, 500));
Camera.CurrentUnit?.Control.AddBottomBind(new ButtonBinding(Q, Screen.Window.Close));
Camera.CurrentUnit?.Control.AddBottomBind(new ButtonBinding(new Button(VirtualKey.None), MoveLib.Angle.MoveAngle.ResetAngle, 0, new object[] { Camera.CurrentUnit }));
//Camera.CurrentUnit?.Control.AddBottomBind(new ButtonBinding(new() { H }, Discard, 200, new object[] { Camera.CurrentUnit }));
//Camera.CurrentUnit?.Control.AddBottomBind(new ButtonBinding(new Button(VirtualKey.None), unit.Update));
//Camera.CurrentUnit?.Control.AddBottomBind(new ButtonBinding(new Button(VirtualKey.None), devil.Update));
Camera.CurrentUnit?.Control.AddBottomBind(jumb);
Camera.CurrentUnit?.Control.AddBottomBind(forward);
Camera.CurrentUnit?.Control.AddBottomBind(backward);
Camera.CurrentUnit?.Control.AddBottomBind(left);
Camera.CurrentUnit?.Control.AddBottomBind(right);
#endregion

#region Dialog
UIButton buttonFirst = new UIButton(new Vector2f(Screen.GetPercentWidth(30), Screen.GetPercentHeight(50)), new Vector2f(Screen.GetPercentWidth(15), Screen.GetPercentHeight(6)), "Yes", PathResolver.GetPath(fontArialBold), SFML.Graphics.Color.White);
buttonFirst.RenderOrder = RenderOrder.DialogItem;
buttonFirst.Text.HorizontalAlignment = HorizontalAlign.Center;

UIButton buttonSecond = new UIButton(new Vector2f(Screen.GetPercentWidth(30), Screen.GetPercentHeight(58)), new Vector2f(Screen.GetPercentWidth(15), Screen.GetPercentHeight(4)), "No", PathResolver.GetPath(fontArialBold), SFML.Graphics.Color.White);
buttonSecond.RenderOrder = RenderOrder.DialogItem;
buttonSecond.Text.HorizontalAlignment = HorizontalAlign.Center;

RectangleShape ShapeBackround = new RectangleShape(new SFML.System.Vector2f(Screen.ScreenWidth, Screen.ScreenHeight))
{
    Position = new SFML.System.Vector2f(Screen.ScreenWidth / 2, Screen.ScreenHeight / 2),
    FillColor = new SFML.Graphics.Color(0, 0, 0, 100),
};
DialogueSession dialogManager = new DialogueSession(devil, unit, ShapeBackround);
dialogManager.BackgroundShape.RenderOrder = RenderOrder.DialogBackground;
dialogManager.BackgroundShape.HorizontalAlignment = HorizontalAlign.Center;
dialogManager.BackgroundShape.VerticalAlignment = VerticalAlign.Center;

UIText textDialogFirstStep = new UIText("Hello traveler, my name is Mitchell, I am a local Mon...Mythical creature, I serve one very powerful magician, he wants to see you. He is located a little further from this place, I will take you, but first we will go to the village, okay?", 20, new Vector2f(0, 0), fontArialBold, SFML.Graphics.Color.Cyan);
textDialogFirstStep.PositionOnScreen = new Vector2f(Screen.GetPercentWidth(50), Screen.GetPercentHeight(90));
textDialogFirstStep.VerticalAlignment = VerticalAlign.Center;
textDialogFirstStep.HorizontalAlignment = HorizontalAlign.Center;
textDialogFirstStep.RenderOrder = RenderOrder.DialogItem;
textDialogFirstStep.TextBounds = new FloatRect(0, 0, width: 500, height: 50);
textDialogFirstStep.TextResizeMode = TextResizeMode.AutoFit;


var stepFirst = new DialogueStep(textDialogFirstStep, new Dictionary<int, UIButton>() { { 1, buttonFirst }, { 2, buttonSecond } });


RectangleShape backgroundQuestText = new RectangleShape(new SFML.System.Vector2f(Screen.ScreenWidth, Screen.GetPercentHeight(20)))
{
    Position = new SFML.System.Vector2f(Screen.ScreenWidth / 2, Screen.GetPercentHeight(80)),
    FillColor = new SFML.Graphics.Color(0, 0, 0, 180),
};
UIShape uiBackgroundQuestText = new UIShape(backgroundQuestText);
uiBackgroundQuestText.RenderOrder = RenderOrder.DialogBackground;
uiBackgroundQuestText.HorizontalAlignment = HorizontalAlign.Center;

stepFirst.BackgroundQuestText = uiBackgroundQuestText;
dialogManager.AddLevel(0, new Dictionary<int, DialogueStep>()
{
    { 0, stepFirst }
});

#endregion


SoundEmitter soundTouchBorderWall = new SoundDynamic(PathResolver.GetPath(Path.Combine("Resources", "soundHitToBorder.wav")));
soundTouchBorderWall.Sound.MinDistance = 300f;
soundTouchBorderWall.Sound.Attenuation = 1f;


ObstacleLib.SpriteLib.SpriteObstacle spriteTouchBorderWall = new(new AnimationState(new ImageLoadOptions() { FrameLoadMode = FrameLoadMode.FullFrame }, false, PathResolver.GetPath(Path.Combine("Resources", "effectHitToBorder.gif"))));
spriteTouchBorderWall.Animation.IsAnimation = true;
spriteTouchBorderWall.Animation.Speed = 20;
spriteTouchBorderWall.IsPassability = true;
spriteTouchBorderWall.Scale = 40;
VisualImpactData visualTouchBorderWall = new VisualImpactData(spriteTouchBorderWall, 500, false);

Predicate<IObject> detectFun = (obj) =>
obj.GetUsedTexture(unit)?.PathTexture == PathResolver.GetMainPath(Path.Combine("Resources", "brie.png"));


FadingSprite fadingSprite = new FadingSprite(ImageLoader.Load(null, true, PathResolver.GetPath(Path.Combine("Resources", "Image", "WallTexture", "Wall3.png"))).First(), FadingType.Appears, FadingTextLife.OneShotFreeze, 2000, null);
fadingSprite.PositionOnScreen = new Vector2f(Screen.GetPercentWidth(50), Screen.GetPercentHeight(50));
fadingSprite.Sprite.Scale = new Vector2f(0.5f, 0.5f);
FadingSprite fadingSprite1 = new FadingSprite(ImageLoader.Load(null, true, PathResolver.GetPath(Path.Combine("Resources", "Image", "WallTexture", "Wall1.png"))).First(), FadingType.Appears, FadingTextLife.OneShotFreeze, 2000, null);
fadingSprite1.PositionOnScreen = new Vector2f(Screen.GetPercentWidth(50), Screen.GetPercentHeight(50));
fadingSprite1.Sprite.Scale = new Vector2f(0.5f, 0.5f);

TriggerCollision triggerHitBoxTouch1 = new TriggerCollision(devil, ObjectSide.Bottom, ObjectSide.Top,
    (owner) => { fadingSprite.Controller.FadingType = FadingType.Appears; fadingSprite.Restart(); fadingSprite.Controller.FadingTextLife = FadingTextLife.PingPongDispose; fadingSprite.Owner = owner; },
    (owner) => { fadingSprite1.Controller.FadingType = FadingType.Appears; fadingSprite1.Restart(); fadingSprite1.Controller.FadingTextLife = FadingTextLife.PingPongDispose; fadingSprite1.Owner = owner; });

//TriggerHandler.AddTriger(unit, triggerHitBoxTouch1);










TriggerTouch triggerCollision = new(detectFun, null, null);
triggerCollision.OnTriggered += (unit) =>
{
    if (triggerCollision.TriggerObject is null)
        return;

    var closest = new Vector2f(
    Math.Clamp((float)unit.X.Axis,
        (float)(triggerCollision.TriggerObject?.HitBox[CoordinatePlane.X, SideSize.Smaller]?.Side ?? 0f),
        (float)(triggerCollision.TriggerObject?.HitBox[CoordinatePlane.X, SideSize.Larger]?.Side ?? 0f)),
    Math.Clamp((float)unit.Y.Axis,
        (float)(triggerCollision.TriggerObject?.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.Side ?? 0f),
        (float)(triggerCollision.TriggerObject?.HitBox[CoordinatePlane.Y, SideSize.Larger]?.Side ?? 0f))
);

    float dx = (float)(triggerCollision.TriggerObject.X.Axis - unit.X.Axis);
    float dy = (float)(triggerCollision.TriggerObject.Y.Axis - unit.Y.Axis);
    float angleToTarget = MathF.Atan2(dy, dx);

    soundTouchBorderWall.Play(unit.Map, new Vector3f(
                closest.X,
                closest.Y,
                (float)unit.Z.Axis
           ));

    BeyondRenderManager.Create(unit, visualTouchBorderWall, closest.X, closest.Y, unit.Z.Axis);

    if (unit is Unit uu)
    {
        uu.Jump();
        uu.KnockbackAngle = angleToTarget + MathF.PI;//  angleToTarget + MathF.PI;
        uu.Knockback();
    }
};

//TriggerHandler.AddTriger(unit, triggerCollision);
//_ = GameInitializer.InitializeAsync(map);
GameManager.Start();









