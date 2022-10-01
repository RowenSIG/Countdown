using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public List<DefusalBase> allDefusalPrefabs;

    public List<Transform> defusalTransforms;

    private List<DefusalBase> defusals;

    public void Set(List<DefusalInstruction> zList)
    {
        foreach(var instruction in zList)
        {
            SpawnDefusal(instruction);
        }
    }

    private void SpawnDefusal(DefusalInstruction zInstruction)
    {
        var prefab = allDefusalPrefabs.Find(p => p.Type == zInstruction.Type);

        var index = defusals.Count;
        var clone = GameObject.Instantiate(prefab, defusalTransforms[index]);

        clone.Setupinstruction(zInstruction);
        defusals.Add(clone);
    }

    
}
