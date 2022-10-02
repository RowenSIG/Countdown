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

    private List<eColour> chosenColours = new List<eColour>();

    public override void ConfigureDefusal()
    {
        ResetChosenColours();

        ChooseWireColours();
        ChooseRandomWireColour();
        ChooseAndAssignRiddleForColour();
        AssignColoursToMaterials();
    }

    public override void RefreshDefusal() 
    {
        ChooseRandomWireColour();
        ChooseAndAssignRiddleForColour();
        AssignColoursToMaterials();
    }

    public override DefusalInstruction GetDefusalInstruction()
    {
        var instruction =new WireCutDefusalInstruction();
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
        foreach(var colour in colours)
        {
            allColours.Add((eColour)colour);
        }

        for(int i = 0 ; i < 3 ; i++)
        {
            var rand = Random.Range(0, allColours.Count);
            chosenColours.Add(allColours[rand]);
            allColours.RemoveAt(rand);
        }
    }

    private void ChooseRandomWireColour()
    {
        var rand = Random.Range(0 , chosenColours.Count);
        chosenColour = chosenColours[rand];
    }

    private void ChooseAndAssignRiddleForColour()
    {
        var tempList = new List<ColourRiddle>();
        foreach(var colourRidde in riddles)
        {
            if(colourRidde.colour == chosenColour)
                tempList.Add(colourRidde);
        }

        var rand = Random.Range(0, tempList.Count);
        var chosen = tempList[rand];

        riddleText.text = chosen.riddle;
    }

    private void AssignColoursToMaterials()
    {
        for(int i = 0 ; i < chosenColours.Count; i++)
        {
            var material =wireMaterials[i];
            var colour = chosenColours[i];

            var color = ColourProvider.GetColor(colour);
            material.SetColor("Main", color);
        }
    }

    private void ResetChosenColours()
    {
        chosenColours.Clear();
    }
}