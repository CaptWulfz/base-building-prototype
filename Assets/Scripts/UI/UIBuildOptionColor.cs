using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class UIBuildOptionColor : MonoBehaviour
{
    private Image optionColor;

    private void Awake()
    {
        this.optionColor = this.GetComponent<Image>();
    }

    public void SetColor(Color color)
    {
        if (this.optionColor == null)
            this.optionColor = this.GetComponent<Image>();

        this.optionColor.color = color;
    }
}
