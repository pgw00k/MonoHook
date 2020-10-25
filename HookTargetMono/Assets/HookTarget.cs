using System;
using UnityEngine;
using UnityEngine.UI;

public class HookTarget : MonoBehaviour
{
    public Text Log;

    static string HookStaticString = "HookStatic";

    public static string TestStaticString
    {
        get
        {
            return "This is original,not hook.";
        }
    }

    public void HookMethod()
    {
        Console.WriteLine("HookMethod Start");
        Log.text = "HookMethod";
        Console.WriteLine("HookMethod Over");
    }

    public void HookStatic()
    {
        Console.WriteLine("HookStatic Start");
        Log.text = HookStaticString;
        Console.WriteLine("HookStatic Over");
    }

    /// <summary>
    /// 带有参数的目标函数
    /// </summary>
    /// <param name="s"></param>
    public void TargetMethodWithParam(string s)
    {
        Console.WriteLine("TargetMethodWithParam start");
        Log.text = s;
        Console.WriteLine("TargetMethodWithParam over");
    }

    public static void StaticOutput()
    {
        Console.WriteLine("StaticOutput Start.");
    }

    public void CheckTestString()
    {
        Console.WriteLine("CheckTestString Start");
        Log.text = TestStaticString;
        Console.WriteLine("CheckTestString Over");
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
