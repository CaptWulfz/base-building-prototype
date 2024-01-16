using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyingStateBehavior : StateBehavior
{
    protected override GameState GameState => GameState.DESTROYING;
    private Controls.DestroyingActions destroyingControls;

    public override void OnEnter()
    {
        this.destroyingControls = InputManager.Instance.Controls.Destroying;
        this.destroyingControls.Enable();
        this.destroyingControls.Destroy.performed += _ => OnDestroy();
        BuildingManager.Instance.ToggleActiveBuildingsEditMode(true);
        ToggleDestroyModeUI(true);
    }

    public override void OnExit()
    {
        this.destroyingControls.Disable();
        this.destroyingControls.Destroy.performed -= _ => OnDestroy();
        BuildingManager.Instance.HideDestroyingPlaceholder();
        BuildingManager.Instance.ToggleActiveBuildingsEditMode(false);
        ToggleDestroyModeUI(false);
    }

    public override void OnUpdate()
    {
        if (!InputManager.Instance.AllowInput)
            return;

        TryDisplayPlaceholder();
    }

    #region Input System Actions
    private void OnDestroy()
    {
        if (!InputManager.Instance.AllowInput)
            return;

        Vector2 mouse = this.destroyingControls.MousePosition.ReadValue<Vector2>();
        BuildingManager.Instance.TryDestroyBuilding(mouse);

    }

    private void TryDisplayPlaceholder()
    {
        if (!InputManager.Instance.AllowInput)
            return;

        Vector2 mouse = this.destroyingControls.MousePosition.ReadValue<Vector2>();
        BuildingManager.Instance.TryDisplayDestroyingPlaceholder(mouse);
    }
    #endregion

    #region Event Broadcaster Callers
    private void ToggleDestroyModeUI(bool toggle)
    {
        Parameters param = new Parameters();
        param.AddParameter(ParameterNames.DESTROY_MODE_UI_TOGGLE, toggle);
        EventBroadcaster.Instance.PostEvent(UIEvents.TOGGLE_DESTROY_MODE_UI, param);
    }
    #endregion
}