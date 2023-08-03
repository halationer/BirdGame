using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPlayer : MonoBehaviour
{
    public float flyStrength = 1.0f;

    Rigidbody2D rigid;
    Animator animator;
    Vector3 originalPosition;
    bool flyLock = false;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        rigid.simulated = false;
        originalPosition = transform.position;

        GameManager.Instance.OnGameStart += OnGameStart;
        GameManager.Instance.OnGameEnd += OnGameEnd;
        GameManager.Instance.OnGameRestart += OnGameRestart;
        GameManager.Instance.OnGameSimulateEnd += OnGameSimulatedEnd;
    }

    void OnGameStart()
    {
        rigid.simulated = true;
        flyLock = false;
        FlyUpOnce();
    }

    void OnGameEnd()
    {
        flyLock = true;
    }

    void OnGameSimulatedEnd()
    {
        rigid.simulated = false;
        animator.enabled = false;
    }

    void OnGameRestart()
    {
        flyLock = false;
        rigid.simulated = false;
        animator.enabled = true;
        animator.SetTrigger("Reset");
        transform.position = originalPosition;
    }

    private void Update()
    {
        if (GameManager.Instance.state != GameState.Playing) return;

        if (Input.GetMouseButtonDown(0))
        {
            FlyUpOnce();
        }
    }

    void FlyUpOnce()
    {
        if (flyLock) return;

        if(rigid != null)
        {
            rigid.velocity = Vector3.zero;
            rigid.AddForce(Vector3.up * flyStrength, ForceMode2D.Impulse);
        }
        animator.SetTrigger("Fly");
    }

    void CollisionDie()
    {
        GameManager.Instance.EndGame();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "ground":
                CollisionDie();
                GameManager.Instance.SimulateEnd();
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "pipe":
                CollisionDie();
                break;
        }
    }
}
