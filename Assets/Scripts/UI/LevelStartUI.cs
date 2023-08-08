using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStartUI : UISingleton<LevelStartUI>
{
    Text text;

    private void Start()
    {
        gameObject.SetActive(false);
        text = GetComponentInChildren<Text>();
    }

    public void Show(string message)
    {
        text.text = message;
        gameObject.SetActive(true);
    }

    public void AnimEnd()
    {
        gameObject.SetActive(false);
    }
}
