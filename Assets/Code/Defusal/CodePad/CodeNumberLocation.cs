using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeNumberLocation : MonoBehaviour
{
    public NumberDigits indexNumber;
    public NumberDigits codeNumber;
    public void SetCodeNumber(int zIndex, int zNumber)
    {
        indexNumber.gameObject.EnsureActive(true);
        codeNumber.gameObject.EnsureActive(true);
        indexNumber.Show(zIndex);
        codeNumber.Show(zNumber);
    }

    public void Reset()
    {
        indexNumber.gameObject.EnsureActive(false);
        codeNumber.gameObject.EnsureActive(false);
    }
}
