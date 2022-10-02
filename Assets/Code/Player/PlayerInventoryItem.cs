using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryItem : MonoBehaviour
{
    public eCollectableItem Type;

    public GameObject visual;

    public bool Have {get ;private set;}    

    public virtual bool CanCollect() { return Have == false; }

    public virtual void Collected(CollectableItem zItem)
    {
        Have = true;
    }

    public virtual void Show()
    {
        visual.EnsureActive(true);
    }
    public virtual void Hide()
    {
        visual.EnsureActive(false);
    }
}

