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
using UIFramework;

Screen.Initialize(1000, 600);

DateTime from = DateTime.Now;
string mainBold = Path.Combine("Resources", "FontText", "ArialBold.ttf");
FPS fpsChecker = new FPS("FPS: ", 24, new Vector2f(10, 10), ResourceManager.GetMainPath(mainBold), Color.White);


string mainFillWall = Path.Combine("Resources", "Image", "WallTexture", "Wall1.png");
Map map = new(new TexturedWall(ResourceManager.GetMainPath(mainFillWall)), 10, 10);
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
Bottom bottomLeftMouse = new Bottom(VirtualKey.LeftButton);
Bottom bottomC = new Bottom(VirtualKey.C);
Bottom bottomCtrl = new Bottom(VirtualKey.LeftControl);
List<Bottom> controls = new List<Bottom>() { bottomC, bottomCtrl };
ControlLib.BottomBinding keyBindingHideMap = new ControlLib.BottomBinding(controls, miniMap.Hide, 350);
ControlLib.BottomBinding keyBindingForward = new ControlLib.BottomBinding(bottomW, MovePositions.Move, new object[] { map, player, 1, 0 });
ControlLib.BottomBinding keyBindingBackward = new ControlLib.BottomBinding(bottomS, MovePositions.Move, new object[] { map, player, -1, 0 });
ControlLib.BottomBinding keyBindingLeft = new ControlLib.BottomBinding(bottomA, MovePositions.Move, new object[] { map, player, 0, -1 });
ControlLib.BottomBinding keyBindingRight = new ControlLib.BottomBinding(bottomD, MovePositions.Move, new object[] { map, player, 0, 1 });
control.AddBottomBind(new BottomBinding(new Bottom(VirtualKey.Q), Screen.Window.Close));
control.AddBottomBind(new ControlLib.BottomBinding(new Bottom(VirtualKey.None), MoveAngle.ResetAngle, new object[] { player }));
control.AddBottomBind(keyBindingForward);
control.AddBottomBind(keyBindingBackward);
control.AddBottomBind(keyBindingLeft);
control.AddBottomBind(keyBindingRight);
control.AddBottomBind(keyBindingHideMap);

BottomBinding shoot = new ControlLib.BottomBinding(new List<Bottom>(){ bottomLeftMouse }, DrawLib.Drawing.DrawingPoint, 350, new object[] { map, player, 30, Color.Red });
control.AddBottomBind(shoot);
player.OnControlAction += control.MakePressed;


string mainTextureSky = Path.Combine("Resources", "Image", "PartsWorldTexture", "SeamlessSky.jpg");

Sky sky = new Sky(ResourceManager.GetPath(mainTextureSky));
Floor floor = new Floor();
RenderPartsWorld renderPartsWorld = new RenderPartsWorld(sky, floor);
//230 170
Texture tex = new Texture(ResourceManager.GetPath(Path.Combine("Resources", "UI", "pistol.gif")));
UIElement uIElement = new Gun(shoot, ResourceManager.GetPath(Path.Combine("Resources", "UI", "pistol.gif")))
{
    IsAnimation = true,
    SpeedAnimation = 5,
    PercentShiftX = 7,
    PercentShiftY = -38,
    ScaleY = 1.3f,
    ScaleX = 1.25f
};
RenderLib.HitBox.VisualizerHitBox.VisualizerType = RenderLib.HitBox.VisualizerHitBoxType.VisualizeRayRenderable;
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
        UIElement.RenderUIs();
        //algorithm.CalculationAlgorithm();
        RenderLib.HitBox.VisualizerHitBox.Render(map, player);
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
        CircleShape point = new CircleShape(3)
        {
            FillColor = Color.Red,
            Position = new Vector2f(Screen.Setting.HalfWidth, Screen.Setting.HalfHeight)
        };

        Screen.Window.Draw(point);
        Screen.Window.Display();
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}