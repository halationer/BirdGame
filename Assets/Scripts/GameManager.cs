using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public enum GameState
{
    Ready,
    Playing,
    End
}

public class GameManager : MonoBehaviour
{
    public GameState state { get; private set; }

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this; 
    }

    public event Action OnGameStart;
    public event Action OnGameEnd;
    public event Action OnGameRestart;

    // Start is called before the first frame update
    void Start()
    {
        state = GameState.Ready;
    }

    public void StartGame()
    {
        if (state != GameState.Ready) return;

        state = GameState.Playing;
        OnGameStart?.Invoke();
    }

    public void EndGame()
    {
        if(state != GameState.Playing) return;

        state = GameState.End;
        OnGameEnd?.Invoke();
    }

    public void RestartGame()
    {
        if (state != GameState.End) return;

        state = GameState.Ready;
        OnGameRestart?.Invoke();
    }

    [MenuItem("Game/Restart", false, 1000)]
    public static void MenuRestart()
    {
        Instance.RestartGame();
    }

    [MenuItem("Game/Restart", true, 1000)]
    public static bool VarifyRestart()
    {
        return Instance != null;
    }
}
