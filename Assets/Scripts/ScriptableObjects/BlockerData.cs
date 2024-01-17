using UnityEngine;

[CreateAssetMenu(fileName = "BlockerData", menuName = "Database/BlockerData")]
public class BlockerData : ScriptableObject
{
    public string BlockerName;
    public Sprite[] BlockerSprites;
}
