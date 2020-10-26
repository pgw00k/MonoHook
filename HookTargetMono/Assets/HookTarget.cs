using System;
using UnityEngine;
using UnityEngine.UI;

public class HookTarget : MonoBehaviour
{
    public Text Log;

    static string HookStaticString = "HookStatic";

    /// <summary>
    /// 用以测试属性读取的HOOK
    /// <para>属性的控制器其实就是 get_xxx 和 set_xxx </para>
    /// </summary>
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

    /// <summary>
    /// 通过MONO方式进行DLL动态加载，并运行接口函数
    /// </summary>
    public void LoadDLLTest()
    {
        string assFullName = "HookTest.dll";
        string classFullName = "HookTest.Program";
        string methodName = "Main";
        string log = "";

        if(!System.IO.File.Exists(assFullName))
        {
            log = $"{assFullName} is not exist.";
            Console.WriteLine(log);
            Log.text = log;
            return;
        }

        log = $"get {assFullName} file.\n";
        Log.text = log;

        try
        {
            System.Reflection.Assembly ass = System.Reflection.Assembly.LoadFile(assFullName);
            if (ass != null)
            {
                log = $"Load {ass.FullName} success.";
                Console.WriteLine(log);
                Log.text += log + "\n";
            }
            else
            {
                log = $"Load {assFullName} fail.";
                Console.WriteLine(log);
                Log.text = log;
                return;
            }

            Type classType = ass.GetType(classFullName);
            if (classType != null)
            {
                log = $"Load {classType.FullName} success.";
                Console.WriteLine(log);
                Log.text += log + "\n";
            }
            else
            {
                log = $"Load {classFullName} fail.";
                Console.WriteLine(log);
                Log.text = log;
                return;
            }

            System.Reflection.MethodInfo method = classType.GetMethod(methodName,
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (method != null)
            {
                log = $"Load {method.Name} success.";
                Console.WriteLine(log);
                Log.text += log + "\n";
            }
            else
            {
                log = $"Load {methodName} fail.";
                Console.WriteLine(log);
                Log.text = log;
                return;
            }
            method.Invoke(null, null);
        }catch(System.Exception e)
        {
            log = $"{e.GetType().Name} : {e.Message}\n{e.StackTrace}\n";
            Console.WriteLine(log);
            Log.text += log+"\n";
        }


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
