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

    public override bool CanInteract()
    {
        if(busy)
            return false;
        if(Defused)
            return false;
        return true;
    }

    public override void Interact()
    {
        Room.Instance.StartDefusal(this);
    }

    protected virtual void Awake()
    {
        defusalCamera.gameObject.EnsureActive(false);
    }
    public void Update()
    {
        if(Room.Instance.CurrentDefusal != this)
            return;

        if(PlaySession.Paused)
            return;
        
        if(busy)
            return;

        if(Defused)
            return;

        bool cancel = PlayerInputManager.GetButtonDown(0, ePadButton.FACE_RIGHT); 
        cancel |= Input.GetKeyDown(KeyCode.Q);
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
        progress = Player.Instance.instructions.GetInstruction(Type);

        StartCoroutine(CoStartWait());
        defusalCamera.gameObject.EnsureActive(true);
        StartDefusalInternal();
    }
    public void Cancel()
    {
        CancelInternal();
        statusLight.SetIdle();
        Room.Instance.CancelDefusal();
    }
    public void Close()
    {
        defusalCamera.gameObject.EnsureActive(false);
    }

    protected abstract void SetupInternal();
    protected abstract void StartDefusalInternal();
    protected abstract void UpdateInternal();

    protected virtual void CancelInternal() {}
    private IEnumerator<YieldInstruction> CoStartWait()
    {
        busy = true;
        yield return new WaitForSeconds(0.5f);
        busy = false;
    }


    
    float lastDpadHorizontal = 0f;
    protected float DpadHorizontal()
    {
        var thisDpadHorizontal =  PlayerInputManager.GetAxis(0, ePadAxis.DPAD_HORIZONTAL);
        if(lastDpadHorizontal > 0f && thisDpadHorizontal > 0f)
        {
            return 0;
        }
        else if(lastDpadHorizontal <0f && thisDpadHorizontal <0f)
        {
            return 0;
        }

        lastDpadHorizontal = thisDpadHorizontal;
        return thisDpadHorizontal;
    } 
    float lastDpadVertical = 0f;
    protected float DpadVertical()
    {
        var thisDpadVertical =  PlayerInputManager.GetAxis(0, ePadAxis.DPAD_VERTICAL);
        if(lastDpadVertical > 0f && thisDpadVertical > 0f)
        {
            return 0;
        }
        else if(lastDpadVertical <0f && thisDpadVertical <0f)
        {
            return 0;
        }

        lastDpadVertical = thisDpadVertical;
        return thisDpadVertical;
    }
}

