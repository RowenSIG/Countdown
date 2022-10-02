using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticLockConfigurator : DefusalConfigurator
{
    public override eDefusalType Type => eDefusalType.MAGNETIC_LOCK;
    private static readonly List<int> singleBatteryVoltages = new List<int>() { -1, 2, 5, 9 };

    public Battery batteryPrefab;

    public List<Transform> locations;
    private List<Battery> batteries = new List<Battery>();

    private int voltage1;
    private int voltage2;

    public override void ConfigureDefusal()
    {
        ResetBatteries();

        ChooseTargetVoltage();
        SpawnBatteries();
        PlaceBatteriesInLocations();
    }

    public override void RefreshDefusal()
    {
        ResetBatteries();

        ChooseTargetVoltage();
        SpawnBatteries();
        PlaceBatteriesInLocations();
    }

    public override DefusalInstruction GetDefusalInstruction()
    {
        var instruction = new MagneticLockDefusalInstruction();
        instruction.battery1 = voltage1;
        instruction.battery2 = voltage2;
        return instruction;
    }

    
    public override void Reset()
    {
        ResetBatteries();
    }

    private void ResetBatteries()
    {
        foreach(var battery in batteries)
        {
            GameObject.Destroy(battery.gameObject);
        }
        batteries.Clear();
    }

    private void ChooseTargetVoltage()
    {
        var tempList = new List<int>();
        tempList.AddRange(singleBatteryVoltages);

        var rand1 = Random.Range(0 , tempList.Count);
        voltage1 = tempList[rand1];
        tempList.RemoveAt(rand1);

        var rand2 = Random.Range(0, tempList.Count);
        voltage2 = tempList[rand2];

        Debug.Log($"MagneticLockConfigurator: Battery rand[{rand1}, {rand2}] of [{singleBatteryVoltages.Count}] => voltages[{voltage1},{voltage2}]");
    }

    private void SpawnBatteries()
    {
        foreach(var value in singleBatteryVoltages)
        {
            var clone = GameObject.Instantiate(batteryPrefab);
            clone.Setup(value);
            batteries.Add(clone);
        }
    }

    private void PlaceBatteriesInLocations()
    {
        batteries.ShuffleInPlace();

        for(int i = 0 ; i < batteries.Count; i++)
        {
            var location = locations[i];
            var battery = batteries[i];

            battery.transform.SetParent(location, worldPositionStays: false);
        }
    }
}