using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrontendMenu : MonoBehaviour
{
    public GameObject topRight;
    public Text bestAttemptCount;
    public void LoadGame()
    {
        Player.UseTouchControls = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("BombScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    public void LoadGameTouchScreen()
    {
        Player.UseTouchControls = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("BombScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    void Update()
    {
        var input = PlayerInputManager.GetButtonDown(0, ePadButton.FACE_DOWN);
        if(input)
            LoadGame();
    }

    private void Start()
    {
        SaveGame.Load();
    
        var bestAttempt = SaveGame.BestAttempt;

        topRight.EnsureActive(bestAttempt.HasValue);
        if(bestAttempt.HasValue)
        {
            bestAttemptCount.text = bestAttempt.Value.ToString();
        }
    }

}
