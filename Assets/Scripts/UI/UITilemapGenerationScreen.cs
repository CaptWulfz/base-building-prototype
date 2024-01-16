using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITilemapGenerationScreen : MonoBehaviour
{
    [SerializeField] GameObject bg;
    [SerializeField] InputField xField;
    [SerializeField] InputField yField;

    private const int MIN_VALUE = 2;
    private const int MAX_VALUE = 150;

    private int sizeX;
    private int sizeY;

    private void Awake()
    {
        EventBroadcaster.Instance.RemoveObserver(UIEvents.TOGGLE_TILEMAP_GENERATION_SCREEN);
        EventBroadcaster.Instance.AddObserver(UIEvents.TOGGLE_TILEMAP_GENERATION_SCREEN, ToggleTilemapGenerationScreenEvent);

        this.xField.text = MIN_VALUE.ToString();
        this.yField.text = MIN_VALUE.ToString();
        EvaluateInputs();
    }

    public void EvaluateInputs()
    {
        this.sizeX = int.Parse(this.xField.text);
        this.sizeY = int.Parse(this.yField.text);

        this.sizeX = Mathf.Clamp(this.sizeX, MIN_VALUE, MAX_VALUE);
        this.sizeY = Mathf.Clamp(this.sizeY, MIN_VALUE, MAX_VALUE);

        this.xField.text = this.sizeX.ToString();
        this.yField.text = this.sizeY.ToString();
    }

    public void OnGenerate()
    {
        TilemapManager.Instance.GenerateTilemap(new Vector2Int(this.sizeX, this.sizeY));
    }

    #region Event Broadcaster Events
    private void ToggleTilemapGenerationScreenEvent(Parameters param)
    {
        bool toggle = param.GetParameter<bool>(ParameterNames.TILEMAP_GENERATION_SCREEN_TOGGLE, false);
        this.bg.SetActive(toggle);
    }
    #endregion
}
