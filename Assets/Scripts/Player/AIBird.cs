using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIType
{
    BirdCommon,
    BirdSwing,
    BirdQuick,
}

public class AIBird : Player, IFactoryObject, IScoreObject
{
    public AIType type = AIType.BirdCommon;

    [HideInInspector]
    public BirdFactory factory;
    [HideInInspector]
    public Item dropItem;

    float swingTime = 0;
    public float swingRange = 2.0f;
    public float swingSpeed = 2.0f;

    private int typeIndex; 
    public int TypeIndex { get => typeIndex; set => typeIndex = value; }


    [SerializeField]
    private int score = 1;
    public int Score { get => score; }

    private void OnEnable()
    {
        OnGameStart();
    }

    private void OnDisable()
    {
        OnGameEnd();
    }

    protected override void OnGameStart()
    {
        base.OnGameStart();

        if(animator != null) 
        { 
            animator.enabled = true;
        }

        swingTime = 0f;
        originalPosition = transform.position;
        GetComponent<Collider2D>().enabled = true;
    }

    protected override void OnGameEnd()
    {
        base.OnGameEnd();

        swingTime = 0f;
        GetComponent<Collider2D>().enabled = false;
    }
    
    protected override void Move()
    {
        if (moveLock) return;

        Vector3 moveDistance = Vector3.right;
        animator.SetTrigger("Fly");
        moveDistance.Normalize();
        moveDistance *= moveSpeed;
        moveDistance *= Time.deltaTime;
        transform.position = transform.position + moveDistance;

        if(type == AIType.BirdSwing)
        {
            swingTime += Time.deltaTime * swingSpeed;
            Vector3 swingPos = transform.position;
            swingPos.y = Mathf.Sin(swingTime) * swingRange + originalPosition.y;
            transform.position = swingPos;
        }
    }

    protected override void Attack()
    {
        if (attackLock) return;
        if (type == AIType.BirdQuick) return;
        FireOnce();
    }

    public void DestroySelf()
    {
        animator.SetTrigger("Die");
    }

    public void DestroySelf_AnimEvent()
    {
        if(dropItem != null)
        {
            Item item = Instantiate(dropItem);
            item.transform.position = transform.position;
            dropItem = null;
        }

        factory?.DestroyObj(gameObject);
    }
}
