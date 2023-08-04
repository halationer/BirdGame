using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    Text scoreText;

    private void Start()
    {
        scoreText = GetComponent<Text>();
        ScoreManager.Instance.OnScoreChange += ChangeScore;
    }

    void ChangeScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
