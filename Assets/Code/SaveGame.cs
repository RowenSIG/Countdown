using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveGame
{
    private static  string PlayerPrefsPrefix => Application.dataPath + "Save";
    private static string bestAttemptKey => PlayerPrefsPrefix + "_Save_";

    private static int? bestAttempt;
    public static int? BestAttempt => bestAttempt;
    public static void Load()
    {
        // var value = PlayerPrefs.GetInt(bestAttemptKey, -1);
        // if(value != -1)
        // {
        //     bestAttempt = value;
        // }
    }
    public static void Save()
    {
        // if(bestAttempt.HasValue)
        //     PlayerPrefs.SetInt(bestAttemptKey, bestAttempt.Value);
        // PlayerPrefs.Save();
    }

    public static void RecordAttempt(int zCount)
    {
        if(bestAttempt.HasValue && bestAttempt.Value < zCount)
        {
            return;
        }
        bestAttempt = zCount;
        Save();
    }
}

public static class PlaySession
{
    public static int attempts = 1;
    public static bool Paused { get ; private set; }

    public static void Start()
    {
        attempts = 1;
        Paused = false;
    }

    public static void Pause()
    {
        Paused = true;
    }
    public static void UnPause()
    {
        Paused = false;
    }
    public static void Success()
    {
        SaveGame.RecordAttempt(attempts);
    }
}