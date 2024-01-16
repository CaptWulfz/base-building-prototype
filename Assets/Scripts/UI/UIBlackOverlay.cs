using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBlackOverlay : MonoBehaviour
{
    [SerializeField] GameObject blackBg;
    [SerializeField] GameObject overlayText;

    private void Awake()
    {
        GameManager.Instance.RegisterBlackOverlay(this);
        ToggleOverlay(false);
    }

    public void ToggleOverlay(bool toggle)
    {
        this.blackBg.SetActive(toggle);
    }

    public void ToggleOverlayText(bool toggle)
    {
        this.overlayText.SetActive(toggle);
    }
}
