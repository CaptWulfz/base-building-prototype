using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITurnOffDestroyModeButton : MonoBehaviour
{
    private void Awake()
    {
        EventBroadcaster.Instance.RemoveObserver(UIEvents.TOGGLE_DESTROY_MODE_UI);
        EventBroadcaster.Instance.AddObserver(UIEvents.TOGGLE_DESTROY_MODE_UI, ToggleDestroyModeUIEvent);
        this.gameObject.SetActive(false);
    }

    #region Unity Button Events
    public void OnTurnOffDestroyMode()
    {
        GameManager.Instance.ChangeState(GameState.VIEWING);
    }
    #endregion

    #region Event Broadcaster Events
    private void ToggleDestroyModeUIEvent(Parameters param)
    {
        bool toggle = param.GetParameter<bool>(ParameterNames.DESTROY_MODE_UI_TOGGLE, false);
        this.gameObject.SetActive(toggle);
    }
    #endregion
}
