using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidConfigurator : DefusalConfigurator
{
    public override eDefusalType Type => eDefusalType.LIQUID;

    public List<Beaker> setOfBeakers;

    private List<eColour> chosenSourceColours = new List<eColour>(); 
    private List<eColour> colourOrder = new List<eColour>();

    
    public override void ConfigureDefusal()
    {
        ResetChosenSourceColours();
        ResetColourOrder();
        ResetBeakers();

        ChooseColourOrder();
        ChooseSourceColours();

        AssignColoursToBeakers();
    }

    public override void RefreshDefusal() 
    {
        ResetChosenSourceColours();
        ResetColourOrder();
        ResetBeakers();

        ChooseColourOrder();
        ChooseSourceColours();

        AssignColoursToBeakers();
    }

    public override DefusalInstruction GetDefusalInstruction()
    {
        var instruction = new LiquidDefusalInstruction();
        instruction.colourOrder.AddRange(colourOrder);
        return instruction;
    }
        
    public override void Reset()
    {
        ResetChosenSourceColours();
        ResetColourOrder();
        ResetBeakers();
    }


    private void ResetChosenSourceColours()
    {
        chosenSourceColours.Clear();
    }
    private void ResetColourOrder()
    {
        colourOrder.Clear();
    }
    private void ResetBeakers()
    {
        foreach(var beaker in setOfBeakers)
        {
            beaker.Reset();
        }
    }

    private void ChooseColourOrder()
    {
        var mixes = ColourProvider.Instance.colourMixes;
        var rand = Random.Range(0, mixes.Count);
        var mix = mixes[rand];

        Debug.Log($"LiquidConfigurator: RandomMix[{rand}]of[{mixes.Count}] = colour[{mix.colourOrder[0]},{mix.colourOrder[1]}]");
        colourOrder.AddRange(mix.colourOrder);
    }

    private void ChooseSourceColours()
    {
        var colours = System.Enum.GetValues(typeof(eColour));
        var allOtherColours= new List<eColour>();

        foreach(eColour colour in colours)
        {
            if(colourOrder.Contains(colour) == false)
                allOtherColours.Add(colour);
        }

        for(int i = 0 ; i < allOtherColours.Count; i++)
        {
            var rand = Random.Range(0, allOtherColours.Count);
            var colour = allOtherColours[rand];
            chosenSourceColours.Add(colour);
            allOtherColours.RemoveAt(rand);
            i --;
        }
    }

    private void AssignColoursToBeakers()
    {
        //which ones?! let's just random it...
        var indices = new List<int>() { 0,1,2,3 };
        indices.ShuffleInPlace();

        var count = 0;
        foreach(var colour in colourOrder)
        {
            var index = indices[count];
            var beaker = setOfBeakers[index];
            beaker.Setup(colour);
            count ++;
        }

        int numLeft = setOfBeakers.Count - count;
        for(int i = 0 ; i < numLeft; i ++)
        {
            var index = indices[count];
            var colour = chosenSourceColours[i];
            var beaker = setOfBeakers[index];
            beaker.Setup(colour);
            count ++;
        }
    }
}