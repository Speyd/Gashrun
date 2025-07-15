using UIFramework;
using ControlLib.Buttons;
using UIFramework.Text;
using SFML.System;
using FpsLib;
using MoveLib.Move;
using UIFramework.Render;
using UIFramework.Text.AlignEnums;
using UIFramework.Windows.Button;
using UIFramework.Animation;
using TextureLib.Loader.LoaderMode;
using UIFramework.Shape;
using TextureLib.Loader.ImageProcessing;
using ScreenLib;
using TextureLib.Loader;
using ObjectFramework;
using SFML.Graphics;
using ObstacleLib.TexturedWallLib;
using ProtoRender.Object;
using PartsWorldLib.Up;
using PartsWorldLib.Down;
using InteractionFramework.HitAction;
using InteractionFramework.VisualImpact.Data;
using InteractionFramework.Audio.SoundType;
using InteractionFramework.HitAction.DrawableBatch;
using DataPipes;
using AnimationLib;
using UIFramework.Weapon;
using UIFramework.Weapon.BulletMagazine;
using UIFramework.Weapon.Bullets.Variants;
using EffectLib.EffectCore;
using ObstacleLib;
using ProtoRender.RenderAlgorithm;
using TextureLib.Textures;
using ProtoRender.RenderInterface;
using System.Collections.Concurrent;
using ObstacleLib.SpriteLib;
using UIFramework.Sprite;


namespace Gashrun;
public static class GameInitializer
{
    public static UIAnimation ProgressAnimation { get; private set; }
    public static Menu MainMenu { get; private set; }
    public static UIText FpsText { get; private set; }


    private static Action OnLoadCompleted;

    private static bool _isLoaded = false;
    public static bool IsLoaded 
    {
        get => _isLoaded;
        private set
        {
            _isLoaded = value;
            if (value)
                OnLoadCompleted.Invoke();
        }
    }

    public static async Task InitializeAsync()
    {
        LoadCameraUnit();
        LoadResources();

        await Task.Run(() =>
        {
            ShowProgress(true);

            InitializeUI();

            MainMenu.Run();

            InitializeHitDataCache();

            InitializePartsWorld();

            InitializeControls();

            InitializeGun();

            ShowProgress(false);
        });
    }

    private static void ShowProgress(bool isVisible)
    {
        if (ProgressAnimation != null)
            ProgressAnimation.IsHide = !isVisible;
    }

    private static void LoadResources()
    {
        ImageLoadOptions imageLoadOptions = new ImageLoadOptions() { FrameLoadMode = FrameLoadMode.FullFrame };

        ProgressAnimation = new UIAnimation(new Vector2f(Screen.GetPercentWidth(95), Screen.GetPercentHeight(95)));
        ProgressAnimation.AddFrames(ImageLoader.Load(imageLoadOptions, true, PathResolver.GetMainPath(Path.Combine("Resources", "output.gif"))));
        ProgressAnimation.Owner = Camera.CurrentUnit;
        ProgressAnimation.IsHide = true;
        ProgressAnimation.IsAnimation = true;
        ProgressAnimation.Speed = 25;
        ProgressAnimation.ScaleX = 0.1f;
        ProgressAnimation.ScaleY = 0.1f;
        ProgressAnimation.RenderOrder = RenderOrder.SystemNotification;
    }

    private static void LoadCameraUnit()
    {
        Unit unit = new Unit(new ObstacleLib.SpriteLib.SpriteObstacle(PathResolver.GetMainPath(Path.Combine("Resources", "Image", "WallTexture", "Wall1.png"))), 100);
        Camera.CurrentUnit = unit;
    }



    private static void InitializeHitDataCache()
    {
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
        HitDataCache.Load(PathResolver.GetMainPath(Path.Combine("Resources", "Image", "WallTexture", "Wall1.png")), hitBrickWall);
    }

    private static void InitializeGun()
    {
        string fontArialBold = PathResolver.GetMainPath(Path.Combine("Resources", "FontText", "ArialBold.ttf"));

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
        pistolAnimation.RenderOrder = RenderOrder.SystemNotification;

        UIText pistolBulletText = new UIText("", 30, new Vector2f(0, 0), fontArialBold, SFML.Graphics.Color.Cyan, Camera.CurrentUnit);
        pistolBulletText.PositionOnScreen = new Vector2f(Screen.GetPercentWidth(95), Screen.GetPercentHeight(98));
        pistolBulletText.VerticalAlignment = VerticalAlign.Center;
        pistolBulletText.HorizontalAlignment = HorizontalAlign.Center;
        StandartBullet pistolBullet = new StandartBullet(20, null);
        Magazine pistolMagazine = new Magazine(20, 140, pistolBullet, VirtualKey.R, pistolBulletText);

        ControlLib.Buttons.ButtonBinding shootPistol = new ControlLib.Buttons.ButtonBinding(new ControlLib.Buttons.Button(VirtualKey.LeftButton), 300);
        Gun pistol = new Gun(Camera.CurrentUnit, pistolAnimation, pistolMagazine, shootPistol);
        pistol.Animation.IsHide = true;
        pistol.Magazine.UIText.IsHide = true;

        OnLoadCompleted += () => { pistol.Magazine.UIText.IsHide = false; };
        OnLoadCompleted += () => { pistol.Animation.IsHide = false; };
        #endregion
    }

    private static void InitializePartsWorld()
    {
        GameManager.PartsWorld.UpPart = new Sky(PathResolver.GetMainPath(Path.Combine("Resources", "Image", "PartsWorldTexture", "nightSky.jpg")));
        var floor = new TexturedFloor(PathResolver.GetMainPath(Path.Combine("Resources", "Image", "PartsWorldTexture", "Stone.png")),
                                                    PathResolver.GetMainPath(Path.Combine("Resources", "Shader", "FloorSetting.glsl")));
        floor.DownAngleLogScale = 2.7f;
        GameManager.PartsWorld.DownPart = floor;

    }

    private static void InitializeUI()
    {
        string fontArialBold = PathResolver.GetMainPath(Path.Combine("Resources", "FontText", "ArialBold.ttf"));

        FpsText = new UIText("", 30, new Vector2f(0, 0), fontArialBold, SFML.Graphics.Color.Cyan, Camera.CurrentUnit);
        FpsText.IsHide = true;

        MainMenu = new Menu(Camera.CurrentUnit);

        #region Setting Background Menu
        var backgroundShape = new RectangleShape(new SFML.System.Vector2f(Screen.ScreenWidth, Screen.ScreenHeight))
        {
            Position = new SFML.System.Vector2f(Screen.ScreenWidth / 2, Screen.ScreenHeight / 2),
        };
        UIShape backgroundMenu = new UIShape(backgroundShape)
        {
            HorizontalAlignment = HorizontalAlign.Center,
            VerticalAlignment = VerticalAlign.Center,
            RenderOrder = RenderOrder.Background
        };
        backgroundMenu.AnimationState.AddFrames(ImageLoader.Load(null, true, PathResolver.GetMainPath(Path.Combine("Resources", "Game.gif"))));
        backgroundMenu.AnimationState.IsAnimation = true;
        backgroundMenu.AnimationState.Speed = 30;

        MainMenu.SetBackground(backgroundMenu);
        #endregion

        Vector2f textGamePos = new Vector2f(Screen.GetPercentWidth(50), Screen.GetPercentHeight(20));
        UISprite uISprite = new UISprite(PathResolver.GetMainPath(Path.Combine("Resources", "textGameMenu.png")));
        uISprite.PositionOnScreen = textGamePos;

        uISprite.HorizontalAlignment = HorizontalAlign.Center;
        uISprite.VerticalAlignment = VerticalAlign.Center;

        MainMenu.AddSprites(uISprite);

        #region Button "Press to Start"
        Vector2f buttonPos = new Vector2f(Screen.GetPercentWidth(50), Screen.GetPercentHeight(50));
        UIButton startGameButton = new UIButton(buttonPos, new Vector2f(150, 35), "Press to Start", fontArialBold, SFML.Graphics.Color.Black)
        {
            HorizontalAlignment = HorizontalAlign.Center,
            VerticalAlignment = VerticalAlign.Center,
            RenderOrder = RenderOrder.Dialog,
            ClickMode = ButtonClickMode.ClickOnly,
        };

        startGameButton.Text.HorizontalAlignment = HorizontalAlign.Center;
        startGameButton.Text.VerticalAlignment = VerticalAlign.Center;

        startGameButton.OnClick += MainMenu.Stop;
        startGameButton.OnClick += async () => await LoadMapAsync();
        MainMenu.AddButton(startGameButton);
        #endregion
    }

    private static void InitializeControls()
    {
        ControlLib.Buttons.Button W = new ControlLib.Buttons.Button(VirtualKey.W);
        ControlLib.Buttons.Button S = new ControlLib.Buttons.Button(VirtualKey.S);
        ControlLib.Buttons.Button A = new ControlLib.Buttons.Button(VirtualKey.A);
        ControlLib.Buttons.Button D = new ControlLib.Buttons.Button(VirtualKey.D);
        ControlLib.Buttons.Button Q = new ControlLib.Buttons.Button(VirtualKey.Q);
        ControlLib.Buttons.Button F6 = new ControlLib.Buttons.Button(VirtualKey.F6);
        ControlLib.Buttons.Button LeftArrow = new ControlLib.Buttons.Button(VirtualKey.LeftArrow);

        ControlLib.Buttons.ButtonBinding forward = new ControlLib.Buttons.ButtonBinding(W, MovePositions.Move, new object[] { Camera.CurrentUnit, 1, 0 });
        ControlLib.Buttons.ButtonBinding backward = new ControlLib.Buttons.ButtonBinding(S, MovePositions.Move, new object[] { Camera.CurrentUnit, -1, 0 });
        ControlLib.Buttons.ButtonBinding left = new ControlLib.Buttons.ButtonBinding(A, MovePositions.Move, new object[] { Camera.CurrentUnit, 0, -1 });
        ControlLib.Buttons.ButtonBinding right = new ControlLib.Buttons.ButtonBinding(D, MovePositions.Move, new object[] { Camera.CurrentUnit, 0, 1 });

        Camera.CurrentUnit?.Control.AddBottomBind(new ButtonBinding(new Button(VirtualKey.None), () => { FpsText.SetText($"Fps: {FPS.TextFPS}"); }));
        Camera.CurrentUnit?.Control.AddBottomBind(new ButtonBinding(F6, () => { FpsText.IsHide = !FpsText.IsHide; }, 500));
        Camera.CurrentUnit?.Control.AddBottomBind(new ButtonBinding(Q, Screen.Window.Close));
        Camera.CurrentUnit?.Control.AddBottomBind(new ButtonBinding(new Button(VirtualKey.None), MoveLib.Angle.MoveAngle.ResetAngle, new object[] { Camera.CurrentUnit }));
        Camera.CurrentUnit?.Control.AddBottomBind(forward);
      
        Camera.CurrentUnit?.Control.AddBottomBind(backward);
        Camera.CurrentUnit?.Control.AddBottomBind(left);
        Camera.CurrentUnit?.Control.AddBottomBind(right);
    }

    private static async Task LoadMapAsync()
    {
        ShowProgress(true);

        if (Camera.CurrentUnit != null)
        {
            Camera.CurrentUnit.Control.FreezeControlsKey = true;
            Camera.CurrentUnit.Control.FreezeControlsMouse = true;
        }

        await Task.Run(() =>
        {
            TexturedWall boundaryWall = new TexturedWall(null, false, PathResolver.GetMainPath(Path.Combine("Resources", "Image", "WallTexture", "Wall1.png")));
            Map map = new Map(boundaryWall, 50, 50);
            Camera.map = map;

            if (Camera.CurrentUnit != null)
                map.AddObstacle(5, 5, Camera.CurrentUnit);

            SpriteObstacle spriteObstacle = new SpriteObstacle(PathResolver.GetMainPath(Path.Combine("Resources", "Image", "Sprite", "Devil")));
            spriteObstacle.Scale = 80;
            spriteObstacle.ShiftCubedX = 50;
            spriteObstacle.ShiftCubedY = 50;

            ImageLoadOptions spriteObstacle1Opt = new ImageLoadOptions() { FrameLoadMode = FrameLoadMode.FullFrame };
            SpriteObstacle spriteObstacle1 = new SpriteObstacle(PathResolver.GetMainPath(Path.Combine("Resources", "Image", "Sprite", "Barel")), spriteObstacle1Opt);
            spriteObstacle1.Scale = 80;
            spriteObstacle1.Animation.IsAnimation = true;
            spriteObstacle1.Animation.Speed = 30;
            spriteObstacle1.Z.Axis = -200;
            spriteObstacle1.ShiftCubedX = 20;
            spriteObstacle1.ShiftCubedY = 20;

            ImageLoadOptions flame1Opt = new ImageLoadOptions() { FrameLoadMode = FrameLoadMode.FullFrame };
            SpriteObstacle flame1 = new SpriteObstacle(PathResolver.GetMainPath(Path.Combine("Resources", "Image", "Sprite", "Flame")), flame1Opt);
            flame1.Animation.RectMode = FrameRectMode.UseCurrentFrameRect;
            flame1.Scale = 35;
            flame1.Animation.IsAnimation = true;
            flame1.Animation.Speed = 30;
            flame1.Z.Axis = -350;
            flame1.ShiftCubedX = 20;
            flame1.ShiftCubedY = 60;

            ImageLoadOptions flame2Opt = new ImageLoadOptions() { FrameLoadMode = FrameLoadMode.FullFrame };
            SpriteObstacle flame2 = new SpriteObstacle(PathResolver.GetMainPath(Path.Combine("Resources", "Image", "Sprite", "Flame")), flame2Opt);
            flame2.Animation.RectMode = FrameRectMode.UseCurrentFrameRect;
            flame2.Scale = 35;
            flame2.Animation.IsAnimation = true;
            flame2.Animation.Speed = 30;
            flame2.Z.Axis = -350;
            flame2.ShiftCubedX = 60;
            flame2.ShiftCubedY = 20;

            map.AddObstacle(3, 3, spriteObstacle);
            map.AddObstacle(1, 1, spriteObstacle1);
            map.AddObstacle(1, 1, flame1);
            map.AddObstacle(1, 1, flame2);


        });

        if (Camera.CurrentUnit != null)
        {
            Camera.CurrentUnit.Control.FreezeControlsKey = false;
            Camera.CurrentUnit.Control.FreezeControlsMouse = false;
        }

        ShowProgress(false);
        IsLoaded = true;
    }
}