using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HookTarget : MonoBehaviour
{
    public Text Log;

    static string HookStaticString = "HookStatic";

    public void HookMethod()
    {
        Console.WriteLine("HookMethod");
        Log.text = "HookMethod";
    }

    public void HookStatic()
    {        
        Log.text = HookStaticString;
        Console.WriteLine("HookStatic");
    }

    public void Start()
    {
        //Shell.OpenConsole();
        Console.WriteLine("Console start.");
    }

    public void OnDestroy()
    {
        Console.WriteLine("Console over.");
        //Shell.CloseConsole();
    }

}
