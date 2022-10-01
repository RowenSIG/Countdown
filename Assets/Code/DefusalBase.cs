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
    SCREW_DRIVER_PANEL= 6,
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

public abstract class DefusalBase : MonoBehaviour
{
    public abstract eDefusalType Type {get;}
    public bool Defused => instruction.Defused;
    protected DefusalInstruction instruction;
    public void Setupinstruction(DefusalInstruction zInstruction)
    {
        instruction = zInstruction;
    }
    public bool AttemptDefusal(DefusalInstruction zInstruction)
    {
        var success = instruction.Match(zInstruction);
        return success;
    }
}

