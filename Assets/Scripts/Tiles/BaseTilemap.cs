using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BaseTilemap : MonoBehaviour
{
    public Vector3Int vectorPos;
    public Vector2Int tilemapSize;

    // Start is called before the first frame update
    void Awake()
    {
        TilemapManager.Instance.RegisterTilemap(this.GetComponent<Tilemap>());
        TilemapManager.Instance.RegisterTilemapGrid(this.transform.parent.GetComponent<Grid>());
    }
}
