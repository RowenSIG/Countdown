using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : InteractiveItem
{
    public override Action Action1 => () => Room.Instance.CollectItem(this);
    public override Action Action2 => () => Room.Instance.DiscardItem(this);

    public Animation useAnimation;

}
