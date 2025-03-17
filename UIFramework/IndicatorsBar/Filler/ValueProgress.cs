using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UIFramework.IndicatorsBar.Filler;
public class ValueProgress
{

    public float Value { get; private set; }
    public float Percent { get; private set; }

    public ValueProgress(float value, float maxValue, float minValue)
    {
        SetValue(value, maxValue, minValue);
    }
    public ValueProgress()
    {
        Value = 0;
        Percent = 0;
    }

    internal void SetValue(float value, float maxValue, float minValue)
    {
        if (value < minValue)
            value = minValue;

        Value = value;
        Percent = value * 100 / maxValue;
    }
}
