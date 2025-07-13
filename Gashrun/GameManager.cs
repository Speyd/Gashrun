using InteractionFramework;
using ProtoRender.RenderAlgorithm;
using UIFramework.Text;
using RenderLib.Algorithm;
using RenderLib.HitBox;
using ScreenLib;
using PartsWorldLib;
using MiniMapLib;
using UIFramework.Render;
using InteractionFramework.VisualImpact;
using FpsLib;
using TextureLib.Loader;
using DataPipes.Pool;
using HitBoxLib.PositionObject;
using HitBoxLib.Segment.SignsTypeSide;
using ProtoRender.Object;
using ProtoRender.RenderInterface;
using SFML.System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.InteropServices;
using ObstacleLib.TexturedWallLib;
using System.Numerics;

namespace Gashrun;
public static class GameManager
{
    private static bool hasStarted = false;

    public static RenderPartsWorld PartsWorld { get; private set; } = new RenderPartsWorld();
    public static MiniMap MiniMap { get; private set; } = new MiniMap(PathResolver.GetPath(@"Resources\Image\BorderMiniMap\Border.png"));

    public static void Start()
    {
        if (hasStarted)
        {
            Console.WriteLine("GameManager.Start() has already been called.");
            return;
        }

        hasStarted = true;
        VisualizerHitBox.VisualizerType = VisualizerHitBoxType.None;
        try
        {
            while (Screen.Window.IsOpen)
            {
                //------------Update User Info------------
                FPS.Track();
                Camera.UpdateListener();

                //------------Clear Screen------------
                Screen.Window.DispatchEvents();
                Screen.Window.Clear();


                //------------Controll------------
                Camera.CurrentUnit?.Control.MakePressedParallel(Camera.CurrentUnit);


                //------------Render------------
                DrawingQueue.ExecuteAll();
                WriteQueue.ExecuteAll();

                if (Camera.CurrentUnit is not null)
                {
                    RenderAlgorithm.CalculationAlgorithm(Camera.CurrentUnit);

                    VisualizerHitBox.Render(Camera.CurrentUnit);

                    PartsWorld.Render(Camera.CurrentUnit);

                    BeyondRenderManager.Render(Camera.CurrentUnit);

                }
                //if (Camera.CurrentUnit?.Map is not null)
                   // MiniMap.Render(Camera.CurrentUnit.Map, Camera.CurrentUnit);



                UIRender.DrawingByPriority();
                Screen.OutputPriority?.DrawingByPriority();

                Screen.Window.Display();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }
    }
}