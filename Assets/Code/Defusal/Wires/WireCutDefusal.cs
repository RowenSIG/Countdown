using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireCutDefusal : DefusalBase
{
    public override eDefusalType Type => eDefusalType.WIRE_CUT;

    public GameObject cutters;
    public Animator cutterAnim;

    [SerializeField]
    private List<WireCutWire> wires = new List<WireCutWire>();
    private eColour currentWireColour;

    private WireCutDefusalInstruction Progress => progress as WireCutDefusalInstruction;
    private int CurrentColourIndex => wires.FindIndex(0, p => p.Colour == currentWireColour);

    protected override void Awake()
    {
        base.Awake();

        var instruction = new WireCutDefusalInstruction();
        instruction.wireColours = new List<eColour>() { eColour.BLUE, eColour.RED, eColour.GREEN };
        instruction.chosenWireIndex = 2;
        SetupWithInstruction(instruction);

        progress = new WireCutDefusalInstruction();
        cutters.EnsureActive(false);
    }

    protected override void UpdateInternal()
    {
        UpdateMovement();
        UpdateCutInput();
    }

    void UpdateMovement()
    {
        var move = PlayerInputManager.GetAxis(0, ePadAxis.DPAD_HORIZONTAL);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            move += 1;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            move -= 1;

        var currentColourIndex = CurrentColourIndex;

        if (move < 0)
        {
            currentColourIndex -= 1;
        }
        else if (move > 0)
        {
            currentColourIndex += 1;
        }

        currentColourIndex = Mathf.Clamp(currentColourIndex, 0, wires.Count - 1);
        currentWireColour = wires[currentColourIndex].Colour;
        cutters.transform.position = wires[currentColourIndex].anchor.transform.position;
    }

    void UpdateCutInput()
    {
        var button = Input.GetKeyDown(KeyCode.E);
        if(button)
        {
            Progress.chosenWireIndex = CurrentColourIndex;
        
            // cutterAnim.SetTrigger("Cut");

            StartCoroutine(CoAttemptDefusal());

        }
    }

    protected override void SetupInternal()
    {
        var colourInstruction = instruction as WireCutDefusalInstruction;

        for(int i = 0 ; i < colourInstruction.wireColours.Count; i++)
        {
            wires[i].Setup(colourInstruction.wireColours[i]);
        }
    }

    protected override void StartDefusalInternal()
    {
        cutters.EnsureActive(true);
    }

    IEnumerator<YieldInstruction> CoAttemptDefusal()
    {
        busy = true;
        yield return new WaitForSeconds(0.5f);

        AttemptDefusal(progress);

        cutters.EnsureActive(false);
        busy = false;

        if(Defused)
        {
            yield return new WaitForSeconds(0.7f);
            Room.Instance.CancelDefusal();
        }
    }
}
