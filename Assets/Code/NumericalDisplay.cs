using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumericalDisplay : MonoBehaviour
{   

    public List<NumberDigits> numbers;

    public void UpdateDisplay(List<int> zList)
    {
        for(int i = 0 ; i < zList.Count; i++)
        {
            var value = zList[i];
            
            var numberIndex = i;
            var number = numbers[numberIndex];
            
            number.Set(value);
        }
        var clearCount = zList.Count - numbers.Count;
        for(int i = 0 ; i < clearCount; i++)
        {
            var number = numbers[i];
            number.Clear();
        }
    } 

    //update with just a plain number...
}
