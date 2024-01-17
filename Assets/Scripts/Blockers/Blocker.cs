using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{
    [SerializeField] SpriteRenderer blockerRenderer;

    public void SetData(Sprite blockerSprite)
    {
        this.blockerRenderer.sprite = blockerSprite;
    }
}
