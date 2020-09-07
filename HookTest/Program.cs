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
            //需要放置在和.exe文件相同路径下，即工作路径下
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
            HookClass.HookMethod();

        }
    }

    public class HookClass
    {

        /// <summary>
        /// 将要被调用的新方法
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void NewMethod()
        {
            Console.WriteLine("NewMethod In.");
        }

        /// <summary>
        /// 用来占位的旧方法（可以不写内容）
        /// </summary> 
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void DoOld()
        {
            Console.WriteLine("Old");
        }

        /// <summary>
        /// 执行Hook方法的入口，直接在注入成功后调用
        /// </summary>
        public static void HookMethod()
        {
            Console.WriteLine("HookMethod Start.");
            //获取需要Hook的类型
            Type type = Type.GetType("HookTarget,Assembly-CSharp.dll");
            if (type != null)
            {
                Console.WriteLine("Get type " + type.Name);
            }
            else
            {
                Console.WriteLine("Can not find class");
                return;
            }

            //获取类型上的目标方法
            MethodInfo miTarget = type.GetMethod("HookMethod");
            if (miTarget != null)
            {
                Console.WriteLine("Get target Method " + miTarget.Name);
            }
            else
            {
                Console.WriteLine("Can not find Method");
                return;
            }

            //Hook后重新定向的新方法，这里直接指向本类
            type = typeof(HookClass);
            if (type != null)
            {
                Console.WriteLine("Get type " + type.Name);
            }
            else
            {
                Console.WriteLine("Can not find class");
                return;
            }

            // 找到本类上 名为 NewMethod 的方法，准备替换目标方法
            MethodInfo miReplacement = type.GetMethod("NewMethod", BindingFlags.Static | BindingFlags.Public);
            if (miReplacement != null)
            {
                Console.WriteLine("Get replace Method " + miReplacement.Name);
            }
            else
            {
                Console.WriteLine("Can not find Method");
                return;
            }

            // 这个方法是用来调用原始方法的，在这里也可以在本类中声明一个 DoOld 方法用来占位
            MethodInfo miProxy = type.GetMethod("DoOld", BindingFlags.Static | BindingFlags.Public);
            if (miProxy != null)
            {
                Console.WriteLine("Get proxy Method " + miProxy.Name);
            }
            else
            {
                Console.WriteLine("Can not find Method");
                return;
            }

            // 创建一个 Hook 并 Install ，这样就能把方法都重定向到新方法来
            MethodHook _hook = new MethodHook(miTarget, miReplacement, miProxy);

            //Proxy为空会直接替换到目标方法，带有Proxy的话才会在最后调用原始方法
            //MethodHook _hook = new MethodHook(miTarget, miReplacement, null);

            _hook.Install();
        }
    }
}
