using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlaceholder : MonoBehaviour
{
    [SerializeField] SpriteRenderer buildingRenderer;
    [SerializeField] SpriteRenderer tileRenderer;

    private Color32 availableColor = new Color32(31, 18, 224, 191);
    private Color32 unavailableColor = new Color32(224, 20, 18, 191);

    public bool IsReady { get; private set; }

    private BuildingData buildingData;
    private int colorSpriteIdx = 0;
    private int rotationSpriteIdx = 0;
    private bool tileAvailable = false;


    private void Awake()
    {
        BuildingManager.Instance.RegisterBuildingPlacholder(this);
        this.gameObject.SetActive(false);
    }

    public void SetData(BuildingData data)
    {
        this.buildingData = data;
        this.colorSpriteIdx = 0;
        this.rotationSpriteIdx = 0;
        ChangeBuildingSprite();
        ChangeTileHighlightColor();
        this.IsReady = true;
    }

    public void ClearData()
    {
        this.buildingData = null;
        this.colorSpriteIdx = 0;
        this.rotationSpriteIdx = 0;
        this.tileAvailable = false;
        this.IsReady = false;
    }

    public void TogglePlacholder(bool toggle)
    {
        this.gameObject.SetActive(toggle);
    }

    public void ChangePosition(Vector3 newPos)
    {
        this.gameObject.transform.position = newPos;
    }

    private void ChangeBuildingSprite()
    {
        this.buildingRenderer.sprite = this.buildingData.BuildingSprites[this.colorSpriteIdx].RotationSprites[this.rotationSpriteIdx];
    }

    private void ChangeTileHighlightColor()
    {
        this.tileRenderer.color = this.tileAvailable ? availableColor : unavailableColor;
    }
    
    public PendingBuilding GetPendingBuildingData()
    {
        return new PendingBuilding(this.buildingData, this.colorSpriteIdx, this.rotationSpriteIdx);
    }


    #region Placeholder Sprite Change Methods
    public void ChangePlaceholderColor()
    {
        int idx = this.colorSpriteIdx;

        if (this.buildingData.BuildingSprites.Length > 1)
        {
            idx++;
            if (idx >= this.buildingData.BuildingSprites.Length)
                idx = 0;
        }

        if (this.colorSpriteIdx == idx)
            return;

        this.colorSpriteIdx = idx;
        ChangeBuildingSprite();
    }

    public void ChangePlaceholderRotation()
    {
        int idx = this.rotationSpriteIdx;

        if (this.buildingData.BuildingSprites[this.colorSpriteIdx].RotationSprites.Length > 1)
        {
            idx++;
            if (idx >= this.buildingData.BuildingSprites[this.colorSpriteIdx].RotationSprites.Length)
                idx = 0;
        }
        if (this.rotationSpriteIdx == idx)
            return;

        this.rotationSpriteIdx = idx;
        ChangeBuildingSprite();
    }
    #endregion

    #region Tile Highlight Change Methods
    public void ChangeTileHighlightColor(bool available)
    {
        if (this.tileAvailable == available)
            return;

        this.tileAvailable = available;
        ChangeTileHighlightColor();
        
    }
    #endregion
}

public struct PendingBuilding
{
    public BuildingData buildingData;
    public int colorIdx;
    public int rotationIdx;

    public PendingBuilding(BuildingData data, int color, int rotation)
    {
        this.buildingData = data;
        this.colorIdx = color;
        this.rotationIdx = rotation;
    }
}
