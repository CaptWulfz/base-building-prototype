using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Pointer Events
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        BuildingManager.Instance.AllowEditing = false;
        BuildingManager.Instance.HideBuildingPlaceholder(false);
        BuildingManager.Instance.HideDestroyingPlaceholder();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        BuildingManager.Instance.AllowEditing = true;
    }
    #endregion
}
