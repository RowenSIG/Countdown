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

    public eMode Mode { get; private set; }

    public DefusalBase CurrentDefusal { get; private set; } = null;

    private InteractiveItem currentInteractiveTarget;

    public Transform playerSpawn;

    public void Awake()
    {
        Mode = eMode.NORMAL;
        instance = this;
    }

    public void Start()
    {
        BombConfigurator.Instance.Configure();
        ConfigureBomb();
        PlaySession.Start();
    }

    public void ConfigureBomb()
    {
        var configuration = BombConfigurator.Instance.bombConfiguration;
        Bomb.Instance.Setup(configuration.instructions);
    }
    public void OnDestroy()
    {
        instance = null;
    }

    public void Explode()
    {
        StartCoroutine(CoExplode());
    }

    public void CurrentPlayerPointingTarget(Collider zCollider)
    {
        //get this object's interaction possibilities...
        currentInteractiveTarget = zCollider.GetComponentInParent<InteractiveItem>();

        if (currentInteractiveTarget == null
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
        if (currentInteractiveTarget != null && currentInteractiveTarget.CanInteract())
        {
            Countdown.Instance.SpendTime();
            currentInteractiveTarget.Interact();
        }
    }

    public void SecondaryInteractionWithTarget()
    {
        //for now, turn it off:

        if (currentInteractiveTarget != null)
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

        if (zDefusal.Defused)
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


    private IEnumerator<YieldInstruction> CoExplode()
    {
        Debug.Log("Explode!");

        Mode = eMode.BOMB_EXPOLODING;
        Bomb.Instance.Explode();
        Player.Instance.Explode();

        yield return new WaitForSeconds(2f);

        Player.Instance.Reset();
        BombConfigurator.Instance.Reset();
        Bomb.Instance.Reset();
        ResettingItems.Reset();
        ConfigureBomb();
        InGameMenu.Instance.Reset();
        Countdown.Instance.Reset();

        Mode = eMode.NORMAL;

        PlaySession.attempts += 1;
    }


    public void BombDefused()
    {
        StartCoroutine(CoBombDefused());
    }
    IEnumerator<YieldInstruction> CoBombDefused()
    {
        yield return new WaitForSeconds(0.5f);

        UnityEngine.SceneManagement.SceneManager.LoadScene("SuccessScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
