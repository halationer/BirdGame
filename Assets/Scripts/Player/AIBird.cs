using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBird : Player, IFactoryObject
{
    [HideInInspector]
    public BirdFactory factory;

    public int TypeIndex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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

        GetComponent<Collider2D>().enabled = true;
    }

    protected override void OnGameEnd()
    {
        base.OnGameEnd();

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
    }

    protected override void Attack()
    {
        if (attackLock) return;
        FireOnce();
    }

    public void DestroySelf()
    {
        factory?.DestroyBird(gameObject);
    }
}
