using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyingPlaceholder : MonoBehaviour
{
    [SerializeField] SpriteRenderer tileRenderer;

    private Color32 availableColor = new Color32(31, 18, 224, 191);
    private Color32 unavailableColor = new Color32(224, 20, 18, 191);
    private bool tileAvailable = false;

    private void Awake()
    {
        BuildingManager.Instance.RegisterDestroyingPlacholder(this);
        ChangeTileHighlightColor();
        this.gameObject.SetActive(false);
    }

    public void TogglePlaceholder(bool toggle)
    {
        this.gameObject.SetActive(toggle);
    }

    public void ChangePosition(Vector3 newPos)
    {
        this.gameObject.transform.position = newPos;
    }

    private void ChangeTileHighlightColor()
    {
        this.tileRenderer.color = this.tileAvailable ? availableColor : unavailableColor;
    }

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
