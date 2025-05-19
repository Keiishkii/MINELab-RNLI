using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        Parent parent = new Child();
    }
}


public class Parent
{
    public static string ID = "";
}

public class Child : Parent
{
    public new static string ID = "";
}