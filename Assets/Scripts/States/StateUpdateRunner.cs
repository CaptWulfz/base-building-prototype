using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateUpdateRunner : MonoBehaviour
{ 
    public bool RunUpdate { get; set; }
    public Action UpdateAction { get; set; }

    private void Awake()
    {
        this.RunUpdate = false;
        this.UpdateAction = null;
    }

    private void Update()
    {
        if (!this.RunUpdate)
            return;

        UpdateAction?.Invoke();
    }
}
