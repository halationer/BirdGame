using AOT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAttack : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float attackSpeed = 1.0f;
    public GameObject bulletType;

    Rigidbody2D rigid;
    Animator animator;
    Vector3 originalPosition;
    bool attackLock = true;
    bool moveLock = true;

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

    void OnGameStart()
    {
        attackLock = false;
        moveLock = false;
    }

    void OnGameEnd()
    {
        animator.enabled = false;
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

        AxisMove();
        Attack();
    }

    void AxisMove()
    {
        if (moveLock) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDistance = new Vector3(horizontal, vertical, 0);
        moveDistance.Normalize();
        moveDistance *= moveSpeed;
        moveDistance *= Time.deltaTime;
        Vector3 preMovePositon = transform.position + moveDistance;

        if (Screen.safeArea.Contains(Camera.main.WorldToScreenPoint(preMovePositon)))
        {
            transform.position = preMovePositon;
        }
    }

    void Attack()
    {
        if (attackLock) return;

        if (Input.GetAxisRaw("Fire1") == 1.0f)
        {
            FireOnce();
        }
    }

    void FireOnce()
    {
        attackLock = true;
        GameObject bullet = Instantiate(bulletType);
        bullet.transform.position = transform.position;
        float sleepTime = 1.0f / attackSpeed;
        Invoke("ResetFire", sleepTime);
    }

    void ResetFire()
    {
        attackLock = false;
    }

    void CollisionDie()
    {
        GameManager.Instance.EndGame();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "ground":
            case "pipe":
                CollisionDie(); break;
        }
    }
}
