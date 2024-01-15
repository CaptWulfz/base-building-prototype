using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] SpriteRenderer buildingRenderer;
    [SerializeField] UIBuildingProgressBar buildingProgressBar;

    private const float IN_PROGRESS_ALPHA_VALUE = 0.25f;
    private const float DONE_ALPHA_VALUE = 1f;
    private const float BUILDING_MODE_ALPHA_VALUE = 0.5f;

    private Color inProgressColor;
    private Color doneColor;
    private Color buildingModeColor;

    private string buildingId;
    private float buildTime;
    private BuildingType buildingType;
    private BuildingState buildingState;

    private LandTile inhabitedTile;

    private bool buildStart;
    private float currBuildTime;

    public void SetData(BuildingData data, int colorIdx, int spriteIdx, bool buildNow = false)
    {
        this.buildingId = data.BuildingId;
        this.buildingRenderer.sprite = data.BuildingSprites[colorIdx].RotationSprites[spriteIdx];
        this.buildTime = data.BuildTime;
        this.buildingType = data.BuildingType;
        this.buildStart = false;

        this.inProgressColor = new Color(this.buildingRenderer.color.r, this.buildingRenderer.color.g, this.buildingRenderer.color.b, IN_PROGRESS_ALPHA_VALUE);
        this.doneColor = new Color(this.buildingRenderer.color.r, this.buildingRenderer.color.g, this.buildingRenderer.color.b, DONE_ALPHA_VALUE);
        this.buildingModeColor = new Color(this.buildingRenderer.color.r, this.buildingRenderer.color.g, this.buildingRenderer.color.b, BUILDING_MODE_ALPHA_VALUE);
    }

    private void Update()
    {
        if (!this.buildStart)
            return;

        this.currBuildTime += Time.deltaTime;

        UpdateBuildingProgressBarUI();

        if (this.currBuildTime >= this.buildTime)
        {
            FinishBuilding();
        }
    }

    public void Build()
    {
        this.buildingRenderer.color = this.inProgressColor;
        this.buildingState = BuildingState.IN_PROGRESS;
        this.currBuildTime = 0f;
        this.buildStart = true;

        //Parameters param = new Parameters();
        //param.AddParameter<bool>(ParameterNames.BUILDING_PROGRESS_TOGGLE, true);
        //EventBroadcaster.Instance.PostEvent(UIEvents.TOGGLE_BUILDING_PROGRESS_BAR, param);
        this.buildingProgressBar.ToggleBuildingProgressBar(true);
    }

    public void TrySetToBuildingModeTransparency()
    {
        if (GameManager.Instance.GameState == GameState.BUILDING)
            this.buildingRenderer.color = this.buildingModeColor;
    }

    public void ResetBuildingModeTransparency()
    {
        this.buildingRenderer.color = this.buildingState == BuildingState.DONE ? this.doneColor : this.inProgressColor;
    }

    private void FinishBuilding()
    {
        this.buildingRenderer.color = this.doneColor;
        TrySetToBuildingModeTransparency();
        this.buildingState = BuildingState.DONE;
        this.currBuildTime = 0f;
        this.buildStart = false;
    }

    private void UpdateBuildingProgressBarUI()
    {
        //Parameters param = new Parameters();
        //param.AddParameter<float>(ParameterNames.BUILDING_PROGRESS_VALUE, this.currBuildTime / this.buildTime);
        //EventBroadcaster.Instance.PostEvent(UIEvents.UPDATE_BUILDING_PROGRESS_BAR, param);

        this.buildingProgressBar.UpdateBuildingProgressBar(this.currBuildTime / this.buildTime);
    }
}

public enum BuildingState
{
    IN_PROGRESS,
    DONE
}

public enum BuildingType
{
    WALL,
    HOUSE,
    TOWER
}
