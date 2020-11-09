using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 普通类型（不是 MonoBehaviour ）
/// </summary>
public class NormalType
{
    protected int _NTest = 444;
    protected string _TestNormalString = "That is normal string,no hook.";

    public int NTest
    {
        get
        {
            System.Console.WriteLine("Origin get NTest");
            return _NTest;
        }
    }


    public string TestNormalString
    {
        get
        {
            return _TestNormalString;
        }
    }

}
