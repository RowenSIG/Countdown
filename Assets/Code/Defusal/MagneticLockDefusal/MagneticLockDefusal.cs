using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticLockDefusal : DefusalBase
{
    public override eDefusalType Type => eDefusalType.MAGNETIC_LOCK;

    public Battery handBattery1;
    public Battery handBattery2;

    public Battery boardBattery1;
    public Battery boardBattery2;

    public NumberDigits requiredNumber;
    public NumberDigits actualNumber;

    private MagneticLockDefusalInstruction Progress => progress as MagneticLockDefusalInstruction;

    protected override void Awake()
    {
        base.Awake();

        var instruction = new MagneticLockDefusalInstruction();
        instruction.voltage = 8;
        SetupWithInstruction(instruction);

        progress = new MagneticLockDefusalInstruction();
        Progress.voltage = 8;
        Progress.battery1 = 5;
        Progress.battery2 = 3;
    }


    protected override void UpdateInternal()
    {
        var place = PlayerInputManager.GetButtonDown(0, ePadButton.FACE_DOWN);
        place |= Input.GetKeyDown(KeyCode.E);

        if (place)
        {
            handBattery1.gameObject.EnsureActive(false);
            handBattery2.gameObject.EnsureActive(false);

            boardBattery1.gameObject.EnsureActive(true);
            boardBattery1.Setup(Progress.battery1);

            if (Progress.battery2 != 0)
            {
                boardBattery2.gameObject.EnsureActive(true);
                boardBattery2.Setup(Progress.battery2);
            }

            StartCoroutine(CoAttemptDefusal());
        }
    }

    protected override void SetupInternal()
    {
        handBattery1.gameObject.EnsureActive(false);
        handBattery2.gameObject.EnsureActive(false);
    }

    protected override void StartDefusalInternal()
    {
        boardBattery1.gameObject.EnsureActive(false);
        boardBattery2.gameObject.EnsureActive(false);

        requiredNumber.Show(Progress.voltage);
        actualNumber.Clear();

        ShowHandBatteries();
    }

    private void ShowHandBatteries()
    {
        actualNumber.Clear();

        if (Progress.battery1 != 0)
        {
            handBattery1.gameObject.EnsureActive(true);
            handBattery1.Setup(Progress.battery1);
        }
        else
        {
            handBattery1.gameObject.EnsureActive(false);
        }


        if (Progress.battery2 != 0)
        {
            handBattery2.gameObject.EnsureActive(true);
            handBattery2.Setup(Progress.battery2);
        }
        else
        {
            handBattery2.gameObject.EnsureActive(false);
        }
    }


    private IEnumerator<YieldInstruction> CoAttemptDefusal()
    {
        busy = true;
        yield return new WaitForSeconds(0.2f);

        actualNumber.Show(Progress.voltage);

        yield return new WaitForSeconds(1f);

        AttemptDefusal(Progress);

        busy= false;
        
        if(Defused)
        {
            yield return new WaitForSeconds(0.7f);
            Room.Instance.CancelDefusal();
        }
    }
}

