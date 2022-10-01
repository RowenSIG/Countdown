using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCollectibleItem
{
    INVALID = 0,

    WIRE_CUTTERS = 1,
    ELECTRIC_WIRE = 2,
    LIQUID_BEAKER = 3,
    SCREW_DRIVER = 4,
}
public class CollectableItem : InteractiveItem
{
    public eCollectibleItem Type;

    public Animation useAnimation;

    public void Collect()
    {
        //just disappear
        gameObject.EnsureActive(false);
    }

    public void Use()
    {
        useAnimation.Play();
    }
}
