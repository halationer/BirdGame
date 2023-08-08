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
public enum GameResult
{
    Win,
    Lose,
}

public class GameManager : MonoBehaviour
{
    public GameState state { get; private set; }
    public GameResult result { get; private set; }

    public static bool Win { get => Instance.result == GameResult.Win; }

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

    public void WinGame()
    {
        EndGame(GameResult.Win);
    }

    public void LoseGame()
    {
        EndGame();
    }

    private void EndGame(GameResult result = GameResult.Lose)
    {
        if(state != GameState.Playing) return;

        state = GameState.End;
        this.result = result; 
        OnGameEnd?.Invoke();
    }

    public void RestartGame()
    {
        if (state != GameState.End) return;

        state = GameState.Ready;
        OnGameRestart?.Invoke();
    }

    [MenuItem("Game/End", false, 1000)]
    public static void MenuEnd()
    {
        Instance.WinGame();
    }

    [MenuItem("Game/End", true, 1000)]
    public static bool VarifyEnd()
    {
        return Instance != null && Instance.state == GameState.Playing;
    }

    //[MenuItem("Game/Restart", false, 1001)]
    //public static void MenuRestart()
    //{
    //    Instance.RestartGame();
    //}

    //[MenuItem("Game/Restart", true, 1001)]
    //public static bool VarifyRestart()
    //{
    //    return Instance != null && Instance.state == GameState.End;
    //}
}
