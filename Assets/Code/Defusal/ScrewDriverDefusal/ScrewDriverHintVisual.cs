using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewDriverHintVisual : MonoBehaviour
{
    [System.Serializable]
    public class PatternVisual
    {
        public int badScrewIndex;
        public GameObject pattern;
    }
    public List<PatternVisual> patterns;

    public void Setup(List<bool> zScrewOrder)
    {
        var badScrewIndex = 0;
        for (int i = 0; i < zScrewOrder.Count; i++)
        {
            if (zScrewOrder[i] == false)
                badScrewIndex = i;
        }

        foreach (var pattern in patterns)
        {
            if (pattern.badScrewIndex == badScrewIndex)
            {
                pattern.pattern.EnsureActive(true);
            }
            else
            {
                pattern.pattern.EnsureActive(false);
            }
        }
    }
    public void Reset()
    {
        foreach(var visual in patterns)
        {
            visual.pattern.EnsureActive(false);
        }
    }
}
