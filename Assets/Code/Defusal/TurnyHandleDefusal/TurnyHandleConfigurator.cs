using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnyHandleConfigurator : DefusalConfigurator
{
    public override eDefusalType Type => eDefusalType.TURNY_HANDLE;
   
    public List<TurnyHandleLocation> locations;

    public GameObject clockwisePrefab;
    public GameObject counterClockwisePrefab;

    private List<eTurnDirection> turns = new List<eTurnDirection>();
    private int chosenLocation;

    public override void ConfigureDefusal()
    {
        ResetLocations();

        GenerateTurns();
        PickLocation();
        AssignTurnsToLocation();
    }

    public override void RefreshDefusal()
    {
        ResetLocations();

        PickLocation();
        AssignTurnsToLocation();
    }

    public override DefusalInstruction GetDefusalInstruction()
    {
        var instruction = new TurnyHandleDefusalInstruction();
        instruction.order.AddRange(turns);
        return instruction;
    }

    private void ResetLocations()
    {
        foreach (var location in locations)
        {
            location.Reset();
        }
    }
    private void GenerateTurns()
    {
        turns.Clear();
        for (int i = 0; i < 3; i++)
        {
            var rand = Random.Range(0, 1f);

            if (rand >= 0.5f)
                turns.Add(eTurnDirection.CLOCKWISE);
            else
                turns.Add(eTurnDirection.COUNTER_CLOCKWISE);
        }
    }

    private void PickLocation()
    {
        chosenLocation = Random.Range(0, locations.Count);
    }

    private void AssignTurnsToLocation()
    {
        var location = locations[chosenLocation];

        var clones = new List<GameObject>();

        foreach (var turn in turns)
        {
            GameObject clone = null;
            switch (turn)
            {
                case eTurnDirection.CLOCKWISE:
                    clone = GameObject.Instantiate(clockwisePrefab);
                    break;

                case eTurnDirection.COUNTER_CLOCKWISE:
                    clone = GameObject.Instantiate(counterClockwisePrefab);
                    break;
            }
            clones.Add(clone);
        }

        location.Setup(clones);
    }
}