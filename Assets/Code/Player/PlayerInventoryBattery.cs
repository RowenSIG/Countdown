using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryBattery : PlayerInventoryItem
{
    private List<int> voltages = new List<int>();

    public Battery battery1;
    public Battery battery2;

    public override void Collected(CollectableItem zItem)
    {
        base.Collected(zItem);

        var battery = zItem as Battery;
        if (voltages.Count < 2)
        {
            voltages.Add(battery.voltage);
        }
        else
        {
            voltages[0] = voltages[1];
            voltages[1] = battery.voltage;
        }

        if (voltages.Count > 0)
            battery1.Setup(voltages[0]);
        if (voltages.Count > 1)
            battery2.Setup(voltages[1]);
    }

    public override void Show()
    {
        base.Show();

        battery1.gameObject.EnsureActive(voltages.Count >= 1);
        battery2.gameObject.EnsureActive(voltages.Count >= 2);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override bool CanCollect()
    {
        return voltages.Count < 2;        
    }

    public override void Reset()
    {
        base.Reset();
        voltages.Clear();
        battery1.Reset();
        battery2.Reset();
    }
}
