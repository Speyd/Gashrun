﻿using InteractionFramework;
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

                if (Camera.CurrentUnit is not null && Camera.map is not null && Camera.map.IsLoad)
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

public static class RenderAlgorithm
{
    //------------------------------Pool-------------------------------
    static ObjectPool<Result> resultobjectPool = new ObjectPool<Result>();
    static ListPool<InfoObject> infoObjectPool = new ListPool<InfoObject>();

    //------------------------------Setting Render-------------------------------
    /// <summary> List of unique types of objects that render themselves (not with rays) </summary>
    private static ConcurrentDictionary<IUnit, ConcurrentDictionary<Type, bool>> UniqueSelfDrawableTypes { get; } = new();
    /// <summary> List with rendering methods of objects that render themselves (not rays) </summary>
    private static ConcurrentDictionary<IUnit, ConcurrentDictionary<Type, Action<Result, IUnit>>> CachedDelegates { get; } = new();

    /// <summary> Flag to check if a new object type that renders itself has been added </summary>
    private static ConcurrentDictionary<IUnit, bool> HasNewTypes { get; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether height perspective should be used during rendering.
    /// When set to <c>true</c>, the algorithm will take the height of objects into account when determining visibility and rendering them.
    /// </summary>
    public static bool UseHeightPerspective { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether vertical perspective should be used during rendering.
    /// When set to <c>true</c>, the algorithm will consider vertical perspective when rendering objects, adjusting their visibility and positioning accordingly.
    /// </summary>
    public static bool UseVerticalPerspective { get; set; } = true;

    /// <summary> Prepares objects that render themselves and their corresponding render methods. </summary>
    public static void PrepareRenderObjects(IUnit unit)
    {
        if (!UniqueSelfDrawableTypes.TryGetValue(unit, out var types) || types is null)
            return;

        if (!CachedDelegates.TryGetValue(unit, out var delegates))
        {
            delegates = new ConcurrentDictionary<Type, Action<Result, IUnit>>();
            CachedDelegates[unit] = delegates;
        }

        foreach (var type in types.Keys)
        {
            if (!delegates.TryGetValue(type, out var del))
            {
                var method = type.GetMethod(ISelfRenderable.NameRenderFun,
                                            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (method != null)
                {
                    del = (Action<Result, IUnit>)Delegate.CreateDelegate(typeof(Action<Result, IUnit>), method);
                    delegates[type] = del;
                }
            }

            del?.Invoke(new Result(), unit);
        }
    }

    /// <summary> Processes obstacles in the map for rendering. </summary>
    private static bool ObjectIdentifier(IUnit unit, List<InfoObject> infoObject,
     double x, double y,
     int mappedX, int mappedY,
     double depth_h, double depth_v,
     double auxiliary, bool isVertical)
    {
        try
        {
            bool isAdded = false;

            if (unit.Map is null || !unit.Map.Obstacles.TryGetValue((mappedX, mappedY), out var obstacles))
                return false;

            double coordinate = isVertical ? y : x;
            double depth = isVertical ? depth_v : depth_h;
            if (!HasNewTypes.TryGetValue(unit, out var hasNew))
                hasNew = false;

            foreach (var obstacle in obstacles.Keys)
            {
                if (obstacle == null)
                    continue;

                switch (obstacle)
                {
                    case ISelfRenderable self:
                        if (!UniqueSelfDrawableTypes.ContainsKey(unit))
                            UniqueSelfDrawableTypes[unit] = new ConcurrentDictionary<Type, bool>();

                        if (!HasNewTypes.ContainsKey(unit))
                            HasNewTypes[unit] = false;

                        self.ProcessForRendering(UniqueSelfDrawableTypes[unit], ref hasNew);
                        break;

                    case IRayRenderable ray:
                        ray.ProcessForRendering(infoObject, coordinate, depth, unit.MaxRenderTile);
                        isAdded = true;
                        break;

                    default:
                        throw new Exception($"Invalid object for rendering (CheckAndAddObstacle)\nType: {obstacle.GetType()}");
                }
            }

            HasNewTypes[unit] = hasNew;
            if (hasNew && HasNewTypes.TryGetValue(unit, out var _))
            {
                PrepareRenderObjects(unit);
                HasNewTypes[unit] = false;
            }
            return isAdded;
        }
        catch (TargetInvocationException tex)
        {
            Console.WriteLine($"Invocation error: {tex.InnerException?.Message}");
            Console.WriteLine($"Inner stack: {tex.InnerException?.StackTrace}");
        }

        return false;
    }

    /// <summary> Filters visible obstacles by height and range for rendering. </summary>
    private static List<InfoObject> FilterVisibleObstacles(List<InfoObject> info,
        bool useHeightPerspective, bool useVerticalPerspective)
    {
        int count = info.Count;

        if (count <= 1) return info;
        if (!useHeightPerspective && count == 2)
        {
            var first = info[0];
            var second = info[1];

            return new List<InfoObject> { first.depth < second.depth ? first : second };
        }

        var span = CollectionsMarshal.AsSpan(info);
        span.Sort((a, b) => a.depth.CompareTo(b.depth));

        var filtered = new List<InfoObject>(count);
        InfoObject? current = null;
        foreach (var item in span)
        {
            if (current == null)
            {
                filtered.Add(item);
                current = item;
                continue;
            }

            if (!useVerticalPerspective && item.depth > current.depth)
            {
                bool isLower = item.Object?.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.Side
                               < current?.Object?.HitBox[CoordinatePlane.Z, SideSize.Smaller]?.Side;
                bool isHigher = item.Object?.HitBox[CoordinatePlane.Z, SideSize.Larger]?.Side
                                > current?.Object?.HitBox[CoordinatePlane.Z, SideSize.Larger]?.Side;

                if (isLower || isHigher)
                {
                    filtered.Add(item);
                    current = item;
                }
            }
            else if (useVerticalPerspective && item.depth > current.depth)
            {
                filtered.Add(item);
                current = item;
            }
        }

        return filtered;
    }

    /// <summary> Determines in which axis the ray should move based on the calculated angle. </summary>
    private static void CheckVericals(ref double coordinate, ref double auxiliaryA, double mapCoordinate, double ratio)
    {
        if (ratio >= 0)
        {
            coordinate = mapCoordinate + Screen.Setting.Tile;
            auxiliaryA = 1;
        }
        else
        {
            coordinate = mapCoordinate;
            auxiliaryA = -1;
        }
    }

    /// <summary> Renders objects visible by rays using the sorted obstacle data. </summary>
    private static void RenderRayObstacles(IUnit unit, int ray, double carAngleRay, List<InfoObject> InfoObject, Result ParallelResult)
    {
        var visibleObstacles = FilterVisibleObstacles(InfoObject, UseHeightPerspective, UseVerticalPerspective);

        int sizeVisibleObst = visibleObstacles.Count;
        for (int obst = 0; obst < sizeVisibleObst; obst++)
        {
            if (obst > 0 && sizeVisibleObst > 1)
                ParallelResult.PositionPreviousObject = visibleObstacles[obst - 1].Object?.GetPositionOnScreen(ParallelResult, unit);

            ParallelResult.CalculationSettingRender(unit, ray, visibleObstacles[obst].depth, visibleObstacles[obst].coordinate, carAngleRay);
            visibleObstacles[obst].Object?.Render(ParallelResult, unit);
        }
    }

    /// <summary> Main rendering algorithm using raycasting and Bresenham's algorithm. </summary>
    public static void CalculationAlgorithm(IUnit unit)
    {
        if (unit.Map is null)
            return;

        double carAngle = unit.Angle - unit.HalfFov;
        Vector2i coordinates = Screen.MappingVector(unit.X.Axis, unit.Y.Axis);

        double unitX = unit.X.Axis;
        double unitY = unit.Y.Axis;
        double unitDeltaAngle = unit.DeltaAngle;
        double unitMaxRenderTile = unit.MaxRenderTile;
        int tile = Screen.Setting.Tile;

        Parallel.For(0, Screen.Setting.AmountRays, Screen.Setting.ParallelOptions, ray =>
        {
            var ParallelResult = resultobjectPool.Get();
            var ParallelInfoObj = infoObjectPool.Get();

            double hx = 0, x = 0, auxiliaryX = 0, depth_h = 0;
            double vy = 0, y = 0, auxiliaryY = 0, depth_v = 0;

            double carAngleRay = carAngle + ray * unitDeltaAngle;
            double sinA = Math.Sin(carAngleRay);
            double cosA = Math.Cos(carAngleRay);
            ParallelResult.SinCarAngle = sinA;
            ParallelResult.CosCarAngle = cosA;

            CheckVericals(ref x, ref auxiliaryX, coordinates.X, cosA);
            for (int j = 0; j < unitMaxRenderTile; j += tile)
            {
                depth_v = (x - unitX) / cosA;
                vy = unitY + depth_v * sinA;

                (int, int) mappX = Screen.Mapping(x + auxiliaryX, vy);
                if (unit.Map.CheckTrueCoordinates(mappX))
                {
                    if (ObjectIdentifier(unit, ParallelInfoObj, x, vy, mappX.Item1, mappX.Item2, depth_h, depth_v, auxiliaryX, true) && !UseHeightPerspective)
                        break;
                }
                else
                    break;

                x += auxiliaryX * tile;
            };

            CheckVericals(ref y, ref auxiliaryY, coordinates.Y, sinA);
            for (int j = 0; j < unitMaxRenderTile; j += tile)
            {
                depth_h = (y - unitY) / sinA;
                hx = unitX + depth_h * cosA;

                (int, int) mappY = Screen.Mapping(hx, y + auxiliaryY);
                if (unit.Map.CheckTrueCoordinates(Screen.Mapping(hx, y + auxiliaryY)))
                {
                    if (ObjectIdentifier(unit, ParallelInfoObj, hx, y, mappY.Item1, mappY.Item2, depth_h, depth_v, auxiliaryY, false) && !UseHeightPerspective)
                        break;
                }
                else
                    break;

                y += auxiliaryY * tile;
            };

            RenderRayObstacles(unit, ray, carAngleRay, ParallelInfoObj, ParallelResult);

            resultobjectPool.Return(ParallelResult);
            infoObjectPool.Return(ParallelInfoObj);
        });

        if (CachedDelegates.TryGetValue(unit, out var delegates))
        {
            foreach (var cd in delegates)
                cd.Value(new Result(), unit);
        }

        ZBuffer.Render();
    }
}