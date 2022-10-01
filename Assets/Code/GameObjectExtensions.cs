using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions 
{
    public static void EnsureActive(this GameObject zGameObject, bool zActive)
    {
        if(zGameObject.activeSelf != zActive)
            zGameObject.SetActive(zActive);
    }
}
