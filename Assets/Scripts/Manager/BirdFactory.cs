using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFactory : FactoryBase<BirdFactory>
{
    public float existDistance = 10.0f;

    public AIBoss bossType;
    public HPBar bossBar;
    public Transform bossStayPos;

    AIBoss bossInstance;
    Dictionary<GameObject, Coroutine> allBirdList = new();

    protected override void Start()
    {
        base.Start();

        GameManager.Instance.OnGameEnd += OnGameEnd;
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
        if(allBirdList.ContainsKey(obj))
        {
            var coroutine = allBirdList[obj];
            if(coroutine != null) StopCoroutine(coroutine);
        }
    }

    IEnumerator DestroyBirdByTime(GameObject bird)
    {
        float existTime = Mathf.Abs( existDistance / bird.GetComponent<AIBird>().moveSpeed);
        yield return new WaitForSeconds(existTime);
        DestroyObj(bird);
    }

    public GameObject GenerateBird(GameObject birdType)
    {
        if(objList.IndexOf(birdType) == -1)
        {
            Debug.LogWarning("Bird type doesn't exist in factory!");
            return null;
        }
        GameObject bird = ActiveObj(transform, birdType);
        allBirdList[bird] = StartCoroutine(DestroyBirdByTime(bird));
        return bird;
    }

    public AIBoss GenerateBoss()
    {
        if (bossInstance == null)
        {
            bossInstance = Instantiate(bossType);
            bossInstance.bossStayPos = bossStayPos;
            bossInstance.RunBoss();

            bossBar.obj = bossInstance.gameObject;
            bossBar.gameObject.SetActive(true);

            bossInstance.onDie += delegate ()
            {
                bossBar.obj = null;
                bossBar.gameObject.SetActive(false);
                bossInstance = null;
            };
        }
        return bossInstance;
    }
}
