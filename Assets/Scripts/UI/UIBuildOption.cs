using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildOption : MonoBehaviour, IUIClickable
{
    [SerializeField] Image optionImage;
    [SerializeField] Text optionName;
    [SerializeField] Text optionBuildTime;

    private const string OPTION_NAME_FORMAT = "Name: {0}";
    private const string OPTION_BUILD_TIME_FORMAT = "Build Time: {0}s";

    private string buildingId;
    private BuildingType buildingType;

    public void SetupUIItem(BuildingData data)
    {
        this.buildingId = data.BuildingId;
        this.buildingType = data.BuildingType;
        this.optionImage.sprite = data.BuildingSprites[0].RotationSprites[0];
        this.optionName.text = string.Format(OPTION_NAME_FORMAT, data.BuildingName);
        this.optionBuildTime.text = string.Format(OPTION_BUILD_TIME_FORMAT, data.BuildTime);
    }

    public void ClearData()
    {
        this.buildingId = null;
        this.optionImage.sprite = null;
        this.optionName.text = null;
        this.optionBuildTime.text = null;
    }

    public void OnClick()
    {
        if (!InputManager.Instance.AllowInput)
            return;

        BuildingManager.Instance.SelectActiveBuildingData(this.buildingId, this.buildingType);
    }
}
