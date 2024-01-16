using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildOption : MonoBehaviour, IUIClickable
{
    [Header("Option Settings")]
    [SerializeField] Image optionImage;
    [SerializeField] Text optionName;
    [SerializeField] Text optionBuildTime;

    [Header("Option Color Settings")]
    [SerializeField] UIBuildOptionColor[] optionColors;

    private const string OPTION_NAME_FORMAT = "Name: {0}";
    private const string OPTION_BUILD_TIME_FORMAT = "{0}s";

    private string buildingId;
    private BuildingType buildingType;

    public void SetupUIItem(BuildingData data)
    {
        this.buildingId = data.BuildingId;
        this.buildingType = data.BuildingType;
        this.optionImage.sprite = data.BuildingSprites[0].RotationSprites[0];
        this.optionName.text = data.BuildingName;
        this.optionBuildTime.text = string.Format(OPTION_BUILD_TIME_FORMAT, data.BuildTime);
        SetBuildOptionColors(data);
    }

    public void ClearData()
    {
        this.buildingId = null;
        this.optionImage.sprite = null;
        this.optionName.text = null;
        this.optionBuildTime.text = null;
        foreach (UIBuildOptionColor optionColor in this.optionColors)
            optionColor.gameObject.SetActive(false);
    }

    private void SetBuildOptionColors(BuildingData data)
    {
        for (int i = 0; i < this.optionColors.Length; i++)
        {
            UIBuildOptionColor optionColor = this.optionColors[i];
            
            if (i < data.BuildingSprites.Length)
            {
                BuildingSprites sprite = data.BuildingSprites[i];
                Color32 colorToSet = Color.gray;
                switch (sprite.BuildingColor)
                {
                    case BuildingColor.DEFAULT:
                        colorToSet = Color.gray;
                        break;
                    case BuildingColor.BLUE:
                        colorToSet = Color.blue;
                        break;
                    case BuildingColor.GREEN:
                        colorToSet = Color.green;
                        break;
                    case BuildingColor.RED:
                        colorToSet = Color.red;
                        break;
                    case BuildingColor.ORANGE:
                        colorToSet = new Color32(230, 144, 78, 255);
                        break;
                }

                optionColor.SetColor(colorToSet);
                optionColor.gameObject.SetActive(true);
            } else
            {
                optionColor.gameObject.SetActive(false);
            }
        }
    }

    public void OnClick()
    {
        if (!InputManager.Instance.AllowInput)
            return;

        BuildingManager.Instance.SelectActiveBuildingData(this.buildingId, this.buildingType);
    }
}
