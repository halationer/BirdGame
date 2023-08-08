using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
    BirdAttack bird;
    const int imageWidth = 57;

    private void Start()
    {
        bird = FindObjectOfType<BirdAttack>();
    }

    private void Update()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2((bird.life - 1) * imageWidth, rectTransform.sizeDelta.y);
    }
}
