using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class PlayerTouchControls : MonoBehaviour
{
    public static PlayerTouchControls Instance { get; private set; }
    private Vector2? touch1Down;
    private Vector2? touch2Down;

    private Vector2? leftTouchDown;
    private Vector2? leftTouchPos;
    private Vector2? rightTouchDown;
    private Vector2? rightTouchPos;

    private float screenDeadZoneInches = 0.3f;
    private float screenPartialZoneInches = 0.2f;
    private float deadZoneSize => screenDeadZoneInches * Screen.dpi;
    private float partialZone => screenPartialZoneInches * Screen.dpi;

    private Rect leftOfScreen = new Rect(0, 0, Screen.width * 0.4f, Screen.height);
    private Rect rightOfScreen = new Rect(0.6f * Screen.width, 0, Screen.width * 0.4f, Screen.height);

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
        var touch1Pos = GetTouchPos(leftOfScreen);
        var (touch1, dir1) = GetStickState(touch1Pos, touch1Down);
        touch1Down = touch1;
        padState.leftStick = dir1;

         var touch2Pos = GetTouchPos(rightOfScreen);
        var (touch2, dir2) = GetStickState(touch2Pos, touch2Down);
        touch2Down = touch2;
        padState.rightStick = dir2;
    }
    private void UpdateX()
    {
        var touch1Pos = GetTouchPos(0);
        var (touch1, dir1) = GetStickState(touch1Pos, touch1Down);
        touch1Down = touch1;
        padState.leftStick = dir1;

        var touch2Pos = GetTouchPos(1);
        var (touch2, dir2) = GetStickState(touch2Pos, touch2Down);
        touch2Down = touch2;
        padState.rightStick = dir2;
    }

    private void LateUpdate()
    {
        padState.button1 = false;
        padState.button2 = false;
        padState.button3 = false;
    }

    private Vector2? GetTouchPos(Rect zScreenRect)
    {
        var touches = Input.touches;
        for(int i = 0 ; i< touches.Length; i++)
        {
            var touch = touches[i];

            if(zScreenRect.Contains(touch.position))
                    return touch.position;
        }
        return null;
    }
    private Vector2? GetTouchPos(int zTouchId)
    {
         bool hasTouch = Input.touchCount > zTouchId;

        if (hasTouch == false)
        {
            return null;
        }
        else
        {
            var touch = Input.touches[zTouchId];
            var pos = touch.position;
            return pos;
        }
    }
    private (Vector2?, Vector2) GetStickState(Vector2? zPos, Vector2? zDown)
    {
        if (zPos == null)
        {
            return (null, Vector2.zero);
        }
        else
        {
            var pos = zPos.Value;

            var from = pos;
            if (zDown.HasValue)
            {
                from = zDown.Value;
            }

            var point = (pos - from);
            var dist = point.magnitude;
            var mult = 1f;

            if(dist < deadZoneSize)
            {
                point = Vector2.zero;
            }
            else if(dist < (deadZoneSize + partialZone))
            {
                mult = Mathf.InverseLerp(deadZoneSize, deadZoneSize + partialZone, dist);
            }

            point = point.normalized * mult;
            return (from, point);

        }
    }


    public void OnButton1()
    {
        padState.button1 = true;
    }
    public void OnButton2()
    {
        padState.button2 = true;
    }
    public void OnButton3()
    {
        padState.button3 = true;
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
