using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidDefusal : DefusalBase
{
    public override eDefusalType Type => eDefusalType.LIQUID;

    public GameObject beaker;
    public GameObject beakerFilling;

    public MeshRenderer beakerRenderer;

    public GameObject receptacleFilling; 
    public MeshRenderer receptacleFillingRenderer;
    private LiquidDefusalInstruction Progress => progress as LiquidDefusalInstruction;
    
    protected override void Awake()
    {
        base.Awake();
    
        var instruction = new LiquidDefusalInstruction();
        instruction.colourOrder = new List<eColour> { eColour.BLUE, eColour.RED };
        SetupWithInstruction(instruction);

        progress = new LiquidDefusalInstruction();
        Progress.colourOrder = new List<eColour>() { eColour.BLUE, eColour.RED };

    }

    protected override void UpdateInternal()
    {
        //just check if we're clicking to pour:
        bool pour = PlayerInputManager.GetButtonDown(0, ePadButton.FACE_DOWN);

        pour |= Input.GetKeyDown(KeyCode.E);

        if(pour)
        {
            receptacleFilling.EnsureActive(true);
            var colour =  ColourProvider.GetColourMix(Progress.colourOrder);
            receptacleFillingRenderer.material = ColourProvider.GetMaterial(colour);

            beakerFilling.EnsureActive(false);

            StartCoroutine(CoAttemptDefusal());
        }
    }

    protected override void SetupInternal()
    {
        //hide beaker:
        beaker.EnsureActive(false);
        receptacleFilling.EnsureActive(false);

    }

    protected override void StartDefusalInternal()
    {
        //make sure the colour of our renderer is correct:
        var colour = ColourProvider.GetColourMix(Progress.colourOrder);

        beakerRenderer.material = ColourProvider.GetMaterial(colour);
        beaker.EnsureActive(true);
        beakerFilling.EnsureActive(true);

        receptacleFilling.EnsureActive(false);
    }


    IEnumerator<YieldInstruction> CoAttemptDefusal()
    {
        busy = true;
        yield return new WaitForSeconds(0.5f);

        AttemptDefusal(progress);
        busy = false;

        beaker.EnsureActive(false);
    }

}


