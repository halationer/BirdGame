using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPlayer : MonoBehaviour
{
    public float flyStrength = 1.0f;

    Rigidbody2D rigid;
    Animator animator;
    Vector3 originalPosition;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        rigid.simulated = false;
        originalPosition = transform.position;

        GameManager.Instance.OnGameStart += OnGameStart;
        GameManager.Instance.OnGameEnd += OnGameEnd;
        GameManager.Instance.OnGameRestart += OnGameRestart;
    }

    void OnGameStart()
    {
        rigid.simulated = true;
    }

    void OnGameEnd()
    {
        rigid.simulated = false;
        animator.enabled = false;
    }

    void OnGameRestart()
    {
        animator.enabled = true;
        animator.SetTrigger("Reset");
        transform.position = originalPosition;
    }

    private void Update()
    {
        if (GameManager.Instance.state == GameState.End) return;

        if (Input.GetMouseButtonDown(0))
        {
            FlyUpOnce();
        }
    }

    void FlyUpOnce()
    {
        if(rigid != null)
        {
            rigid.velocity = Vector3.zero;
            rigid.AddForce(Vector3.up * flyStrength, ForceMode2D.Impulse);
        }
        animator.SetTrigger("Fly");
    }

    void CollisionDie()
    {
        GameManager.Instance.LoseGame();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "ground":
            case "pipe":
                CollisionDie(); break;                
        }
    }
}
