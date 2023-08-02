using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeFactory : MonoBehaviour
{
    public GameObject pipeType;
    public int initialPipeNum = 5;
    public float gapX = 3.0f;
    public float moveSpeed = 5.0f;
    public float existDistance = 10.0f;
    public float rangeY = 5.0f;
    public bool debugRange = false;

    float ExistTime { get => existDistance / moveSpeed; }
    float GenerateTime { get => gapX / moveSpeed;  }
    List<Coroutine> coroutineList = new List<Coroutine>();
    List<GameObject> allPipeList = new List<GameObject>(); 
    Queue<GameObject> deactivePipe = new Queue<GameObject>();

    private void Start()
    {
        InitPipes();

        GameManager.Instance.OnGameStart += OnGameStart;
        GameManager.Instance.OnGameEnd += OnGameEnd;
        GameManager.Instance.OnGameRestart += OnGameRestart;
    }

    void OnGameStart()
    {
        coroutineList.Add(StartCoroutine(GeneratePipeLoop()));
    }

    void OnGameEnd()
    {
        foreach (Coroutine coroutine in coroutineList)
            StopCoroutine(coroutine);
        coroutineList.Clear();
    }

    void OnGameRestart()
    {
        foreach (GameObject pipe in allPipeList)
        {
            if(pipe.activeSelf)
            {
                DestroyPipe(pipe);
            }
        }
    }

    void InitPipes()
    {
        for(int i = 0; i < initialPipeNum; ++i)
        {
            MakeNewPipe();
        }
    }

    void MakeNewPipe()
    {
        GameObject newPipe = Instantiate(pipeType, transform);
        newPipe.SetActive(false);
        deactivePipe.Enqueue(newPipe);
        allPipeList.Add(newPipe);
    }

    GameObject ActivePipe()
    {
        if (deactivePipe.Count == 0)
        {
            MakeNewPipe();
        }
        GameObject pipe = deactivePipe.Dequeue();
        pipe.transform.position = transform.position;
        pipe.SetActive(true);
        return pipe;
    }

    void DestroyPipe(GameObject pipe)
    {
        pipe.SetActive(false);
        deactivePipe.Enqueue(pipe);
    }

    void RandomY(GameObject pipe)
    {
        pipe.transform.position += Vector3.up * (debugRange ? rangeY : Random.Range(0, rangeY));
    }

    IEnumerator GeneratePipe()
    {
        GameObject pipe = ActivePipe();
        RandomY(pipe);
        yield return new WaitForSeconds(ExistTime);
        DestroyPipe(pipe);
    }

    IEnumerator GeneratePipeLoop()
    {
        while(true)
        {
            coroutineList.Add(StartCoroutine(GeneratePipe()));
            yield return new WaitForSeconds(GenerateTime);
        }
    }
}
