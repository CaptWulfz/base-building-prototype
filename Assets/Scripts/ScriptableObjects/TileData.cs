using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileData", menuName = "Database/TileData")]
public class TileData : ScriptableObject
{
    public string TileName;
    public Sprite TileSprite;
}
