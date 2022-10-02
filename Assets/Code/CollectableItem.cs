using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCollectableItem
{
    INVALID = 0,

    WIRE_CUTTERS = 1,
    LIQUID_BEAKER = 3,
    SCREW_DRIVER = 4,
    BATTERY = 5,
}
public class CollectableItem : InteractiveItem
{
    public eCollectableItem Type;

    public Animation useAnimation;

    public void Collect()
    {
        //just disappear
        gameObject.EnsureActive(false);
    }

    public override bool CanInteract()
    {
        //does player already have one?
        bool canPickUp = Player.Instance.CanCollect(Type);        
        return canPickUp;
    }

}
