using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beaker : CollectableItem
{
    public eColour colour;

    public GameObject filling;
    public MeshRenderer fillingRenderer;

    public void Setup(eColour zColour)
    {
        colour = zColour;
        var material = ColourProvider.GetMaterial(zColour);
        fillingRenderer.material = material;
        filling.EnsureActive(true);
    }

    public override void Reset()
    {
        base.Reset();
        filling.EnsureActive(false);
    }
}
