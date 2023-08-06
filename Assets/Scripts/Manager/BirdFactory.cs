using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFactory : FactoryBase
{
    public List<List<float>> birdGenerateTable;
    public float existDistance = 10.0f;

    public GameObject bossObject;
    public float bossAppearTime = 60.0f;

    Dictionary<GameObject, Coroutine> allBirdList = new();

    protected override void Start()
    {
        base.Start();

        if (birdGenerateTable == null)
        {
            birdGenerateTable = new();

            birdGenerateTable.Add(new()
            {
                0.5f,
                0.5f,
                1.0f,
                0.5f,
                0.5f,
                2.0f,
            });
            birdGenerateTable.Add(new()
            {
                3.0f,
                0.5f,
                0.5f,
                4.0f,
                0.5f,
                0.5f
            });
            birdGenerateTable.Add(new()
            {
                4.0f,
                8.0f
            });
        }

        GameManager.Instance.OnGameStart += OnGameStart;
        GameManager.Instance.OnGameEnd += OnGameEnd;
    }

    void OnGameStart()
    {
        List<Coroutine> list = new List<Coroutine>();
        for (int typeIndex = 0; typeIndex < objList.Count; ++typeIndex)
        {
            list.Add(StartCoroutine(GenerateBirdLoop(typeIndex)));
        }

        StartCoroutine(GenerateBoss(list, bossAppearTime));
    }

    void OnGameEnd()
    {
        StopAllCoroutines();
    }

    protected override GameObject MakeNewObj(int typeIndex)
    {
        GameObject newBird = base.MakeNewObj(typeIndex);
        newBird.GetComponent<AIBird>().factory = this;
        allBirdList.Add(newBird, null);
        return newBird;
    }

    public override void DestroyObj(GameObject obj)
    {
        base.DestroyObj(obj);
        var coroutine = allBirdList[obj];
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    GameObject ActiveBird(GameObject obj = null)
    {
        return ActiveObj(transform, obj);
    }

    public void DestroyBird(GameObject bird)
    {
        DestroyObj(bird);
    }

    IEnumerator DestroyBirdByTime(GameObject bird)
    {
        float existTime = Mathf.Abs( existDistance / bird.GetComponent<AIBird>().moveSpeed);
        yield return new WaitForSeconds(existTime);
        DestroyBird(bird);
    }

    IEnumerator GenerateBird(int typeIndex, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject bird = ActiveBird(objList[typeIndex]);
        allBirdList[bird] = StartCoroutine(DestroyBirdByTime(bird));
    }

    IEnumerator GenerateBirdLoop(int typeIndex)
    {
        yield return new WaitForSeconds(2.0f);

        while (true)
        {
            foreach (float time in birdGenerateTable[typeIndex])
            {
                yield return GenerateBird(typeIndex, time);
            }
        }
    }

    IEnumerator GenerateBoss(List<Coroutine> coroutines, float time)
    {
        yield return new WaitForSeconds(time);

        foreach(Coroutine coroutine in coroutines)
        {
            StopCoroutine(coroutine);
        }

        if (bossObject != null)
        {
            bossObject.GetComponent<AIBoss>()?.GenerateBoss();
        }
    }
}
