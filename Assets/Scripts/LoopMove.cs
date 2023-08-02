using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopMove : MonoBehaviour
{
    public bool moving = true;
    public bool loop = true;
    public float moveSpeed = 5.0f;

    [Range(0f, 100.0f)]
    public float loopOffset = 10.0f;


    Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
        GameManager.Instance.OnGameStart += delegate () { moving = true; };
        GameManager.Instance.OnGameEnd += delegate () { moving = false; };
        GameManager.Instance.OnGameRestart += delegate () { moving = true; };
    }

    private void Update()
    {
        if(moving)
        {
            Move();
        }
    }

    public void Move()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        if (!loop) return;
        
        float currentOffset = (transform.position - originalPosition).magnitude;
        if( currentOffset > loopOffset)
        {
            transform.Translate(-Mathf.Sign(moveSpeed) * loopOffset, 0, 0);
        }
    }
}
