using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public static InGameMenu Instance {get; private set;}
   
    public GameObject interactionButton1;
    public Text interactionLabel1;
    public GameObject interactionButton2;
    public Text interactionLabel2;


    private Action interactionAction1;
    private Action interactionAction2;

    public void Awake()
    {
        Instance = this;
    }
    public void OnDestroy()
    {
        Instance = null;
    }

    public void SetVisibleInteractions(Action zInteractionAction1, string zInteractionLabel1, Action zInteractionAction2, string zInteractionLabel2 )
    {
        if(zInteractionAction1 != null)
        {
            interactionButton1.SetActive(true);
            interactionLabel1.text = zInteractionLabel1;
        }

        if(zInteractionAction2 != null)
        {
            interactionButton2.SetActive(true);
            interactionLabel2.text = zInteractionLabel2;
        }
        else
        {
            interactionButton2.SetActive(false);
        }

    }

    public void ClearInteractions()
    {
        interactionButton1.SetActive(false);
        interactionButton2.SetActive(false);
    }


    public void OnInteraction1()
    {
        interactionAction1?.Invoke();
    }
    public void OnInteraction2()
    {
        interactionAction2?.Invoke();
    }
}

