using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainHUD : MonoBehaviour
{
    [SerializeField] UIBuildOptionsScreen buildOptionsScreen;
    [SerializeField] GameObject buildButton;
    [SerializeField] GameObject destroyButton;
    [SerializeField] Transform[] destroyButtonPositions;

    private RectTransform destroyButtonRect;

    private void Awake()
    {
        EventBroadcaster.Instance.RemoveObserver(UIEvents.TOGGLE_BUILD_BUTTON);
        EventBroadcaster.Instance.AddObserver(UIEvents.TOGGLE_BUILD_BUTTON, ToggleBuildButtonEvent);

        EventBroadcaster.Instance.RemoveObserver(UIEvents.CHANGE_DESTROY_BUTTON_POSITION);
        EventBroadcaster.Instance.AddObserver(UIEvents.CHANGE_DESTROY_BUTTON_POSITION, ChangeDestroyButtonPositionEvent);
        
        this.destroyButtonRect ??= this.destroyButton.GetComponent<RectTransform>();
    }

    #region HUD Actions
    public void OpenBuildOptionsScreen()
    {
        this.buildOptionsScreen.gameObject.SetActive(true);
    }

    public void TurnOnBuildingMode()
    {
        GameManager.Instance.ChangeState(GameState.BUILDING);
    }

    public void TurnOnDestroyMode()
    {
        GameManager.Instance.ChangeState(GameState.DESTROYING);
    }
    #endregion

    #region Event Broadcaster Events
    private void ToggleBuildButtonEvent(Parameters param)
    {
        bool toggle = param.GetParameter<bool>(ParameterNames.BUILD_BUTTON_TOGGLE, false);
        this.buildButton.SetActive(toggle);
    }

    private void ChangeDestroyButtonPositionEvent(Parameters param)
    {
        DestroyButtonPosition pos = param.GetParameter<DestroyButtonPosition>(ParameterNames.DESTROY_BUTTON_POSITION, DestroyButtonPosition.ONE);

        Vector2 anchorMin = Vector2.zero;
        Vector2 anchorMax = Vector2.zero;

        switch(pos)
        {
            case DestroyButtonPosition.ONE:
                anchorMin = Vector2.zero;
                anchorMax = Vector2.zero;
                break;
            case DestroyButtonPosition.TWO:
                anchorMin = new Vector2(0f, 0.5f);
                anchorMax = new Vector2(0f, 0.5f);
                break;
        }

        this.destroyButtonRect.anchorMin = anchorMin;
        this.destroyButtonRect.anchorMax = anchorMax;
        this.destroyButton.transform.position = this.destroyButtonPositions[(int)pos].position;
    }
    #endregion
}

public enum DestroyButtonPosition
{
    ONE,
    TWO
}
