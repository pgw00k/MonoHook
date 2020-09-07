using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
public class Shell
{
    /// <summary>
    /// 开启控制台命令行窗口
    /// </summary>
    /// <returns></returns>
    [DllImport("kernel32.dll")]
    public static extern Boolean AllocConsole();
    [DllImport("kernel32.dll")]
    public static extern Boolean FreeConsole();

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetStdHandle(UInt32 nStdHandle);

    [DllImport("kernel32.dll")]
    private static extern void SetStdHandle(UInt32 nStdHandle, IntPtr handle);

    private const UInt32 StdOutputHandle = 0xFFFFFFF5;

    public static TextWriter OldOutWriter = null;

    /// <summary>
    /// 重定向控制台输出
    /// </summary>
    public static void ReConnect()
    {

        //stdout's handle seems to always be equal to 7
        //IntPtr defaultStdout = new IntPtr(7);
        //IntPtr currentStdout = GetStdHandle(StdOutputHandle);
        //if (currentStdout != defaultStdout)
        //    //reset stdout
        //    SetStdHandle(StdOutputHandle, defaultStdout);

        //reopen stdout
        TextWriter writer = new StreamWriter(Console.OpenStandardOutput())
        {
            AutoFlush = true
        };
        Console.SetOut(writer);
    }

    /// <summary>
    /// 开启控制台并且重定向输出到控制台
    /// </summary>
    public static void OpenConsole()
    {
        OldOutWriter = Console.Out;
        AllocConsole();
        ReConnect();
    }

    /// <summary>
    /// 关闭控制台
    /// </summary>
    public static void CloseConsole()
    {
        Console.SetOut(OldOutWriter);
        FreeConsole();
    }
}
