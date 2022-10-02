using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WiresConfigurator : DefusalConfigurator
{
    public override eDefusalType Type => eDefusalType.WIRE_CUT;

    public TextMesh riddleText;

    [System.Serializable]
    public class ColourRiddle
    {
        public eColour colour;
        public string riddle;
    }
    public List<ColourRiddle> riddles;
    public List<Material> wireMaterials;
    private eColour chosenColour;

    public WireCutter wireCutterPrefab;
    public List<Transform> wireCutterLocations;
    private int chosenWireCutterLocation;
    private WireCutter wireCutter;


    private List<eColour> chosenColours = new List<eColour>();

    public override void ConfigureDefusal()
    {
        ResetChosenColours();
        ResetWireCutter();

        ChooseWireColours();
        ChooseRandomWireColour();
        ChooseAndAssignRiddleForColour();
        AssignColoursToMaterials();

        ChooseWireCutterLocation();
        AssignWireCutterToLocation();
    }

    public override void RefreshDefusal()
    {
        ChooseRandomWireColour();
        ChooseAndAssignRiddleForColour();
        AssignColoursToMaterials();
        AssignWireCutterToLocation();
    }

    public override DefusalInstruction GetDefusalInstruction()
    {
        var instruction = new WireCutDefusalInstruction();
        instruction.wireColours.AddRange(chosenColours);
        instruction.chosenWireIndex = chosenColours.IndexOf(chosenColour);
        return instruction;
    }

    public override void Reset()
    {
        ResetChosenColours();
    }

    private void ChooseWireColours()
    {
        var allColours = new List<eColour>();
        var colours = System.Enum.GetValues(typeof(eColour));
        foreach (var colour in colours)
        {
            allColours.Add((eColour)colour);
        }

        for (int i = 0; i < 3; i++)
        {
            var rand = Random.Range(0, allColours.Count);
            chosenColours.Add(allColours[rand]);
            allColours.RemoveAt(rand);
        }
    }

    private void ChooseRandomWireColour()
    {
        var rand = Random.Range(0, chosenColours.Count);
        chosenColour = chosenColours[rand];
    }

    private void ChooseAndAssignRiddleForColour()
    {
        var tempList = new List<ColourRiddle>();
        foreach (var colourRidde in riddles)
        {
            if (colourRidde.colour == chosenColour)
                tempList.Add(colourRidde);
        }

        var rand = Random.Range(0, tempList.Count);
        var chosen = tempList[rand];

        var wrapped = FixRiddleLength(chosen.riddle);
        riddleText.text = wrapped;
    }

    private void ChooseWireCutterLocation()
    {
        chosenWireCutterLocation = Random.Range(0, wireCutterLocations.Count);
    }

    private void AssignColoursToMaterials()
    {
        for (int i = 0; i < chosenColours.Count; i++)
        {
            var material = wireMaterials[i];
            var colour = chosenColours[i];

            var color = ColourProvider.GetColor(colour);
            material.SetColor("_Color", color);
        }
    }

    private void AssignWireCutterToLocation()
    {
        var location = wireCutterLocations[chosenWireCutterLocation];
        var clone = GameObject.Instantiate(wireCutterPrefab, location, false);
        wireCutter = clone;
    }

    private void ResetWireCutter()
    {
        if (wireCutter != null)
        {
            GameObject.Destroy(wireCutter.gameObject);
        }
    }

    private void ResetChosenColours()
    {
        chosenColours.Clear();
    }


    private string FixRiddleLength(string zRiddle)
    {
        var charlimit = 18;

        //ummmmm
        if(zRiddle.Length < charlimit)
            return zRiddle;

        var firstSpace = zRiddle.IndexOf(' ', charlimit);
        if(firstSpace < 0)
            return zRiddle;
        var sub = zRiddle.Insert(firstSpace, "\n");
        return sub;
    }
}