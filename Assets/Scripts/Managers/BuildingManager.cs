using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    private BuildingDataMap buildingDataMap;
    private Building buildingRef;
    private BuildingPlaceholder buildingPlaceholder;
    private DestroyingPlaceholder destroyingPlaceholder;

    private List<Building> builtBuildingsList;

    public int MaxBuildingCount { get; private set; }
    private bool buildingPlaceholderActive;

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
            newBuilding.gameObject.SetActive(true);
            newBuilding.Build();
            newBuilding.transform.position = TilemapManager.Instance.GetWorldCenterFromTile(tileDestination.TilePos);
            this.builtBuildingsList.Add(newBuilding);
            tileDestination.TileState = TileState.UNAVAILABLE;
            tileDestination.inhabitingBuilding = newBuilding;
            this.currBuildingCount++;

            UpdateBuildingCounterUI();
        }
    }

    public void DestroyBuilding(LandTile tile)
    {
        Building building = tile.inhabitingBuilding;

        if (building == null)
            return;

        PoolManager.Instance.BuildingPool.Release(building, () =>
        {
            building.ClearData();
        }, () =>
        {
            building.gameObject.SetActive(false);
            this.builtBuildingsList.Remove(building);
            this.currBuildingCount--;
            tile.TileState = TileState.AVAILABLE;
            tile.inhabitingBuilding = null;
        });

        UpdateBuildingCounterUI();
    }

    public void ToggleActiveBuildingsEditMode(bool toggle)
    {
        foreach (Building building in this.builtBuildingsList)
        {
            if (toggle)
                building.TrySetToEditModeTransparency();
            else
                building.ResetEditModeTransparency();
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

    public void TryDestroyBuilding(Vector2 mousePos)
    {
        LandTile tile = TilemapManager.Instance.GetTileFromMousePos(mousePos);

        if (tile == null)
            return;

        if (tile.TileState == TileState.UNAVAILABLE)
            DestroyBuilding(tile);
    }

    #region Builing Placeholder
    public void RegisterBuildingPlacholder(BuildingPlaceholder placeholder)
    {
        this.buildingPlaceholder = placeholder;
    }

    public void TryDisplayBuildingPlaceholder(Vector2 mousePos)
    {
        TileState hoveredTile = TilemapManager.Instance.GetTileAvailabilityFromMousePos(mousePos);

        if (hoveredTile == TileState.NO_TILE)
        {
            this.buildingPlaceholder.TogglePlaceholder(false);
            this.buildingPlaceholderActive = false;
        } else
        {
            if (!this.buildingPlaceholder.IsReady)
                return;
            this.buildingPlaceholder.ChangeTileHighlightColor(hoveredTile == TileState.AVAILABLE);
            this.buildingPlaceholder.TogglePlaceholder(true);
            this.buildingPlaceholder.ChangePosition(TilemapManager.Instance.GetTileCenterFromMousePos(mousePos));

            this.buildingPlaceholderActive = true;
        }
    }

    public void HideBuildingPlaceholder()
    {
        this.buildingPlaceholder.ClearData();
        this.buildingPlaceholder.TogglePlaceholder(false);
        this.buildingPlaceholderActive = false;
    }

    public void ChangePlaceholderColor()
    {
        if (!this.buildingPlaceholderActive)
            return;

        if (!this.buildingPlaceholder.IsReady)
            return;

        this.buildingPlaceholder.ChangePlaceholderColor();
    }

    public void ChangePlaceholderRotation()
    {
        if (!this.buildingPlaceholderActive)
            return;

        if (!this.buildingPlaceholder.IsReady)
            return;

        this.buildingPlaceholder.ChangePlaceholderRotation();
    }
    #endregion

    #region Destroying Placeholder
    public void RegisterDestroyingPlacholder(DestroyingPlaceholder placeholder)
    {
        this.destroyingPlaceholder = placeholder;
    }

    public void TryDisplayDestroyingPlaceholder(Vector2 mousePos)
    {
        TileState hoveredTile = TilemapManager.Instance.GetTileAvailabilityFromMousePos(mousePos);

        if (hoveredTile == TileState.NO_TILE)
        {
            this.destroyingPlaceholder.TogglePlaceholder(false);
        } else
        {
            this.destroyingPlaceholder.ChangeTileHighlightColor(hoveredTile == TileState.UNAVAILABLE);
            this.destroyingPlaceholder.TogglePlaceholder(true);
            this.destroyingPlaceholder.ChangePosition(TilemapManager.Instance.GetTileCenterFromMousePos(mousePos));
        }
    }
    public void HideDestroyingPlaceholder()
    {
        this.destroyingPlaceholder.TogglePlaceholder(false);
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
