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
    public List<int> code = new List<int>();

    protected override bool MatchInternal(DefusalInstruction zInstruction)
    {
        var zCode = zInstruction as CodeDefusalInstruction;

        for (int i = 0; i < zCode.code.Count; i++)
        {
            if (code[i] != zCode.code[i])
                return false;
        }

        if(code.Count == zCode.code.Count)
            Defused = true;
        return true;
    }
}

public class WireCutDefusalInstruction : DefusalInstruction
{
    public override eDefusalType Type => eDefusalType.WIRE_CUT;

    public List<eColour> wireColours = new List<eColour>();
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

    public List<eColour> colourOrder = new List<eColour>();
    public eColour FinalColour => ColourProvider.GetColourMix(colourOrder);

    protected override bool MatchInternal(DefusalInstruction zInstruction)
    {
        var zLiquid = zInstruction as LiquidDefusalInstruction;

        if (FinalColour != zLiquid.FinalColour)
            return false;

        Defused = true;
        return true;
    }
}

public class MagneticLockDefusalInstruction : DefusalInstruction
{
    public override eDefusalType Type => eDefusalType.MAGNETIC_LOCK;

    public int voltage;

    public int battery1;
    public int battery2;

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

    public List<bool> order = new List<bool>();
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

    public List<eTurnDirection> order = new List<eTurnDirection>();
    private int successes = 0;

    protected override bool MatchInternal(DefusalInstruction zInstruction)
    {
        var zTurnyHandle = zInstruction as TurnyHandleDefusalInstruction;

        var nextExpectedDirection = order[successes];
        var attemptDirection = zTurnyHandle.order[successes];
        if(attemptDirection != nextExpectedDirection)
            return false;

        successes += 1;

        if(successes >= order.Count)
        {
            Defused = true;
        }
        return true;
    }
}