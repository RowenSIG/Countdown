using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DefusalConfigurator : MonoBehaviour
{
    public abstract eDefusalType Type {get;}

    public abstract void ConfigureDefusal();

    public abstract void RefreshDefusal();

    public abstract void Reset();

    public abstract DefusalInstruction GetDefusalInstruction();
}
