using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildingCounter : MonoBehaviour
{
    [SerializeField] private Text counterText;

    private const string COUNTER_FORMAT = "Buildings: {0} / {1}";

    private void Awake()
    {
        EventBroadcaster.Instance.RemoveObserver(UIEvents.UPDATE_BUILDING_COUNTER);
        EventBroadcaster.Instance.AddObserver(UIEvents.UPDATE_BUILDING_COUNTER, UpdateBuildingCounterEvent);

        this.counterText.text = string.Format(COUNTER_FORMAT, 0, BuildingManager.Instance.MaxBuildingCount);
    }

    #region Event Broadcaster Events
    private void UpdateBuildingCounterEvent(Parameters param)
    {
        int buildingCount = param.GetParameter<int>(ParameterNames.BUILDING_COUNT, 0);

        this.counterText.text = string.Format(COUNTER_FORMAT, buildingCount, BuildingManager.Instance.MaxBuildingCount);
    }
    #endregion
}
