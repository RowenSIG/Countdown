using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireCutWire : MonoBehaviour
{
    public MeshRenderer topBit;
    public MeshRenderer bottomBit;

    public GameObject anchor;

    public eColour Colour {get ; private set;}
    public Animator cutAnimator;

    public void Setup(eColour zColour)
    {
        Colour = zColour;
        var material = ColourProvider.GetMaterial(zColour);
        topBit.material = material;
        bottomBit.material = material;
    }
    public void PlayCut()
    {
        cutAnimator.SetTrigger("Play");
    }
}
