using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EndUI : MonoBehaviour
{
    public Text score, bestScore;
    public Button restart;

    private void OnEnable()
    {
        if(ScoreManager.Instance != null)
        {
            score.text = ScoreManager.Instance.Score.ToString();
            bestScore.text = ScoreManager.Instance.BestScore.ToString();
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);

        GameManager.Instance.OnGameEnd += delegate () { gameObject.SetActive(true); };
        restart.onClick.AddListener(RestartButtonClick);
    }

    void RestartButtonClick()
    {
        GameManager.Instance.RestartGame();
        gameObject.SetActive(false);
    }
}
