using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuccessMenu : MonoBehaviour
{
    public GameObject previousBestArea;
    public Text previousBest;

    public Text sessionScore;
    public void Start()
    {
        sessionScore.text = PlaySession.attempts.ToString();

        if(SaveGame.BestAttempt.HasValue)
        {
            previousBestArea.EnsureActive(true);
            previousBest.text = SaveGame.BestAttempt.Value.ToString();
        }
    }

    public void OnReturnToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
