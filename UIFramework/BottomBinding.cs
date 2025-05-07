﻿using ControlLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIFramework;

public class BottomBinding
{
    /// <summary> Name of the action that will happen when clicked </summary>
    public string NameAction { get; set; } = String.Empty;
    public List<Bottom> Bottoms { get; init; }
    /// <summary> The function that is called when pressed </summary>
    public Delegate ExecutableFunction { get; set; }
    /// <summary> Additional parameters for the button function </summary>
    public object[] FixedParameters { get; init; }


    /// <summary> Max delay between clicks </summary>
    public long WaitingTimeMilliseconds { get; set; }
    /// <summary>Freezes the button press</summary>
    public bool IsFreeze { get; set; }
    /// <summary> Is the object pending? </summary>
    public bool IsWaiting { get; private set; } = false;
    /// <summary> Stores the state of a button press </summary>
    public bool IsPress { get; private set; } = false;

    private Stopwatch stopwatch = new Stopwatch();


    public BottomBinding(List<Bottom> bottoms, Delegate executableFunction, long waitingTimeMilliseconds, object[] fixedParameters)
    {
        Bottoms = bottoms;
        ExecutableFunction = executableFunction;
        FixedParameters = fixedParameters;
        WaitingTimeMilliseconds = waitingTimeMilliseconds;
    }
    public BottomBinding(List<Bottom> bottoms, Delegate executableFunction, long waitingTimeMilliseconds)
         : this(bottoms, executableFunction, waitingTimeMilliseconds, new object[0])
    { }
    public BottomBinding(List<Bottom> bottoms, long waitingTimeMilliseconds)
        : this(bottoms, () => { }, waitingTimeMilliseconds, new object[0])
    { }
    public BottomBinding(Bottom bottom, long waitingTimeMilliseconds)
        : this(new List<Bottom>() { bottom }, () => { }, waitingTimeMilliseconds, new object[0])
    { }
    public BottomBinding(Bottom bottom, Delegate executableFunction, object[] fixedParameters)
        : this(new List<Bottom>() { bottom }, executableFunction, 0, fixedParameters)
    { }
    public BottomBinding(Delegate executableFunction, object[] fixedParameters)
         : this(new List<Bottom>(), executableFunction, 0, fixedParameters)
    { }
    public BottomBinding(Bottom bottom, Delegate executableFunction, long waitingTimeMilliseconds)
       : this(new List<Bottom>() { bottom }, executableFunction, waitingTimeMilliseconds, new object[0])
    { }
    public BottomBinding(Delegate executableFunction, long waitingTimeMilliseconds)
         : this(new List<Bottom>(), executableFunction, waitingTimeMilliseconds, new object[0])
    { }
    public BottomBinding(Bottom bottom, Delegate executableFunction)
        : this(new List<Bottom>() { bottom }, executableFunction, 0, new object[0])
    { }


    public void AddBottom(Bottom bottom)
    {
        foreach (var bottoms in Bottoms)
        {
            if (bottom.Key == bottom.Key)
                return;
        }

        Bottoms.Add(bottom);
    }

    private bool IsReadyToPress()
    {
        if (WaitingTimeMilliseconds <= 0)
            return true;

        if (!IsWaiting && !stopwatch.IsRunning)
        {
            stopwatch.Start();
            IsWaiting = true;

            return true;
        }
        else if (stopwatch.IsRunning && stopwatch.ElapsedMilliseconds >= WaitingTimeMilliseconds)
        {
            IsWaiting = false;

            stopwatch.Stop();
            stopwatch.Reset();
        }

        return false;
    }

    /// <summary> Calling a button function </summary>
    private void PracticingPressing(params object[] externalParams)
    {
        try
        {
            object[] allParams = new object[FixedParameters.Length + externalParams.Length];
            externalParams.CopyTo(allParams, 0);

            if (FixedParameters.Length > 0)
                FixedParameters.CopyTo(allParams, externalParams.Length);

            ExecutableFunction.DynamicInvoke(allParams);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка в PracticingPressing: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
        }
    }

    /// <summary> Check if all existing buttons are pressed in ButtonBinding </summary>
    public void Listen(params object[] externalParams)
    {
        if (IsFreeze || Bottoms.Count == 0)
            return;

        IsPress = false;

        int countTurnBottom = 0;
        foreach (var bottom in Bottoms)
        {
            if (bottom.IsKeyPressed())
                countTurnBottom++;
        }

        if (countTurnBottom == Bottoms.Count && IsReadyToPress())
        {
            PracticingPressing(externalParams);
            IsPress = true;
        }

    }
}
