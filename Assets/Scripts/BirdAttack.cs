using AOT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAttack : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float attackSpeed = 1.0f;
    public Transform bulletStart;
    public float bulletSpeed = 10.0f;

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
        rigid.gravityScale = 1.0f;
        rigid.velocity = Vector3.zero;
        rigid.AddForce(Vector2.down * 5.0f, ForceMode2D.Impulse);
    }

    void OnGameRestart()
    {
        animator.enabled = true;
        animator.SetTrigger("Reset");
        attackLock = true;
        moveLock = true;
        transform.position = originalPosition; 
        rigid.gravityScale = 0f;
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
        if (moveDistance.magnitude < 0.1f) return;

        animator.SetTrigger("Fly");
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
        if (BulletPool.Instance == null) return;

        attackLock = true;
        GameObject bullet = BulletPool.Instance.ActiveBullet(bulletStart);
        bullet.GetComponent<LoopMove>().moveSpeed = bulletSpeed;
        bullet.tag = tag + "_bullet";
        bullet.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        float sleepTime = 1.0f / attackSpeed;
        Invoke("ResetFire", sleepTime);
    }

    void ResetFire()
    {
        attackLock = false;
    }

    public void CollisionDie()
    {
        GameManager.Instance.EndGame();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "ground":
            case "pipe":
            case "enemy":
                CollisionDie(); break;
        }
    }
}
