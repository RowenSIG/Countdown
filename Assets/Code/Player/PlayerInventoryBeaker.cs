using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryBeaker : PlayerInventoryItem
{
    private List<eColour> colourOrder = new List<eColour>();
    public MeshRenderer fillRenderer;
    public override void Collected(CollectableItem zItem)
    {
        base.Collected(zItem);

        //how do we know what we have?!
        var beaker = zItem as Beaker;
        if(colourOrder.Count < 2)
        {
            colourOrder.Add(beaker.colour);
        }
        else
        {
            var mix = ColourProvider.GetColourMix(colourOrder);
            colourOrder[0] = mix;
            colourOrder[1] = beaker.colour;
        }

        if(colourOrder.Count == 1)
        {
            var material = ColourProvider.GetMaterial(colourOrder[0]);
            fillRenderer.material= material;
        }
        else if(colourOrder.Count > 1)
        {
            var mix = ColourProvider.GetColourMix(colourOrder);
            var material = ColourProvider.GetMaterial(mix);
            fillRenderer.material = material;
        }
    }
}

