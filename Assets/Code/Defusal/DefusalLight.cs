using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefusalLight : MonoBehaviour
{
    public GameObject idleLight;
    public GameObject defusedLight;
    public GameObject explodedLight;
    public void SetIdle()
    {
        idleLight.EnsureActive(true);
        defusedLight.EnsureActive(false);
        explodedLight.EnsureActive(false);
    }
    public void SetDefused()
    {
        idleLight.EnsureActive(false);
        defusedLight.EnsureActive(true);
        explodedLight.EnsureActive(false);

    }
    public void SetExploded()
    {
        idleLight.EnsureActive(false);
        defusedLight.EnsureActive(false);
        explodedLight.EnsureActive(true);

    }
}
