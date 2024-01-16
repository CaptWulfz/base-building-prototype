using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildingProgressBar : MonoBehaviour
{
    private Slider progressBar;

    private void OnEnable()
    {
        this.progressBar.value = 0f;
    }

    private void Awake()
    {
        //EventBroadcaster.Instance.RemoveObserver(UIEvents.UPDATE_BUILDING_PROGRESS_BAR);
        //EventBroadcaster.Instance.AddObserver(UIEvents.UPDATE_BUILDING_PROGRESS_BAR, UpdateBuildingProgressBarEvent);

        //EventBroadcaster.Instance.RemoveObserver(UIEvents.TOGGLE_BUILDING_PROGRESS_BAR);
        //EventBroadcaster.Instance.AddObserver(UIEvents.TOGGLE_BUILDING_PROGRESS_BAR, ToggleBuildingProgressBarEvent);

        this.progressBar = GetComponent<Slider>();
        this.progressBar.value = 0f;
        this.gameObject.SetActive(false);
    }

    public void UpdateBuildingProgressBar(float progress)
    {
        progress = progress > 1 ? 1 : progress;
        this.progressBar.value = progress;

        if (progress >= 1f)
            this.gameObject.SetActive(false);
    }

    public void ToggleBuildingProgressBar(bool toggle)
    {
        this.gameObject.SetActive(toggle);
    }

    //#region Event Broadcaster Events
    //private void UpdateBuildingProgressBarEvent(Parameters param)
    //{
    //    float progress = param.GetParameter<float>(ParameterNames.BUILDING_PROGRESS_VALUE, 0f);
    //    progress = progress > 1 ? 1 : progress;
    //    this.progressBar.value = progress;

    //    if (progress >= 1f)
    //        this.gameObject.SetActive(false);
    //}

    //private void ToggleBuildingProgressBarEvent(Parameters param)
    //{
    //    bool active = param.GetParameter<bool>(ParameterNames.BUILDING_PROGRESS_TOGGLE, false);
    //    this.gameObject.SetActive(active);
    //}
    //#endregion
}
