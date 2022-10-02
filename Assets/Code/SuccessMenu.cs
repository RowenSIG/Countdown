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

        previousBestArea.EnsureActive(SaveGame.BestAttempt.HasValue);
        if(SaveGame.BestAttempt.HasValue)
        {
            previousBest.text = SaveGame.BestAttempt.Value.ToString();
        }
    }

    private void Update()
    {
        bool padInput = PlayerInputManager.GetButtonDown(0, ePadButton.FACE_DOWN);
        if(padInput)
            OnReturnToMenu();
    }
    public void OnReturnToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
