using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveGame
{
    private static string PlayerPrefsPrefix => Application.dataPath + "Save";
    private static string bestAttemptKey => PlayerPrefsPrefix + "_Save_";

    private static int? bestAttempt;
    public static int? BestAttempt => bestAttempt;
    public static void Load()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        var value = PlayerPrefs.GetInt(bestAttemptKey, -1);
        if (value != -1)
        {
            bestAttempt = value;
        }

#endif
    }
    public static void Save()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (bestAttempt.HasValue)
            PlayerPrefs.SetInt(bestAttemptKey, bestAttempt.Value);
        PlayerPrefs.Save();
#endif
    }

    public static void RecordAttempt(int zCount)
    {
        if (bestAttempt.HasValue && bestAttempt.Value < zCount)
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
    public static bool Paused { get; private set; }

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