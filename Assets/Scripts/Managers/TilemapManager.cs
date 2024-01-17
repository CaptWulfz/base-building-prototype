using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : Singleton<TilemapManager>
{
    private Grid tilemapGrid;
    private Tilemap tilemap;

    private TileData tileData;
    protected override void Initialize()
    {
        this.tileData = Resources.Load<TileData>(ResourcesPaths.TILE_DATA_PATH);
        base.Initialize();
    }

    public void RegisterTilemap(Tilemap tilemap)
    {
        this.tilemap = tilemap;
    }

    public void RegisterTilemapGrid(Grid grid)
    {
        this.tilemapGrid = grid;
    }

    public Vector3 GetWorldCenterFromTile(Vector3Int tilePos)
    {
        return this.tilemapGrid.GetCellCenterWorld(tilePos);
    }

    public Vector3 GetTileCenterFromMousePos(Vector2 mousePos)
    {
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int tilePos = this.tilemapGrid.WorldToCell(mousePos);
        return GetWorldCenterFromTile(tilePos);
    }

    public void GenerateTilemap(Vector2Int size)
    {
        GameManager.Instance.ToggleBlackOverlayText(true);
        BuildingManager.Instance.CreateBuildingPool(size.x * size.y / 2);

        Parameters param = new Parameters();
        param.AddParameter(ParameterNames.TILEMAP_GENERATION_SCREEN_TOGGLE, false);
        EventBroadcaster.Instance.PostEvent(UIEvents.TOGGLE_TILEMAP_GENERATION_SCREEN, param);

        float tilesPerSideX = size.x / 2;
        int intTilesPerSideX = Mathf.FloorToInt(tilesPerSideX);
        int positiveXLimit = size.x % 2 == 0 ? intTilesPerSideX - 1 : intTilesPerSideX;

        float tilesPerSideY = size.y / 2;
        int intTilesPerSideY = Mathf.FloorToInt(tilesPerSideY);
        int positiveYLimit = size.y % 2 == 0 ? intTilesPerSideY - 1 : intTilesPerSideY;

        Vector3Int newTilePos = Vector3Int.zero;

        // Iterate through x axis ascending first
        for (int i = 0; i <= positiveXLimit; i++)
        {
            newTilePos.x = i;

            // Iterate through y axis ascending first
            for (int j = 0; j <= positiveYLimit; j++)
            {
                newTilePos.y = j;
                CreateNewTileAtPositon(newTilePos);
            }

            for (int j = -1; j >= -intTilesPerSideY; j--)
            {
                newTilePos.y = j;
                CreateNewTileAtPositon(newTilePos);
            }
        }

        // Iterate through x axis descending
        for (int i = -1; i >= -intTilesPerSideX; i--)
        {
            newTilePos.x = i;

            // Iterate through y axis ascending first
            for (int j = 0; j <= positiveYLimit; j++)
            {
                newTilePos.y = j;
                CreateNewTileAtPositon(newTilePos);
            }

            for (int j = -1; j >= -intTilesPerSideY; j--)
            {
                newTilePos.y = j;
                CreateNewTileAtPositon(newTilePos);
            }
        }

        GameManager.Instance.ToggleBlackOverlay(false);
        
        Parameters param2 = new Parameters();
        param2.AddParameter<bool>(ParameterNames.CAMERA_HELP_SCREEN_TOGGLE, true);
        EventBroadcaster.Instance.PostEvent(UIEvents.TOGGLE_CAMERA_HELP_SCREEN, param2);
    }

    public void CreateNewTileAtPositon(Vector3Int position)
    {
        if (this.tilemap != null)
        {
            LandTile newTile = ScriptableObject.CreateInstance<LandTile>();
            newTile.sprite = this.tileData.TileSprite;
            newTile.TileState = TileState.AVAILABLE;
            newTile.TilePos = position;
            this.tilemap.SetTile(position, newTile);
        }
    }

    public void DeleteTileAtPosition(Vector3Int position)
    {
        if (this.tilemap != null)
        {
            this.tilemap.SetTile(position, null);
        }
    }

    //public void TryBuildOnTile(Vector2 mousePos)
    //{
    //    mousePos = Camera.main.ScreenToWorldPoint(mousePos);
    //    Vector3Int tilePos = this.tilemapGrid.WorldToCell(mousePos);
    //    if (this.tilemap.HasTile(tilePos))
    //    {
    //        LandTile clickedTile = this.tilemap.GetTile<LandTile>(tilePos);
    //        if (clickedTile.TileState == TileState.AVAILABLE)
    //        {
    //            BuildingManager.Instance.BuildBuilding("WALL", BuildingType.WALL, 0, 0, clickedTile, tilePos);
    //        }
    //    }
    //}

    public TileState GetTileAvailabilityFromMousePos(Vector2 mousePos)
    {
        TileState hoveredTileState = TileState.NO_TILE;
        LandTile hoveredTile = GetTileFromMousePos(mousePos);
        if (hoveredTile != null)
            hoveredTileState = hoveredTile.TileState;

        return hoveredTileState;
    }

    public LandTile GetTileFromMousePos(Vector2 mousePos)
    {
        LandTile tile = null;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int tilePos = this.tilemapGrid.WorldToCell(mousePos);
        if (this.tilemap.HasTile(tilePos))
        {
            tile = this.tilemap.GetTile<LandTile>(tilePos);
        }

        return tile;
    }
}
