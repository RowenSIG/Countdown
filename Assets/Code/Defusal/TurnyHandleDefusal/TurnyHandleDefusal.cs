using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnyHandleDefusal : DefusalBase
{
    public override eDefusalType Type => eDefusalType.TURNY_HANDLE;

    private TurnyHandleDefusalInstruction Progress => progress as TurnyHandleDefusalInstruction;

    public GameObject turnyHandle;

    public Transform neutralRotation;
    public Transform counterClockwiseRotation;
    public Transform clockwiseRotation;

    public DefusalLight lightZero;
    public DefusalLight lightOne;
    public DefusalLight lightTwo;

    protected override void Awake()
    {
        base.Awake();

        var turny = new TurnyHandleDefusalInstruction();
        turny.order = new List<eTurnDirection>() { eTurnDirection.CLOCKWISE, eTurnDirection.COUNTER_CLOCKWISE, eTurnDirection.COUNTER_CLOCKWISE };
        SetupWithInstruction(turny);

        progress = new TurnyHandleDefusalInstruction();
    }
    protected override void SetupInternal()
    {
        turnyHandle.transform.rotation = neutralRotation.rotation;

        lightZero.SetIdle();
        lightOne.SetIdle();
        lightTwo.SetIdle();
    }
    protected override void StartDefusalInternal()
    {
        turnyHandle.transform.rotation = neutralRotation.rotation;
    }

    protected override void UpdateInternal()
    {
        CheckTurnyInput();
    }

    private void CheckTurnyInput()
    {
        var horizontal = DpadHorizontal();
        
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            horizontal -= 1;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            horizontal += 1;

        if (horizontal < 0)
        {
            Progress.order.Add(eTurnDirection.COUNTER_CLOCKWISE);
            StartCoroutine(CoAttemptDefusal());
        }

        else if (horizontal > 0)
        {
            Progress.order.Add(eTurnDirection.CLOCKWISE);
            StartCoroutine(CoAttemptDefusal());
        }
    }

    private IEnumerator<YieldInstruction> CoAttemptDefusal()
    {
        busy = true;

        yield return new WaitForSeconds(0.2f);

        //turn handle
        var lastTurn = LastTurn();

        if (lastTurn == eTurnDirection.CLOCKWISE)
            turnyHandle.transform.rotation = clockwiseRotation.rotation;
        else if (lastTurn == eTurnDirection.COUNTER_CLOCKWISE)
            turnyHandle.transform.rotation = counterClockwiseRotation.rotation;

        bool success = AttemptDefusal(progress);

        if (success)
        {
            switch (Progress.order.Count - 1)
            {
                default:
                case 0:
                    lightZero.SetDefused();
                    break;

                case 1:
                    lightOne.SetDefused();
                break;

                case 2:
                    lightTwo.SetDefused();
                    break;
            }
        }
        else
        {
            lightZero.SetExploded();
            lightOne.SetExploded();
            lightTwo.SetExploded();
        }
        yield return new WaitForSeconds(0.7f);

        turnyHandle.transform.rotation = neutralRotation.rotation;
        busy = false;

        if(Defused)
        {
            PlayerTouchControls.visualState = eTouchScreenVisual.TWIN_STICK_BUTTONS;
            yield return new WaitForSeconds(0.7f);
            Room.Instance.DefuseProgress(this);
            Room.Instance.CancelDefusal();
        }

        if(success == false)
        {
            PlayerTouchControls.visualState = eTouchScreenVisual.TWIN_STICK_BUTTONS;
            Room.Instance.Explode();
        }
    }

    private eTurnDirection LastTurn()
    {
        return Progress.order[Progress.order.Count - 1];
    }
}
