using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "Database/BuildingData")]
public class BuildingData : ScriptableObject
{
    public string BuildingId;
    public string BuildingName;
    public BuildingSprites[] BuildingSprites;
    public float BuildTime;
    public BuildingType BuildingType;
}

[System.Serializable]
public class BuildingSprites
{
    public BuildingColor BuildingColor;
    public Sprite[] RotationSprites;
}

public enum BuildingColor
{
    DEFAULT,
    BLUE,
    GREEN,
    RED,
    ORANGE
}
