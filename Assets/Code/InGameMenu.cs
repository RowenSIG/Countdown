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

    public GameObject interactionCursor;

    public void Awake()
    {
        Instance = this;
    }
    public void OnDestroy()
    {
        Instance = null;
    }

    private void Update()
    {
        if(Room.Instance.Mode == eMode.NORMAL)
            interactionCursor.EnsureActive(true);
        else
            interactionCursor.EnsureActive(false);
    }

    public void SetVisibleInteractions(string zInteractionLabel1, string zInteractionLabel2 )
    {
        if(string.IsNullOrEmpty(zInteractionLabel1) == false)
        {
            interactionButton1.EnsureActive(true);
            interactionLabel1.text = zInteractionLabel1;
        }
        else
        {
            interactionButton1.EnsureActive(false);
        }

        if(string.IsNullOrEmpty(zInteractionLabel2) == false)
        {
            interactionButton2.EnsureActive(true);
            interactionLabel2.text = zInteractionLabel2;
        }
        else
        {
            interactionButton2.EnsureActive(false);
        }

    }

    public void ClearInteractions()
    {
        interactionButton1.EnsureActive(false);
        interactionButton2.EnsureActive(false);
    }


    public void OnInteraction1()
    {
        Room.Instance.InteractWithTarget();
    }
    public void OnInteraction2()
    {
        Room.Instance.SecondaryInteractionWithTarget();
    }
}

