using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    Text scoreText;
    
    void Start()
    {
        gameObject.SetActive(false);
        scoreText = GetComponent<Text>();

        GameManager.Instance.OnGameStart += delegate () { gameObject.SetActive(true); };
        GameManager.Instance.OnGameEnd += delegate () { gameObject.SetActive(false); };
    }

    void Update()
    {
        scoreText.text = ScoreManager.Instance.Score.ToString();
    }
}
