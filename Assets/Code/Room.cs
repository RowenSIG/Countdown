using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private static Room instance;
    public static Room Instance => instance;

    public void Awake()
    {
        instance = this;
    }
    public void OnDestroy()
    {
        instance = null;
    }

    public void CurrentPlayerPointingTarget(Collider zCollider)
    {
        //get this object's interaction possibilities...
        var interactiveItem = zCollider.GetComponent<InteractiveItem>();

        if(interactiveItem == null)
        {
            CurrentPlayerPointingAtNothing();
            return;
        }
        //show the menu?
        InGameMenu.Instance.SetVisibleInteractions(interactiveItem.Action1, interactiveItem.ActionLabel1, interactiveItem.Action2, interactiveItem.ActionLabel2);
    }

    public void CurrentPlayerPointingAtNothing()
    {
        InGameMenu.Instance.ClearInteractions();
    }

    public void InteractWithTarget(InteractiveItem zItem)
    {

    }

    public void DiscardItem(InteractiveItem zItem)
    {

    }

    public void CollectItem(CollectibleItem zItem)
    {

    }
}
