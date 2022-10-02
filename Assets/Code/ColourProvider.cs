using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourProvider : MonoBehaviour
{
    public static ColourProvider Instance {get ; private set;}

    [System.Serializable]
    public class ColourMatch
    {
        public eColour colour;
        public Color color;
        public Material material;
    }

    [System.Serializable]
    public class ColourMix
    {
        public List<eColour> colourOrder;
        public eColour result;
    }

    [SerializeField]
    private List<ColourMatch> colours;

    [SerializeField]
    public List<ColourMix> colourMixes;
    

    public void Awake()
    {
        Instance = this;
    }
    public void OnDestroy()
    {
        Instance = null;
    }

    public static Color GetColor(eColour zColour)
    {
        var match = Instance.colours.Find(p => p.colour == zColour);
        if(match == null)
            return default;
        return match.color;
    }
    public static Material GetMaterial(eColour zColour)
    {
        var match = Instance.colours.Find(p => p.colour == zColour);
        if(match == null)
            return null;
        return match.material;
    }

    public static eColour GetColourMix(List<eColour> zColours)
    {
        foreach(var mix in Instance.colourMixes)
        {
            if(mix.colourOrder.Count != zColours.Count)
                continue;

            bool success = true;
            for(int i = 0 ; i < zColours.Count; i++)
            {
                if(mix.colourOrder[i] != zColours[i])
                    success = false;
            }

            if(success)
                return mix.result;
        }

        //fail.
        return eColour.BROWN;
    }
}


