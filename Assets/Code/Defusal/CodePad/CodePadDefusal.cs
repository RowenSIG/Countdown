using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodePadDefusal : DefusalBase
{

    public List<CodePadButton> buttons;
    public Transform finger;
    public Animator fingerAnim ;
    public int currentButtonValue = 0;
    private CodePadButton CurrentButton => buttons.Find(p => p.buttonValue == currentButtonValue);

    private CodeDefusalInstruction Progress => progress as CodeDefusalInstruction;

    public NumericalDisplay codeDisplay;

    public override eDefusalType Type => eDefusalType.CODE;

    public int CodeLength = 3;

    protected override void Awake()
    {
        finger.gameObject.EnsureActive(false);
        base.Awake();
    }

    protected override void SetupInternal()
    {
        finger.gameObject.EnsureActive(false);
    }

    protected override void StartDefusalInternal()
    {
        finger.gameObject.EnsureActive(true);
        SetFingerPos(0);
    }

    private void SetFingerPos(int zValue)
    {
        currentButtonValue = zValue;
        var target = buttons.Find(p => p.buttonValue == zValue);
        finger.position = target.fingerAnchor.position;
    }

    protected override void UpdateInternal()
    {
        CheckNavigation();
        CheckPress();

        UpdateDisplay();
    }

    protected override void CancelInternal()
    {
        base.CancelInternal();
        finger.gameObject.EnsureActive(false);
    }



    private void CheckNavigation()
    {
        var currentButton = CurrentButton;
        var horizontal = DpadHorizontal();
        var vertical = DpadVertical(); 

        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            horizontal -= 1;
        if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            horizontal += 1;
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            vertical += 1;
        if(Input.GetKeyDown(KeyCode.DownArrow)|| Input.GetKeyDown(KeyCode.S))
            vertical -= 1;

        

        if (horizontal < 0 && currentButton.left != null)
        {
            SetFingerPos(currentButton.left.buttonValue);
            return;
        }

        if (horizontal > 0 && currentButton.right != null)
        {
            SetFingerPos(currentButton.right.buttonValue);
            return;
        }

        if(vertical > 0 && currentButton.up != null)
        {
            SetFingerPos(currentButton.up.buttonValue);
            return;
        }

        if(vertical < 0 && currentButton.down != null)
        {
            SetFingerPos(currentButton.down.buttonValue);
            return;
        }
    }

    private void CheckPress()
    {
        var pressButton = false;
        pressButton |= PlayerTouchControls.GetButtonDown(ePadButton.FACE_DOWN); 
        pressButton |= PlayerInputManager.GetButtonDown(0, ePadButton.FACE_DOWN);
        pressButton |= Input.GetKeyDown(KeyCode.E);
        if(pressButton && Progress.code.Count < CodeLength)
        {
            Progress.code.Add(currentButtonValue);
            
            //CurrentButton.buttonAnim.SetTrigger("Press");
            //fingerAnim.SetTrigger("Press");            

            StartCoroutine(CoAttemptDefusal());
        }
    }

    private void UpdateDisplay()
    {
        codeDisplay.UpdateDisplay(Progress.code);
    }

    IEnumerator<YieldInstruction> CoAttemptDefusal()
    {
        busy = true;

        yield return new WaitForSeconds(0.3f);

        var result = AttemptDefusal(progress);

        busy = false;
        
        if(Defused)
        {
            PlayerTouchControls.visualState = eTouchScreenVisual.TWIN_STICK_BUTTONS;

            finger.gameObject.EnsureActive(false);
            yield return new WaitForSeconds(0.7f);
            Room.Instance.DefuseProgress(this);
            Room.Instance.CancelDefusal();
        }
        
        if(result == false)
        {
            PlayerTouchControls.visualState = eTouchScreenVisual.TWIN_STICK_BUTTONS;
            Room.Instance.Explode();
        }
    }    
}


