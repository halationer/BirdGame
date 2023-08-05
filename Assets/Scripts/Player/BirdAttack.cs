using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAttack : Player
{
    protected override void OnGameEnd()
    {
        base.OnGameEnd();

        rigid.gravityScale = 1.0f;
        rigid.velocity = Vector3.zero;
        rigid.AddForce(Vector2.down * 5.0f, ForceMode2D.Impulse);
    }

    protected override void Move()
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

    public void Die()
    {
        GameManager.Instance.EndGame();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ע�⣬collision.gameObject ��Զ��ȡ���Ķ���ֱ�ӹ��ص������Ĵ�� object
        // �� collision.collider ���Ի�ȡ��׼ȷ����ײ����
        switch (collision.collider.tag) 
        {
            case "enemy":
                Die(); break;
        }
    }
}
