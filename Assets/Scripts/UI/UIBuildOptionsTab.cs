using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIBuildOptionsTab : MonoBehaviour, IUIClickable
{
    [SerializeField] BuildOptionsTab buildOptionsTab;

    private Action<BuildOptionsTab> onSelect;

    public void SetClickAction(Action<BuildOptionsTab> onSelect)
    {
        this.onSelect = onSelect;
    }

    public void OnClick()
    {
        this.onSelect?.Invoke(this.buildOptionsTab);
    }
}
