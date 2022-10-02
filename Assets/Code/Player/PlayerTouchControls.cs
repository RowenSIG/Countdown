using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchControls : MonoBehaviour
{
    public static PlayerTouchControls Instance { get; private set; }
    private Vector2? touch1Down;
    private Vector2? touch2Down;

    [System.Serializable]
    private class GamePadState
    {
        public Vector2 leftStick;
        public Vector2 rightStick;

        public bool button1;
        public bool button2;
        public bool button3;
    }

    private GamePadState padState = new GamePadState();

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
        var (touch1, dir1) = GetStickState(0, touch1Down);
        touch1Down = touch1;
        padState.leftStick = dir1;

        var (touch2, dir2) = GetStickState(1, touch2Down);
        touch2Down = touch2;
        padState.rightStick = dir2;
    }

    private void LateUpdate()
    {
        padState.button1 = false;
        padState.button2 = false;
        padState.button3 = false;
    }

    private (Vector2?, Vector2) GetStickState(int zTouchId, Vector2? zDown)
    {
        bool hasTouch = Input.touchCount > zTouchId;

        if (hasTouch == false)
        {
            return (null, Vector2.zero);
        }
        else
        {
            var touch = Input.touches[zTouchId];
            var pos = touch.position;

            var from = pos;
            if (zDown.HasValue)
            {
                from = zDown.Value;
            }

            var dir = pos - from;
            return (from, dir);
        }
    }


    public void OnButton1()
    {
        padState.button1 = true;
    }
    public void OnButton2()
    {
        padState.button1 = true;
    }
    public void OnButton3()
    {
        padState.button1 = true;
    }


    public static float GetAxis(ePadAxis zAxis)
    {
        switch(zAxis)
        {
            case ePadAxis.L_STICK_VERTICAL:
            case ePadAxis.DPAD_VERTICAL:
                return Instance.padState.leftStick.y;
            
            case ePadAxis.L_STICK_HORIZONTAL:
            case ePadAxis.DPAD_HORIZONTAL:
                return Instance.padState.leftStick.x;

            case ePadAxis.R_STICK_VERTICAL:
                return Instance.padState.rightStick.y;
            case ePadAxis.R_STICK_HORIZONTAL:
                return Instance.padState.rightStick.x;
        }
        return 0f;
    }
    public static bool GetButtonDown(ePadButton zButton)
    {
        switch(zButton)
        {
            case ePadButton.FACE_DOWN: return Instance.padState.button1;

            case ePadButton.FACE_RIGHT: return Instance.padState.button2;

            case ePadButton.FACE_UP: return Instance.padState.button3;
        }
        return false;
    }
}
