using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    private static Countdown instance;
    public static Countdown Instance => instance;

    public int TimeLeft {get ; private set;}

    public void Awake()
    {
        instance = this;
        Reset();
    }
    public void OnDestroy()
    {
        instance = null;
    }

    public void Reset()
    {
        TimeLeft = 100;
    }

    public void SpendTime()
    {
        TimeLeft -= 1;

        if(TimeLeft <= 0)
        {
            InGameMenu.Instance.TimeUp();
            Room.Instance.Explode();
        }
    }
}
