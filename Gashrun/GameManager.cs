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
using ObjectFramework;
using UIFramework;
using MiniMapLib.ObjectInMap.Obstacles;
using MiniMapLib.ObjectInMap.Player;
using MiniMapLib.Window;
using ProtoRender.Map;
using ScreenLib.Output;
using MiniMapLib.ObjectInMap.Positions;
using SFML.Graphics;
using SFML.Window;
using DataPipes.DTO.Converters;
using MiniMapLib.DTO;
using DataPipes.DTO.Register;
using DataPipes.DTO;
using System.Text.Json.Serialization;
using System.Text.Json;
using MiniMapLib.Setting;
using HitBoxLib.Operations;
using static System.Net.Mime.MediaTypeNames;

namespace Gashrun;
public static class GameManager
{
    private static bool hasStarted = false;
    public static Action? DeferredAction;

    private static readonly Queue<Action> mainThreadActions = new();
    public static RenderPartsWorld PartsWorld { get; private set; } = new RenderPartsWorld();
    public static MiniMap MiniMap { get; private set; } = Deserializer.FromJson<MiniMapDTO, MiniMap>(PathResolver.GetPath(@"Resources\miniMap_3.json"));//  new MiniMap(PathResolver.GetPath(@"Resources\Image\BorderMiniMap\Border.png"));

    public static void RunOnMainThread(Action action)
    {
        lock (mainThreadActions)
        {
            mainThreadActions.Enqueue(action);
        }
    }

    private static void ExecuteMainThreadActions()
    {
        lock (mainThreadActions)
        {
            while (mainThreadActions.Count > 0)
            {
                var action = mainThreadActions.Dequeue();
                action?.Invoke();
            }
        }
    }

    public static void Start()
    {
        if (hasStarted)
        {
            Console.WriteLine("GameManager.Start() has already been called.");
            return;
        }
        hasStarted = true;
        
        try
        {
            VisualizerHitBox.VisualizerType = VisualizerHitBoxType.VisualizeAll;
            while (Screen.Window.IsOpen)
            {
                if (DeferredAction is not null)
                {
                    var temp = DeferredAction;
                    DeferredAction = null;
                    temp.Invoke();
                }


                //------------Update User Info------------
                FPS.Track();
                Camera.UpdateListener();

                //------------Clear Screen------------
                Screen.Window.DispatchEvents();
                Screen.Window.Clear();
              
                ExecuteMainThreadActions();
                //------------Controll------------
                Camera.CurrentUnit?.Control.MakePressedParallel(Camera.CurrentUnit);

                //------------Render------------
                DrawingQueue.ExecuteAll();
                WriteQueue.ExecuteAll();

                if (Camera.CurrentUnit is not null && Camera.CurrentUnit.Map is not null)
                {
                    RenderAlgorithm.CalculationAlgorithm(Camera.CurrentUnit);

                    VisualizerHitBox.Render(Camera.CurrentUnit);

                    PartsWorld.Render(Camera.CurrentUnit);

                    BeyondRenderManager.Render(Camera.CurrentUnit);

                    if (Camera.CurrentUnit?.Map is not null)
                     MiniMap.Render(Camera.CurrentUnit.Map, Camera.CurrentUnit);
                }



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

