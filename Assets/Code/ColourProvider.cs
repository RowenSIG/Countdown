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

    [SerializeField]
    private List<ColourMatch> colours;

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
        return match.color;
    }
    public static Material GetMaterial(eColour zColour)
    {
        var match = Instance.colours.Find(p => p.colour == zColour);
        return match.material;
    }
}


