using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateDebuger : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnGameStart += delegate () { Debug.Log("Game is Playing"); };
        GameManager.Instance.OnGameEnd += delegate () { Debug.LogWarning("Game is End"); };
        GameManager.Instance.OnGameRestart += delegate () { Debug.Log("Game is Restart"); };
    }
}
