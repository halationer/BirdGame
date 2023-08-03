using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BirdFactory : MonoBehaviour
{
    public GameObject birdType;
    public int initialBirdNum = 5;
    public float gapX = 3.0f;
    public float moveSpeed = -5.0f;
    public float existDistance = 10.0f;
    public float rangeY = 5.0f;
    public bool debugRange = false;

    float ExistTime { get => Mathf.Abs(existDistance / moveSpeed); }
    float GenerateTime { get => Mathf.Abs(gapX / moveSpeed); }
    Dictionary<GameObject, Coroutine> allBirdList = new();
    Queue<GameObject> deactiveBird = new();

    private void Start()
    {
        InitBirds();

        GameManager.Instance.OnGameStart += OnGameStart;
        GameManager.Instance.OnGameEnd += OnGameEnd;
        GameManager.Instance.OnGameRestart += OnGameRestart;
    }

    void OnGameStart()
    {
        StartCoroutine(GenerateBirdLoop());
        //coroutineList.Add(StartCoroutine(GenerateBirdLoop()));
    }

    void OnGameEnd()
    {
        //foreach (Coroutine coroutine in coroutineList)
        //    StopCoroutine(coroutine);
        //coroutineList.Clear();
        StopAllCoroutines();
    }

    void OnGameRestart()
    {
        foreach (var bird in allBirdList)
        {
            if (bird.Key.activeSelf)
            {
                DestroyBird(bird.Key);
            }
        }
    }

    void InitBirds()
    {
        for (int i = 0; i < initialBirdNum; ++i)
        {
            MakeNewBird();
        }
    }

    void MakeNewBird()
    {
        GameObject newBird = Instantiate(birdType, transform);
        newBird.SetActive(false);
        newBird.GetComponent<AIBird>().factory = this;
        deactiveBird.Enqueue(newBird);
        allBirdList.Add(newBird, null);
    }

    GameObject ActiveBird()
    {
        if (deactiveBird.Count == 0)
        {
            MakeNewBird();
        }
        GameObject bird = deactiveBird.Dequeue();
        bird.GetComponent<AIBird>().moveSpeed = moveSpeed;
        bird.transform.position = transform.position;
        bird.SetActive(true);
        return bird;
    }

    public void DestroyBird(GameObject bird)
    {
        bird.SetActive(false);
        deactiveBird.Enqueue(bird);
        var coroutine = allBirdList[bird];
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    void RandomY(GameObject bird)
    {
        bird.transform.position += Vector3.up * (debugRange ? rangeY : Random.Range(0, rangeY));
    }

    IEnumerator DestroyBirdByTime(GameObject bird)
    {
        yield return new WaitForSeconds(ExistTime);
        DestroyBird(bird);
    }

    void GenerateBird()
    {
        GameObject bird = ActiveBird();
        RandomY(bird);
        allBirdList[bird] = StartCoroutine(DestroyBirdByTime(bird));
    }

    IEnumerator GenerateBirdLoop()
    {
        while (true)
        {
            GenerateBird();
            yield return new WaitForSeconds(GenerateTime);

            GenerateBird();
            yield return new WaitForSeconds(GenerateTime * 0.5f);

            GenerateBird();
            yield return new WaitForSeconds(GenerateTime * 0.5f);
        }
    }
}
