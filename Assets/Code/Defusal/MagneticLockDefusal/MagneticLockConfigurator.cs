using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticLockConfigurator : DefusalConfigurator
{
    public override eDefusalType Type => eDefusalType.MAGNETIC_LOCK;
    private static readonly List<int> singleBatteryVoltages = new List<int>() { -1, 2, 5, 9 };
    private static readonly List<int> doubleBatteryVoltages = new List<int>() { 1, 4, 7, 8, 11, 14 };

    public Battery batteryPrefab;

    public List<Transform> locations;
    private List<Battery> batteries = new List<Battery>();

    private int voltage;

    public override void ConfigureDefusal()
    {
        ClearBatteries();

        ChooseTargetVoltage();
        SpawnBatteries();
        PlaceBatteriesInLocations();
    }

    public override void RefreshDefusal()
    {
        ClearBatteries();

        ChooseTargetVoltage();
        SpawnBatteries();
        PlaceBatteriesInLocations();
    }

    public override DefusalInstruction GetDefusalInstruction()
    {
        var instruction = new MagneticLockDefusalInstruction();
        instruction.voltage = voltage;
        return instruction;
    }

    private void ClearBatteries()
    {
        foreach(var battery in batteries)
        {
            GameObject.Destroy(battery.gameObject);
        }
        batteries.Clear();
    }

    private void ChooseTargetVoltage()
    {
        var rand = Random.Range(0, doubleBatteryVoltages.Count);
        voltage = doubleBatteryVoltages[rand];
    }

    private void SpawnBatteries()
    {
        foreach(var value in singleBatteryVoltages)
        {
            var clone = GameObject.Instantiate(batteryPrefab);
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