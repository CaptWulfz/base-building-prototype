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
    }

    public override void OnExit()
    {
        this.buildingControls.Disable();
        this.buildingControls.Build.performed -= _ => OnBuild();
        this.buildingControls.ChangeColor.performed -= _ => OnChangeColor();
        this.buildingControls.RotateBuilding.performed -= _ => OnRotateBuilding();
        BuildingManager.Instance.RemoveActiveBuildingData();
        BuildingManager.Instance.HideBuildingPlaceholder();
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

        //Vector2 mousePos = this.buildingControls.MousePosition.ReadValue<Vector2>();
        //TilemapManager.Instance.TryBuildOnTile(mousePos);
    }

    private void TryDisplayPlaceholder()
    {
        if (!InputManager.Instance.AllowInput)
            return;

        Vector2 mouse = this.buildingControls.MousePosition.ReadValue<Vector2>();
        BuildingManager.Instance.TryDisplayPlaceholder(mouse);
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
}