using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatingInteractiveItem : InteractiveItem
{
    public int useLimit;
    public Animator animator;

    public void Interact()
    {
        if(useLimit > 0)
        {
            useLimit -= 1;

            animator.SetTrigger("Play");
        }
    }

    public override bool CanInteract()
    {
        return useLimit > 0;
    }

}
