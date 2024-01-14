using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlaceholder : MonoBehaviour
{
    [SerializeField] SpriteRenderer buildingRenderer;
    [SerializeField] SpriteRenderer tileRenderer;

    private Color availableColor = new Color(31, 18, 224, 191);
    private Color unavailableColor = new Color(224, 20, 18, 191);

    private BuildingData buildingData;
    private int colorSpriteIdx = 0;
    private int rotationSpriteIdx = 0;

    public void SetData(string buildingId, BuildingType type)
    {
        this.buildingData = BuildingManager.Instance.GetBuildingDataById(buildingId, type);
    }

    private void ChangeBuildingSprite()
    {
        this.buildingRenderer.sprite = this.buildingData.BuildingSprites[this.colorSpriteIdx].RotationSprites[this.rotationSpriteIdx];
    }

    public void ChangePlaceholderColor(int idx)
    {
        this.colorSpriteIdx = idx;
        ChangeBuildingSprite();
    }

    public void ChangePlaceholderRotation(int idx)
    {
        this.rotationSpriteIdx = idx;
        ChangeBuildingSprite();
    }

    public void ChangeTileHighlightColor(bool available)
    {
        this.tileRenderer.color = available ? availableColor : unavailableColor;
    }
}
