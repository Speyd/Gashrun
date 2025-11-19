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
using EffectLib.EffectCore;
using TextureLib.Textures;
using ObstacleLib.SpriteLib;
using SFML.Graphics.Glsl;
using InteractionFramework.Audio;
using UIFramework.Sights;
using SFML.Window;
using BehaviorPatternsFramework.Enum;
using BehaviorPatternsFramework.Behavior;
using BehaviorPatternsFramework.Move;
using BehaviorPatternsFramework.PatternAttack;
using BehaviorPatternsFramework.PatternMove;
using BehaviorPatternsFramework.PatternDetection;
using BehaviorPatternsFramework.PatternObservations;
using UIFramework.Weapon.Bullets;
using BehaviorPatternsFramework;
using BehaviorPatternsFramework.PatternMove.Dodge;
using BehaviorPatternsFramework.PatternAttack.Strategy;
using MoveLib;
using MoveLib.Angle;
using MoveLib.Move;
using AnimationLib.Core.Elements;
using AnimationLib.Core;
using AnimationLib.Selector;
using AnimationLib.Enum;
using AnimationLib.Core.Utils;
using System.Diagnostics;
using InteractionFramework.Death;


Screen.Initialize(1000, 600);

#region Static Properties
FPS.BufferSize = 50;

RayTracingLib.Raycast.CoordinatesMoving = 3;
Console.WriteLine(RayTracingLib.Raycast.CoordinatesMoving);
SpriteObstacle.GlobalMinDistance = 0;
RenderAlgorithm.UseHeightPerspective = false;
RenderAlgorithm.UseVerticalPerspective = false;
PhysicsHandler.delayMs = 2;

MoveLib.Move.Collision.RadiusCheckTouch = 3;
PathResolver.SearchPattern = "Resources";
#endregion


#region Buttons
ControlLib.Buttons.Button W = new ControlLib.Buttons.Button(VirtualKey.W);
ControlLib.Buttons.Button S = new ControlLib.Buttons.Button(VirtualKey.S);
ControlLib.Buttons.Button A = new ControlLib.Buttons.Button(VirtualKey.A);
ControlLib.Buttons.Button R = new ControlLib.Buttons.Button(VirtualKey.R);
ControlLib.Buttons.Button D = new ControlLib.Buttons.Button(VirtualKey.D);
ControlLib.Buttons.Button Q = new ControlLib.Buttons.Button(VirtualKey.Q);
ControlLib.Buttons.Button F6 = new ControlLib.Buttons.Button(VirtualKey.F6);
ControlLib.Buttons.Button Space = new ControlLib.Buttons.Button(VirtualKey.Space);
ControlLib.Buttons.Button H = new ControlLib.Buttons.Button(VirtualKey.H);
ControlLib.Buttons.Button L = new ControlLib.Buttons.Button(VirtualKey.L);

ControlLib.Buttons.Button LeftArrow = new ControlLib.Buttons.Button(VirtualKey.LeftArrow);

ControlLib.Buttons.Button LeftButton = new ControlLib.Buttons.Button(VirtualKey.LeftButton);
ControlLib.Buttons.Button RightButton = new ControlLib.Buttons.Button(VirtualKey.RightButton);

#endregion

#region Effect

EffectManager.CurrentEffect = new CustomEffect(DefaultPresets.DarkEffect);
EffectManager.CurrentEffect.EffectEnd = 5;


CustomEffect effectMainMap = new CustomEffect(DefaultPresets.DarkEffect);
effectMainMap.EffectColor = new Vec4(0f, 0f, 0f, 1f);
effectMainMap.EffectEnd = 5;
effectMainMap.EffectStrength = 1.2f;

CustomEffect effectBorderWall = new CustomEffect();
effectBorderWall.EffectColor = new SFML.Graphics.Glsl.Vec4(0, 0, 0, 0);
effectBorderWall.EffectEnd = 4;
#endregion

#region Paths

#region Font
string fontArialBold = PathResolver.GetPath(Path.Combine("Resources", "FontText", "ArialBold.ttf"));
#endregion

#region Texture Object
string RedBarrierPng = PathResolver.GetPath(Path.Combine("Resources", "Image", "WallTexture", "Barrier.png"));
string BrickWindowPng = PathResolver.GetPath(Path.Combine("Resources", "Image", "WallTexture", "BrickWindow.png"));
string BrickWallPng = PathResolver.GetPath(Path.Combine("Resources", "Image", "WallTexture", "BrickWall.png"));
string BrickBloodWallPng = PathResolver.GetPath(Path.Combine("Resources", "Image", "WallTexture", "BrickBloodWall.png"));
string BrickDoorPng = PathResolver.GetPath(Path.Combine("Resources", "Image", "WallTexture", "BrickDoor.png"));
string BigLampePng = PathResolver.GetPath(Path.Combine("Resources", "Image", "Sprite", "BigLampe"));
string HumanPng = PathResolver.GetPath(Path.Combine("Resources", "Image", "Sprite", "Human"));
string RabbitGif = PathResolver.GetPath(Path.Combine("Resources", "Image", "Sprite", "Rabbit"));
string DevilPng = PathResolver.GetPath(Path.Combine("Resources", "Image", "Sprite", "Devil"));
string RotatingNewspaperGif = PathResolver.GetPath(Path.Combine("Resources", "Image", "Sprite", "RotatingNewspaper"));
#endregion

#region UI
string LoadingPng = PathResolver.GetPath(Path.Combine("Resources", "UI", "Loading.png"));
string PistolGif = PathResolver.GetPath(Path.Combine("Resources", "UI", "pistol.gif"));
#endregion

#region Visual Effect
string VisualEffectBulletGlassGif = PathResolver.GetPath(Path.Combine("Resources", "VisualEffect", "visualEffectBulletGlass.gif"));
string VisualEffectBulletBrickGif = PathResolver.GetPath(Path.Combine("Resources", "VisualEffect", "visualEffectBulletBrick.gif"));
string VisualEffectBulletBorderGif = PathResolver.GetPath(Path.Combine("Resources", "VisualEffect", "visualEffectBulletBorder.gif"));
#endregion

#region Parts World
string WoodFloorPng = PathResolver.GetPath(Path.Combine("Resources", "Image", "PartsWorldTexture", "WoodFloor.png"));
string NightSkJpg = PathResolver.GetPath(Path.Combine("Resources", "Image", "PartsWorldTexture", "NightSky.jpg"));
string WoodCeilingPng = PathResolver.GetPath(Path.Combine("Resources", "Image", "PartsWorldTexture", "WoodCeiling.png"));
string GrassFloorPng = PathResolver.GetPath(Path.Combine("Resources", "Image", "PartsWorldTexture", "Grass.png"));
#endregion

#region Sound
string SoundHitGlassWav = PathResolver.GetPath(Path.Combine("Resources", "Sound", "HitGlass.wav"));
string SoundHitWoodWav = PathResolver.GetPath(Path.Combine("Resources", "Sound", "HitWood.wav"));
string SoundHitBrickWav = PathResolver.GetPath(Path.Combine("Resources", "Sound", "HitBrick.wav"));
string SoundBulletFlyWav = PathResolver.GetPath(Path.Combine("Resources", "Sound", "BulletFly.wav"));
string SoundLampSwitchWav = PathResolver.GetPath(Path.Combine("Resources", "Sound", "LampSwitch.wav"));
string SoundOpenDoorWav = PathResolver.GetPath(Path.Combine("Resources", "Sound", "OpenDoor.wav"));
string SoundCloseDoorWav = PathResolver.GetPath(Path.Combine("Resources", "Sound", "CloseDoor.wav"));
string SoundHitBorderWav = PathResolver.GetPath(Path.Combine("Resources", "Sound", "HitBorder.wav"));
#endregion

#region Hit Drawable Batch
string GlassHitDrawableBatchDir = PathResolver.GetPath(Path.Combine("Resources", "HitDrawableBatch", "Glass"));
string WoodHitDrawableBatchDir = PathResolver.GetPath(Path.Combine("Resources", "HitDrawableBatch", "Wood"));
string OrangeBrickHitDrawableBatchDir = PathResolver.GetPath(Path.Combine("Resources", "HitDrawableBatch", "OrangeBrick"));
#endregion

#region Shader
string pathCeilingWithoutBOMShader = PathResolver.GetPath(Path.Combine("Resources", "Shader", "CeilingSettingWithoutBOM.glsl"));
string pathFloorWithoutBOMShader = PathResolver.GetPath(Path.Combine("Resources", "Shader", "FloorSettingWithoutBOM.glsl"));
string pathCeilingShader = PathResolver.GetPath(Path.Combine("Resources", "Shader", "CeilingSetting.glsl"));
string pathFloorShader = PathResolver.GetPath(Path.Combine("Resources", "Shader", "FloorSetting.glsl"));
#endregion

#endregion

#region Parts World
var nightSkyObject = new Sky(NightSkJpg);
var grassFloorObject = new TexturedFloor(GrassFloorPng, pathFloorWithoutBOMShader)
{
    DownAngleLogScale = 2.7f,
    Scale = 0.4f,
    TextureScrollingSpeed = 0.4f,
};
var woodCeilingObject = new TexturedCeiling(WoodCeilingPng, pathCeilingWithoutBOMShader);
var woodFloorObject = new TexturedFloor(WoodFloorPng, pathFloorWithoutBOMShader)
{
    DownAngleLogScale = 2.7f,
    Scale = 0.4f,
    TextureScrollingSpeed = 0.3f,
};

GameManager.PartsWorld.UpPart = nightSkyObject;
GameManager.PartsWorld.DownPart = grassFloorObject;
#endregion

#region Base UIText
UIText ArialBold_Cyan_20 = new UIText("", 20, new Vector2f(0, 0), fontArialBold, SFML.Graphics.Color.Cyan);
#endregion

#region Sound

#region Hit
SoundDynamic SoundHitGlass = new(SoundHitGlassWav);
SoundHitGlass.Sound.MinDistance = 15f;
SoundHitGlass.Sound.Attenuation = 1f;

SoundDynamic SoundHitBrickWall = new(SoundHitBrickWav);
SoundHitBrickWall.Sound.MinDistance = 15f;
SoundHitBrickWall.Sound.Attenuation = 1f;

SoundDynamic SoundHitWood = new(SoundHitWoodWav);
SoundHitWood.Sound.Volume = 50f;
SoundHitWood.Sound.MinDistance = 15f;
SoundHitWood.Sound.Attenuation = 1f;

SoundDynamic SoundHitBorderWall = new(SoundHitBorderWav);
SoundHitBorderWall.Sound.MinDistance = 15f;
SoundHitBorderWall.Sound.Attenuation = 1.5f;
#endregion


SoundDynamic SoundBulletFlyWall = new(SoundBulletFlyWav);
SoundBulletFlyWall.Sound.MinDistance = 15f;
SoundBulletFlyWall.Sound.Attenuation = 0.5f;

SoundDynamic SoundLampSwitch = new(SoundLampSwitchWav);
SoundLampSwitch.Sound.MinDistance = 10f;
SoundLampSwitch.Sound.Attenuation = 0.6f;

SoundDynamic SoundOpenDoor = new(SoundOpenDoorWav);
SoundOpenDoor.Sound.MinDistance = 15f;
SoundOpenDoor.Sound.Attenuation = 1.5f;

SoundDynamic SoundCloseDoor = new(SoundCloseDoorWav);
SoundCloseDoor.Sound.MinDistance = 15f;
SoundCloseDoor.Sound.Attenuation = 1.5f;

#endregion

#region VisualImpactData

#region Falling Particles
Frame spriteEffectFallingParticlesHitFrame = new Frame(new ImageLoadOptions() { ProcessorOptions = new() { FrameLoadMode = FrameLoadMode.FullFrame } }, VisualEffectBulletGlassGif);
spriteEffectFallingParticlesHitFrame.BaseSelector = new AnimationSelector();
spriteEffectFallingParticlesHitFrame.SpeedAnimation = 80;

SpriteObstacle spriteEffectFallingParticlesHit = new SpriteObstacle(spriteEffectFallingParticlesHitFrame);
spriteEffectFallingParticlesHit.IsPassability = true;
spriteEffectFallingParticlesHit.Scale = 120;

VisualImpactData visualImpactEffectFallingParticles = new(spriteEffectFallingParticlesHit, 1000, false);
#endregion

#region Explosion Particles
Frame spriteEffectExplosionParticlesHitFrame = new Frame(new ImageLoadOptions() { ProcessorOptions = new() { FrameLoadMode = FrameLoadMode.FullFrame } }, VisualEffectBulletBrickGif);
spriteEffectExplosionParticlesHitFrame.BaseSelector = new AnimationSelector();
spriteEffectExplosionParticlesHitFrame.SpeedAnimation = 65;

SpriteObstacle spriteEffectExplosionParticlesHit = new SpriteObstacle(spriteEffectExplosionParticlesHitFrame);
spriteEffectExplosionParticlesHit.IsPassability = true;
spriteEffectExplosionParticlesHit.Scale = 30;

VisualImpactData visualImpactEffectExplosionParticles = new(spriteEffectExplosionParticlesHit, 300, false);
#endregion

#region Red Wave
Frame spriteEffectRedWaveHitFrame = new Frame(new ImageLoadOptions() { ProcessorOptions = new() { FrameLoadMode = FrameLoadMode.FullFrame } }, VisualEffectBulletBorderGif);
spriteEffectRedWaveHitFrame.BaseSelector = new AnimationSelector();
spriteEffectRedWaveHitFrame.SpeedAnimation = 20;

SpriteObstacle spriteEffectRedWaveHit = new SpriteObstacle(spriteEffectRedWaveHitFrame);
spriteEffectRedWaveHit.IsPassability = true;
spriteEffectRedWaveHit.Scale = 30;

VisualImpactData visualImpactEffectRedWave = new(spriteEffectRedWaveHit, 500, false);
#endregion

#endregion

#region HitData

#region Border Wall
HitEffect borderWallHitEffect = new HitEffect(visualImpactEffectRedWave, null, SoundHitBorderWall);
#endregion

#region BrickWallWithGlass
HitDrawableBatch brickWallWithGlassHitDrawableBatch = new(null, GlassHitDrawableBatchDir);
HitEffect brickWallWithGlassHitEffect = new HitEffect(visualImpactEffectFallingParticles, brickWallWithGlassHitDrawableBatch, SoundHitGlass);
#endregion

#region BrickWall
HitDrawableBatch brickWallHitDrawableBatch = new(null, OrangeBrickHitDrawableBatchDir);
HitEffect brickWallHitEffect = new HitEffect(visualImpactEffectExplosionParticles, brickWallHitDrawableBatch, SoundHitBrickWall);
#endregion

#region BrickDoor
HitDrawableBatch wallDoorHitDrawableBatch = new(null, WoodHitDrawableBatchDir);
HitEffect wallDoorHitEffect = new HitEffect(visualImpactEffectFallingParticles, wallDoorHitDrawableBatch, SoundHitWood);
#endregion


HitDataCache.Load(BrickDoorPng, wallDoorHitEffect, new EffectArea(new Vector2f(250, 375), new Vector2f(747, 1264)));
HitDataCache.Append(BrickDoorPng, brickWallHitEffect);

HitDataCache.Load(BrickWindowPng, brickWallWithGlassHitEffect, new EffectArea(new Vector2f(297, 410), new Vector2f(725, 918)));
HitDataCache.Append(BrickWindowPng, brickWallHitEffect);

HitDataCache.Load(BrickWallPng, brickWallHitEffect);

HitDataCache.Load(RedBarrierPng, borderWallHitEffect);
#endregion

#region Create Main Map

#region Map
TexturedWall boundaryWall = new TexturedWall(new ImageLoadOptions() { LoadAsync = false, CreateNew = false }, RedBarrierPng);
boundaryWall.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(100000);
boundaryWall.HitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(100000);

var map = new Map(boundaryWall, 50, 50);
EffectManager.CurrentEffect = effectMainMap;
#endregion

#region Unit

#region Main Unit
Frame unitFrame = new Frame(HumanPng);
unitFrame.BaseSelector = new ViewAngleSelector();

Unitu unit = new Unitu(new SpriteObstacle(unitFrame), 100000);
Camera.CurrentUnit = unit;
unit.HitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(50);
unit.HitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(50);
unit.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(50);
unit.HitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(50);
unit.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(300);
unit.HitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(300);
unit.ShiftCubedX = 50;
unit.ShiftCubedY = 50;
unit.MoveSpeed = 400;

map?.AddObstacle(5, 5, unit);
#endregion

#region Devil

Frame t1 = new Frame(new ImageLoadOptions() { LoadAsync = false }, PathResolver.GetPath(Path.Combine("Resources", "Image", "Sprite", "Demon", "1")));
Frame t2 = new Frame(new ImageLoadOptions() { LoadAsync = false }, PathResolver.GetPath(Path.Combine("Resources", "Image", "Sprite", "Demon", "2")));
Frame t3 = new Frame(new ImageLoadOptions() { LoadAsync = false }, PathResolver.GetPath(Path.Combine("Resources", "Image", "Sprite", "Demon", "3")));
Frame t4 = new Frame(new ImageLoadOptions() { LoadAsync = false }, PathResolver.GetPath(Path.Combine("Resources", "Image", "Sprite", "Demon", "4")));


AnimationClip animation = new AnimationClip(new ImageLoadOptions() { LoadAsync = false }, PathResolver.GetPath(Path.Combine("Resources", "Image", "Sprite", "Demon", "1")));
//animation.AddElement(t1);
animation.AddElement(t2);
animation.AddElement(t3);
animation.AddElement(t4);
Console.WriteLine(animation.CountElements);
animation.BaseSelector = new AnimationSelector();
animation.FrameSelector = new ViewAngleSelector();
animation.PlayMode = PlayMode.Loop;

AnimationClip animation2 = new AnimationClip(t1);
animation2.AddElement(t1);
animation2.FrameSelector = new ViewAngleSelector();


Frame t5 = new Frame(new ImageLoadOptions() { LoadAsync = false }, PathResolver.GetPath(Path.Combine("Resources", "Image", "Sprite", "Demon", "5")));
t5.RectMode = FrameRectMode.UseCurrentFrameRect;
Frame t6 = new Frame(new ImageLoadOptions() { LoadAsync = false }, PathResolver.GetPath(Path.Combine("Resources", "Image", "Sprite", "Demon", "6")));
t6.RectMode = FrameRectMode.UseCurrentFrameRect;

Frame t7 = new Frame(new ImageLoadOptions() { LoadAsync = false }, PathResolver.GetPath(Path.Combine("Resources", "Image", "Sprite", "Demon", "7")));
t7.RectMode = FrameRectMode.UseCurrentFrameRect;

Frame t8 = new Frame(new ImageLoadOptions() { LoadAsync = false }, PathResolver.GetPath(Path.Combine("Resources", "Image", "Sprite", "Demon", "8")));
t8.RectMode = FrameRectMode.UseCurrentFrameRect;

AnimationClip animation3 = new AnimationClip();
animation3.AddElement(t5);
animation3.AddElement(t6);
animation3.AddElement(t7);
animation3.AddElement(t8);
animation3.BaseSelector = new AnimationSelector();
animation3.FrameSelector = new ViewAngleSelector();
animation3.PlayMode = PlayMode.Once;


Animator animationController = new Animator(animation2);
animationController.AddAnimation("MoveHorizontal", animation, 1);
animationController.AddAnimation("Attack", animation3, 2);
animationController.GetAnimationEntry("Attack").UpdateMode = UpdateMode.Conditional;

Unit devil = new Unit(new SpriteObstacle(animationController), 100);


devil.MoveSpeed = 300;
devil.X.OnStartMove = () => { animationController.Play("MoveHorizontal"); };
devil.Y.OnStartMove = () => { animationController.Play("MoveHorizontal"); };

devil.X.OnStopMove = () => { if (!devil.Y.IsMoving) animationController.Stop("MoveHorizontal"); };
devil.Y.OnStopMove = () => { if (!devil.X.IsMoving) animationController.Stop("MoveHorizontal"); };
devil.GroundLevel = -150;

devil.Scale = 100;
devil.Angle = 0.7f;
devil.HitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(20);
devil.HitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(20);
devil.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(20);
devil.HitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(20);
devil.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(300);
devil.HitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(300);
map?.AddObstacle(4, 3, devil);


devil.DialogSprite = new UISprite(PathResolver.GetPath(Path.Combine("Resources", "Image", "Sprite", "Devil", "3.png")));
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


Frame devilDeathFrame = new Frame(new ImageLoadOptions() { LoadAsync = true }, PathResolver.GetPath(Path.Combine("Resources", "Image", "Sprite", "Demon", "9")));
devilDeathFrame.BaseSelector = new AnimationSelector();
devil.DeathAnimation = new DeathEffect(devilDeathFrame, DeathPhase.FrozenFinalFrame, 5500);
#endregion

#region Rebbit
Frame rabbitFrame = new(new ImageLoadOptions() { ProcessorOptions = new() { FrameLoadMode = FrameLoadMode.FullFrame } }, RabbitGif);
rabbitFrame.BaseSelector = new AnimationSelector();
rabbitFrame.PlayMode = PlayMode.Once;
rabbitFrame.SpeedAnimation = 7;

Unit rabbit = new Unit(new SpriteObstacle(rabbitFrame), 100);
rabbit.MoveSpeed = 300;
rabbit.Z.Axis = -400;

rabbit.Scale = 60;
rabbit.HitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(20);
rabbit.HitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(20);
rabbit.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(20);
rabbit.HitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(20);
rabbit.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(200);
rabbit.HitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(200);
map?.AddObstacle(14, 12, rabbit);
#endregion

#endregion

#region Create Structure

#region Mistic House
//Angleeeeeeee: X: 1000 | Y: 600
//Angleeeeeeee: X: 900 | Y: 600
//Angleeeeeeee: X: 800 | Y: 600
//Angleeeeeeee: X: 800 | Y: 500
//I see you! X: 800 | Y: 400
//Angleeeeeeee: X: 400 | Y: 300

TexturedWall MainMapMisticBrickWall = new TexturedWall(new ImageLoadOptions() { LoadAsync = false, CreateNew = false }, BrickWallPng);
map.AddObstacle(8, 1, MainMapMisticBrickWall.GetDeepCopy());
map.AddObstacle(8, 3, MainMapMisticBrickWall.GetDeepCopy());
map.AddObstacle(8, 5, MainMapMisticBrickWall.GetDeepCopy());
//map.AddObstacle(8, 6, MainMapMisticBrickWall.GetDeepCopy());
map.AddObstacle(9, 6, MainMapMisticBrickWall.GetDeepCopy());
map.AddObstacle(13, 6, MainMapMisticBrickWall.GetDeepCopy());
map.AddObstacle(14, 6, MainMapMisticBrickWall.GetDeepCopy());
map.AddObstacle(14, 5, MainMapMisticBrickWall.GetDeepCopy());
map.AddObstacle(14, 3, MainMapMisticBrickWall.GetDeepCopy());
map.AddObstacle(14, 1, MainMapMisticBrickWall.GetDeepCopy());

TexturedWall MainMapMisticBrickWindow = new TexturedWall(new ImageLoadOptions() { LoadAsync = false, CreateNew = false }, BrickWindowPng);
map.AddObstacle(8, 2, MainMapMisticBrickWindow.GetDeepCopy());
map.AddObstacle(8, 4, MainMapMisticBrickWindow.GetDeepCopy());
//map.AddObstacle(10, 6, MainMapMisticBrickWindow.GetDeepCopy());
map.AddObstacle(12, 6, MainMapMisticBrickWindow.GetDeepCopy());
map.AddObstacle(14, 4, MainMapMisticBrickWindow.GetDeepCopy());
map.AddObstacle(14, 2, MainMapMisticBrickWindow.GetDeepCopy());

TexturedWall MainMapMisticBrickDoor = new TexturedWall(new ImageLoadOptions() { LoadAsync = false, CreateNew = false }, BrickDoorPng);
map.AddObstacle(11, 6, MainMapMisticBrickDoor);
#endregion

#endregion

#endregion

#region Create Mistic House Map

Map? houseMap = null;
Vector2i positionMisticHouseBrickDoor = new Vector2i(0, 3);
Vector2i positionMisticHouseNewspaperHint = new Vector2i(4, 3);

int multLampEffect = 3;
CustomEffect effectLamp = new CustomEffect(DefaultPresets.DarkEffect);
effectLamp.EffectEnd = 2;

Action CreateHouseMap = () =>
{
    houseMap = new Map(8, 7);

    TexturedWall MisticHouseBrickDoor = new TexturedWall(new ImageLoadOptions() { LoadAsync = false, CreateNew = false }, BrickDoorPng);
    houseMap.AddObstacle(positionMisticHouseBrickDoor.X, positionMisticHouseBrickDoor.Y, MisticHouseBrickDoor);

    TexturedWall MisticHouseBrickWall = new TexturedWall(new ImageLoadOptions() { LoadAsync = false, CreateNew = false }, BrickWallPng);
    houseMap.AddObstacle(0, 0, MisticHouseBrickWall.GetDeepCopy());
    houseMap.AddObstacle(0, 1, MisticHouseBrickWall.GetDeepCopy());
    houseMap.AddObstacle(0, 6, MisticHouseBrickWall.GetDeepCopy());
    houseMap.AddObstacle(0, 5, MisticHouseBrickWall.GetDeepCopy());
    houseMap.AddObstacle(1, 0, MisticHouseBrickWall.GetDeepCopy());
    houseMap.AddObstacle(1, 6, MisticHouseBrickWall.GetDeepCopy());
    houseMap.AddObstacle(3, 0, MisticHouseBrickWall.GetDeepCopy());
    houseMap.AddObstacle(3, 6, MisticHouseBrickWall.GetDeepCopy());
    houseMap.AddObstacle(4, 0, MisticHouseBrickWall.GetDeepCopy());
    houseMap.AddObstacle(4, 6, MisticHouseBrickWall.GetDeepCopy());
    houseMap.AddObstacle(6, 0, MisticHouseBrickWall.GetDeepCopy());
    houseMap.AddObstacle(6, 6, MisticHouseBrickWall.GetDeepCopy());
    houseMap.AddObstacle(7, 0, MisticHouseBrickWall.GetDeepCopy());
    houseMap.AddObstacle(7, 6, MisticHouseBrickWall.GetDeepCopy());
    houseMap.AddObstacle(7, 5, MisticHouseBrickWall.GetDeepCopy());
    houseMap.AddObstacle(7, 1, MisticHouseBrickWall.GetDeepCopy());

    TexturedWall MisticHouseBrickBloodWall = new TexturedWall(new ImageLoadOptions() { LoadAsync = false, CreateNew = false }, BrickBloodWallPng);
    houseMap.AddObstacle(7, 3, MisticHouseBrickBloodWall.GetDeepCopy());

    TexturedWall MisticHouseBrickWindow = new TexturedWall(new ImageLoadOptions() { LoadAsync = false, CreateNew = false }, BrickWindowPng);
    houseMap.AddObstacle(0, 4, MisticHouseBrickWindow.GetDeepCopy());
    houseMap.AddObstacle(0, 2, MisticHouseBrickWindow.GetDeepCopy());
    houseMap.AddObstacle(2, 0, MisticHouseBrickWindow.GetDeepCopy());
    houseMap.AddObstacle(2, 6, MisticHouseBrickWindow.GetDeepCopy());
    houseMap.AddObstacle(5, 0, MisticHouseBrickWindow.GetDeepCopy());
    houseMap.AddObstacle(5, 6, MisticHouseBrickWindow.GetDeepCopy());
    houseMap.AddObstacle(7, 4, MisticHouseBrickWindow.GetDeepCopy());
    houseMap.AddObstacle(7, 2, MisticHouseBrickWindow.GetDeepCopy());

    #region Unit
    Frame newspaperHintFrame = new(new ImageLoadOptions() { ProcessorOptions = new() { FrameLoadMode = FrameLoadMode.FullFrame } }, RotatingNewspaperGif);
    newspaperHintFrame.BaseSelector = new AnimationSelector();
    newspaperHintFrame.SpeedAnimation = 20;

    //Unit? newspaperHint = new Unit(new SpriteObstacle(newspaperHintFrame), 100);
    //newspaperHint.HitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(10);
    //newspaperHint.HitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(10);
    //newspaperHint.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(10);
    //newspaperHint.HitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(10);
    //newspaperHint.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(150);
    //newspaperHint.HitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(150);

    //newspaperHint.Scale = 64;
    //newspaperHint.ShiftCubedX = 50;
    //newspaperHint.ShiftCubedY = 50;


    //SpriteObstacle? woodTable = new SpriteObstacle(PathResolver.GetMainPath(Path.Combine("Resources", "WoodTable.gif")), new ImageLoadOptions() { FrameLoadMode = FrameLoadMode.FullFrame }, false);
    //woodTable.Scale = 164;
    //woodTable.ShiftCubedX = 50;
    //woodTable.ShiftCubedY = 50;
    //woodTable.Z.Axis = -370;
    //woodTable.Animation.IsAnimation = false;

    Frame lampeMisticHouseFrame = new(BigLampePng);
    lampeMisticHouseFrame.PlayMode = PlayMode.Pause;
    SpriteObstacle? lampeMisticHouse = new SpriteObstacle(lampeMisticHouseFrame);
    lampeMisticHouse.HitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(20);
    lampeMisticHouse.HitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(20);
    lampeMisticHouse.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(20);
    lampeMisticHouse.HitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(20);
    lampeMisticHouse.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(350);
    lampeMisticHouse.HitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(350);
    lampeMisticHouse.Scale = 150;
    lampeMisticHouse.Z.Axis = -100;
    lampeMisticHouse.ShiftCubedX = 20;
    lampeMisticHouse.ShiftCubedY = 80;
    houseMap?.AddObstacle(6, 1, new SpriteObstacle(lampeMisticHouse, new ImageLoadOptions(){ CreateNew = true }) { ShiftCubedX = 80, ShiftCubedY = 20 });
    houseMap?.AddObstacle(1, 1, new SpriteObstacle(lampeMisticHouse, new ImageLoadOptions() { CreateNew = true }) { ShiftCubedX = 20, ShiftCubedY = 20 });
    houseMap?.AddObstacle(6, 5, new SpriteObstacle(lampeMisticHouse, new ImageLoadOptions() { CreateNew = true }) { ShiftCubedX = 80, ShiftCubedY = 80 });
    houseMap?.AddObstacle(1, 5, lampeMisticHouse);


    //houseMap?.AddObstacle(positionMisticHouseNewspaperHint.X, positionMisticHouseNewspaperHint.Y, newspaperHint);
    // houseMap?.AddObstacle(positionMisticHouseNewspaperHint.X, positionMisticHouseNewspaperHint.Y, woodTable);
    #endregion
};
#endregion

#region UI

#region UIText
UIText FpsText = new UIText("", 20, new Vector2f(0, 0), fontArialBold, SFML.Graphics.Color.Cyan)
{
    IsHide = true,
};

#endregion

#region Fading Text
FadingText fadingTextOpenButton = new FadingText(ArialBold_Cyan_20, FadingType.Appears, FadingTextLife.OneShotFreeze, 2000, null);
fadingTextOpenButton.RenderText.Text.DisplayedString = "Press E to open";
fadingTextOpenButton.PositionOnScreen = new Vector2f(Screen.GetPercentWidth(65), Screen.GetPercentHeight(65));
#endregion

#region Animation State
Frame hpBarAnimation = new Frame(PathResolver.GetPath(Path.Combine("Resources", "UI", "small.gif")))
{
    BaseSelector = new AnimationSelector(),
    SpeedAnimation = 30
};
#endregion

#region Shape
UIShape LoadShape = new UIShape(new RectangleShape(new Vector2f(Screen.ScreenWidth, Screen.ScreenHeight)));
LoadShape.Animation.AddElement(ImageLoader.Load(null, LoadingPng).FirstOrDefault() ?? TextureWrapper.Placeholder);
LoadShape.RenderOrder = RenderOrder.SystemNotification;
#endregion

#region FillBar
//FillBar HpBar = new FillBar(new AnimationContent(hpBarAnimation), new ColorContent(SFML.Graphics.Color.Red), unit.Hp, unit)
//{
//    BorderThickness = 10,
//    Width = 400,
//    Height = 100,
//    PositionOnScreen = new Vector2f(0, Screen.ScreenHeight),
//    BorderFillColor = SFML.Graphics.Color.Black,
//};
#endregion

#endregion

#region Create Triggers

#region Base Triggers

#region Button Triggers
var buttonTriggerE = new TriggerButton(new ButtonBinding(new ControlLib.Buttons.Button(VirtualKey.E), () => { }), null, null)
{
    CooldownMs = 100,
};
#endregion

#region TriggerTouch
Action<IUnit> GetActionCollisionKnockback(TriggerTouch trigger)
{
    return (unit) =>
    {
        if (trigger.TriggerObject is null)
            return;

        var closest = new Vector2f(
        Math.Clamp((float)unit.X.Axis,
            (float)(trigger.TriggerObject?.HitBox[CoordinatePlane.X, SideSize.Smaller]?.Side ?? 0f),
            (float)(trigger.TriggerObject?.HitBox[CoordinatePlane.X, SideSize.Larger]?.Side ?? 0f)),
        Math.Clamp((float)unit.Y.Axis,
            (float)(trigger.TriggerObject?.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.Side ?? 0f),
            (float)(trigger.TriggerObject?.HitBox[CoordinatePlane.Y, SideSize.Larger]?.Side ?? 0f))
    );

        float dx = (float)((trigger.TriggerObject?.X.Axis - unit.X.Axis) ?? 0);
        float dy = (float)((trigger.TriggerObject?.Y.Axis - unit.Y.Axis) ?? 0);
        float angleToTarget = MathF.Atan2(dy, dx);

        SoundHitBorderWall.Play(unit.Map, new Vector3f(
                    closest.X,
                    closest.Y,
                    (float)unit.Z.Axis
               ));

        BeyondRenderManager.Create(unit, visualImpactEffectRedWave, closest.X, closest.Y, unit.Z.Axis);

        if (unit is Unit uu)
        {
            uu.Jump();
            uu.KnockbackAngle = angleToTarget + MathF.PI;
            uu.Knockback();
        }
    };
}
#endregion

#endregion

#region Trigger Mistic House Door

#region Open Entrance Door
var fadingTextOpenDoor = new FadingText(fadingTextOpenButton);
var distanceOpenEntranceDoorDistancTrigger = new TriggerDistance((target) => target is not null && target == MainMapMisticBrickDoor,
    (owner) => { fadingTextOpenDoor.Controller.FadingType = FadingType.Appears; fadingTextOpenDoor.Controller.FadingTextLife = FadingTextLife.OneShotFreeze; fadingTextOpenDoor.Owner = owner; },
    (owner) => { fadingTextOpenDoor.SwapType(); fadingTextOpenDoor.Controller.FadingTextLife = FadingTextLife.OneShotDispose; });

var distanceOpenExitDoorTrigger = new TriggerDistance
    (
    (target) =>
    {
        if (houseMap != null && houseMap.Obstacles.TryGetValue(
            (positionMisticHouseBrickDoor.X * Screen.Setting.Tile,
            positionMisticHouseBrickDoor.Y * Screen.Setting.Tile),
         out var value))
        {
            return target is not null && value.ContainsKey(target);
        }

        return false;
    },
    (owner) => { fadingTextOpenDoor.Controller.FadingType = FadingType.Appears; fadingTextOpenDoor.Controller.FadingTextLife = FadingTextLife.OneShotFreeze; fadingTextOpenDoor.Owner = owner; },
    (owner) => { fadingTextOpenDoor.SwapType(); fadingTextOpenDoor.Controller.FadingTextLife = FadingTextLife.OneShotDispose; }

);


UIShape blackSreenOpenDoor = new UIShape(LoadShape);
Vector2i safePositionOpenEntranceDoor = new Vector2i(MainMapMisticBrickDoor.CellX / Screen.Setting.Tile, MainMapMisticBrickDoor.CellY / Screen.Setting.Tile + 1);
Vector2i safePositionOpenExitDoor = new Vector2i(positionMisticHouseBrickDoor.X + 1, positionMisticHouseBrickDoor.Y);

TriggerAnd openEntranceDoorTrigger = new TriggerAnd(buttonTriggerE, distanceOpenEntranceDoorDistancTrigger);
openEntranceDoorTrigger.OnTriggered = (unit) =>
{
    var npc = distanceOpenEntranceDoorDistancTrigger.GetTarget();
    if (npc is null || npc.Value.TargetObject?.Map is null)
        return;

    if (blackSreenOpenDoor.Owner != unit)
        blackSreenOpenDoor.Owner = unit;
    blackSreenOpenDoor.IsHide = false;

    Camera.CurrentUnit.Map = null;
    EnsureHouseMapAndApply(unit);

    EffectManager.CurrentEffect = effectLamp;
    SoundOpenDoor.Play(null, new Vector3f((float)unit.X.Axis, (float)unit.Z.Axis, (float)unit.Y.Axis), ListenerType.Mono);

};

TriggerHandler.AddTriger(unit, openEntranceDoorTrigger);
#region Func
void EnsureHouseMapAndApply(IObject addObject)
{
    if (houseMap is null)
    {
        GameManager.DeferredAction += () =>
        {
            CreateHouseMap?.Invoke();
            ApplyHouseMapChanges(addObject);

        };
    }
    else
        ApplyHouseMapChanges(addObject);
}
void ApplyHouseMapChanges(IObject addObject)
{
    if (Camera.CurrentUnit is not null)
        houseMap?.AddObstacle(safePositionOpenExitDoor.X, safePositionOpenExitDoor.Y, addObject);

    GameManager.PartsWorld.UpPart = woodCeilingObject;
    GameManager.PartsWorld.DownPart = woodFloorObject;
    blackSreenOpenDoor.IsHide = true;
}
#endregion

#endregion

#region Open Exit Door
TriggerAnd openExitDoorTrigger = new TriggerAnd(buttonTriggerE, distanceOpenExitDoorTrigger);
openExitDoorTrigger.OnTriggered = (unit) =>
{

    var npc = distanceOpenExitDoorTrigger.GetTarget();
    if (npc is null || npc.Value.TargetObject?.Map is null)
        return;


    if (blackSreenOpenDoor.Owner != unit)
        blackSreenOpenDoor.Owner = unit;
    blackSreenOpenDoor.IsHide = false;

    Camera.CurrentUnit.Map = null;
    map?.AddObstacle(safePositionOpenEntranceDoor.X, safePositionOpenEntranceDoor.Y, Camera.CurrentUnit);

    GameManager.PartsWorld.UpPart = nightSkyObject;
    GameManager.PartsWorld.DownPart = grassFloorObject;
    EffectManager.CurrentEffect = effectMainMap;

    blackSreenOpenDoor.IsHide = true;
    SoundOpenDoor.Play(null, new Vector3f((float)unit.X.Axis, (float)unit.Z.Axis, (float)unit.Y.Axis), ListenerType.Mono);

};

TriggerHandler.AddTriger(unit, openExitDoorTrigger);
#endregion

#endregion

#region Trigger Open NewspaperHint Mistic House
FadingText fadingTextOpenNewspaperHint = new FadingText(ArialBold_Cyan_20, FadingType.Appears, FadingTextLife.OneShotFreeze, 2000, null);
fadingTextOpenNewspaperHint.RenderText.Text.DisplayedString = "Press E to open";
fadingTextOpenNewspaperHint.PositionOnScreen = new Vector2f(Screen.GetPercentWidth(65), Screen.GetPercentHeight(65));

var buttonOpenNewspaperHintTrigger = new TriggerButton(new ButtonBinding(new ControlLib.Buttons.Button(VirtualKey.E), () => { }, 1000), null, null);
var distanceOpenNewspaperHintTrigger = new TriggerDistance
    (
    (target) =>
    {
        if (houseMap != null && houseMap.Obstacles.TryGetValue(
            (positionMisticHouseNewspaperHint.X * Screen.Setting.Tile,
            positionMisticHouseNewspaperHint.Y * Screen.Setting.Tile),
         out var value))
        {
            return target is not null && value.ContainsKey(target);
        }

        return false;
    },
    (owner) => { fadingTextOpenNewspaperHint.Controller.FadingType = FadingType.Appears; fadingTextOpenNewspaperHint.Controller.FadingTextLife = FadingTextLife.OneShotFreeze; fadingTextOpenNewspaperHint.Owner = owner; },
    (owner) => { fadingTextOpenNewspaperHint.SwapType(); fadingTextOpenNewspaperHint.Controller.FadingTextLife = FadingTextLife.OneShotDispose; }
);


TriggerAnd openNewspaperHintTrigger = new TriggerAnd(buttonOpenNewspaperHintTrigger, distanceOpenNewspaperHintTrigger);
openNewspaperHintTrigger.OnTriggered = (unit) =>
{
    var npc = distanceOpenNewspaperHintTrigger.GetTarget();
    if (npc is null)
        return;

    if (blackSreenOpenDoor.Owner != unit)
        blackSreenOpenDoor.Owner = unit;
    blackSreenOpenDoor.IsHide = false;

};
TriggerHandler.AddTriger(unit, openNewspaperHintTrigger);
#endregion

#region Trigger Touch Big Lampe
FadingText fadingTextTouchBigLampe = new FadingText(fadingTextOpenButton);
var distanceOpenBigLampe = new TriggerDistance
    (
    (target) =>
    {
        return target?.GetUsedTexture()?.PathTexture?.Contains(BigLampePng) == true;
    },
    (owner) => { fadingTextTouchBigLampe.Controller.FadingType = FadingType.Appears; fadingTextTouchBigLampe.Controller.FadingTextLife = FadingTextLife.OneShotFreeze; fadingTextTouchBigLampe.Owner = owner; },
    (owner) => { fadingTextTouchBigLampe.SwapType(); fadingTextTouchBigLampe.Controller.FadingTextLife = FadingTextLife.OneShotDispose; }
);

TriggerAnd touchBigLampeTrigger = new TriggerAnd(new TriggerButton(buttonTriggerE), distanceOpenBigLampe) { CooldownMs = 300};

const string LampeOn = "on";
touchBigLampeTrigger.OnTriggered = (unit) =>
{
    var npc = distanceOpenBigLampe.GetTarget();
    if (npc is null || npc.Value.TargetObject?.Map is null || npc.Value.TargetObject is not IAnimatable animation)
        return;

    var obj = npc.Value.TargetObject;
    SoundLampSwitch.Play(obj.Map, new Vector3f((float)obj.X.Axis, (float)obj.Y.Axis, (float)obj.Z.Axis));

    int index = 0;
    var currentTexture = animation.Animation.CurrentTexture?.PathTexture;
    if (animation.Animation.CurrentFrame is null)
        return;

    for (; index != animation.Animation.CurrentFrame.CountElements; index++)
    {
        var frame = animation.Animation.CurrentFrame.GetElement(index);
        if (currentTexture is not null && frame?.PathTexture != currentTexture)
        {
            if (frame?.PathTexture.Contains(LampeOn) == true)
                effectLamp.EffectEnd += multLampEffect;
            else
                effectLamp.EffectEnd -= multLampEffect;

            animation.Animation.CurrentFrame.SetCurrentElement(index);
            break;
        }
    }
};
TriggerHandler.AddTriger(unit, touchBigLampeTrigger);

#endregion
#endregion



#region Bullets
Frame devilBulletUnitFrame = new Frame(DevilPng)
{
    SpeedAnimation = 30,
    BaseSelector = new AnimationSelector(),
};

var devilBulletUnit = new Unit(new SpriteObstacle(new Frame(DevilPng)), 0);
devilBulletUnit.Scale = 15;
devilBulletUnit.HitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(10);
devilBulletUnit.HitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(10);
devilBulletUnit.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(10);
devilBulletUnit.HitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(10);
devilBulletUnit.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(100);
devilBulletUnit.HitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(100);

UnitBullet BulletDevil = new UnitBullet(20, 15, devilBulletUnit, null) { SoundFly = SoundBulletFlyWall };
#endregion

#region Gun

#region Button Bindings
var shootPistol = new ControlLib.Buttons.ButtonBinding(LeftButton, null, 100);
#endregion

#region Pistol
ImageLoadOptions pistolImageLoadOptions = new ImageLoadOptions() { LoadAsync = false, ProcessorOptions = new() { FrameLoadMode = FrameLoadMode.FullFrame } };
UIAnimation pistolAnimation = new UIAnimation(pistolImageLoadOptions, PistolGif)
{
    BaseSelector = new AnimationSelector(),
    SpeedAnimation = 35,
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

Magazine pistolMagazine = new Magazine(20, 140, BulletDevil, R.Key, pistolBulletText);
Gun pistol = new Gun(pistolAnimation, pistolMagazine, shootPistol);
#endregion
#endregion


#region Control
// Object params: IUnit obj, double directionX(1), double directionY(0)
ControlLib.Buttons.ButtonBinding forward = new ButtonBinding(W, MoveLib.Move.MovePositions.Move, 0);
// Object params: IUnit obj, double directionX(-1), double directionY(0)
ControlLib.Buttons.ButtonBinding backward = new ButtonBinding(S, MoveLib.Move.MovePositions.Move, 0);
// Object params: IUnit obj, double directionX(0), double directionY(-1)
ControlLib.Buttons.ButtonBinding left = new ButtonBinding(A, MoveLib.Move.MovePositions.Move, 0);
// Object params: IUnit obj, double directionX(0), double directionY(1)
ControlLib.Buttons.ButtonBinding right = new ButtonBinding(D, MoveLib.Move.MovePositions.Move, 0);

// Object params: null
ControlLib.Buttons.ButtonBinding jumb = new ButtonBinding(Space, null);
ControlLib.Buttons.ButtonBinding closeWindow = new ButtonBinding(Q, Screen.Window.Close);
// Object params: IUnit obj
ControlLib.Buttons.ButtonBinding autoResetAngle = new ButtonBinding(new Button(VirtualKey.None), MoveLib.Angle.MoveAngle.ResetAngle, 0);
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








FadingText fadingDialog = new FadingText(ArialBold_Cyan_20, FadingType.Appears, FadingTextLife.OneShotFreeze, 2000, null);
fadingDialog.RenderText.Text.DisplayedString = "Press E to open";
fadingDialog.PositionOnScreen = new Vector2f(Screen.GetPercentWidth(65), Screen.GetPercentHeight(65));

var buttonDialog = new TriggerButton(new ButtonBinding(new ControlLib.Buttons.Button(VirtualKey.E), () => { }, 1000), null, null);
var distanceDialogTrigger = new TriggerDistance((target) => target is not null && target == devil,
    (owner) => { fadingTextOpenDoor.Controller.FadingType = FadingType.Appears; fadingTextOpenDoor.Controller.FadingTextLife = FadingTextLife.OneShotFreeze; fadingTextOpenDoor.Owner = owner; },
    (owner) => { fadingTextOpenDoor.SwapType(); fadingTextOpenDoor.Controller.FadingTextLife = FadingTextLife.OneShotDispose; });

var buttonDialogTrigger = new TriggerButton(new ButtonBinding(new ControlLib.Buttons.Button(VirtualKey.E), () => { }, 1000), null, null);
TriggerAnd DialogTrigger = new TriggerAnd(buttonDialogTrigger, distanceDialogTrigger);
DialogTrigger.OnTriggered = async (unit) =>
{
    await dialogManager.RunAsync();
};
TriggerHandler.AddTriger(unit, DialogTrigger);

#endregion


#region Assignment of Dependencies

#region Gun

#region unit

var pistolUnit = new Gun(pistol, null, unit);

#endregion

#region devil

var pistolDevil = new Gun(pistol, new(RightButton, null, 100), devil);
pistolDevil.SpriteAnimationName = "Attack";
#endregion


#endregion

#region Control

#region unit

unit?.Control.AddBottomBind(new ButtonBinding(forward, null, new object[] { unit, 1, 0 }));
unit?.Control.AddBottomBind(new ButtonBinding(backward, null, new object[] { unit, -1, 0 }));
unit?.Control.AddBottomBind(new ButtonBinding(left, null, new object[] { unit, 0, -1 }));
unit?.Control.AddBottomBind(new ButtonBinding(right, null, new object[] { unit, 0, 1 }));

unit?.Control.AddBottomBind(new ButtonBinding(autoResetAngle, null, new object[] { unit }));

var fpsUnt = new UIText(FpsText, unit);
unit?.Control.AddBottomBind(new ButtonBinding(new Button(VirtualKey.None), () => { fpsUnt.SetText($"Fps: {FPS.TextFPS}"); }));
unit?.Control.AddBottomBind(new ButtonBinding(F6, () => { fpsUnt.IsHide = !fpsUnt.IsHide; }, 500));

unit?.Control.AddBottomBind(new ButtonBinding(jumb, devil.Jump));



unit?.Control.AddBottomBind(closeWindow);


unit?.Control.AddBottomBind(pistolDevil.UIAnimation.BottomBinding);
#endregion

#region Rabbit
rabbit.behavioral = new AIController(rabbit);

AIStateMachine machineMovementRabbit = new();
var blockedMovementRabbit = (AIContext ctx) =>
    ctx.Controller?.GetMachine(AIBehaviorType.Emotion)?.IsSignaled == true ||
    ctx.Controller?.GetMachine(AIBehaviorType.Pursuit)?.IsRunning == true;

machineMovementRabbit.AddBehavior(new MoveBehavior());
machineMovementRabbit.AddBehavior(new JumpBehavior() { IsBlocked = blockedMovementRabbit, 
FuncSuccess = (cxt) =>
{
    if (rabbit.Animation.CurrentFrame is not null)
    {
        rabbit.Animation.CurrentFrame.IsFinishMode = false;

    }
},
});
machineMovementRabbit.AddBehavior(new WaitBehavior() { WaitDurationMs = 200 });
machineMovementRabbit.AddBehavior(new MoveCamera() { AngleStrategy = new RandomAngleStrategy() });


machineMovementRabbit.AddTransition<JumpBehavior, MoveBehavior>(BehaviorStatus.Success);
machineMovementRabbit.AddTransition<MoveBehavior, MoveCamera>(BehaviorStatus.Success);
machineMovementRabbit.AddTransition<MoveCamera, WaitBehavior>(BehaviorStatus.Success);
machineMovementRabbit.AddTransition<WaitBehavior, JumpBehavior>(BehaviorStatus.Success);

machineMovementRabbit.SetBehavior<JumpBehavior>();
rabbit.behavioral.AddStateMachine(AIBehaviorType.Movement, machineMovementRabbit);

rabbit.behavioral.Start();
#endregion

#region Devil
devil.behavioral = new AIController(devil);


AIStateMachine machineVision = new();
machineVision.AddBehavior(new PatrolBehavior());
machineVision.SetBehavior<PatrolBehavior>();
devil.behavioral.AddStateMachine(AIBehaviorType.Vision, machineVision);

var blockedFromZone = (AIContext ctx) =>  ctx.Controller?.GetMachine(AIBehaviorType.ZoneControl)?.IsRunning == true;
AIStateMachine machineZone = new();
machineZone.AddBehavior(new ZoneRestrictionBehavior(new(devil.CellX, devil.CellY)) { Radius = 300 });
//machineZone.AddTransition<ZoneRestrictionBehavior, ZoneRestrictionBehavior>(BehaviorStatus.Success);
//machineZone.SetBehavior<ZoneRestrictionBehavior>();
//devil.behavioral.AddStateMachine(AIBehaviorType.ZoneControl, machineZone);

AIStateMachine machineEmotion = new(true);
machineEmotion.AddBehavior(new PingPongMovement() { MovementDurationMs = 400, IsBlocked = blockedFromZone });
machineEmotion.AddTransition<PingPongMovement, PingPongMovement>(BehaviorStatus.Success);
machineEmotion.SetBehavior<PingPongMovement>();
devil.behavioral.AddStateMachine(AIBehaviorType.Emotion, machineEmotion);

AIStateMachine machinePursuit = new();
var blockedPursuit = (AIContext ctx) =>
    blockedFromZone.Invoke(ctx) == true ||
    ctx.Controller?.GetMachine(AIBehaviorType.Emotion)?.IsSignaled == true;
machinePursuit.AddBehavior(new PersecutionBehavior() { IsBlocked = blockedPursuit });
machinePursuit.SetBehavior<PersecutionBehavior>();
devil.behavioral.AddStateMachine(AIBehaviorType.Pursuit, machinePursuit);



AIStateMachine machineCombat = new();
InfoGun infoGunMachineCombatDevil = new(pistolDevil.ShootBinding!, pistolDevil.Magazine.UpdateReloadStatus, () => pistol.Magazine.GetNextBullet()?.Speed ?? 0f, BulletHandler.SleepMs);
List<IAimStrategy> aimStrategiesMachineCombatDevil = new List<IAimStrategy>()
{
    new DirectAimStrategy(),
    new PredictiveAimStrategy(),
};
machineCombat.AddBehavior(new AttackBehavior(infoGunMachineCombatDevil, aimStrategiesMachineCombatDevil) { IsBlocked = blockedFromZone });
var dodgeStepsDevil = new List<DodgeStep>()
{ 
    new(DodgeDirection.Left, 100),
     new(DodgeDirection.Right, 200),
     new(DodgeDirection.Left, 200),
     new(DodgeDirection.Right, 200),
     new(DodgeDirection.Left, 100),
     new(DodgeDirection.Right, 300),
};
machineCombat.AddBehavior(new DodgeBehavior(dodgeStepsDevil) { IsBlocked = blockedFromZone });
machineCombat.AddTransition<AttackBehavior, DodgeBehavior>(BehaviorStatus.Success);
machineCombat.AddTransition<DodgeBehavior, AttackBehavior>(BehaviorStatus.Success);
machineCombat.SetBehavior<AttackBehavior>();
devil.behavioral.Context.OnEvent += evt =>
{
    if (evt == AttackBehavior.ReloadTriggerEvent)
    {
        devil.behavioral.GetMachine(AIBehaviorType.Emotion)?.Signal();
    }
};
//devil.behavioral.Context.OnEvent += evt =>
//{
//    if (evt == AttackBehavior.ReloadCompletedEvent)
//        devil.behavioral.GetMachine(AIBehaviorType.Emotion)?.Unsignal();
//};
devil.behavioral.AddStateMachine(AIBehaviorType.Combat, machineCombat);


AIStateMachine machineMovement = new();
var blockedMovement = (AIContext ctx) =>
    blockedFromZone.Invoke(ctx) == true ||
    ctx.Controller?.GetMachine(AIBehaviorType.Emotion)?.IsSignaled == true ||
    ctx.Controller?.GetMachine(AIBehaviorType.Combat)?.IsRunning == true ||
    ctx.Controller?.GetMachine(AIBehaviorType.Pursuit)?.IsRunning == true;

machineMovement.AddBehavior(new CircularMovement() { RotationDirection = 1, IsBlocked = blockedMovement });
machineMovement.AddBehavior(new PingPongMovement() { MovementDurationMs = 500, IsBlocked = blockedMovement });
machineMovement.AddBehavior(new CircularMovement() { RotationDirection = -1, IsBlocked = blockedMovement });

machineMovement.AddTransition<CircularMovement, PingPongMovement>(BehaviorStatus.Success);
machineMovement.AddTransition<PingPongMovement, CircularMovement>(BehaviorStatus.Success);
machineMovement.AddTransition<CircularMovement, CircularMovement>(BehaviorStatus.Success);

machineMovement.SetBehavior<PingPongMovement>();
devil.behavioral.AddStateMachine(AIBehaviorType.Movement, machineMovement);

devil.behavioral.Start();
#endregion
#endregion

#region Triggers

#region unit

#region Trigger Touch Red Barrier
Predicate<IObject> detectRedBarrierFun = (obj) =>
obj is not null && obj.GetUsedTexture(unit)?.PathTexture == RedBarrierPng;

var unitTriggerTouchRedBarrier = new TriggerTouch(detectRedBarrierFun!, null, null);
unitTriggerTouchRedBarrier.OnTriggered += (unit) => GetActionCollisionKnockback(unitTriggerTouchRedBarrier).Invoke(unit);

TriggerHandler.AddTriger(unit!, unitTriggerTouchRedBarrier);
#endregion

#endregion

#endregion

#endregion


//FadingSprite fadingSprite = new FadingSprite(ImageLoader.Load(null, true, PathResolver.GetPath(Path.Combine("Resources", "Image", "WallTexture", "Wall3.png"))).First(), FadingType.Appears, FadingTextLife.OneShotFreeze, 2000, null);
//fadingSprite.PositionOnScreen = new Vector2f(Screen.GetPercentWidth(50), Screen.GetPercentHeight(50));
//fadingSprite.Sprite.Scale = new Vector2f(0.5f, 0.5f);
//FadingSprite fadingSprite1 = new FadingSprite(ImageLoader.Load(null, true, PathResolver.GetPath(Path.Combine("Resources", "Image", "WallTexture", "Wall1.png"))).First(), FadingType.Appears, FadingTextLife.OneShotFreeze, 2000, null);
//fadingSprite1.PositionOnScreen = new Vector2f(Screen.GetPercentWidth(50), Screen.GetPercentHeight(50));
//fadingSprite1.Sprite.Scale = new Vector2f(0.5f, 0.5f);

//TriggerCollision triggerHitBoxTouch1 = new TriggerCollision(devil, ObjectSide.Bottom, ObjectSide.Top,
//    (owner) => { fadingSprite.Controller.FadingType = FadingType.Appears; fadingSprite.Restart(); fadingSprite.Controller.FadingTextLife = FadingTextLife.PingPongDispose; fadingSprite.Owner = owner; },
//    (owner) => { fadingSprite1.Controller.FadingType = FadingType.Appears; fadingSprite1.Restart(); fadingSprite1.Controller.FadingTextLife = FadingTextLife.PingPongDispose; fadingSprite1.Owner = owner; });

//TriggerHandler.AddTriger(unit, triggerHitBoxTouch1);



GameManager.Start();