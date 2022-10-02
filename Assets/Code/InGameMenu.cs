using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public static InGameMenu Instance { get; private set; }

    public GameObject interactionButton1;
    public Text interactionLabel1;
    public GameObject interactionButton2;
    public Text interactionLabel2;

    public GameObject interactionCursor;

    public Animator timeUpAnimator;

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InGamePause.Instance.gameObject.EnsureActive(false);
        timeUpAnimator.SetTrigger("Reset");
    }
    public void OnDestroy()
    {
        Instance = null;
    }

    public void Reset()
    {
        timeUpAnimator.SetTrigger("Reset");
    }

    public void TimeUp()
    {
        timeUpAnimator.SetTrigger("Play");
    }

    private void Update()
    {
        if (Room.Instance.Mode == eMode.NORMAL)
            interactionCursor.EnsureActive(true);
        else
            interactionCursor.EnsureActive(false);

        bool pause = PlayerInputManager.GetButtonDown(0, ePadButton.START);
        pause |= Input.GetKeyDown(KeyCode.Escape);

        if (pause)
        {
            TogglePause();
        }
    }

    public void SetVisibleInteractions(string zInteractionLabel1, string zInteractionLabel2)
    {
        if (string.IsNullOrEmpty(zInteractionLabel1) == false)
        {
            interactionButton1.EnsureActive(true);
            interactionLabel1.text = zInteractionLabel1;
        }
        else
        {
            interactionButton1.EnsureActive(false);
        }

        if (string.IsNullOrEmpty(zInteractionLabel2) == false)
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

    private void TogglePause()
    {
        PlaySession.Pause();
        gameObject.EnsureActive(false);
        InGamePause.Instance.gameObject.EnsureActive(true);
    }
}

