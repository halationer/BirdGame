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

    public int Score { get; private set; } = 0;
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
            Score++;
            BestScore = Mathf.Max(BestScore, Score);
        }
    }
}
