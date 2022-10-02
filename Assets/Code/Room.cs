using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eMode
{
    NORMAL = 0,
    DEFUSAL = 1,
    BOMB_EXPOLODING = 2,
}
public class Room : MonoBehaviour
{
    private static Room instance;
    public static Room Instance => instance;

    public eMode Mode {get; private set;}

    public DefusalBase CurrentDefusal {get; private set;} = null;
    public void Awake()
    {
        Mode = eMode.NORMAL;
        instance = this;
    }

    public void Start()
    {
        BombConfigurator.Instance.Configure();

        var configuration = BombConfigurator.Instance.bombConfiguration;
        Bomb.Instance.Setup(configuration.instructions);
    }
    public void OnDestroy()
    {
        instance = null;
    }

    private InteractiveItem currentInteractiveTarget;

    public void CurrentPlayerPointingTarget(Collider zCollider)
    {
        //get this object's interaction possibilities...
        currentInteractiveTarget = zCollider.GetComponentInParent<InteractiveItem>();

        if(currentInteractiveTarget == null
            || currentInteractiveTarget.CanInteract() == false)
        {
            CurrentPlayerPointingAtNothing();
            return;
        }
        //show the menu?
        InGameMenu.Instance.SetVisibleInteractions(currentInteractiveTarget.ActionLabel1
                                                , currentInteractiveTarget.ActionLabel2);
    }

    public void CurrentPlayerPointingAtNothing()
    {
        InGameMenu.Instance.ClearInteractions();
    }

    public void InteractWithTarget()
    {
        if(currentInteractiveTarget != null && currentInteractiveTarget.CanInteract())
        {
            currentInteractiveTarget.Interact();
        }
    }

    public void SecondaryInteractionWithTarget()
    {
        //for now, turn it off:
        
        if(currentInteractiveTarget != null)
        {
            currentInteractiveTarget.gameObject.EnsureActive(false);
            currentInteractiveTarget = null;
        }
    }

    public void ItemCollected(CollectableItem zCollectable)
    {
        var collectableType = zCollectable.Type;
        Player.Instance.GainCollectableItem(zCollectable);
    }

    public void CancelDefusal()
    {
        Mode = eMode.NORMAL;
        CurrentDefusal.Close();
        Player.Instance.ReturnToNormal();
        CurrentDefusal = null;
    }

    public void StartDefusal(DefusalBase zDefusal)
    {
        //here we have to enter into defusal mode...
        //as this is unique to each defusal instance, i reckon they should handle that.

        if(zDefusal.Defused)
        {
            //do nothign
        }
        else
        {
            CurrentDefusal = zDefusal;
            //so we just make sure that's the situation
            Mode = eMode.DEFUSAL;
            zDefusal.StartDefusal();
            Player.Instance.StartDefusal();
        }
    }
    
}
