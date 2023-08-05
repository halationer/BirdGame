using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Action<int> OnScoreChange;

    private int score = 0;
    public int Score
    {
        get { return score; }
        set { score = value; OnScoreChange(score); }
    }
    public int BestScore { get; private set; } = 0;

    private void Start()
    {
        GameManager.Instance.OnGameRestart += OnGameRestart;
    }

    void OnGameRestart()
    {
        Score = 0;
    }

    public void TriggerAddScore(Collider2D other)
    {
        if(other.gameObject.tag == "player")
        {
            AddScore();
        }
    }

    public void AddScore(int score = 1)
    {
        Score += score;
        BestScore = Mathf.Max(BestScore, Score);
    }
}
