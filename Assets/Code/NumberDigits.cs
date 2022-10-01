using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberDigits : MonoBehaviour
{
    public List<GameObject> digits;
    public GameObject noValue;

    int? value;
    public void Show(int zValue)
    {
        if (value.HasValue && value == zValue)
            return;
        value = zValue;
        for (int i = 0; i < digits.Count; i++)
        {
            digits[i].EnsureActive(i == zValue);
        }
        noValue.EnsureActive(false);
    }
    public void Clear()
    {
        value = null;
        noValue.EnsureActive(true);
        foreach (var digit in digits)
            digit.EnsureActive(false);
    }
}
