using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatingInteractiveItem : InteractiveItem
{
    public int useLimit;
    private int uses = 0; 
    public Animator animator;

    protected void Awake()
    {
        ResettingItems.Register(this);
    }
    protected void OnDestroy()
    {
        ResettingItems.DeRegister(this);
    }

    public override void Interact()
    {
        if(uses < useLimit)
        {
            uses += 1;

            animator.SetTrigger("Play");
        }
    }

    public override bool CanInteract()
    {
        return uses < useLimit;
    }


    public override void Reset()
    {
        uses = 0;
        animator.SetTrigger("Reset");
    }

}

public static class ResettingItems
{
    private static List<AnimatingInteractiveItem> items = new List<AnimatingInteractiveItem>();

    public static void Register(AnimatingInteractiveItem zItem)
    {
        items.Add(zItem);
    }
    public static void DeRegister(AnimatingInteractiveItem zItem)
    {
        items.Remove(zItem);
    }

    public static void Reset()
    {
        foreach(var item in items)
            item.Reset();
    }
}