using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
    public List<Level> levels;
    Level currentLevel;

    int iter = 0;

    private void Start()
    {
        GameManager.Instance.OnGameStart += OnGameStart;
        GameManager.Instance.OnGameEnd += OnGameEnd;
        GameManager.Instance.OnGameRestart += OnGameRestart;
    }

    void OnGameStart()
    {
        if(LoadNextLevel())
            Debug.LogFormat("Current Level Name: {0}", currentLevel.levelName);
    }

    void OnGameEnd()
    {
        StopCurrentLevel();
    }

    void OnGameRestart()
    {
        ResetLevels();
    }

    public void LoadLevel(Level levelType)
    {
        StopCurrentLevel();
        currentLevel = Instantiate(levelType, transform);
        currentLevel.onLevelEnd += delegate () { LoadNextLevel(); };
    }

    public void LoadLevel(int levelId)
    {
        Level levelType = levels.Find(level => level.levelId == levelId);
        iter = levels.IndexOf(levelType);
        LoadLevel(levelType);
    }


    public bool LoadNextLevel()
    {
        if(iter < levels.Count)
        {
            LoadLevel(levels[iter++]);
            return true;
        }
        Debug.LogWarning("Already pass the final level");
        GameManager.Instance.WinGame();
        return false;
    }

    public void ResetLevels()
    {
        StopCurrentLevel();
        iter = 0;
    }

    public void StopCurrentLevel()
    {
        if(currentLevel != null)
        {
            Destroy(currentLevel);
        }
    }
}
