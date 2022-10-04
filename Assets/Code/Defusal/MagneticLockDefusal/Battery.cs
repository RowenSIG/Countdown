using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : CollectableItem
{
    public int voltage;

    public NumberDigits number;
    public GameObject negative;

    public void Setup(int zVoltage)
    {
        voltage = zVoltage;
        number.Set(Mathf.Abs(voltage));

        if (voltage < 0)
            negative.EnsureActive(true);
        else 
            negative.EnsureActive(false);
    }

    public override void Reset()
    {
        base.Reset();
        voltage = 0;
        number.Clear();
    }
}
