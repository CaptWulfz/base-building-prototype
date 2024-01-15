using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LandTile : Tile
{
    public TileState TileState { get; set; }
}

public enum TileState
{
    NO_TILE,
    AVAILABLE,
    UNAVAILABLE
}
