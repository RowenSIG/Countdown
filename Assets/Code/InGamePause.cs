using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGamePause : MonoBehaviour
{
    public static InGamePause Instance {get; private set;}

    public Text currentSessionScore;

    private void Awake()
    {
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    private void Update()
    {
        var unpause = PlayerInputManager.GetButtonDown(0 , ePadButton.START);
        unpause |= Input.GetKeyDown(KeyCode.Escape);

        if(unpause) 
            TogglePause();

        var attempts = PlaySession.attempts;
        currentSessionScore.text = attempts.ToString();
    }

    public void OnQuit()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    public void OnUnPause()
    {
        TogglePause();
    }

    private void TogglePause()
    {
        PlaySession.UnPause();
        gameObject.EnsureActive(false);
        InGameMenu.Instance.gameObject.EnsureActive(true);
    }
}
