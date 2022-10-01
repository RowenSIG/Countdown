using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticLockLocation : MonoBehaviour
{
    public Battery battery;

    public void Setup(int zBatteryVoltage)
    {
        battery.voltage = zBatteryVoltage;
    }

}
