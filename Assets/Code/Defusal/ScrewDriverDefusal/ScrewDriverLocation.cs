using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewDriverLocation : MonoBehaviour
{
    public Transform position;

    private GameObject screwDriver;
    
    public void Setup(GameObject zScrewDriver)
    {
        screwDriver = zScrewDriver;
        screwDriver.transform.SetParent(position, worldPositionStays: false);
    }

    public void Reset()
    {
        GameObject.Destroy(screwDriver);
    }
}
