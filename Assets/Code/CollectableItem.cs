using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCollectableItem
{
    WIRE_CUTTERS = 1,
    LIQUID_BEAKER = 3,
    SCREW_DRIVER = 4,
    BATTERY = 5,
}
public class CollectableItem : InteractiveItem
{
    public eCollectableItem Type;

    public Animation useAnimation;

    public override void Interact()
    {
        //just disappear
        gameObject.EnsureActive(false);
        Room.Instance.ItemCollected(this);
    }

    public override bool CanInteract()
    {
        //does player already have one?
        bool canPickUp = Player.Instance.CanCollect(Type);        
        return canPickUp;
    }

}
