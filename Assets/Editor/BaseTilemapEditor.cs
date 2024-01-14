using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BaseTilemap))]
public class BaseTilemapEditor : Editor
{
    private BaseTilemap baseTilemap;

    private void OnEnable()
    {
        this.baseTilemap = (BaseTilemap)target;
    }

    public override void OnInspectorGUI()
    {
        this.baseTilemap.vectorPos = EditorGUILayout.Vector3IntField("Tile Position", this.baseTilemap.vectorPos);
        this.baseTilemap.tilemapSize = EditorGUILayout.Vector2IntField("Tilemap Size", this.baseTilemap.tilemapSize);

        if (GUILayout.Button("Create Tile"))
        {
            Debug.Log("QQQ Vector 3: " + this.baseTilemap.vectorPos);
            TilemapManager.Instance.CreateNewTileAtPositon(this.baseTilemap.vectorPos);
        }

        if (GUILayout.Button("Delete Tile"))
        {
            TilemapManager.Instance.DeleteTileAtPosition(this.baseTilemap.vectorPos);
        }

        if (GUILayout.Button("Generate Tilemap by Size"))
        {
            TilemapManager.Instance.GenerateTilemap(this.baseTilemap.tilemapSize);
        }

        //if (GUILayout.Button("Delete Tilemap"))
        //{
            
        //}
    }
}
