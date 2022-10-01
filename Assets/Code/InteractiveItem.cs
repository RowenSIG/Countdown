using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveItem : MonoBehaviour
{
    public virtual System.Action Action1 => () => Room.Instance.InteractWithTarget(this);
    public virtual System.Action Action2 => () => Room.Instance.DiscardItem(this);
    public string ActionLabel1;
    public string ActionLabel2;
}
