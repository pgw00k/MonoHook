using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HookTest
{
    public class HookClass
    {
        /// <summary>
        /// 执行Hook方法的入口，直接在注入成功后调用
        /// </summary>
        public static void HookMethod()
        {
            Console.WriteLine("HookMethod Start.");
            //获取需要Hook的类型
            Type type = Type.GetType("HookTarget,Assembly-CSharp");
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
            MethodInfo miTarget = type.GetMethod("HookStatic");
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


        /// <summary>
        /// Hook带有参数的方法的例子
        /// </summary>
        public static void HookMethodWithParam()
        {
            Console.WriteLine("HookMethodWithParam Start.");
            //获取需要Hook的类型
            Type type = Type.GetType("HookTarget,Assembly-CSharp.dll");
            //获取类型上的目标方法
            MethodInfo miTarget = type.GetMethod("TargetMethodWithParam");

            //Hook后重新定向的新方法，这里直接指向本类
            type = typeof(HookClass);

            // 找到本类上 名为 NewMethod 的方法，准备替换目标方法
            MethodInfo miReplacement = type.GetMethod("NewMethodWithParam", BindingFlags.Static | BindingFlags.Public);

            // 这个方法是用来调用原始方法的，在这里也可以在本类中声明一个 DoOld 方法用来占位
            MethodInfo miProxy = type.GetMethod("DoOldWithParam", BindingFlags.Static | BindingFlags.Public);

            // 创建一个 Hook 并 Install ，这样就能把方法都重定向到新方法来
            MethodHook _hook = new MethodHook(miTarget, miReplacement, miProxy);

            _hook.Install();
        }

        /// <summary>
        /// Hook类型的属性
        /// </summary>
        public static void HookPropertyMethod()
        {
            Console.WriteLine("HookPropertyMethod Start.");
            //获取需要Hook的类型
            Type type = Type.GetType("HookTarget,Assembly-CSharp.dll");

            //获取类型上的目标方法
            MethodInfo miTarget = type.GetMethod("get_TestStaticString", BindingFlags.Static | BindingFlags.Public);

            if (miTarget != null)
            {
                Console.WriteLine("Get target Method " + miTarget.Name);
            }
            else
            {
                Console.WriteLine("Can not find target Method get_TestStaticString");
                return;
            }

            //Hook后重新定向的新方法，这里直接指向本类
            type = typeof(HookClass);

            // 找到本类上 名为 NewMethod 的方法，准备替换目标方法
            MethodInfo miReplacement = type.GetMethod("get_NewStaticString", BindingFlags.Static | BindingFlags.Public);
            if (miReplacement != null)
            {
                Console.WriteLine("Get replace Method " + miReplacement.Name);
            }
            else
            {
                Console.WriteLine("Can not find Method");
                return;
            }

            // 创建一个 Hook 并 Install ，这样就能把方法都重定向到新方法来
            MethodHook _hook = new MethodHook(miTarget, miReplacement, null);

            _hook.Install();

            Console.WriteLine("HookPropertyMethod Over.");
        }

        /// <summary>
        /// 将要被调用的新方法
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void NewMethod(object target)
        {
            Console.WriteLine("NewMethod In.");

            HookTarget tar = target as HookTarget;
            if (tar != null)
            {
                Console.WriteLine("Get Instance target.");
            }

            // 需要在新指向中重新主动调用占位的旧方法，即可达到调用旧方法的效果
            DoOld(target);
        }

        /// <summary>
        /// 用来占位的旧方法（可以不写内容）
        /// </summary>     
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void DoOld(object target)
        {
            //Console.WriteLine("Old");
        }


        /// <summary>
        /// 将要被调用的新方法
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void NewMethodWithParam(object target, string s)
        {
            Console.WriteLine("NewMethodWithParam In.");
            Console.WriteLine("Param is " + s);

            //需要在新指向中重新主动调用占位的旧方法，即可达到调用旧方法的效果
            DoOldWithParam(target, "BBB");
        }

        /// <summary>
        /// 用来占位的旧方法（可以不写内容）
        /// </summary>     
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void DoOldWithParam(object target, string s)
        {
        }

        
        public static string NewStaticString
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return "Hello,Hook!";
            }
        }
    }
}
