using System;

[Serializable]
public abstract class GameEvent
{
    public string text;

    public virtual string ClassInfo() {
        return "GameEvent";
    }
}