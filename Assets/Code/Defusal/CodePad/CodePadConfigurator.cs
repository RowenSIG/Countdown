using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodePadConfigurator : DefusalConfigurator
{
    public override eDefusalType Type => eDefusalType.CODE;

    public List<CodeNumberLocation> allLocations;

    private List<CodeNumberLocation> chosenLocations = new List<CodeNumberLocation>();
    private List<int> code = new List<int>();
    
    public override void ConfigureDefusal()
    {
        ResetCode();
        ResetLocations();

        GenerateCode();
        PickLocations();
        AssignCodeDigitsToLocations();
    }

    public override void RefreshDefusal() 
    {
        ResetCode();
        ResetLocations();

        GenerateCode();
        AssignCodeDigitsToLocations();
    }

    public override DefusalInstruction GetDefusalInstruction()
    {
        var instruction = new CodeDefusalInstruction();
        instruction.code.AddRange(code);
        return instruction;        
    }

    private void GenerateCode()
    {
        for(int i = 0; i < 3; i++)
        {
            var rand = Random.Range(0, 9);
            code.Add(rand);
        }
    }

    private void PickLocations()
    {
        chosenLocations.Clear();

        var tempList = new List<CodeNumberLocation>(allLocations);
        for(int i = 0 ; i < 3; i++)
        {
            var rand = Random.Range(0, tempList.Count);
            chosenLocations.Add(tempList[rand]);
            tempList.RemoveAt(rand);
        }
    }
    private void AssignCodeDigitsToLocations()
    {
        for(int i = 0 ; i < 3 ; i ++)
        {
            var number = code[i];
            var location = chosenLocations[i];

            location.SetCodeNumber(i, number);
        }
    }
    private void ResetLocations()
    {
        foreach(var location in allLocations)
        {
            location.Reset();
        }
    }

    private void ResetCode()
    {
        code.Clear();
    }
}