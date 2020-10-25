using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HookTest
{
    public class Program
    {

        /// <summary>
        /// 注入后执行的主要逻辑
        /// </summary>
        public static void Main()
        {
            //测试是否注入成功
            Shell.OpenConsole();
            Console.WriteLine("Run!");

            //测试是否能够正常加载 MonoHook插件到内存中
            //需要放置在和当前.exe文件相同路径下，即工作路径，否则会闪退
            Assembly ass = Assembly.LoadFrom("MonoHook.dll");
            if (ass != null)
            {
                Console.WriteLine("Load ok  " + ass.FullName);
            }
            else
            {
                Console.WriteLine("Can not find Assembly");
                return;
            }

            //开始Hook
            HookUI.Start();           
        }
    }

    public class HookUI
    {
        protected static UnityEngine.GameObject _Instance = null;

        public static void Start()
        {
            _Instance = new UnityEngine.GameObject("HookUI");
            HookUIMono UIMono = _Instance.AddComponent<HookUIMono>();
        }
    }

    public class HookUIMono: UnityEngine.MonoBehaviour
    {
        public void OnGUI()
        {
            if (UnityEngine.GUILayout.Button("Hook"))
            {
                HookClass.HookMethod();
            }
            if (UnityEngine.GUILayout.Button("Test"))
            {
                Console.WriteLine("Test");
                HookTarget.StaticOutput();
            }

            if (UnityEngine.GUILayout.Button("HookMethodWithParam"))
            {
                HookClass.HookMethodWithParam();
            }

            if (UnityEngine.GUILayout.Button("HookPropertyMethod"))
            {
                HookClass.HookPropertyMethod();
            }
        }
    }
}
