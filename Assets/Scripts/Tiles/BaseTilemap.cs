using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BaseTilemap : MonoBehaviour
{
    public Vector3Int vectorPos;
    public Vector2Int tilemapSize;

    private Controls.MouseActions mouseControls;

    // Start is called before the first frame update
    void Awake()
    {
        TilemapManager.Instance.RegisterTilemap(this.GetComponent<Tilemap>());
        TilemapManager.Instance.RegisterTilemapGrid(this.transform.parent.GetComponent<Grid>());
        this.mouseControls = InputManager.Instance.Controls.Mouse;
        this.mouseControls.LeftClick.performed += _ => MouseClick();
    }

    private void MouseClick()
    {
        if (!InputManager.Instance.AllowInput)
            return;

        Vector2 mousePos = this.mouseControls.Position.ReadValue<Vector2>();
        TilemapManager.Instance.TryBuildOnTile(mousePos);
    }
}
