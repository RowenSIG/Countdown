using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    private static Countdown instance;
    public static Countdown Instance => instance;

    private int countdown = 10;

    public void Awake()
    {
        instance = this;
    }
    public void OnDestroy()
    {
        instance = null;
    }

    public void SpendTime()
    {
        countdown -= 1;
    }
}
