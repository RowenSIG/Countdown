using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

public static  class Debug 
{
    [Conditional("ENABLE_LOGS")]
    public static void Log(string zOutput)
    {
        UnityEngine.Debug.Log(zOutput);
    }
   
}
