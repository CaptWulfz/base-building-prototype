using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBehavior
{
    protected virtual GameState GameState { get => GameState.LOADING; }

    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();
}
