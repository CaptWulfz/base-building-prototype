using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public Controls Controls { get; private set; }

    public bool AllowInput { get; private set; }

    protected override void Initialize()
    {
        this.Controls = new Controls();

        this.AllowInput = true;
        base.Initialize();
    }

    public void ToggleAllowInput(bool allow)
    {
        this.AllowInput = allow;
    }
}
