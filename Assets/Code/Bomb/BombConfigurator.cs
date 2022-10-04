using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombConfigurator : MonoBehaviour
{
    public static BombConfigurator Instance { get; private set; }
    private class DefusalCost
    {
        public eDefusalType defusal;
        public int cost;
    }
    private List<DefusalCost> defusalCosts = new List<DefusalCost>()
    {
        new DefusalCost() { defusal = eDefusalType.CODE, cost = 4 },
        new DefusalCost() { defusal = eDefusalType.WIRE_CUT, cost = 2 },
        new DefusalCost() { defusal = eDefusalType.LIQUID, cost = 3 },
        new DefusalCost() { defusal = eDefusalType.MAGNETIC_LOCK, cost = 4 },
        new DefusalCost() { defusal = eDefusalType.SCREW_DRIVER_PANEL, cost = 3 },
        new DefusalCost() { defusal = eDefusalType.TURNY_HANDLE, cost = 2},
    };

    public class BombConfiguration
    {
        public List<DefusalInstruction> instructions = new List<DefusalInstruction>();
    }

    public List<DefusalConfigurator> configurators;
    public List<DefusalConfigurator> chosenConfigurators = new List<DefusalConfigurator>();

    public BombConfiguration bombConfiguration;

    private void Awake()
    {
        Instance = this;
    }
    public void Configure()
    {
        ResetConfigurators();
        ConfigureLevel();
    }
    private void ConfigureLevel()
    {
        bombConfiguration = new BombConfiguration();

        foreach (var configurator in configurators)
            configurator.ConfigureDefusal();

        //so...
        ChooseDefusals();
    }

    public void Reset()
    {
        ResetConfigurators();

        bombConfiguration.instructions.Clear();
        foreach (var configurator in configurators)
        {
            configurator.RefreshDefusal();
        }
        foreach (var configurator in chosenConfigurators)
        {
            bombConfiguration.instructions.Add(configurator.GetDefusalInstruction());
        }
    }

    private void ResetConfigurators()
    {
        foreach (var configurator in configurators)
        {
            configurator.Reset();
        }
    }
    private void ChooseDefusals()
    {
        var tempList = new List<DefusalCost>();
        foreach (var cost in defusalCosts)
            tempList.Add(cost);

        tempList.ShuffleInPlace();
        var selection = new List<eDefusalType>();
        int total = 0;
        for (int i = 0; i < tempList.Count && selection.Count < 3; i++)
        {
            var choice = tempList[i];
            if (choice.cost + total < 10)
            {
                selection.Add(choice.defusal);
            }
        }

        // selection = new List<eDefusalType>() { eDefusalType.MAGNETIC_LOCK, eDefusalType.LIQUID, eDefusalType.SCREW_DRIVER_PANEL };
        // selection = new List<eDefusalType>() { eDefusalType.CODE, eDefusalType.TURNY_HANDLE, eDefusalType.WIRE_CUT };
        // selection = new List<eDefusalType>() { eDefusalType.CODE, eDefusalType.SCREW_DRIVER_PANEL, eDefusalType.WIRE_CUT };

        Debug.Log($"BombConfigurator - Selection [{selection[0]}], [{selection[1]}], [{selection[2]}]");

        foreach (var defusal in selection)
        {
            var configurator = configurators.Find(p => p.Type == defusal);
            chosenConfigurators.Add(configurator);

            var instruction = configurator.GetDefusalInstruction();
            bombConfiguration.instructions.Add(instruction);
        }
    }

}
