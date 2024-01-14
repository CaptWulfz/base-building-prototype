using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingDataMap", menuName = "Database/BuildingDataMap")]
public class BuildingDataMap : ScriptableObject
{
    public int MaxBuildingCount;
    public BuildingDataGroup[] BuildingDataGroups;
}

[System.Serializable]
public class BuildingDataGroup
{
    public BuildingType BuildingType;
    public BuildingData[] BuildingData;
}
