using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewDriverDefusal : DefusalBase
{

    public override eDefusalType Type => eDefusalType.SCREW_DRIVER_PANEL;

    public List<GameObject> screws;
    public List<Transform> anchors;
    int currentScrewIndex;

    public GameObject screwDriver;

    public GameObject panelCover;

    private ScrewDriverDefusalInstruction Progress => progress as ScrewDriverDefusalInstruction;

    protected override void Awake()
    {
        base.Awake();

        var instruction = new ScrewDriverDefusalInstruction();
        instruction.order = new List<bool>() { true, false, true, true };
        SetupWithInstruction(instruction);

        progress = new ScrewDriverDefusalInstruction();
    }

    protected override void UpdateInternal()
    {
        if (Progress.haveScrewDriver)
        {
            CheckNavigation();
            CheckScrew();
        }
    }

    protected override void SetupInternal()
    {
        panelCover.EnsureActive(true);
        screwDriver.EnsureActive(false);
    }

    protected override void StartDefusalInternal()
    {
        SetScrewDriverPos(0);

        screwDriver.EnsureActive(Progress.haveScrewDriver);

        panelCover.EnsureActive(true);
        foreach (var screw in screws)
            screw.EnsureActive(true);
    }

    protected override void CancelInternal()
    {
        base.CancelInternal();
        screwDriver.EnsureActive(false);
    }


    private void SetScrewDriverPos(int zIndex)
    {
        currentScrewIndex = zIndex;
        screwDriver.transform.position = anchors[zIndex].position;
    }

    private void CheckScrew()
    {
        bool input = PlayerInputManager.GetButtonDown(0, ePadButton.FACE_DOWN);
        input |= Input.GetKeyDown(KeyCode.E);

        bool screwNotScrewed = screws[currentScrewIndex].activeSelf;
        if (input && screwNotScrewed)
        {
            StartCoroutine(CoAttemptDefusal());
        }
    }

    private void CheckNavigation()
    {
        var horizontal = PlayerInputManager.GetAxis(0, ePadAxis.DPAD_HORIZONTAL);
        var vertical = PlayerInputManager.GetAxis(0, ePadAxis.DPAD_VERTICAL);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            horizontal -= 1;
        if (Input.GetKeyDown(KeyCode.RightArrow))
            horizontal += 1;
        if (Input.GetKeyDown(KeyCode.UpArrow))
            vertical += 1;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            vertical -= 1;

        switch (currentScrewIndex)
        {
            case 0:
                if (horizontal > 0)
                    SetScrewDriverPos(1);
                else if (vertical < 0)
                    SetScrewDriverPos(2);
                break;

            case 1:
                if (horizontal < 0)
                    SetScrewDriverPos(0);
                else if (vertical < 0)
                    SetScrewDriverPos(3);
                break;

            case 2:
                if (horizontal > 0)
                    SetScrewDriverPos(3);
                else if (vertical > 0)
                    SetScrewDriverPos(0);
                break;

            case 3:
                if (horizontal < 0)
                    SetScrewDriverPos(2);
                else if (vertical > 0)
                    SetScrewDriverPos(1);
                break;
        }
    }

    private IEnumerator<YieldInstruction> CoAttemptDefusal()
    {
        busy = true;
        yield return new WaitForSeconds(0.5f);

        Progress.attemptIndex = currentScrewIndex;
        bool result = AttemptDefusal(progress);

        screws[currentScrewIndex].EnsureActive(false);

        if (Defused)
        {
            panelCover.EnsureActive(false);
        }

        yield return new WaitForSeconds(0.2f);

        busy = false;

        if (Defused)
        {
            screwDriver.EnsureActive(false);
            yield return new WaitForSeconds(0.7f);
            Room.Instance.CancelDefusal();
        }

        if (result == false)
        {
            Room.Instance.Explode();
        }

    }
}
