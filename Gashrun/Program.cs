using Gashrun;
using ObjectFramework;
using ScreenLib;
using TextureLib.Loader;
using UIFramework;
using FpsLib;
using EffectLib.EffectCore;
using EffectLib.Effect;
using RenderLib.Algorithm;


FPS.BufferSize = 50;
RayTracingLib.Raycast.CoordinatesMoving = 2;
Gashrun.RenderAlgorithm.UseHeightPerspective = false;

Screen.Initialize(1000, 600);

EffectManager.CurrentEffect = new CustomEffect(DefaultPresets.DarkEffect);
var effect = EffectManager.CurrentEffect.EffectColor;
EffectManager.CurrentEffect.EffectColor = new(effect.X, effect.Y, effect.Z, 0);
EffectManager.CurrentEffect.EffectEnd = 15;

_ = GameInitializer.InitializeAsync();
GameManager.Start();


//var bath = new HitDrawableBatch(ImageLoader.Load(null, true, PathResolver.GetPath(Path.Combine("Resources", "Image", "BulletHole", "UniversalHole"))));
//bath.Mode = HitDrawSelectMode.Random;
//var bath1 = new HitDrawableBatch(new CircleShape(30));

//HitEffect hitEffectData = new HitEffect(null, bath, null);
//HitDataCache.Load(PathResolver.GetMainPath(Path.Combine("Resources", "Image", "WallTexture", "Wall1.png")), hitEffectData);
////EffectLib.VisualEffectHelper.SetVisualEffect(new Darkness(0.0001f));
//RayTracingLib.Raycast.CoordinatesMoving = 2;

//TexturedWall wall = new(null, true, PathResolver.GetMainPath(Path.Combine("Resources", "Image", "WallTexture", "Wall1.png")));
////MultiWall multiWall = Deserializer.FromJSON<MultiWallDTO, MultiWall>(PathResolver.GetPath(Path.Combine("Resources", "multiwall_1.json")));
////CustomEffect effect = new EffectLib.Effect.BlinEffect();

//wall.Effect = new CustomEffect(DefaultPresets.BlinEffect);
////RegistratorDTO<Gashrun.IEffectDTO, EffectLib.EffectCore.IEffect>.EnsureInitialized();
////RegistratorDTO<IObjectDTO, IObject>.EnsureInitialized();
////RegistratorDTO<TexturedWallDTO1, TexturedWall>.EnsureInitialized();
//Map map = new Map(wall, 30, 30);
////GameManager.MiniMap.Zoom.MinZoom.ToString();
//GameManager.MiniMap.Setting.MapScaleY = 2;
//GameManager.MiniMap.Setting.MapScaleX = 2;
//GameManager.MiniMap.Setting.Positions = PositionsMiniMap.UpperRightCorner;
//GameManager.MiniMap.Obstacle.RenderMode = MiniMapLib.ObjectInMap.Obstacles.DisplayRenderMode.SpecificArea;
////GameManager.MiniMap.Setting.IsRender = true;
////GameManager.MiniMap.Setting.CoordinatesInWindow = new(GetLowerX(GameManager.MiniMap.Setting), +GameManager.MiniMap.Setting.GetWindowSize().Y / 2);
////Serializer<MiniMapDTO, MiniMap>.ToJSON(GameManager.MiniMap, PathResolver.GetPath("Resources"));
////Deserializer.FromJSON<MiniMapDTO, MiniMap>(GameManager.MiniMap, PathResolver.GetPath(Path.Combine("Resources", "miniMap_1.json")));
////await Serializer<TexturedWallDTO, TexturedWall>.SerializationJSONAsync(wall, PathResolver.GetPath("Resources"));
////var floor = new Gashrun.TexturedCeiling(PathResolver.GetPath(Path.Combine("Resources", "Image", "PartsWorldTexture", "Stone.png")), PathResolver.GetPath(Path.Combine("Resources", "Shader", "CeilingSetting.glsl")));
////floor.Effect = new CustomEffect(DefaultPresets.IceEffect);
////await Serializer<TexturedCeilingDTO, TexturedCeiling>.ToJSONAsync(floor, PathResolver.GetPath("Resources"));
////Serializer<IMapDTO, IMap>.ToJSON(map, PathResolver.GetPath("Resources"));
////TexturedCeiling c = Deserializer.FromJSON<TexturedCeilingDTO, TexturedCeiling>(PathResolver.GetPath(Path.Combine("Resources", "texturedceiling_1.json")));
////Map map = new Map(10, 10);
////Deserializer.FromJSON<IMapDTO, IMap>(map, PathResolver.GetPath(Path.Combine("Resources", "imap_3.json")));
////TexturedWall? wall1 = Deserializer.DeserializationJSON<TexturedWallDTO, TexturedWall>(PathResolver.GetPath(Path.Combine("Resources", "texturedwall_4.json")));
////if (wall1 is not null)
////    map.AddObstacle(4, 4, wall);

//Unit unit = new Unit(new ObstacleLib.SpriteLib.SpriteObstacle(PathResolver.GetMainPath(Path.Combine("Resources", "Image", "WallTexture", "Wall1.png"))), 100);
//unit.Z.Axis = 0;
////unit.MoveSpeed = 600;
//unit.MinDistanceFromWall = 0;
//unit.HitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(20);
//unit.HitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(20);
//unit.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(20);
//unit.HitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(20);
//unit.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(100);
//unit.HitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(100);
//EffectManager.CurrentEffect = new CustomEffect(DefaultPresets.BlinEffect);
////var sprite = new SpriteObstacle(PathResolver.GetMainPath(Path.Combine("Resources", "Image", "Sprite", "Devil")));
////sprite.Effect = new CustomEffect(DefaultPresets.BlinEffect);
////sprite.Scale = 100;
////sprite.ShiftCubedX = 50;
////sprite.ShiftCubedY = 50;

////map.AddObstacle(5, 5, sprite);
//map.AddObstacle(2, 3, unit);
//ObstacleLib.TexturedWallLib.TexturedWall wall1 = new ObstacleLib.TexturedWallLib.TexturedWall(null, false, PathResolver.GetMainPath(Path.Combine("Resources", "Image", "WallTexture", "Wall1.png")));
//map.AddObstacle(6, 3, wall);

//unit.Map = map;
//Camera.CurrentUnit = unit;
//GameManager.PartsWorld.UpPart = new Sky(PathResolver.GetPath(Path.Combine("Resources", "Image", "PartsWorldTexture", "nightSky.jpg")));
////var r = new TexturedFloor(PathResolver.GetPath(Path.Combine("Resources", "Image", "PartsWorldTexture", "Stone.png")), PathResolver.GetPath(Path.Combine("Resources", "Shader", "FloorSetting.glsl")));
////r.Effect = new CustomEffect(DefaultPresets.BlinEffect);
////GameManager.PartsWorld.DownPart = r;



//string mainBold = Path.Combine("Resources", "FontText", "ArialBold.ttf");
//unit.Control.AddBottomBind(new ButtonBinding(new Button(VirtualKey.None), MoveLib.Angle.MoveAngle.ResetAngle, new object[] { unit }));
//RenderText textMa = new RenderText("", 30, new Vector2f(Screen.ScreenWidth, Screen.ScreenHeight), PathResolver.GetMainPath(mainBold), SFML.Graphics.Color.Yellow);
//UIText uIText = new UIText(textMa, unit);
//uIText.PositionOnScreen = new Vector2f(Screen.GetPercentWidth(2), Screen.GetPercentHeight(2));
//unit.Control.AddBottomBind(new ButtonBinding(new Button(VirtualKey.None), () => { uIText.SetText($"Fps: {FPS.TextFPS}"); }));
//unit.Control.AddBottomBind(new ButtonBinding(new Button(VirtualKey.Q), Screen.Window.Close));
//RenderAlgorithm.UseVerticalPerspective = false;
//RenderAlgorithm.UseHeightPerspective = false;

////ImageLoader.AlphaThreshold = 0;
////ImageLoader.LoadSettingMapping = "RGBA";

//AnimationState a = new AnimationState(null, true, PathResolver.GetPath(Path.Combine("Resources", "UI", "small.gif")))
//{
//    IsAnimation = true,
//    Speed = 30
//};
////Serializer<AnimationStateDTO, AnimationState>.ToJSON(a, PathResolver.GetPath("Resources"));
////_ =  a.AddFramesAsync(UIFramework.Animation.ImageLoader.LoadAsync(PathResolver.GetPath(Path.Combine("Resources", "UI", "small.gif"))));
////new ColorContent(Color.Green)
////AnimationState a = Deserializer.FromJSON<AnimationStateDTO, AnimationState>(PathResolver.GetMainPath(Path.Combine("Resources", "animationS+tate_1.json")));
//FillBar bar = new FillBar(new AnimationContent(a), new ColorContent(SFML.Graphics.Color.Red), unit.Hp, unit)
//{
//    BorderThickness = 10,
//    Width = 400,
//    Height = 100,
//    PositionOnScreen = new Vector2f(0, Screen.ScreenHeight),
//    BorderFillColor = SFML.Graphics.Color.Black,
//};

//ControlLib.Buttons.ButtonBinding shoot = new ControlLib.Buttons.ButtonBinding(new ControlLib.Buttons.Button(VirtualKey.LeftButton), 150);
//ImageLoadOptions opt = new()
//{
//   FrameLoadMode = FrameLoadMode.FullFrame
//};
//UIAnimation gunAnimation = new UIAnimation(opt, PathResolver.GetPath(Path.Combine("Resources", "UI", "pistol.gif")))
//{
//    IsAnimation = true,
//    Speed = 20,
//    PercentShiftX = 6f,
//    PercentShiftY = -38,
//    ScaleY = 1.3f,
//    ScaleX = 1.25f
//};
//UIText textMagazine = new UIText(textMa);
//textMagazine.HorizontalAlignment = HorizontalAlign.Right;
//textMagazine.VerticalAlignment = VerticalAlign.Bottom;


//SpriteObstacle unitBullet = new SpriteObstacle(ImageLoader.Load(null, true, PathResolver.GetMainPath(Path.Combine("Resources", "Image", "Sprite", "Devil"))));
//unitBullet.Scale = 64;

//Unit unitBulletT = new Unit(map, unitBullet, 1);
//unitBulletT.HitBox.MainHitBox.HeightRenderMode = RenderHeightMode.EdgeBased;
//unitBulletT.IsRenderable = false;
//unitBulletT.MinDistanceFromWall = 0;
//unitBulletT.MoveSpeed = 20;
//unitBulletT.Animation.IsAnimation = true;
//unitBulletT.Animation.Speed = 50;

//UnitBullet unitBullet1 = new UnitBullet(50, unitBulletT, null);
//unitBullet1.Unit.Scale = 20;
//Magazine magazine = new(20, 20, unitBullet1, VirtualKey.R, textMagazine);
//Gun gun = new Gun(unit, gunAnimation, magazine, shoot);
//RoundSight roundSight = new(5);
//roundSight.Owner = unit;

//UIShape uIShape = new UIShape(new RectangleShape(new Vector2f(Screen.GetPercentWidth(15), Screen.GetPercentHeight(25))));
//var b = new UIBorder(null, null, "D:\\C++ проекты\\Gashrun\\Resources\\Image\\BorderMiniMap\\Border4.png")
//{
//    Owner = unit
//};
//b.IsAnimation = false;
//b.Speed = 30;
//b.RenderOrder = RenderOrder.Dialog;
////uIShape.Border = b;
////uIShape.VerticalAlignment = VerticalAlign.Center;
////uIShape.HorizontalAlignment = HorizontalAlign.Center;

////uIShape.PositionOnScreen = new Vector2f(300, 300);
////uIShape.Owner = unit;

//ControlLib.Buttons.Button bottomW = new ControlLib.Buttons.Button(VirtualKey.W);
//ControlLib.Buttons.Button bottomS = new ControlLib.Buttons.Button(VirtualKey.S);
//ControlLib.Buttons.Button bottomA = new ControlLib.Buttons.Button(VirtualKey.A);
//ControlLib.Buttons.Button bottomD = new ControlLib.Buttons.Button(VirtualKey.D);
//ControlLib.Buttons.ButtonBinding keyBindingForward = new ControlLib.Buttons.ButtonBinding(bottomW, MovePositions.Move, new object[] { unit, 1, 0 });
//ControlLib.Buttons.ButtonBinding keyBindingBackward = new ControlLib.Buttons.ButtonBinding(bottomS, MovePositions.Move, new object[] { unit, -1, 0 });
//ControlLib.Buttons.ButtonBinding keyBindingLeft = new ControlLib.Buttons.ButtonBinding(bottomA, MovePositions.Move, new object[] { unit, 0, -1 });
//ControlLib.Buttons.ButtonBinding keyBindingRight = new ControlLib.Buttons.ButtonBinding(bottomD, MovePositions.Move, new object[] { unit, 0, 1 });
//ControlLib.Buttons.Button bottomRightArrow = new ControlLib.Buttons.Button(VirtualKey.RightArrow);
//unit.Control.AddBottomBind(keyBindingForward);
//unit.Control.AddBottomBind(keyBindingBackward);
//unit.Control.AddBottomBind(keyBindingLeft);
//unit.Control.AddBottomBind(keyBindingRight);

//Unit dialogSpeaker = new Unit(new SpriteObstacle(PathResolver.GetMainPath(Path.Combine("Resources", "Image", "Sprite", "Devil"))), 100, false);
//dialogSpeaker.HitBox[CoordinatePlane.X, SideSize.Smaller]?.SetOffset(30);
//dialogSpeaker.HitBox[CoordinatePlane.X, SideSize.Larger]?.SetOffset(30);
//dialogSpeaker.HitBox[CoordinatePlane.Y, SideSize.Smaller]?.SetOffset(30);
//dialogSpeaker.HitBox[CoordinatePlane.Y, SideSize.Larger]?.SetOffset(30);
//dialogSpeaker.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.SetOffset(100);
//dialogSpeaker.HitBox[CoordinatePlane.Z, SideSize.Larger]?.SetOffset(100);
//dialogSpeaker.DialogSprite = null;
//dialogSpeaker.DialogSprite = new UISprite(PathResolver.GetMainPath(Path.Combine("Resources", "Image", "Sprite", "Devil", "3.png")));
//dialogSpeaker.DialogSprite.Scale = new Vector2f(0.5f, 0.5f);
//dialogSpeaker.DialogSprite.PositionOnScreen = new Vector2f(Screen.GetPercentWidth(10), Screen.Setting.HalfHeight);
//dialogSpeaker.DialogSprite.VerticalAlignment = VerticalAlign.Center;
//dialogSpeaker.DialogSprite.HorizontalAlignment = HorizontalAlign.Center;
//dialogSpeaker.DialogSprite.RenderOrder = RenderOrder.Dialog;
//dialogSpeaker.DisplayName = new UIText(textMa);
//dialogSpeaker.DisplayName.PositionOnScreen = new Vector2f(Screen.GetPercentWidth(10), Screen.Setting.HalfHeight + dialogSpeaker.DialogSprite.Sprite.GetGlobalBounds().Height / 1.5f);
//dialogSpeaker.DisplayName.SetText("Monster");
//dialogSpeaker.DisplayName.VerticalAlignment = VerticalAlign.Center;
//dialogSpeaker.DisplayName.HorizontalAlignment = HorizontalAlign.Center;
//dialogSpeaker.DisplayName.RenderOrder = RenderOrder.Dialog;
//dialogSpeaker.Scale = 64;

//map.AddObstacle(6, 5, dialogSpeaker);
//dialogSpeaker.Map = map;


//UIButton buttonFirst = new UIButton(new Vector2f(Screen.GetPercentWidth(30), Screen.GetPercentHeight(50)), new Vector2f(Screen.GetPercentWidth(15), Screen.GetPercentHeight(6)), "First button", PathResolver.GetPath(mainBold));
//buttonFirst.Shape.Border = b;

//buttonFirst.Shape.VerticalAlignment = VerticalAlign.Center;
//buttonFirst.Shape.HorizontalAlignment = HorizontalAlign.Left;
//buttonFirst.Text.VerticalAlignment = VerticalAlign.Center;
//buttonFirst.Text.HorizontalAlignment = HorizontalAlign.Center;
//buttonFirst.CenterTextInShape();
//buttonFirst.Shape.RectangleShape.FillColor = SFML.Graphics.Color.Blue;

//UIButton buttonSecond = new UIButton(new Vector2f(Screen.GetPercentWidth(30), Screen.GetPercentHeight(58)), new Vector2f(Screen.GetPercentWidth(15), Screen.GetPercentHeight(4)), "Second button", PathResolver.GetPath(mainBold));

////UIButton buttonFirst1 = new UIButton(new Vector2f(300, 300), new Vector2f(150, 25), "First1 button", PathResolver.GetPath(mainBold));
////UIButton buttonSecond1 = new UIButton(new Vector2f(300, 350), new Vector2f(150, 25), "Second1 button", PathResolver.GetPath(mainBold));

//RectangleShape ShapeBackround = new RectangleShape(new SFML.System.Vector2f(Screen.ScreenWidth, Screen.ScreenHeight))
//{
//    Position = new SFML.System.Vector2f(Screen.ScreenWidth / 2, Screen.ScreenHeight / 2),
//    //FillColor = new SFML.Graphics.Color(0, 0, 0, 255),
//};
//DialogueSession dialogManager = new DialogueSession(dialogSpeaker, unit, ShapeBackround);
//_ = dialogManager.BackgroundShape.AnimationState.AddFramesAsync(ImageLoader.LoadAsync(null, true, PathResolver.GetMainPath(Path.Combine("Resources", "video.gif"))));
//dialogManager.BackgroundShape.AnimationState.IsAnimation = true;
//dialogManager.BackgroundShape.AnimationState.Speed = 20;

//dialogManager.AddLevel(0, new Dictionary<int, DialogueStep>()
//{
//    { 0, new DialogueStep(textMa,new Dictionary<int, UIButton>() { { 1, buttonFirst }, { 2, buttonSecond } }) }
//});


//var bd = new TriggerButton(new ButtonBinding(new ControlLib.Buttons.Button(VirtualKey.E), () => { }), null, null);
//FadingText fadingText = new FadingText(textMa, FadingType.Appears, FadingTextLife.OneShotFreeze, 2000, null);
//fadingText.RenderText.Text.DisplayedString = "Press E";
//fadingText.PositionOnScreen = new Vector2f(Screen.GetPercentWidth(75), Screen.GetPercentHeight(75));
//var dd = new TriggerDistance(1,
//    (owner) => { fadingText.Controller.FadingType = FadingType.Appears; fadingText.Controller.FadingTextLife = FadingTextLife.OneShotFreeze; fadingText.Owner = owner; },
//    (owner) => { fadingText.SwapType(); fadingText.Controller.FadingTextLife = FadingTextLife.OneShotDispose; }, typeof(IDialogObject));
//TriggerAnd triggerAnd = new TriggerAnd(dd, bd);
//triggerAnd.OnTriggered = (unit) =>
//{
//    var npc = dd.GetTarget();
//    if (npc != null && !(typeof(IDialogObject).IsAssignableFrom(npc?.GetType())))
//        map.DeleteObstacle(npc);
//    else if (npc != null && (typeof(IDialogObject).IsAssignableFrom(npc?.GetType())))
//    {
//        _ = dialogManager.RunAsync();
//        dd.isSwap = true;
//    }
//};
//TriggerHandler.AddTriger(unit, triggerAnd);
//GameManager.Start();
////while (true)
////{
////    if(a.GetFrame(0) is not null && TextureDataCache.ContainsKey(a.GetFrame(0).PathTexture))
////    {
////        Serializer<AnimationStateDTO, AnimationState>.ToJSON(a, PathResolver.GetPath("Resources"));
////        break;
////    }
////}
