using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingStateBehavior : StateBehavior
{
    protected override GameState GameState { get => GameState.BUILDING; }

    private Controls.BuildingActions buildingControls;

    public override void OnEnter()
    {
        this.buildingControls = InputManager.Instance.Controls.Building;
        this.buildingControls.Enable();
        this.buildingControls.Build.performed += _ => OnBuild();
        this.buildingControls.ChangeColor.performed += _ => OnChangeColor();
        this.buildingControls.RotateBuilding.performed += _ => OnRotateBuilding();
        BuildingManager.Instance.ToggleActiveBuildingsEditMode(true);
        BuildingManager.Instance.AllowEditing = true;
        ToggleBuildingModeUI(true);
    }

    public override void OnExit()
    {
        this.buildingControls.Disable();
        this.buildingControls.Build.performed -= _ => OnBuild();
        this.buildingControls.ChangeColor.performed -= _ => OnChangeColor();
        this.buildingControls.RotateBuilding.performed -= _ => OnRotateBuilding();
        BuildingManager.Instance.RemoveActiveBuildingData();
        BuildingManager.Instance.HideBuildingPlaceholder();
        BuildingManager.Instance.ToggleActiveBuildingsEditMode(false);
        BuildingManager.Instance.AllowEditing = false;
        ToggleBuildingModeUI(false);
    }

    public override void OnUpdate()
    {
        if (!InputManager.Instance.AllowInput)
            return;

        TryDisplayPlaceholder();
    }

    #region Input System Actions
    private void OnBuild()
    {
        if (!InputManager.Instance.AllowInput)
            return;

        Vector2 mouse = this.buildingControls.MousePosition.ReadValue<Vector2>();
        BuildingManager.Instance.TryBuildBuilding(mouse);
    }

    private void TryDisplayPlaceholder()
    {
        if (!InputManager.Instance.AllowInput)
            return;

        Vector2 mouse = this.buildingControls.MousePosition.ReadValue<Vector2>();
        BuildingManager.Instance.TryDisplayBuildingPlaceholder(mouse);
    }

    private void OnChangeColor()
    {
        if (!InputManager.Instance.AllowInput)
            return;

        BuildingManager.Instance.ChangePlaceholderColor();
    }

    private void OnRotateBuilding()
    {
        if (!InputManager.Instance.AllowInput)
            return;

        BuildingManager.Instance.ChangePlaceholderRotation();
    }
    #endregion

    #region Event Broadcaster Callers
    private void ToggleBuildingModeUI(bool toggle)
    {
        Parameters param = new Parameters();
        param.AddParameter(ParameterNames.BUILDING_MODE_UI_TOGGLE, toggle);
        EventBroadcaster.Instance.PostEvent(UIEvents.TOGGLE_BUILDING_MODE_UI, param);
    }
    #endregion
}
