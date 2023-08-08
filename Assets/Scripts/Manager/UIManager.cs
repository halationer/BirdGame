using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public Text levelNameUI;

    public Text wolUI;
    public Color winTextColor, loseTextColor;

    private void Start()
    {
        GameManager.Instance.OnGameEnd += ShowWolUI;
    }

    public void ShowLevelStartUI(string message)
    {
        LevelStartUI.Instance.Show(message);
    }

    public void ShowLevelNameUI(string message)
    {
        levelNameUI.text = message;
    }

    public void ShowWolUI()
    {
        wolUI.text = GameManager.Win ? "Win" : "Lose";
        wolUI.color = GameManager.Win ? winTextColor : loseTextColor;
    }
}
