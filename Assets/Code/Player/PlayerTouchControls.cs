using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTouchScreenVisual
{
    TWIN_STICK_BUTTONS = 0,
    DPAD_BUTTONS = 1
}

[DefaultExecutionOrder(-100)]
public class PlayerTouchControls : MonoBehaviour
{
    public static PlayerTouchControls Instance { get; private set; }

    public static eTouchScreenVisual visualState = eTouchScreenVisual.TWIN_STICK_BUTTONS;

    private Vector2? touch1Down;
    private Vector2? touch2Down;

    private Vector2? leftTouchDown;
    private Vector2? leftTouchPos;
    private Vector2? rightTouchDown;
    private Vector2? rightTouchPos;

    private float screenDeadZoneInches = 0.2f;
    private float screenPartialZoneInches = 0.5f;
    private float deadZoneSize => screenDeadZoneInches * Screen.dpi;
    private float partialZone => screenPartialZoneInches * Screen.dpi;

    private Rect leftOfScreen = new Rect(0, 0, Screen.width * 0.4f, Screen.height);
    private Rect rightOfScreen = new Rect(0.6f * Screen.width, 0, Screen.width * 0.4f, Screen.height);

    public GameObject dpad;
    public GameObject notDpad;

    [System.Serializable]
    private class GamePadState
    {
        public Vector2 leftStick;
        public Vector2 rightStick;

        public Vector2 dpad;

        public bool button1;
        public bool button2;
        public bool button3;

        public bool start;
    }

    private GamePadState padState = new GamePadState();

    private void Awake()
    {
        Instance = this;
        visualState = eTouchScreenVisual.TWIN_STICK_BUTTONS;
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    private void Update()
    {

        padState.dpad = Vector2.zero;
        padState.leftStick = Vector2.zero;
        padState.rightStick = Vector2.zero;
        padState.button1 = button1;
        padState.button2 = button2;
        padState.button3 = button3;

        button1 = false;
        button2 = false;
        button3 = false;


        var touch1Pos = GetTouchPos(leftOfScreen);
        var (touch1, dir1) = GetStickState(touch1Pos, touch1Down);
        touch1Down = touch1;
        padState.leftStick = dir1;

         var touch2Pos = GetTouchPos(rightOfScreen);
        var (touch2, dir2) = GetStickState(touch2Pos, touch2Down);
        touch2Down = touch2;
        padState.rightStick = dir2;

        padState.dpad.x = dpadx;
        padState.dpad.y = dpady;
        dpadx = 0;
        dpady = 0;

        padState.start = buttonx;
        buttonx = false;

        dpad.EnsureActive(visualState == eTouchScreenVisual.DPAD_BUTTONS);
        notDpad.EnsureActive(visualState != eTouchScreenVisual.DPAD_BUTTONS);
    }
   
    private Vector2? GetTouchPos(Rect zScreenRect)
    {
        var touchCount = Input.touchCount;
        var touches = Input.touches;
        for(int i = 0 ; i< touchCount; i++)
        {
            var touch = touches[i];
            if(touch.fingerId == 0)
                continue;
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


    bool button1;
    public void OnButton1()
    {
        button1 = true;
    }
    bool button2;
    public void OnButton2()
    {
        button2 = true;
    }
    bool button3;
    public void OnButton3()
    {
        button3 = true;
    }
    

    
    int dpady;
    public void OnButtonDpadUp()
    {
        dpady = 1;
    }
    public void OnButtonDpadDown()
    {
        dpady = -1;
    }
    int dpadx;
    public void OnButtonDpadRight()
    {
        dpadx = 1;
    }
    public void OnButtonDpadLeft()
    {
        dpadx = -1;
    }

    bool buttonx;
    public void OnX()
    {
        buttonx = true;
    }

    public static float GetAxis(ePadAxis zAxis)
    {
        switch(zAxis)
        {
            case ePadAxis.DPAD_VERTICAL:
                return Instance.padState.dpad.y;
            case ePadAxis.DPAD_HORIZONTAL:
                return Instance.padState.dpad.x;
            
            case ePadAxis.L_STICK_VERTICAL:
                return Instance.padState.leftStick.y;
            case ePadAxis.L_STICK_HORIZONTAL:
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

            case ePadButton.START: return Instance.padState.start;
        }
        return false;
    }

}
