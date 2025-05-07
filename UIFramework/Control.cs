using ControlLib;
using MoveLib.Angle;
using ScreenLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIFramework;
public class Control
{
    public List<BottomBinding> Bindings { get; init; } = new();

    public Control()
    {
        Screen.Window.SetMouseCursorVisible(false);
        Screen.Window.MouseMoved += MoveMouse.OnMouseMoved;
    }

    public void AddBottomBind(BottomBinding bottomBinding)
    {
        Bindings.Add(bottomBinding);
    }
    public void DeleteBottomBind(BottomBinding bottomBinding)
    {
        List<Bottom> deleteBind = bottomBinding.Bottoms;

        foreach (var binding in Bindings)
        {
            List<Bottom> bindBottom = binding.Bottoms;

            int countSimilarities = 0;
            foreach (var item in deleteBind)
            {
                foreach (var item2 in bindBottom)
                {
                    if (item.Key == item2.Key)
                        countSimilarities++;
                }
            }

            if (countSimilarities == bindBottom.Count)
            {
                Bindings.Remove(bottomBinding);
                return;
            }
        }
    }
    public void DeleteBottomBind(string nameAction)
    {
        Bindings.Remove(Bindings.Where(s => s.NameAction == nameAction).First());
    }
    public void MakePressed()
    {
        foreach (var binding in Bindings)
        {
            binding.Listen();
        }
    }
    public void MakePressedParallel()
    {
        Parallel.ForEach(Bindings, binding =>
        {
            binding.Listen();
        });

    }
}
