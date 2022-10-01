using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DefusalInstruction
{
    public abstract eDefusalType Type { get; }

    public bool Defused {get ; protected set; }
    public bool Match(DefusalInstruction zInstruction)
    {
        if (Type != zInstruction.Type)
            return false;

        return MatchInternal(zInstruction);
    }
    protected abstract bool MatchInternal(DefusalInstruction zInstruction);
}

public class CodeDefusalInstruction : DefusalInstruction
{
    public override eDefusalType Type => eDefusalType.CODE;
    public List<int> code;

    protected override bool MatchInternal(DefusalInstruction zInstruction)
    {
        var zCode = zInstruction as CodeDefusalInstruction;
        if (code.Count != zCode.code.Count)
            return false;

        for (int i = 0; i < code.Count; i++)
        {
            if (code[i] != zCode.code[i])
                return false;
        }

        Defused = true;
        return true;
    }
}

public class WireCutDefusalInstruction : DefusalInstruction
{
    public override eDefusalType Type => eDefusalType.WIRE_CUT;

    public List<eColour> wireColours;
    public int chosenWireIndex;

    protected override bool MatchInternal(DefusalInstruction zInstruction)
    {
        var zWireCut = zInstruction as WireCutDefusalInstruction;

        if (chosenWireIndex != zWireCut.chosenWireIndex)
            return false;

        Defused = true;
        return Defused;
    }
}

public class LiquidDefusalInstruction : DefusalInstruction
{
    public override eDefusalType Type => eDefusalType.LIQUID;

    public List<eColour> colourOrder;
    public eColour finalColour;

    protected override bool MatchInternal(DefusalInstruction zInstruction)
    {
        var zLiquid = zInstruction as LiquidDefusalInstruction;

        if (zLiquid.colourOrder.Count != colourOrder.Count)
            return false;

        if (finalColour != zLiquid.finalColour)
            return false;

        for (int i = 0; i < colourOrder.Count; i++)
        {
            if (colourOrder[i] != zLiquid.colourOrder[i])
                return false;
        }

        Defused = true;
        return true;
    }
}

public class MagneticLockDefusalInstruction : DefusalInstruction
{
    public override eDefusalType Type => eDefusalType.MAGNETIC_LOCK;

    public int voltage;

    protected override bool MatchInternal(DefusalInstruction zInstruction)
    {
        var zMagneticLock = zInstruction as MagneticLockDefusalInstruction;

        if (zMagneticLock.voltage != voltage)
            return false;

        Defused = true;
        return true;
    }
}

public class ScrewDriverDefusalInstruction : DefusalInstruction
{
    public override eDefusalType Type => eDefusalType.SCREW_DRIVER_PANEL;

    public List<bool> order;
    private int successes = 0;

    public int attemptIndex;
    protected override bool MatchInternal(DefusalInstruction zInstruction)
    {
        var zScrewDrive = zInstruction as ScrewDriverDefusalInstruction;

        //now then... 
        var canUnscrew = order[zScrewDrive.attemptIndex];

        if(canUnscrew)
        {
            successes += 1;
        }
        else
        {
            return false;
        }

        var numToWin = 0;
        foreach(var value in order)
        {
            if(value)
                numToWin += 1;
        }
        if(successes >= numToWin)
            Defused = true;
        return true;
    }
}

public class TurnyHandleDefusalInstruction : DefusalInstruction
{
    public override eDefusalType Type => eDefusalType.TURNY_HANDLE;

    public List<eTurnDirection> order;
    private int successes = 0;

    public eTurnDirection attemptDirection;

    protected override bool MatchInternal(DefusalInstruction zInstruction)
    {
        var zTurnyHandle = zInstruction as TurnyHandleDefusalInstruction;

        var nextExpectedDirection = order[successes];
        if(zTurnyHandle.attemptDirection != nextExpectedDirection)
            return false;

        successes += 1;

        if(successes >= order.Count)
        {
            Defused = true;
        }
        return true;
    }
}