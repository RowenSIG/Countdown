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

    bool busy = false;

    private CodeDefusalInstruction Progress => progress as CodeDefusalInstruction;

    public NumericalDisplay codeDisplay;

    public override eDefusalType Type => eDefusalType.CODE;

    public int CodeLength = 3;

    protected override void Awake()
    {
        var instruction = new CodeDefusalInstruction();
        instruction.code = new List<int>() { 1, 2, 3 };
        SetupWithInstruction(instruction);

        progress = new CodeDefusalInstruction();

        finger.gameObject.EnsureActive(false);

        base.Awake();
    }
    protected override void SetupInternal()
    {
        progress = new CodeDefusalInstruction();
        finger.gameObject.EnsureActive(false);
        
    }

    protected override void StartDefusalInternal()
    {
        finger.gameObject.EnsureActive(true);
        progress = new CodeDefusalInstruction();
        
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
        if(busy)
            return;
        CheckNavigation();
        CheckPress();

        UpdateDisplay();
    }
    private void CheckNavigation()
    {
        var currentButton = CurrentButton;
        var horizontal = PlayerInputManager.GetAxis(0, ePadAxis.DPAD_HORIZONTAL);
        var vertical = PlayerInputManager.GetAxis(0, ePadAxis.DPAD_VERTICAL);

        if(Input.GetKeyDown(KeyCode.LeftArrow))
            horizontal -= 1;
        if(Input.GetKeyDown(KeyCode.RightArrow))
            horizontal += 1;
        if(Input.GetKeyDown(KeyCode.UpArrow))
            vertical += 1;
        if(Input.GetKeyDown(KeyCode.DownArrow))
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
        var pressButton = Input.GetKeyDown(KeyCode.E);// PlayerInputManager.GetButtonDown(0, ePadButton.FACE_DOWN);
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
        yield return new WaitForSeconds(0.5f);

        AttemptDefusal(progress);

        busy = false;
    }
}


