using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : CollectableItem
{
    public int voltage;

    public NumberDigits number;

    public void Setup(int zVoltage)
    {
        voltage = zVoltage;
        number.Set(voltage);
    }
}
