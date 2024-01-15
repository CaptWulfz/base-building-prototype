using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private Dictionary<GameState, StateBehavior> statesList;
    private StateUpdateRunner stateUpdateRunner;

    public GameState GameState { get; set; }

    protected override void Initialize()
    {
        this.GameState = GameState.VIEWING;
        this.statesList = new Dictionary<GameState, StateBehavior>();
        CreateStateUpdateRunner();
        InitializeStates();
        base.Initialize();
    }

    private void InitializeStates()
    {
        this.statesList.Add(GameState.VIEWING, new ViewingStateBehavior());
        this.statesList.Add(GameState.BUILDING, new BuildingStateBehavior());
    }

    private void CreateStateUpdateRunner()
    {
        this.stateUpdateRunner ??= new GameObject().AddComponent<StateUpdateRunner>();
        this.stateUpdateRunner.gameObject.name = "StateUpdateRunner";
    }

    public void ChangeState(GameState nextState)
    {
        if (nextState == this.GameState)
            return;

        this.stateUpdateRunner.RunUpdate = false;
        this.stateUpdateRunner.UpdateAction = null;
        this.statesList[this.GameState].OnExit();
        this.GameState = nextState;
        this.statesList[this.GameState].OnEnter();
        this.stateUpdateRunner.RunUpdate = true;
        this.stateUpdateRunner.UpdateAction = () =>
        {
            this.statesList[this.GameState].OnUpdate();
        };
    }
}

public enum GameState
{
    LOADING,
    VIEWING,
    BUILDING,
    DESTROYING
}