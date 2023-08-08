using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int levelId;
    public string levelName;
    public List<SpawnRule> rules = new();

    public event Action onLevelEnd;

    int ruleEndCount = 0;
    AIBoss boss;

    private void Start()
    {
        LoadAndStart();
    }

    void LoadAndStart()
    {
        string levelStr = string.Format("Level  {0}  -  {1}", levelId, levelName);
        UIManager.Instance.ShowLevelStartUI(levelStr);
        UIManager.Instance.ShowLevelNameUI(levelStr);

        foreach (var rule in rules)
        {
            if (rule == null) { RuleEnd(); continue; }
            SpawnRule ruleObj = Instantiate<SpawnRule>(rule, transform);
            ruleObj.onRuleEnd += delegate () { RuleEnd(); };
            StartCoroutine(ruleObj.GenerateBirdByRule());
        }
    }

    void RuleEnd()
    {
        ruleEndCount++;
        if(ruleEndCount == rules.Count)
        {
            boss = BirdFactory.Instance.GenerateBoss();
            boss.onDie += LevelEnd;
        }
    }

    void LevelEnd()
    {
        onLevelEnd?.Invoke();
    }

    private void OnDestroy()
    {
        if(boss != null)
        {
            boss.onDie -= LevelEnd;
        }
    }
}
