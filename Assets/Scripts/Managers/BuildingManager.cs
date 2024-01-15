using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    private BuildingDataMap buildingDataMap;
    private Building buildingRef;
    private BuildingPlaceholder buildingPlaceholder;

    private List<Building> builtBuildingsList;

    public int MaxBuildingCount { get; private set; }
    public bool BuildingPlaceholderActive { get; private set; }

    private int currBuildingCount;
    private BuildingData[] allBuildingData;

    // For Placeholder and Building use
    private BuildingData activeBuildingData; 

    protected override void Initialize()
    {
        this.buildingDataMap = Resources.Load<BuildingDataMap>(ResourcesPaths.BUILDING_DATA_MAP_PATH);
        this.buildingRef = Resources.Load<Building>(ResourcesPaths.BUILDING_PATH);
        this.builtBuildingsList = new List<Building>();
        this.MaxBuildingCount = this.buildingDataMap.MaxBuildingCount;
        this.allBuildingData = null;
        this.activeBuildingData = null;
        PoolManager.Instance.CreatePool(PoolId.BUILDING, this.MaxBuildingCount);
        this.currBuildingCount = 0;

        base.Initialize();
    }

    public void BuildBuilding(BuildingData data, int colorIdx, int spriteIdx, LandTile tileDestination)
    {
        if (this.currBuildingCount >= this.MaxBuildingCount)
            return;

        if (data != null)
        {
            Building newBuilding = PoolManager.Instance.BuildingPool.Get(() => {
                return GameObject.Instantiate<Building>(this.buildingRef);
            });
            newBuilding.SetData(data, colorIdx, spriteIdx);
            newBuilding.Build();
            newBuilding.transform.position = TilemapManager.Instance.GetWorldCenterFromTile(tileDestination.TilePos);
            this.builtBuildingsList.Add(newBuilding);
            tileDestination.TileState = TileState.UNAVAILABLE;
            this.currBuildingCount++;

            UpdateBuildingCounterUI();
        }
    }

    public void ToggleActiveBuildingsBuildingMode(bool toggle)
    {
        foreach (Building building in this.builtBuildingsList)
        {
            if (toggle)
                building.TrySetToBuildingModeTransparency();
            else
                building.ResetBuildingModeTransparency();
        }
    }

    public void SelectActiveBuildingData(string buildingId, BuildingType type)
    {
        this.activeBuildingData = GetBuildingDataById(buildingId, type);
        this.buildingPlaceholder.SetData(this.activeBuildingData);
    }

    public void RemoveActiveBuildingData()
    {
        this.activeBuildingData = null;
    }

    public void TryBuildBuilding(Vector2 mousePos)
    {
        if (!this.buildingPlaceholder.IsReady)
            return;

        LandTile tile = TilemapManager.Instance.GetTileFromMousePos(mousePos);

        if (tile == null)
            return;

        if (tile.TileState == TileState.AVAILABLE)
        {
            PendingBuilding pendingBuilding = this.buildingPlaceholder.GetPendingBuildingData();
            BuildBuilding(pendingBuilding.buildingData, pendingBuilding.colorIdx, pendingBuilding.rotationIdx, tile);
        }
    }

    #region Builing Placeholder
    public void RegisterBuildingPlacholder(BuildingPlaceholder placeholder)
    {
        this.buildingPlaceholder = placeholder;
    }

    public void TryDisplayPlaceholder(Vector2 mousePos)
    {
        TileState hoveredTile = TilemapManager.Instance.GetTileAvailabilityFromMousePos(mousePos);

        if (hoveredTile == TileState.NO_TILE)
        {
            this.buildingPlaceholder.TogglePlacholder(false);
            this.BuildingPlaceholderActive = false;
        } else
        {
            if (!this.buildingPlaceholder.IsReady)
                return;
            this.buildingPlaceholder.ChangeTileHighlightColor(hoveredTile == TileState.AVAILABLE);
            this.buildingPlaceholder.TogglePlacholder(true);
            this.buildingPlaceholder.ChangePosition(TilemapManager.Instance.GetTileCenterFromMousePos(mousePos));

            this.BuildingPlaceholderActive = true;
        }
    }

    public void HideBuildingPlaceholder()
    {
        this.buildingPlaceholder.ClearData();
        this.buildingPlaceholder.TogglePlacholder(false);
    }

    public void ChangePlaceholderColor()
    {
        if (!this.BuildingPlaceholderActive)
            return;

        if (!this.buildingPlaceholder.IsReady)
            return;

        this.buildingPlaceholder.ChangePlaceholderColor();
    }

    public void ChangePlaceholderRotation()
    {
        if (!this.BuildingPlaceholderActive)
            return;

        if (!this.buildingPlaceholder.IsReady)
            return;

        this.buildingPlaceholder.ChangePlaceholderRotation();
    }
    #endregion

    private void UpdateBuildingCounterUI()
    {
        Parameters param = new Parameters();
        param.AddParameter<int>(ParameterNames.BUILDING_COUNT, this.currBuildingCount);
        EventBroadcaster.Instance.PostEvent(UIEvents.UPDATE_BUILDING_COUNTER, param);
    }

    #region Getters
    public BuildingData[] GetBuildingData(BuildingType type)
    {
        return GetBuildingDataGroupByType(type).BuildingData;
    }

    public BuildingData[] GetAllBuildingData()
    {
        return this.allBuildingData ??= GetAllBuildingDataRecursive(this.buildingDataMap.BuildingDataGroups[0].BuildingData, 0);
    }

    private BuildingData[] GetAllBuildingDataRecursive(BuildingData[] dataArray, int groupIdx)
    {
        BuildingData[] newArray = null;
        //BuildingData[] sourceArray = dataArray.Length > 0 ? dataArray : this.buildingDataMap.BuildingDataGroups[groupIdx].BuildingData;

        if (groupIdx + 1 < this.buildingDataMap.BuildingDataGroups.Length)
        {
            newArray = new BuildingData[dataArray.Length + this.buildingDataMap.BuildingDataGroups[groupIdx + 1].BuildingData.Length];

            // First Copy
            Array.Copy(dataArray, 0, newArray, 0, dataArray.Length);

            // Second Copy
            Array.Copy(this.buildingDataMap.BuildingDataGroups[groupIdx + 1].BuildingData, 0, newArray, dataArray.Length, this.buildingDataMap.BuildingDataGroups[groupIdx + 1].BuildingData.Length);
            
            groupIdx++;
            return GetAllBuildingDataRecursive(newArray, groupIdx);
        }

        newArray = dataArray;
        return newArray;
    }
    #endregion

    #region Building Data Helpers
    public BuildingData GetBuildingDataById(string buildingId, BuildingType type)
    {
        BuildingDataGroup dataGroup = GetBuildingDataGroupByType(type);
        return Array.Find(dataGroup.BuildingData, (x) => { return buildingId == x.BuildingId; });
    }

    private BuildingDataGroup GetBuildingDataGroupByType(BuildingType type)
    {
        return Array.Find(this.buildingDataMap.BuildingDataGroups, (x) => { return type == x.BuildingType; });
    }
    #endregion
}
