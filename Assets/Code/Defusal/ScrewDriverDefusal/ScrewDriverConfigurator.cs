using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewDriverConfigurator : DefusalConfigurator
{
    public override eDefusalType Type => eDefusalType.SCREW_DRIVER_PANEL;

    public List<ScrewDriverLocation> locations;
    private int chosenLocation;
    public List<ScrewDriverHintVisual> hintVisuals;
    private int chosenHintVisual;

    public List<bool> order = new List<bool>();

    public GameObject screwDriverPrefab;

    public override void ConfigureDefusal()
    {
        ResetLocations();
        ResetHintVisuals();

        //jsut pick a location and put a screwdriver there

        ChooseLocation();
        SpawnScrewDriver();

        GenerateOrder();
        ChooseHintVisual();
        AssignOrderToHintVisual();
    }

    public override void RefreshDefusal()
    {
        ResetLocations();
        ResetHintVisuals();

        SpawnScrewDriver();

        GenerateOrder();
        AssignOrderToHintVisual();
    }

    public override DefusalInstruction GetDefusalInstruction()
    {
        var instruction = new ScrewDriverDefusalInstruction();
        instruction.order.AddRange(order);
        return instruction;
    }

    private void ResetLocations()
    {
        foreach (var location in locations)
            location.Reset();
    }

    private void ChooseLocation()
    {
        chosenLocation = Random.Range(0, locations.Count);
    }

    private void SpawnScrewDriver()
    {
        var location = locations[chosenLocation];
        var clone = GameObject.Instantiate(screwDriverPrefab);
        location.Setup(clone);
    }

    private void GenerateOrder()
    {
        order.Clear();
        var badScrewIndex = Random.Range(0, 4);

        for (int i = 0; i < 4; i++)
        {
            order.Add(i != badScrewIndex);
        }
    }

    private void ResetHintVisuals()
    {
        foreach (var hintVisual in hintVisuals)
            hintVisual.Reset();
    }

    private void ChooseHintVisual()
    {
        chosenHintVisual = Random.Range(0, hintVisuals.Count);
    }

    private void AssignOrderToHintVisual()
    {
        var hintVisual = hintVisuals[chosenHintVisual];
        hintVisual.Setup(order);
    }
}