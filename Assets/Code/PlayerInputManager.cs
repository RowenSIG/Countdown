#define TOUCH_CONTROLS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePadButton
{
    FACE_UP = 10,
    FACE_DOWN = 11,
    FACE_RIGHT = 12,
    FACE_LEFT = 13,

    LEFT_BUMPER = 20,
    RIGHT_BUMPER = 21,

    START = 31,
    BACK = 32,

    L_STICK_IN = 40,
    R_STICK_IN = 41,
}

public enum ePadAxis
{
    L_STICK_VERTICAL = 0,
    L_STICK_HORIZONTAL = 1,
    R_STICK_VERTICAL = 2,
    R_STICK_HORIZONTAL = 3,

    DPAD_VERTICAL = 4,
    DPAD_HORIZONTAL = 5,
}

public enum ePadTrigger
{
    L_TRIGGER = 0,
    R_TRIGGER = 1,
}

public static class PlayerInputManager 
{
    private class ButtonMap
    {
        public ePadButton padButton;
        public string inputManagerString;
        public ButtonMap(ePadButton zPadButton, string zInputManagerString)
        {
            padButton = zPadButton;
            inputManagerString = zInputManagerString;
        }
    }

    private class AxisMap
    {
        public ePadAxis padAxis;
        public string inputManagerString;

        public AxisMap(ePadAxis zAxis, string zString)
        {
            padAxis = zAxis;
            inputManagerString = zString;
        }
    }

    private class TriggerMap
    {
        public ePadTrigger padTrigger;
        public string inputManagerString;

        public TriggerMap(ePadTrigger zTrigger, string zString)
        {
            padTrigger = zTrigger;
            inputManagerString = zString;
        }
    }

    private static List<ButtonMap> buttonMap = new List<ButtonMap>() 
    {
        new ButtonMap(ePadButton.FACE_DOWN, "Button0"),
        new ButtonMap(ePadButton.FACE_RIGHT, "Button1"),
        new ButtonMap(ePadButton.FACE_LEFT, "Button2"),
        new ButtonMap(ePadButton.FACE_UP, "Button3"),

        new ButtonMap(ePadButton.LEFT_BUMPER, "Button4"),
        new ButtonMap(ePadButton.RIGHT_BUMPER, "Button5"),

        new ButtonMap(ePadButton.BACK, "Button6"),
        new ButtonMap(ePadButton.START, "Button7"),

        new ButtonMap(ePadButton.L_STICK_IN, "Button8"),
        new ButtonMap(ePadButton.R_STICK_IN, "Button9"),
    };

    private static List<AxisMap> axisMap = new List<AxisMap>()
    {
        new AxisMap(ePadAxis.L_STICK_HORIZONTAL, "Axis0"),
        new AxisMap(ePadAxis.L_STICK_VERTICAL, "Axis1"),

        new AxisMap(ePadAxis.R_STICK_HORIZONTAL, "Axis2"),
        new AxisMap(ePadAxis.R_STICK_VERTICAL, "Axis3"),
      
        new AxisMap(ePadAxis.DPAD_HORIZONTAL, "Axis4"),
        new AxisMap(ePadAxis.DPAD_VERTICAL, "Axis5"),
    };

    private static List<TriggerMap> triggerMap = new List<TriggerMap>()
    {
        new TriggerMap(ePadTrigger.L_TRIGGER, "Trigger0"),
        new TriggerMap(ePadTrigger.R_TRIGGER, "Trigger1")
    };

    private static Dictionary<ePadButton, string> ButtonToInputMap  = BuildButtonDic();
    private static Dictionary<ePadAxis, string> AxisToInputMap = BuildAxisDic();
    private static Dictionary<ePadTrigger, string> TriggerToInputMap = BuildTriggerDic();
    private static Dictionary<int, string> PadToNameMap = new Dictionary<int, string>() 
    {
        { 0 , "Joy1" }, 
        { 1 , "Joy2" }, 
        { 2 , "Joy3" }, 
        { 3 , "Joy4" }, 
        { 4 , "Joy5" }, 
        { 5 , "Joy6" }, 
        { 6 , "Joy7" }, 
        { 7 , "Joy8" }, 
        { 8 , "Joy9" }, 
        { 9 , "Joy10" }, 
    };

    static Dictionary<int, Dictionary<ePadButton, int>> cache = new Dictionary<int, System.Collections.Generic.Dictionary<ePadButton, int>>() ;

    private static void CacheInput(int zPadIndex, ePadButton zButton)
    {
        if(cache.TryGetValue(zPadIndex, out var buttonCache) == false)
        {
            buttonCache = new Dictionary<ePadButton, int>();
            cache[zPadIndex] = buttonCache;
        }
        if(buttonCache.TryGetValue(zButton, out var frame) == false)
        {
            frame = Time.frameCount;
        }
        buttonCache[zButton] = frame;
    }
    private static bool BlockViaFrameCache(int zPadIndex, ePadButton zButton)
    {
        if(cache.TryGetValue(zPadIndex, out var buttonCache))
        {
            if(buttonCache.TryGetValue(zButton, out var frame))
            {
                var now = Time.frameCount;
                if(now <= frame)
                    return true;
                return false;
            }
        }
        return false;
    }


#if TOUCH_CONTROLS

    public static bool GetButtonDown(int zPadIndex, ePadButton zButton)
    {
        return  PlayerTouchControls.GetButtonDown(zButton);
    }
    public static float GetAxis(int zPadIndex, ePadAxis zAxis)
    {
        return PlayerTouchControls.GetAxis(zAxis);
    }
#else
    public static bool GetButtonDown(int zPadIndex, ePadButton zButton)
    {
        // if(BlockViaFrameCache(zPadIndex, zButton))
        //     return false;

        var mapString = ButtonToInputMap[zButton];
        var padString = PadToNameMap[zPadIndex];
        bool down = Input.GetButtonDown(padString + mapString);
        return down;
    }

    public static float GetAxis(int zPadIndex, ePadAxis zAxis)
    {
        var mapString = AxisToInputMap[zAxis];
        var padString = PadToNameMap[zPadIndex];
        float value = Input.GetAxis(padString + mapString);
        return value;
    }

    public static float GetTrigger(int zPadindex, ePadTrigger zTrigger)
    {
        var mapString = TriggerToInputMap[zTrigger];
        var padString = PadToNameMap[zPadindex];
        float value = Input.GetAxis(padString + mapString);
        value = Mathf.Clamp01(value);
        return value;
    }
#endif

    private static Dictionary<ePadButton, string> BuildButtonDic()
    {
        var dic = new Dictionary<ePadButton, string>();
        foreach(var map in buttonMap)
        {
            dic[map.padButton] = map.inputManagerString;
        }
        return dic;
    }
    private static Dictionary<ePadAxis, string> BuildAxisDic()
    {
        var dic = new Dictionary<ePadAxis, string>();
        foreach(var map in axisMap)
        {
            dic[map.padAxis] = map.inputManagerString;
        }
        return dic;
    }
    private static Dictionary<ePadTrigger, string> BuildTriggerDic()
    {
        var dic = new Dictionary<ePadTrigger, string>();
        foreach(var map in triggerMap)
        {
            dic[map.padTrigger] = map.inputManagerString;
        }
        return dic;
    }
}
