using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ILifeObject
{
    public int maxHp = 10;
    protected int hp = 0;

    public float moveSpeed = 5.0f;
    public float attackSpeed = 1.0f;
    public Transform bulletStart;
    public float bulletSpeed = 10.0f;
    public GameObject bulletType;

    protected Rigidbody2D rigid;
    protected Animator animator;
    protected Vector3 originalPosition;
    protected bool attackLock = true;
    protected bool moveLock = true;

    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        if (rigid != null) rigid.gravityScale = 0f;

        animator = GetComponentInChildren<Animator>();

        originalPosition = transform.position;

        GameManager.Instance.OnGameStart += OnGameStart;
        GameManager.Instance.OnGameEnd += OnGameEnd;
        GameManager.Instance.OnGameRestart += OnGameRestart;
    }

    protected virtual void OnGameStart()
    {
        hp = maxHp;
        attackLock = false;
        moveLock = false;
    }

    protected virtual void OnGameEnd()
    {
        if(animator != null)
        {
            animator.enabled = false;
        }
        attackLock = true;
        moveLock = true;
        CancelInvoke();
    }

    protected virtual void OnGameRestart()
    {
        animator.enabled = true;
        animator.SetTrigger("Reset");
        attackLock = true;
        moveLock = true;
        transform.position = originalPosition;
        rigid.gravityScale = 0f;
    }

    protected virtual void Update()
    {
        if (GameManager.Instance.state == GameState.End) return;

        Move();
        Attack();
    }

    protected virtual void Move()
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

    protected virtual void Attack()
    {
        if (attackLock) return;

        if (Input.GetAxisRaw("Fire1") == 1.0f)
        {
            FireOnce();
        }
    }

    protected virtual void FireOnce()
    {
        if (BulletPool.Instance == null) return;

        attackLock = true;
        GameObject bullet = BulletPool.Instance.ActiveObj(bulletStart, bulletType);
        float sleepTime = 1.0f / attackSpeed;
        Invoke("ResetFire", sleepTime);
    }

    void ResetFire()
    {
        attackLock = false;
    }

    public int GetHP()
    {
        return hp;
    }

    public int GetMaxHP()
    {
        return maxHp;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp < 0) hp = 0;
    }

    public bool isDie()
    {
        return hp == 0;
    }
}
