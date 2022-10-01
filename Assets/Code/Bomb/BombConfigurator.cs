using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombConfigurator : MonoBehaviour
{
    public  static BombConfigurator Instance {get ;private set;}
    private class DefusalCost
    {
        public eDefusalType defusal;
        public int cost;
    }
    private List<DefusalCost> defusalCosts = new List<DefusalCost>() 
    {   
        new DefusalCost() { defusal = eDefusalType.CODE, cost = 3 },
        new DefusalCost() { defusal = eDefusalType.WIRE_CUT, cost = 2 },
        new DefusalCost() { defusal = eDefusalType.LIQUID, cost = 3 },
        new DefusalCost() { defusal = eDefusalType.MAGNETIC_LOCK, cost = 3},
        new DefusalCost() { defusal = eDefusalType.SCREW_DRIVER_PANEL, cost = 2},
        new DefusalCost() { defusal = eDefusalType.TURNY_HANDLE, cost = 2},
        new DefusalCost() { defusal = eDefusalType.WIRE_CUT, cost = 2},
    };

    int numDefusals = 3;
    
    public class BombConfiguration
    {
        public List<DefusalInstruction> instructions = new List<DefusalInstruction>(); 
    }

    public List<DefusalConfigurator> configurators;

    public BombConfiguration bombConfiguration;

    private void Awake()
    {
        Instance = this;
    }
    public void Start()
    {
        ConfigureLevel();
    }
    private void ConfigureLevel()
    {
        bombConfiguration = new BombConfiguration();
        //so...
        var chosenDefusals = ChooseDefusals();
        ConfigureDefusals(chosenDefusals);
    }

    private List<eDefusalType> ChooseDefusals()
    {
        var tempList = new List<eDefusalType>();
        foreach(var cost in defusalCosts)
            tempList.Add(cost.defusal);

        var selection = tempList.GetRandom(numDefusals);
        return selection;
    }

    private void ConfigureDefusals(List<eDefusalType> zList)
    {
        //so we have 3 random ones... now we set each up:
        foreach(var defusal in zList)
        {
            var instruction = ConfigureDefusal(defusal);
            bombConfiguration.instructions.Add(instruction);
        }
    }
    private DefusalInstruction ConfigureDefusal(eDefusalType zDefusalType)
    {
        var configurator = configurators.Find(p => p.Type == zDefusalType);

        configurator.ConfigureDefusal();
        var instruction = configurator.GetDefusalInstruction();
        return instruction;
    }

}
