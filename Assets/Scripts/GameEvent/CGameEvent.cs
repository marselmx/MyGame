using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameEvent : GameEvent
{
    //public Sprite img;
    
    public Rect rect;
    
    public Bounds bounds;
    
    
    public override string ClassInfo() {
        return "CGameEvent";
    }
}