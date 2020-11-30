using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGameEvent : GameEvent
{
    public enum test{ first, second, third}
    
    //public int a1;
    
    //public float a2;
    
    //public double a3;
    
    public bool state;
    
    //public string a5;
    
    public test type;

    public Vector2 position;
    
    public override string ClassInfo() {
        return "BGameEvent";
    }
}