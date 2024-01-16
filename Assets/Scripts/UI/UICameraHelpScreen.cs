using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraHelpScreen : UIDisplay
{
    private void OnEnable()
    {
        InputManager.Instance.Controls.Camera.Disable();    
    }

    private void OnDisable()
    {
        InputManager.Instance.Controls.Camera.Enable();
    }

    private void Awake()
    {
        EventBroadcaster.Instance.RemoveObserver(UIEvents.TOGGLE_CAMERA_HELP_SCREEN);
        EventBroadcaster.Instance.AddObserver(UIEvents.TOGGLE_CAMERA_HELP_SCREEN, ToggleCameraHelpScreenEvent);
        ToggleScreen(false);
    }
    
    private void ToggleScreen(bool toggle)
    {
        this.gameObject.SetActive(toggle);
        BuildingManager.Instance.AllowEditing = !toggle;
    }

    #region Event Broadcaster Events
    private void ToggleCameraHelpScreenEvent(Parameters param)
    {
        bool toggle = param.GetParameter<bool>(ParameterNames.CAMERA_HELP_SCREEN_TOGGLE, false);
        ToggleScreen(toggle);
    }
    #endregion

    #region Unity Button Events
    public void OnClose()
    {
        ToggleScreen(false);
    }
    #endregion
}
