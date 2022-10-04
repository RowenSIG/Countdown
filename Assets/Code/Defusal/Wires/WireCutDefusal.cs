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
        cutters.EnsureActive(false);
    }
    protected override void UpdateInternal()
    {
        if (Progress.haveWireCutters == false)
            return;
        UpdateMovement();
        UpdateCutInput();
    }

    void UpdateMovement()
    {
        var move = DpadHorizontal();

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            move += 1;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
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
        var button = PlayerInputManager.GetButtonDown(0, ePadButton.FACE_DOWN);
        button |= Input.GetKeyDown(KeyCode.E);
        if (button)
        {
            Progress.chosenWireIndex = CurrentColourIndex;

            cutterAnim.SetTrigger("Play");

            StartCoroutine(CoAttemptDefusal());

        }
    }

    protected override void SetupInternal()
    {
        var colourInstruction = instruction as WireCutDefusalInstruction;

        for (int i = 0; i < colourInstruction.wireColours.Count; i++)
        {
            wires[i].Setup(colourInstruction.wireColours[i]);
        }

    }

    protected override void StartDefusalInternal()
    {
        cutters.EnsureActive(Progress.haveWireCutters);
    }


    protected override void CancelInternal()
    {
        base.CancelInternal();
        cutters.EnsureActive(false);
    }


    IEnumerator<YieldInstruction> CoAttemptDefusal()
    {
        busy = true;

        yield return new WaitForSeconds(0.1f);

        wires[CurrentColourIndex].PlayCut();

        yield return new WaitForSeconds(0.5f);

        var result = AttemptDefusal(progress);

        cutters.EnsureActive(false);
        busy = false;

        if (Defused)
        {
            yield return new WaitForSeconds(0.7f);
            Room.Instance.DefuseProgress(this);
            Room.Instance.CancelDefusal();
        }

        if (result == false)
        {
            Room.Instance.Explode();
        }
    }
}
