using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnyHandleLocation : MonoBehaviour
{
    public List<Transform> locations;
    public List<GameObject> turns = new List<GameObject>();

    public void Setup(List<GameObject> zTurns)
    {
        turns.Clear();

        for(int i = 0 ; i < zTurns.Count; i++)
        {
            var turn = zTurns[i];
            var location = locations[i];
            turn.transform.SetParent(location, worldPositionStays: false);
            turns.Add(turn);
        }   
    }

    public void Reset()
    {
        foreach(var code in turns)
        {
            GameObject.Destroy(code);
        }
        turns.Clear();
    }
}
