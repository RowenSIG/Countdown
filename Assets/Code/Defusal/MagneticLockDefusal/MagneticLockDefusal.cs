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

    public NumberDigits requiredNumberTens;
    public NumberDigits requiredNumberOnes;
    public NumberDigits actualNumberTens;
    public NumberDigits actualNumberOnes;

    private MagneticLockDefusalInstruction Progress => progress as MagneticLockDefusalInstruction;

    public Animator lockOpenAnimator;

    protected override void UpdateInternal()
    {
        var place = PlayerInputManager.GetButtonDown(0, ePadButton.FACE_DOWN);
        place |= PlayerTouchControls.GetButtonDown(ePadButton.FACE_DOWN); 
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
        boardBattery1.gameObject.EnsureActive(false);
        boardBattery2.gameObject.EnsureActive(false);
        handBattery1.gameObject.EnsureActive(false);
        handBattery2.gameObject.EnsureActive(false);

        var batteryInstruction = instruction as MagneticLockDefusalInstruction;
        var tens = batteryInstruction.Voltage / 10;
        var ones = batteryInstruction.Voltage % 10;
        requiredNumberTens.Set(tens);
        requiredNumberOnes.Set(ones);

        actualNumberTens.Clear();
        actualNumberOnes.Clear();
    }

    protected override void StartDefusalInternal()
    {
        boardBattery1.gameObject.EnsureActive(false);
        boardBattery2.gameObject.EnsureActive(false);

        ShowHandBatteries();
    }

    private void ShowHandBatteries()
    {
        actualNumberTens.Clear();
        actualNumberOnes.Clear();

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

    protected override void CancelInternal()
    {
        base.CancelInternal();
        handBattery1.gameObject.EnsureActive(false);
        handBattery2.gameObject.EnsureActive(false);
    }



    private IEnumerator<YieldInstruction> CoAttemptDefusal()
    {
        busy = true;
        PlayerTouchControls.visualState = eTouchScreenVisual.TWIN_STICK_BUTTONS;

        yield return new WaitForSeconds(0.2f);

        var tens = Progress.Voltage / 10;
        var ones = Progress.Voltage % 10;
        actualNumberTens.Set(tens);
        actualNumberOnes.Set(ones);

        yield return new WaitForSeconds(1f);

        var result = AttemptDefusal(Progress);

        if(result)
        {
            lockOpenAnimator.SetTrigger("Play");
        }

        busy= false;
        
        if(Defused)
        {
            yield return new WaitForSeconds(0.7f);
            Room.Instance.DefuseProgress(this);
            Room.Instance.CancelDefusal();
        }
        
        if(result == false)
        {
            Room.Instance.Explode();
        }
    }
}

