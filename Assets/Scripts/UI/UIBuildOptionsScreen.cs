using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuildOptionsScreen : MonoBehaviour
{
    [SerializeField] UIBuildOptionsTab[] buildOptionsTabs; 
    [SerializeField] UIBuildOption buildOptionRef;
    [SerializeField] Transform buildOptionParent;

    private List<UIBuildOption> buildOptionsList = new List<UIBuildOption>();

    private BuildOptionsTab selectedTab;
    private BuildingData[] activeBuildingData;

    private void OnEnable()
    {
        ToggleBuildButton(false);
        Initialize();
        GameManager.Instance.ChangeState(GameState.BUILDING);
    }

    private void OnDisable()
    {
        ToggleBuildButton(true);
        ReleaseAllCurrentBuildOptions();
        GameManager.Instance.ChangeState(GameState.VIEWING);
    }

    private void ToggleBuildButton(bool toggle)
    {
        Parameters param = new Parameters();
        param.AddParameter(ParameterNames.BUILD_BUTTON_TOGGLE, toggle);
        EventBroadcaster.Instance.PostEvent(UIEvents.TOGGLE_BUILD_BUTTON, param);
    }

    private void Initialize()
    {
        this.buildOptionRef.gameObject.SetActive(false);
        this.selectedTab = BuildOptionsTab.ALL;

        SetupBuildOptionsTab();
        CreateBuildOptions();
    }

    private void CreateBuildOptions()
    {
        PoolManager.Instance.CreatePool(PoolId.UI_BUILDING_OPTION, BuildingManager.Instance.GetAllBuildingData().Length);

        this.activeBuildingData = GetBuildingDataBySelectedTab();

        for (int i = 0; i < this.activeBuildingData.Length; i++)
        {
            UIBuildOption newOption = PoolManager.Instance.UIBuildOptionPool.Get(() =>
            {
                return GameObject.Instantiate(this.buildOptionRef, this.buildOptionParent);
            });
            if (newOption != null)
            {
                newOption.SetupUIItem(this.activeBuildingData[i]);
                newOption.gameObject.SetActive(true);
                newOption.gameObject.transform.SetAsLastSibling();
                this.buildOptionsList.Add(newOption);
            }
        }
    }

    private BuildingData[] GetBuildingDataBySelectedTab()
    {
        BuildingData[] dataArray = null;

        switch (this.selectedTab)
        {
            case BuildOptionsTab.ALL:
                dataArray = BuildingManager.Instance.GetAllBuildingData();
                break;
            case BuildOptionsTab.WALLS:
                dataArray = BuildingManager.Instance.GetBuildingData(BuildingType.WALL);
                break;
            case BuildOptionsTab.HOUSES:
                dataArray = BuildingManager.Instance.GetBuildingData(BuildingType.HOUSE);
                break;
            case BuildOptionsTab.TOWERS:
                dataArray = BuildingManager.Instance.GetBuildingData(BuildingType.TOWER);
                break;
        }

        return dataArray;
    }

    private void ReleaseAllCurrentBuildOptions()
    {
        while (this.buildOptionsList.Count > 0)
        {
            PoolManager.Instance.UIBuildOptionPool.Release(this.buildOptionsList[0], () => {
                this.buildOptionsList[0].ClearData();
            }, () => {
                this.buildOptionsList[0].gameObject.SetActive(false);
                this.buildOptionsList.RemoveAt(0);
            });
        }
    }

    private void SelectBuildOptionsTab(BuildOptionsTab tab)
    {
        if (this.selectedTab == tab)
            return;

        this.selectedTab = tab;
        ReleaseAllCurrentBuildOptions();
        CreateBuildOptions();
    }

    #region Button Events
    private void SetupBuildOptionsTab()
    {
        foreach (UIBuildOptionsTab tab in this.buildOptionsTabs)
        {
            tab.SetClickAction(SelectBuildOptionsTab);
        }
    }

    public void OnCloseButton()
    {
        this.gameObject.SetActive(false);
    }
    #endregion
}

public enum BuildOptionsTab
{
    ALL,
    WALLS,
    HOUSES,
    TOWERS
}
