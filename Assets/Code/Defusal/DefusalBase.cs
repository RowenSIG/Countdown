using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eDefusalType
{
    INVALID = 0,

    CODE = 1,
    WIRE_CUT = 2,
    LIQUID = 4,
    MAGNETIC_LOCK = 5,
    SCREW_DRIVER_PANEL = 6,
    TURNY_HANDLE = 7
}

public enum eColour
{
    INVALID = 0,

    BLUE = 1,
    RED = 2,
    GREEN = 3,
    YELLOW = 4,
    PURPLE = 5,
    BROWN = 6,
    BLACK = 7,
    WHITE = 8,
}

public enum eTurnDirection
{
    CLOCKWISE = 0,
    COUNTER_CLOCKWISE = 1,
}

public abstract class DefusalBase : InteractiveItem
{
    public abstract eDefusalType Type { get; }
    public bool Defused => instruction.Defused;

    public DefusalLight statusLight;
    protected DefusalInstruction instruction;
    protected DefusalInstruction progress;

    public Camera defusalCamera;
    protected bool busy = false;

    protected virtual void Awake()
    {
        defusalCamera.gameObject.EnsureActive(false);
    }
    public void Update()
    {
        if(Room.Instance.CurrentDefusal != this)
            return;
        
        if(busy)
            return;
        bool cancel = PlayerInputManager.GetButtonDown(0, ePadButton.FACE_RIGHT); 
        if (cancel)
        {
            Cancel();
        }

        UpdateInternal();
    }
    public void SetupWithInstruction(DefusalInstruction zInstruction)
    {
        instruction = zInstruction;
        SetupInternal();
        statusLight.SetIdle();
    }
    public bool AttemptDefusal(DefusalInstruction zInstruction)
    {
        var success = instruction.Match(zInstruction);
        if(success == false)
        {
            statusLight.SetExploded();
        }
        else if(Defused)
        {
            statusLight.SetDefused();
        }
        
        return success;

    }

    public void StartDefusal()
    {
        defusalCamera.gameObject.EnsureActive(true);
        StartDefusalInternal();
    }
    public void Cancel()
    {
        statusLight.SetIdle();
        defusalCamera.gameObject.EnsureActive(false);
        Room.Instance.CancelDefusal();
    }

    protected abstract void SetupInternal();
    protected abstract void StartDefusalInternal();
    protected abstract void UpdateInternal();
}

