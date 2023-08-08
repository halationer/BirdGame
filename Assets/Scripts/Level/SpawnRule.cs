using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
//   开始时间
//   怪物类型
//   最大血量
//   子弹类型
//   生成批次
//   每批数量
//   批次间隔
//   持续时间
//   道具掉落规则
/// </summary>

public class SpawnRule : MonoBehaviour
{
    public float        startTime       = 0f;
    public GameObject   enemyType           ;
    public float        maxHp           = 1f;
    public GameObject   bulletType          ;
    public int          spawnBatch      = 10;
    public int          enemyPerBatch   = 1;
    public float        timeSpace       = 1f;
    public float        spawnTime       = -1f;
    public ItemRule     itemRule            ;

    public Action onRuleEnd;

    public IEnumerator GenerateBirdByRule()
    {
        float time = 0;
        yield return new WaitForSeconds(startTime);

        if(spawnBatch < 0 )
        {
            while(time <= spawnTime)
            {
                yield return SpawnBatch();
                time += timeSpace;
            }
        }
        else
        {
            while (spawnBatch-- > 0)
            {
                yield return SpawnBatch();
            }
        }
        onRuleEnd?.Invoke();
    }

    private IEnumerator SpawnBatch()
    {
        for(int i = 0; i < enemyPerBatch; i++)
        {
            GameObject obj = BirdFactory.Instance.GenerateBird(enemyType);
            AIBird bird = obj.GetComponent<AIBird>();
            bird.maxHp = (int)maxHp;
            bird.bulletType = bulletType;
            if(UnityEngine.Random.Range(0f, 1f) <= itemRule.dropRate)
                bird.dropItem = itemRule.itemType;
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(Mathf.Max(0, timeSpace - enemyPerBatch * 0.3f));
    }
}
