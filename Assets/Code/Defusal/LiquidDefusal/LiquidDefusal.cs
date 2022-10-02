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

    public MeshRenderer requirementRenderer;
    private LiquidDefusalInstruction Progress => progress as LiquidDefusalInstruction;
    
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void UpdateInternal()
    {
        if(Progress.haveBeaker == false)
            return;

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

        var liquid = instruction as LiquidDefusalInstruction;
        var material = ColourProvider.GetMaterial(liquid.FinalColour);
        requirementRenderer.material= material;
    }

    protected override void StartDefusalInternal()
    {        
        //make sure the colour of our renderer is correct:
        var colour = ColourProvider.GetColourMix(Progress.colourOrder);

        beakerRenderer.material = ColourProvider.GetMaterial(colour);
        
        beaker.EnsureActive(Progress.haveBeaker);

        beakerFilling.EnsureActive(true);

        receptacleFilling.EnsureActive(false);
    }

    protected override void CancelInternal()
    {
        base.CancelInternal();
        beaker.EnsureActive(false);
    }


    IEnumerator<YieldInstruction> CoAttemptDefusal()
    {
        busy = true;
        yield return new WaitForSeconds(0.5f);

        var result = AttemptDefusal(progress);
        
        busy = false;

        beaker.EnsureActive(false);

        if(Defused)
        {
            yield return new WaitForSeconds(0.7f);
            Room.Instance.CancelDefusal();
        }
        
        if(result == false)
        {
            Room.Instance.Explode();
        }
    }

}


