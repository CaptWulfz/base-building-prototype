using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainHUD : MonoBehaviour
{
    [SerializeField] UIBuildOptionsScreen buildOptionsScreen;
    [SerializeField] GameObject buildButton;

    private void Awake()
    {
        EventBroadcaster.Instance.RemoveObserver(UIEvents.TOGGLE_BUILD_BUTTON);
        EventBroadcaster.Instance.AddObserver(UIEvents.TOGGLE_BUILD_BUTTON, ToggleBuildButtonEvent);
    }

    #region HUD Actions
    public void OpenBuildOptionsScreen()
    {
        this.buildOptionsScreen.gameObject.SetActive(true);
    }
    #endregion

    #region Event Broadcaster Events
    private void ToggleBuildButtonEvent(Parameters param)
    {
        bool toggle = param.GetParameter<bool>(ParameterNames.BUILD_BUTTON_TOGGLE, false);
        this.buildButton.SetActive(toggle);
    }
    #endregion
}
