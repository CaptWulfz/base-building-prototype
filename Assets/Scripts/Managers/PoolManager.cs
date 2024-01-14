using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public ObjectPool<Building> BuildingPool { get; private set; }
    public ObjectPool<UIBuildOption> UIBuildOptionPool { get; private set; }

    protected override void Initialize()
    {
        base.Initialize();
    }

    public void CreatePool(string poolId, int poolSize)
    {
        switch (poolId)
        {
            case PoolId.BUILDING:
                this.BuildingPool ??= new ObjectPool<Building>(poolSize);
                break;
            case PoolId.UI_BUILDING_OPTION:
                this.UIBuildOptionPool ??= new ObjectPool<UIBuildOption>(poolSize);
                break;
        }
    }
}

public class PoolId
{
    public const string BUILDING = "BUILDING";
    public const string UI_BUILDING_OPTION = "UI_BUILDING_OPTION";
}
