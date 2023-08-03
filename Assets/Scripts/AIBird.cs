using AOT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBird : MonoBehaviour, IFactoryObject
{
    public float moveSpeed = -5.0f;
    public float attackSpeed = 1.0f;
    public Transform bulletStart;
    public float bulletSpeed = -10.0f;

    Rigidbody2D rigid;
    Animator animator;
    Vector3 originalPosition;
    bool attackLock = true;
    bool moveLock = true;

    [HideInInspector]
    public BirdFactory factory;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        if (rigid != null) rigid.gravityScale = 0f;

        animator = GetComponentInChildren<Animator>();

        originalPosition = transform.position;

        GameManager.Instance.OnGameStart += OnGameStart;
        GameManager.Instance.OnGameEnd += OnGameEnd;
        GameManager.Instance.OnGameRestart += OnGameRestart;
    }

    private void OnEnable()
    {
        OnGameStart();
    }

    private void OnDisable()
    {
        OnGameEnd();
    }

    void OnGameStart()
    {
        if(animator != null) 
        { 
            animator.enabled = true; 
        }
        attackLock = false;
        moveLock = false;
    }

    void OnGameEnd()
    {
        if (animator != null)
        {
            animator.enabled = false;
        }
        attackLock = true;
        moveLock = true;
        CancelInvoke();
    }

    void OnGameRestart()
    {
        animator.enabled = true;
        animator.SetTrigger("Reset");
        attackLock = true;
        moveLock = true;
        transform.position = originalPosition;
    }

    private void Update()
    {
        if (GameManager.Instance.state == GameState.End) return;

        VectorMove(Vector3.right);
        Attack();
    }

    void VectorMove(Vector3 moveDistance)
    {
        if (moveLock) return;

        animator.SetTrigger("Fly");
        moveDistance.Normalize();
        moveDistance *= moveSpeed;
        moveDistance *= Time.deltaTime;
        transform.position = transform.position + moveDistance;
    }

    void Attack()
    {
        if (attackLock) return;
        FireOnce();
    }

    void FireOnce()
    {
        if (BulletPool.Instance == null) return;

        attackLock = true;
        GameObject bullet = BulletPool.Instance.ActiveBullet(bulletStart);
        bullet.GetComponent<LoopMove>().moveSpeed = bulletSpeed;
        bullet.tag = tag + "_bullet";
        bullet.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        float sleepTime = 1.0f / attackSpeed;
        Invoke("ResetFire", sleepTime);
    }

    void ResetFire()
    {
        attackLock = false;
    }


    public void DestroySelf()
    {
        factory?.DestroyBird(gameObject);
    }
}
