using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockerDataMap", menuName = "Database/BlockerDataMap")]
public class BlockerDataMap : ScriptableObject
{
    public BlockerData[] BlockerData;
}
