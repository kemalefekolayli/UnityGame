using System;
using UnityEngine;

public abstract class GameEvent
{
    public int Priority { get; set; }
    public abstract void Execute();
    
    public GameEvent(int priority = 0)
    {
        Priority = priority;
    }
}