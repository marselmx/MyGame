using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGameEvent : GameEvent
{
    public enum test{ first, second, third}
    
    public int count = 55;

    public List<string> str_list = new List<string> {"str1", "str2"};

    public string[] arr = new string[3]{"str1", "str2", "str3"};

    public override string ClassInfo() {
        return "AGameEvent";
    }
}