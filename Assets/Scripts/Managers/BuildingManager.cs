using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    private BuildingDataMap buildingDataMap;
    private Building buildingRef;

    public int MaxBuildingCount { get; private set; }

    private int currBuildingCount;
    private BuildingData[] allBuildingData;

    protected override void Initialize()
    {
        this.buildingDataMap = Resources.Load<BuildingDataMap>(ResourcesPaths.BUILDING_DATA_MAP_PATH);
        this.buildingRef = Resources.Load<Building>(ResourcesPaths.BUILDING_PATH);
        this.MaxBuildingCount = this.buildingDataMap.MaxBuildingCount;
        this.allBuildingData = null;
        PoolManager.Instance.CreatePool(PoolId.BUILDING, this.MaxBuildingCount);
        this.currBuildingCount = 0;

        base.Initialize();
    }

    public void BuildBuilding(string buildingId, BuildingType type, int colorIdx, int spriteIdx, LandTile tileDestination, Vector3Int tilePos)
    {
        if (this.currBuildingCount >= this.MaxBuildingCount)
            return;

        BuildingData data = GetBuildingDataById(buildingId, type);
        if (data != null)
        {
            Building newBuilding = PoolManager.Instance.BuildingPool.Get(() => {
                return GameObject.Instantiate<Building>(this.buildingRef);
            });
            newBuilding.SetData(data, colorIdx, spriteIdx);
            newBuilding.Build();
            newBuilding.transform.position = TilemapManager.Instance.GetWorldCenterFromTile(tilePos);
            tileDestination.TileState = TileState.UNAVAILABLE;
            this.currBuildingCount++;

            UpdateBuildingCounterUI();
        }
    }

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
