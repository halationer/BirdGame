using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    Text scoreText;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        scoreText = GetComponent<Text>();
        ScoreManager.Instance.OnScoreChange += ChangeScore;
    }

    void ChangeScore(int score)
    {
        int scoreDiff = score - int.Parse(scoreText.text);
        if (scoreDiff >= 8) animator.SetTrigger("ScaleUpSuper");
        else animator.SetTrigger("ScaleUp");
        scoreText.text = score.ToString();
    }
}
